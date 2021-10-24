using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TodoListCSharp.core;

namespace TodoPlusTest {
    [TestClass]
    public class SettingUnitTest {
        [TestMethod]
        public void SettingLoadCopyTest() {
            Setting setting = new Setting();
            int ret = setting.ReadSettingFromRegistryTable();
            Setting BackupSetting = new Setting(setting);
            Assert.AreEqual(0, ret);
        }
    }
}
