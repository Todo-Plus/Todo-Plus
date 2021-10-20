using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media;
using TodoListCSharp.utils;

namespace TodoListCSharp.core {
    // Setting类，单例模式，beta版本，后续进行设计
    [Serializable]
    public class Setting {
        // Appearance
        public int Alpha { get; set; }
        public int Fontsize { get; set; }
        public string FontFamily { get; set; }
        public Color BackgroundColor { get; set; }

        // Window
        public Rect WindowBounds { get; set; }
        public bool AutoRun { get; set; }
        public bool CloseTips { get; set; }
        public Constants.MainWindowLockStatu LockStatus { set; get; }

        private static readonly string RegistryPath = @"Software\TodoPlus\Settings";
        
        
        // Sync

        public string appid { get; set; }
        public string secretId { get; set; }
        public string secretKey { get; set; }
        public Setting() {
            // default setting
            Alpha = 100;
            Fontsize = 16;
            FontFamily = "Inter";
            BackgroundColor = Color.FromRgb(0xFF, 0xFF, 0xFF);
            AutoRun = false;
            CloseTips = true;
            WindowBounds = Rect.Empty;
            LockStatus = Constants.MainWindowLockStatu.DRAGABLE;
            
            // sync setting
            
        }

        // todo：深拷贝的实现，后面考虑其他方法不用重新添加
        public Setting(Setting _setting) {
            Alpha = _setting.Alpha;
            Fontsize = _setting.Fontsize;
            FontFamily = _setting.FontFamily;
            BackgroundColor = _setting.BackgroundColor;
            AutoRun = _setting.AutoRun;
            CloseTips = _setting.CloseTips;
            WindowBounds = _setting.WindowBounds;
            LockStatus = _setting.LockStatus;
        }

        // 返回值表明读取结果
        public int ReadSettingFromRegistryTable() {
            RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryPath);

            if (key == null) {
                return -1;
            }

            WindowBounds = Rect.Parse($"{key.GetValue("WindowBounds")}");
            Alpha = int.Parse($"{key.GetValue("Alpha")}");
            Fontsize = int.Parse($"{key.GetValue("Font-size")}");
            FontFamily = $"{key.GetValue("Font-family")}";
            BackgroundColor = Utils.HexToMediaColor($"{key.GetValue("Background-color")}");
            AutoRun = bool.Parse($"{key.GetValue("AutoRun")}");
            CloseTips = bool.Parse($"{key.GetValue("CloseTips")}");

            return 0;
        }

        public int SaveSettingToRegistryTable(Window window) {
            Registry.CurrentUser.CreateSubKey(RegistryPath)?.SetValue("Alpha", Alpha);
            Registry.CurrentUser.CreateSubKey(RegistryPath)?.SetValue("Font-size", Fontsize);
            Registry.CurrentUser.CreateSubKey(RegistryPath)?.SetValue("Font-family", FontFamily);
            Registry.CurrentUser.CreateSubKey(RegistryPath)?.SetValue("Background-color", Utils.MediaColorToHex(BackgroundColor));
            Registry.CurrentUser.CreateSubKey(RegistryPath)?.SetValue("WindowBounds", window.RestoreBounds);
            Registry.CurrentUser.CreateSubKey(RegistryPath)?.SetValue("AutoRun", AutoRun);
            Registry.CurrentUser.CreateSubKey(RegistryPath)?.SetValue("CloseTips", CloseTips);
            return 0;
        }
    }
}