using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using RmsAuto.Store.Dac;
using RmsAuto.Store.Entities;

namespace RmsAuto.UnitTesting
{
    [TestClass]
    public class ManagerDacTests
    {
        [TestMethod]
        public void GetDefaultHandySetEntry_SAManagerID_ReturnsNotNull()
        {
            var saUserId = 24;
            var hcse = ManagerDac.GetDefaultHandySetEntry(saUserId);
            Assert.IsNotNull(hcse);
        }
    }
}
