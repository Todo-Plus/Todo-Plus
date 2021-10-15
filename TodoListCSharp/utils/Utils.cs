using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Runtime.InteropServices;
using IWshRuntimeLibrary;
using TodoListCSharp.core;

namespace TodoListCSharp.utils {
    public class Utils {
        // !! dll import
        
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);
        [DllImport("user32.dll")]
        static extern IntPtr GetDesktopWindow();

        public static IntPtr SearchDesktopHandle() {
            IntPtr hRoot = GetDesktopWindow();
            IntPtr hShellDll = IntPtr.Zero;
            IntPtr hDesktop = FindWindowEx(hRoot, IntPtr.Zero, "WorkerW", String.Empty);
            while (true) {
                hShellDll = FindWindowEx(hDesktop, IntPtr.Zero, "SHELLDLL_DefView", String.Empty);
                if (hShellDll != IntPtr.Zero) {
                    return hDesktop;
                }
                hDesktop = FindWindowEx(hRoot, hDesktop, "WorkerW", String.Empty);
            }

            return IntPtr.Zero;
        }

        public static string MediaColorToHex(Color color) {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        public static Color HexToMediaColor(string sColorString) {
            return (Color)ColorConverter.ConvertFromString(sColorString);
        }

        public static int TabListGetPosition(ref List<Tab> tabs, int iIndex) {
            int length = tabs.Count;
            for (int i = 0; i < length; i++) {
                if (tabs[i].Id == iIndex) {
                    return i;
                }
            }

            return -1;
        }

        public static ItemList ListToItemListForIO(List<TodoItem> list) {
            ItemList oRetList = new ItemList();

            if (list.Count <= 1) {
                return oRetList;
            }
            
            list.RemoveAt(0);
            int length = list.Count;
            for (int i = 0; i < length; i++) {
                oRetList.AppendItem(list[i]);
            }

            return oRetList;
        }

        public static Color GenerateAdaptColor(Color color) {
            int Red = (int)color.R;
            int Green = (int)color.G;
            int Blue = (int)color.B;

            if (Red * 0.213 + Green * 0.715 + Blue * 0.072 > 255 / 2.0) {
                return Constants.MEDIA_COLOR_BALCK;
            }
            else {
                return Constants.MEDIA_COLOR_WHITE;
            }
        }
        
        public static string GetAppPathFromQuick(string shortcutPath)
        {
            if (System.IO.File.Exists(shortcutPath))
            {
                WshShell shell = new WshShell();
                IWshShortcut shortct = (IWshShortcut)shell.CreateShortcut(shortcutPath);
                return shortct.TargetPath;
            }
            else
            {
                return "";
            }
        }
        
        public static List<string> GetQuickFromFolder(string directory, string targetPath)
        {
            List<string> tempStrs = new List<string>();
            tempStrs.Clear();
            string tempStr = null;
            string[] files = Directory.GetFiles(directory, "*.lnk");
            if (files == null || files.Length < 1)
            {
                return tempStrs;
            }
            for (int i = 0; i < files.Length; i++)
            {
                tempStr = GetAppPathFromQuick(files[i]);
                if (tempStr == targetPath)
                {
                    tempStrs.Add(files[i]);
                }
            }
            return tempStrs;
        }
        
        public static void DeleteFile(string path)
        {
            FileAttributes attr = System.IO. File.GetAttributes(path);
            if (attr == FileAttributes.Directory)
            {
                Directory.Delete(path, true);
            }
            else
            {
                System.IO.File.Delete(path);
            }
        }
        
        public static void CreateDesktopQuick(string desktopPath = "", string quickName = "", string appPath = "")
        {
            List<string> shortcutPaths = GetQuickFromFolder(desktopPath, appPath);
            //如果没有则创建
            if (shortcutPaths.Count < 1)
            {
                CreateShortcut(desktopPath, quickName, appPath, "软件描述");
            }
        }
        
        public static bool CreateShortcut(string directory, string shortcutName, string targetPath, string description = null, string iconLocation = null)
        {
            try
            {
                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
                string shortcutPath = Path.Combine(directory, string.Format("{0}.lnk", shortcutName));          
                WshShell shell = new IWshRuntimeLibrary.WshShell();
                IWshShortcut shortcut = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(shortcutPath);    
                shortcut.TargetPath = targetPath;                                                              
                shortcut.WorkingDirectory = Path.GetDirectoryName(targetPath);                                 
                shortcut.WindowStyle = 1;                                                                       
                shortcut.Description = description;                                                             
                shortcut.IconLocation = string.IsNullOrWhiteSpace(iconLocation) ? targetPath : iconLocation;    
                shortcut.Save();                                                                                
                return true;
            }
            catch(Exception ex)
            {
                string temp = ex.Message;
                temp = "";
            }
            return false;
        }
    }
}