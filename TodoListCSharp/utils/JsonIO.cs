using System;
using System.IO;
using TodoListCSharp.core;
using TodoListCSharp.interfaces;

namespace TodoListCSharp.utils {
    public class JsonIO: IOInterface {
        /// <summary>
        /// 读取持久化到硬盘中的Json文件，转换为ItemList类型
        /// todo： 对于Json文件的解析需要考虑是否使用第三方库
        /// </summary>
        /// <param name="path">Json文件路径</param>
        /// <param name="output">输出的ItemList</param>
        /// <returns></returns>
        public int FileToList(string path, ref ItemList output) {
            StreamReader file = null;
            try {
                file = File.OpenText(path);
            }
            catch (Exception e) {
                System.Console.WriteLine("File Not Exist");
                return -1;
            }
            
            return 0;
        }

        public int ListToFile(ref ItemList input, string path) {
            throw new System.NotImplementedException();
        }

        public int FileToSetting(string path, ref Setting settings) {
            throw new System.NotImplementedException();
        }

        public int SettingToFile(ref Setting settings, string path) {
            throw new System.NotImplementedException();
        }
    }
}