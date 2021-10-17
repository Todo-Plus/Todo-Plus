namespace TodoListCSharp.interfaces {
    public abstract class SyncerInterface {
        // 初始化容器等相关信息
        public abstract int Initial();
        // 同步文件主函数
        public abstract int Sync();
        // 签名信息过期时进行更新
        public abstract int Refresh();
        // 搜索是否存在对应的文件
        protected abstract int Search();
        // 下载文件
        protected abstract int Download();
        // 上传文件
        protected abstract int Upload();
    }
}