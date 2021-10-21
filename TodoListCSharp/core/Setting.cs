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
        public string region { get; set; }
        public string bucket { get; set; }
        
        public Constants.SyncerType eSyncerType { get; set; }
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
            eSyncerType = Constants.SyncerType.NONE;
            appid = string.Empty;
            secretId = string.Empty;
            secretKey = string.Empty;
            region = string.Empty;
            bucket = string.Empty;

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
            eSyncerType = _setting.eSyncerType;
            appid = _setting.appid;
            secretId = _setting.secretId;
            secretKey = _setting.secretKey;
            region = _setting.region;
            bucket = _setting.bucket;
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

            try {
                eSyncerType = Enum.Parse<Constants.SyncerType>(Utils.GetRegistryValue(key, "SyncerType"));
            }
            catch (Exception e) {
                eSyncerType = Constants.SyncerType.NONE;
            }
            
            appid = Utils.GetRegistryValue(key, "Appid");
            secretId = Utils.GetRegistryValue(key, "Sid");
            secretKey = Utils.GetRegistryValue(key, "Skey");
            region =  Utils.GetRegistryValue(key, "Region");
            bucket =  Utils.GetRegistryValue(key, "Bucket");

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

            Registry.CurrentUser.CreateSubKey(RegistryPath)?.SetValue("SyncerType", eSyncerType);
            Registry.CurrentUser.CreateSubKey(RegistryPath)?.SetValue("Appid", appid);
            Registry.CurrentUser.CreateSubKey(RegistryPath)?.SetValue("Sid", secretId);
            Registry.CurrentUser.CreateSubKey(RegistryPath)?.SetValue("Skey", secretKey);
            Registry.CurrentUser.CreateSubKey(RegistryPath)?.SetValue("Region", region);
            Registry.CurrentUser.CreateSubKey(RegistryPath)?.SetValue("Bucket", bucket);
            return 0;
        }
    }
}