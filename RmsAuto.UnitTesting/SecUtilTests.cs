using System;
using RmsAuto.Common.Misc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Text;

using RmsAuto.Store.Entities;

namespace RmsAuto.UnitTesting
{
    /// <summary>
    ///This is a test class for SecUtilTest and is intended
    ///to contain all SecUtilTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SecUtilTests
    {
        
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

		///// <summary>
		/////A test for ToSecureByteArray
		/////</summary>
		//[TestMethod()]
		//public void SecureByteArray_SerializeDeseriliaze_AreEquals()
		//{
		//    var initialRegData = new ClientRegistrationDataExt
		//    {
		//        ClientCategory = RmsAuto.Store.Acctg.ClientCategory.Physical,
		//        TradingVolume = RmsAuto.Store.Acctg.TradingVolume.Retail,
		//        FieldOfAction = "частное лицо",
		//        ClientName = "Анпилогов Евгений",
		//        Email = "anpilogov@rmsauto.ru",
		//        MainPhone = "0121234567",
		//        AuxPhone1 = "0123456789",
		//        AuxPhone2 = "0123456789",
		//        RmsStoreId = "МАГ1",
		//        DeliveryAddress = "Москва, Ленинские горы, владение 1, стр 75Д",
		//        Login = "anpilogov",
		//        Password = "123456"
		//    };

		//    byte[] secureBytes = SecUtil.ToSecureByteArray(initialRegData, string.Empty);

		//    var deserializedRegData = (ClientRegistrationDataExt)SecUtil
		//        .FromSecureByteArray(secureBytes, string.Empty); 
            
		//    Assert.AreEqual(initialRegData.ClientName, deserializedRegData.ClientName);
            
		//}

		//const int _MaxCipherDataSize = 4000;
		//const char _Dummy = 'Z';
        
		//[TestMethod]
		//public void ToSecureByteArray_LargeRegData_DoesntExceedMaxSize()
		//{
		//    var data = new ClientRegistrationDataExt
		//    {
		//        ClientCategory = RmsAuto.Store.Acctg.ClientCategory.Physical,
		//        TradingVolume = RmsAuto.Store.Acctg.TradingVolume.Retail,
		//        FieldOfAction = string.Empty.PadRight(100, _Dummy),
		//        ClientName = string.Empty.PadRight(100,_Dummy),
		//        Email = string.Empty.PadRight(255, _Dummy),
		//        MainPhone = string.Empty.PadRight(10, _Dummy),
		//        AuxPhone1 = string.Empty.PadRight(10, _Dummy),
		//        AuxPhone2 = string.Empty.PadRight(10, _Dummy),
		//        RmsStoreId = string.Empty.PadRight(10, _Dummy),
		//        DeliveryAddress = string.Empty.PadRight(500, _Dummy),
		//        Login = string.Empty.PadRight(20, _Dummy),
		//        Password = string.Empty.PadRight(10, _Dummy),
		//    };

		//    byte[] secureBytes = SecUtil.ToSecureByteArray(data, string.Empty);
		//    Assert.IsTrue(
		//        secureBytes.Length <= _MaxCipherDataSize,
		//        "Итоговый размер данных превышает допустимый");
		//}

        [TestMethod]
        public void FromSecureByteArray()
        {
            var cipherData = "C3692B903484ADC16432C3331106B6A4BDBBD2A1C3380655E8BDEF98614510D07E4385FCC255845A5E182640838822D1F86E8039B2808FB3B94AFBA4F3031699CBD76F8404838E3DDEDB3E6743FD2CC9D35AD9A40473E252713891BB28C48C6D2F4B0E683895ABAA2A001BD12B63723FA51E36E47F40C97C064C798E387222D17008574C2958220BD382569F6698648FB608BE9EB0F0D2BE54D1AE3A2C3C7922FF5E4444FB15AB9AE8ADB446883781FE50DBE772C5FBE45BC9B441F3BB5CFC198E690B0E6D5E2A505B8D70310C734BEFAFCE94A05A418D09196B3FD42D62DAFE0F47AE2C1726571CE63B9D0F28700B09024417F8879768656536893CBC9A7AB4EE224AF9E687B24D1C282D2FB92B8649B729B2CEA4A5E08AF7A18B7EA7F895B682A379D683E772D410656D48341D5367F6FC9D3529B93BA675DF4184A4D9B58083DCD3C565AF07351DAA08098CFA608FCB0C09C5FDEB85AC53A560026AC8822B69AEFCCD2622149FD209535407959383F533F0ED8B3A3983E0F682E430397296D1A19D02A839EA00904EA3BCA2B0459311CC06B25CCE33EAE613327C31B2C2C1C338D4406659DF3D92CA945B294EF9F7B09DEC9EC90A81B9A3F069898FC70B2945EF45C57E4405BBB26471F3A377CBAFC912191C4A9BBB954072360245181121213406789A474940041300CED3EF0144EC6080AF615D88B8AF008C1D37C5AC6D5EE1FF05AEFFEEE26BF131BAFA7F3E7DA2F6E50C4EF44FF37DF524CA7CDF037FC0A2646F3FC05AABAEF5D2C0F7D0EC839AC0BEED6A3853A0C2C59C0B774E5F2576BC62AEDA71B2D41E3D141892E3465A610F82CB5AD5ED4D11418EBC7A8DA40D0B3948111861AF2D8A05F693B4D3B7CD6C4C3D69BF10A497E44D2089D76F8E8A96461280D51F331E698F5708884CA6CB91E7302129667A7505D90D80C431EC02D30F37E1AACEBB777E7510AEB5264DDB6174B3C1A1F1D953372E9EA3039BA1C141753ADB6C031E7A3D161D1FADA4F4CC7EA8EE55702BC67ACF724B67E0625ABD533612A4334DE552A88960BCFB384F6930C081F0405510D2C76D6BBA4BD9C8D45B779C8F28F3B162BE8C6F142D899597EE4D65880A41843E5F2E1A4909313646386D569F16E43EE9706C5210518A0FAD45A0F01BF55C2E31232F27255798173F174C261D5A120436DA006811D24022B9DEF17C8C10F1E7FC471D82AF3248255624A642A73BD0BB002A368ACF14607937FD463A344B8B8906DF80D2ADBB7FD38D61A81D0836D70169281E2C904829E3CC337356696F6B7A54CF9654ECE861EF4EEF4719F75E905BB0E452C74181F1918F0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000";
            var regData = SecUtil.FromSecureByteArray(StringToByteArray(cipherData), string.Empty);
            Assert.IsNotNull(regData);
        }

        private static byte[] StringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length).
                   Where(x => 0 == x % 2).
                   Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).
                   ToArray();
        } 

    }
}
