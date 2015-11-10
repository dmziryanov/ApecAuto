using RmsAuto.Common.Misc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using RmsAuto.TechDoc.Entities.TecdocBase;
using System.Linq;
using System.Diagnostics;
using RmsAuto.TechDoc.Entities;
using RmsAuto.TechDoc;
using System.Xml;
using RmsAuto.TechDoc.Configuration;
using System.Configuration;
using RmsAuto.Store.BL;
using RmsAuto.Store.Entities;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using RmsAuto.TechDoc.Entities.Helpers;
using System.Collections.Generic;

namespace RmsAuto.UnitTesting
{
    [TestClass]
    public class VinReqTests
    {
        [TestMethod]
        public void GetRequestsTest()
        {
            string clientId = "118";

            var res = RmsAuto.Store.Dac.VinRequestsDac.GetRequests(clientId);
            Assert.IsNotNull(res);
        }
    }
}
