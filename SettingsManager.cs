using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tizen.Applications;

namespace GreentegCoreApp1
{
    public static class SettingsManager
    {
        public static void Default()
        {
            Preference.RemoveAll();
            if (!Preference.Contains(SettingsNames.TemperatureTypeSetting))
            {
                
                SettingsVariables.TemperatureType = TemperatureType.C;
            }
            if (!Preference.Contains(SettingsNames.already_started))
            {
                SettingsVariables.already_started = false;
            }
        }
    }
    public static class SettingsNames
    {
        public static string already_started = "AsSystemSetting";
        public static string TemperatureTypeSetting = "TTSUserSetting";
    }
    public enum TemperatureType
    {
        C,F
    }
    public struct TTV
    {
        public TemperatureType type;
    }
    public static class SettingsVariables
    {
        public static bool already_started { get { return Preference.Get<bool>(SettingsNames.already_started); } set { Preference.Remove(SettingsNames.already_started); Preference.Set(SettingsNames.already_started, value); } }
        public static TemperatureType TemperatureType { get { return Preference.Get<TemperatureType>(SettingsNames.TemperatureTypeSetting); } set { Preference.Remove(SettingsNames.TemperatureTypeSetting); Preference.Set(SettingsNames.TemperatureTypeSetting, value); }}

    }
}
