using System;

namespace Super_Auto_Mobs
{
    [Serializable]
    public struct SettingsData
    {
        public bool IsAutoPlay;
        public LanguageService.Language Language;
        public float Music;
        public float Sound;
    }
}