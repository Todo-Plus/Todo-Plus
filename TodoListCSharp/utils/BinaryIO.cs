using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Documents;
using TodoListCSharp.core;
using TodoListCSharp.interfaces;
using System.Collections.Generic;

namespace TodoListCSharp.utils {
    /// <summary>
    /// 二进制类序列化，返回值以100开头，后面两位标识不同的类型
    /// 返回0表示函数正常运行
    /// </summary>
    public class BinaryIO:IOInterface {
        /// <summary>
        /// 文件二进制数据转换为对应的列表
        /// </summary>
        /// <param name="path">对应的文件路径，如果不存在，则返回空列表</param>
        /// <param name="output">项目列表，通过项目列表获取得到List进行序列化</param>
        /// <returns>
        /// 0 - 正常运行，不影响运行逻辑
        /// 10001 - 文件不存在
        /// </returns>
        public int FileToList(string path, ref ItemList output) {

            if (false == System.IO.File.Exists(path)) {
                output = new ItemList();

                // 文件不存在，但是不影响后续逻辑
                return 0;
            }
            
            FileStream loadFile = new FileStream(path, FileMode.Open, FileAccess.Read);
            IFormatter serializer = new BinaryFormatter();
            // 序列器已过期，后续改为其他方法
            List<TodoItem> list = serializer.Deserialize(loadFile) as List<TodoItem>;

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
            FileStream outputFile = new FileStream(path, FileMode.Create, FileAccess.Write);
            IFormatter serializer = new BinaryFormatter();
            List<TodoItem> list = input.GetItemListForSerializer();
            serializer.Serialize(outputFile, list);

            return 0;
        }

        public int FileToSetting(string path, ref Setting settings) {
            if (false == System.IO.File.Exists(path)) {
                settings = new Setting();
                // 文件不存在，读取默认设置
                return 0;
            }
            
            FileStream loadFile = new FileStream(path, FileMode.Open, FileAccess.Read);
            IFormatter serializer = new BinaryFormatter();
            
            settings = serializer.Deserialize(loadFile) as Setting;

            return 0;
        }

        public int SettingToFile(ref Setting settings, string path) {
            FileStream outputFile = new FileStream(path, FileMode.Create, FileAccess.Write);
            IFormatter serializer = new BinaryFormatter();
            serializer.Serialize(outputFile, settings);

            return 0;
        }
    }
}