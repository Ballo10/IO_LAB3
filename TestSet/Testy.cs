using System;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServerLib;

namespace TestSet
{
    [TestClass]
    public class Testy
    {
        [TestMethod]
        public void TestValidPortNumber()
        {
            TextServerAsync server = new TextServerAsync(IPAddress.Parse("127.0.0.1"), 4000);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestInvalidPortNumber()
        {
            TextServerAsync server = new TextServerAsync(IPAddress.Parse("127.0.0.1"), 4);
        }

        [TestMethod]
        public void TestChangeIP()
        {
            TextServerAsync server = new TextServerAsync(IPAddress.Parse("127.0.0.1"), 4000);
            server.IPAddress = IPAddress.Parse("127.0.0.1");
        }

        [TestMethod]
        public void TestChangeBufferSize()
        {
            TextServerAsync server = new TextServerAsync(IPAddress.Parse("127.0.0.1"), 4000);
            server.BufferSize = 2048;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestNegativeChangeBufferSize()
        {
            TextServerAsync server = new TextServerAsync(IPAddress.Parse("127.0.0.1"), 4000);
            server.BufferSize = -50;
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestHugeChangeBufferSize()
        {
            TextServerAsync server = new TextServerAsync(IPAddress.Parse("127.0.0.1"), 4000);
            server.BufferSize = 1000000000;
        }

    }
}
