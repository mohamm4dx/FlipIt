using FlipIt.Controls;
using Microsoft.Win32;
using System;
using System.Globalization;
using System.Reflection;

namespace FlipIt
{
    internal static class GlobalData
    {
        public static CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

        public static TimeFormat TimeFormat { get; set; } = TimeFormat.TwentyFourHours;

        public static string DateTimeFormat { get; set; } = "MM/dd/yyyy";

        public static bool VisibleSeconds { get; set; } = false;

        public static bool VisibleDate { get; set; } = false;

        public static bool VisibleDayOfWeek { get; set; } = false;

        public static bool VisibleMonth { get; set; } = false;

        public static void Load()
        {
            var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\FlipItPro_ScreenSaver");
            if (key is not null)
            {
                foreach (var prop in typeof(GlobalData).GetProperties(BindingFlags.Public | BindingFlags.Static))
                {
                    try
                    {
                        if (prop.PropertyType.IsEnum)
                        {
                            prop.SetValue(null, Enum.Parse(prop.PropertyType, key.GetValue(prop.Name).ToString()));
                            continue;
                        }
                        else if (prop.PropertyType == typeof(bool))
                        {
                            prop.SetValue(null, Convert.ChangeType(key.GetValue(prop.Name), prop.PropertyType));
                            continue;
                        }
                        else if (prop.PropertyType == typeof(CultureInfo))
                        {
                            prop.SetValue(null, Activator.CreateInstance(prop.PropertyType, key.GetValue(prop.Name)));
                            continue;
                        }
                        prop.SetValue(null, key.GetValue(prop.Name));
                        if (Culture is null) Culture = CultureInfo.InvariantCulture;
                    }
                    catch { }
                }
            }
        }

        public static void Save()
        {
            // Create or get existing subkey
            RegistryKey key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\FlipItPro_ScreenSaver");
            foreach (var prop in typeof(GlobalData).GetProperties(BindingFlags.Public | BindingFlags.Static))
            {
                try { key.SetValue(prop.Name, prop.GetValue(null).ToString()); } catch { }
            }
        }

        public static void Reset()
        {
            Culture = CultureInfo.InvariantCulture;
            TimeFormat = TimeFormat.TwentyFourHours;
            DateTimeFormat = "MM/dd/yyyy";
            VisibleSeconds = false;
            VisibleDate = false;
            VisibleDayOfWeek = false;
            VisibleMonth = false;
        }
    }
}