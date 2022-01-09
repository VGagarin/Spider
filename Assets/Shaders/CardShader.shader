Shader "Custom/CardShader"
{
    Properties
    {
        _MainTex ("Main", 2D) = "white" {}
        
        _ValueTex ("Value", 2D) = "white" {}
        _ValueSizeRatio ("Size Ratio", float) = 1
        
        _SuitTex ("Suit", 2D) = "white" {}
        _SuitSizeRatio ("Size Ratio", float) = 1
        
        _SuitColor ("Suit Color", Color) = (1, 1, 1, 1)
        
        _FigureTex ("Figure", 2D) = "white" {}
        _FigureSizeRatio ("Size Ratio", float) = 1
        _FigureTint ("Figure Tint", Color) = (1, 1, 1, 1) 
    }
    SubShader
    {
        Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
        LOD 200

        CGPROGRAM
        #pragma surface surf CardShader alpha

        sampler2D _MainTex;
        float4 _MainTex_TexelSize;
        sampler2D _ValueTex;
        sampler2D _SuitTex;
        sampler2D _FigureTex;
        float _ValueSizeRatio;
        float _SuitSizeRatio;
        float _FigureSizeRatio;
        float4 _SuitColor;
        float4 _FigureTint;

        inline float4 LightingCardShader(SurfaceOutput s, fixed3 lightDir, fixed atten)
        {
            return float4(s.Albedo, s.Alpha);
        }

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_ValueTex;
            float2 uv_SuitTex;
            float2 uv_FigureTex;
        };

        float2 ScaleUV(float2 UV, float scale, float aspectRatio)
        {
            //+0.5 и -0.5 - двигаем картинку к середине, потом скейлим, потом обратно на место
            return float2((UV.x - 0.5) / scale * aspectRatio + 0.5,
                          (UV.y - 0.5) / scale + 0.5);
        }

        float4 CalculateColor(sampler2D tex, float4 tint, float2 uv, float sizeRatio, float aspectRatio)
        {
            float2 scaledUV = ScaleUV(uv, sizeRatio, aspectRatio);
            float4 color = tex2D(tex, scaledUV.xy);
            color *= tint;
            
            return color;
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            const float aspect = _MainTex_TexelSize.z / _MainTex_TexelSize.w;

            float4 valueColor = CalculateColor(_ValueTex, _SuitColor, IN.uv_ValueTex, _ValueSizeRatio, aspect);
            float4 suitColor = CalculateColor(_SuitTex, _SuitColor, IN.uv_SuitTex, _SuitSizeRatio, aspect);
            float4 figureColor = CalculateColor(_FigureTex, _FigureTint, IN.uv_FigureTex, _FigureSizeRatio, aspect);

            float4 mainColor = tex2D(_MainTex, IN.uv_MainTex);

            float a = mainColor.a;

            if (mainColor.a == 0)
                discard;
            
            mainColor = lerp(mainColor, valueColor, valueColor.a);
            mainColor = lerp(mainColor, suitColor, suitColor.a);
            mainColor = lerp(mainColor, figureColor, figureColor.a);

            o.Albedo = mainColor;
            o.Alpha = a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}