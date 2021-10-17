using System;
using System.Threading.Tasks;
using TodoListCSharp.interfaces;
using COSXML;
using COSXML.Auth;
using COSXML.Model.Object;
using COSXML.Model.Bucket;
using COSXML.CosException;
using COSXML.Model.Tag;
using COSXML.Transfer;
using TodoListCSharp.utils;

namespace TodoListCSharp.core {
    public class TencentCOSSyncer : SyncerInterface {
        private string appid { set; get; }
        private string bucket { set; get; }
        private const string region = "COS_REGION";
        private string secretId { set; get; }
        private string secretKey { set; get; }
        private CosXmlConfig xmlConfig { set; get; }
        private QCloudCredentialProvider cosCredentialProvider { set; get; }
        private CosXml cosXml { set; get; }
        
        public override int Initial() {
            xmlConfig = new CosXmlConfig.Builder()
                .IsHttps(true)
                .SetRegion(region)
                .SetDebugLog(true)
                .Build();
            Refresh();
            return 0;
        }

        public override async Task<int> Sync() {
            int iDownloadResult = await Download();

            if (iDownloadResult != 0) {
                Upload();
                return 0;
            }
            
            string RemoteTempDir = System.IO.Path.GetTempPath();
            string RemoteTempFileName = "save";
            IOInterface io = new BinaryIO();
            Save RemoteSave = new Save();
            Save LocalSave = new Save();
            int ret = io.FileToSave(RemoteTempDir + RemoteTempFileName, ref RemoteSave);
            if (ret != 0) {
                return -1;
            }

            ret = io.FileToSave(Constants.SAVE_FILEPATH, ref LocalSave);
            if (ret != 0) {
                return -1;
            }

            if (LocalSave.version > RemoteSave.version) {
                Upload();
            }
            else {
                io.SaveToFile(ref RemoteSave, Constants.SAVE_FILEPATH);
                // todo: 通知主窗口重新读取文件
            }

            return 0;
        }

        public override int Refresh() {
            long durationSecond = 600; 
            cosCredentialProvider = new DefaultQCloudCredentialProvider(
                secretId, secretKey, durationSecond);
            cosXml = new CosXmlServer(xmlConfig, cosCredentialProvider);
            return 0;
        }

        protected override async Task<int> Download() {
            TransferConfig transferConfig = new TransferConfig();
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);
            string localDir = System.IO.Path.GetTempPath();
            string localFileName = "save";
            
            COSXMLDownloadTask task = new COSXMLDownloadTask(bucket, Constants.TENCENT_OSS_PATH, localDir, localFileName);
            task.progressCallback = delegate(long completed, long total) {
                Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };

            try {
                COSXML.Transfer.COSXMLDownloadTask.DownloadTaskResult result = await
                    transferManager.DownloadAsync(task);
                return 0;
            }
            catch (Exception e) {
                Console.WriteLine("Expection:" + e);
                return -1;
            }
        }

        protected override async Task<int> Upload() {
            TransferConfig transferConfig = new TransferConfig();
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);

            string srcPath = Constants.SAVE_FILEPATH;
            COSXMLUploadTask task = new COSXMLUploadTask(bucket, Constants.TENCENT_OSS_PATH);
            task.SetSrcPath(srcPath);
            
            task.progressCallback = delegate(long completed, long total) {
                Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };

            try {
                COSXML.Transfer.COSXMLUploadTask.UploadTaskResult result = await
                    transferManager.UploadAsync(task);
                return 0;
            }
            catch (Exception e) {
                Console.WriteLine("Expection:" + e);
                return -1;
            }
        }

        protected override int CheckBucket() {
            throw new System.NotImplementedException();
        }
    }
}