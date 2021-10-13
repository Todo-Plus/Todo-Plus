using System;
using System.Collections.Generic;
using System.IO;
using TodoListCSharp.core;
using TodoListCSharp.interfaces;
using System.Text.Json;

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
            if (false == System.IO.File.Exists(path)) {
                output = new ItemList();
                return 0;
            }

            FileStream loadFile = new FileStream(path, FileMode.Open, FileAccess.Read);
            int iFileLength = (int)loadFile.Length;
            byte[] bFileBytes = new byte[iFileLength];
            int ret = loadFile.Read(bFileBytes, 0, iFileLength);
            string sJsonString = System.Text.Encoding.UTF8.GetString(bFileBytes);

            List<TodoItem> list = JsonSerializer.Deserialize<List<TodoItem>>(sJsonString);
            
            if (list.Count == 1) {
                output = new ItemList();
                return 0;
            }
            
            list.RemoveAt(0);
            output = new ItemList(list[0]);
            for (int i = 1; i < list.Count; i++) {
                output.AppendItem(list[i]);
            }
            
            return 0;
        }

        public int ListToFile(ref ItemList input, string path) {
            List<TodoItem> list = input.GetItemListForSerializer();
            byte[] sJsonBytes = JsonSerializer.SerializeToUtf8Bytes(list);

            FileStream outputFIle = new FileStream(path, FileMode.Create, FileAccess.Write);
            outputFIle.Write(sJsonBytes);
            return 0;
        }

        public int FileToSetting(string path, ref Setting settings) {
            throw new System.NotImplementedException();
        }

        public int SettingToFile(ref Setting settings, string path) {
            throw new System.NotImplementedException();
        }

        public int FileToSave(string path, ref Save output) {
            throw new NotImplementedException();
        }

        public int SaveToFile(ref Save input, string path) {
            throw new NotImplementedException();
        }
    }
}