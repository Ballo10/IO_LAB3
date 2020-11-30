using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace TestSet
{
    [TestClass]
    public class Testy
    {
        [TestMethod]
        public void TestWrongPortNumber()
        {
            try
            {
                ServerLib.TextServerAsync server = new ServerLib.TextServerAsync(IPAddress.Parse("127.0.0.1"), 4000);
                Assert.Fail();
            }
            catch (Exception e)
            {

            }
        }
       /* [TestMethod]
        public void TestMethod2()
        {
        }
        [TestMethod]
        public void TestMethod3()
        {
        }
        [TestMethod]
        public void TestMethod4()
        {
        }*/
    }
}
