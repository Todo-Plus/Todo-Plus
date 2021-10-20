using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using TodoListCSharp.core;

namespace TodoPlusTest {
    [TestClass]
    public class TencentCOSSyncerUnitTest {
        [TestMethod]
        public async Task SyncTestAsync() {
            TencentCOSSyncer syncer = new TencentCOSSyncer(
                SecretKeys.id, 
                SecretKeys.sid, 
                SecretKeys.skey,
                SecretKeys.bucket);
            syncer.Initial();

            int ret = await syncer.Sync();
            Assert.AreEqual(ret, 0);
        }
    }
}