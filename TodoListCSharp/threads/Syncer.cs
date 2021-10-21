using System.Timers;
using TodoListCSharp.core;
using TodoListCSharp.interfaces;

namespace TodoListCSharp.threads {
    public class Syncer {
        public int LatestVersion { set; get; }
        public int LocationVersion { set; get; }
        public int RemoteVersion { set; get; }
        public int LatestSyncTime { set; get; }

        private SyncerInterface syncer;
        private Timer timer;

        public delegate void ThreadSaveItemsCallbackFunc();

        public delegate void ThreadRefreshItemsCallbackFunc();

        public event ThreadSaveItemsCallbackFunc ThreadSaveItemsCallback;
        public event ThreadRefreshItemsCallbackFunc ThreadRefreshItemsCallback;

        public void Initial(string appid, string sid, string skey, string region, string bucket) {
            syncer = new TencentCOSSyncer(appid, sid, skey, bucket);
            
            syncer.Initial();
            syncer.NotifiyMainWindowRefreshCallback += () => ThreadRefreshItemsCallback?.Invoke();
            syncer.NotifiyMainWindowSaveCallback += () => ThreadSaveItemsCallback?.Invoke();
            
            timer = new Timer();
            timer.Interval = 15 * 60 * 1000;
            timer.Elapsed += delegate(object sender, ElapsedEventArgs args) {
                syncer.Sync();
            };
        }

        public void SyncMainThread() {
            timer.Start();
            syncer.Sync();
        }

        public void StopThread() {
            timer.Stop();
        }

        public void SyncFileNow() {
            syncer.Sync();
        }
    }
}