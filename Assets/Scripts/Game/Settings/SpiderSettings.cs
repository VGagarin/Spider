using UnityEngine;

namespace Game.Settings
{
    internal static class SpiderSettings
    {
        public static GameRules GameRules { get; }
        public static DealingSettings DealingSettings { get; }
        
        static SpiderSettings()
        {
            GameRules = Resources.Load<GameRules>(GameRules.Path);
            DealingSettings = Resources.Load<DealingSettings>(DealingSettings.Path);
        }
    }
}