using TodoListCSharp.core;

// 文件持久化抽象类，派生出的类用于处理文件相关操作
// 这里的需要对派生出的类进行测试
namespace TodoListCSharp.interfaces {
    public interface IOInterface {
        // Item相关，从文件中读取得到事项，以及内存中的数据持久化到文件
        // 返回类型用于标识错误
        public abstract int FileToList(string path, ref ItemList output);
        public abstract int ListToFile(ref ItemList input, string path);

        // 调整后使用save来进行存取

        public abstract int FileToSave(string path, ref Save output);
        public abstract int SaveToFile(ref Save input, string path);
    }
}