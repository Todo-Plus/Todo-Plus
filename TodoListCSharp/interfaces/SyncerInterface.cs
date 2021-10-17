using System.Threading.Tasks;

namespace TodoListCSharp.interfaces {
    public abstract class SyncerInterface {
        // 初始化容器等相关信息
        public abstract int Initial();
        // 同步文件主函数
        public abstract Task<int> Sync();
        // 签名信息过期时进行更新
        public abstract int Refresh();
        // 下载文件
        protected abstract Task<int> Download();
        // 上传文件
        protected abstract Task<int> Upload();
        protected abstract int CheckBucket();
    }
}