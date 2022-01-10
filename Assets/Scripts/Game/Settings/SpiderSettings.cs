using UnityEngine;

namespace Game.Settings
{
    internal static class SpiderSettings
    {
        public static GameRules GameRules { get; }
        public static DealingSettings DealingSettings { get; }
        public static GameFieldLayoutSettings GameFieldLayoutSettings { get; }
        
        static SpiderSettings()
        {
            GameRules = Resources.Load<GameRules>(GameRules.Path);
            DealingSettings = Resources.Load<DealingSettings>(DealingSettings.Path);
            GameFieldLayoutSettings = Resources.Load<GameFieldLayoutSettings>(GameFieldLayoutSettings.Path);
        }
    }
}