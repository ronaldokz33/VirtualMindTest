using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using VirtualMind.NetTest.Arquitetura.Library.Util;
using VirtualMind.NetTest.VO;

namespace VirtualMind.NetTest.Tests
{
    [TestClass]
    public class PurchaseTest
    {
        string _baseUrl = "http://localhost:53336/";
        private readonly Request _request;
        public PurchaseTest()
        {
            _request = new Request();
        }

        [TestMethod]
        public void PurchaseUSD_ReturnsInsertedObject()
        {
            Purchase pObject = new Purchase()
            {
                userId = DateTime.Now.ToBinary().ToString(),
                amount = 1000,
                currency = "USD"
            };

            var response = _request.Post<Purchase>($"{_baseUrl}api/exchange/purchase", pObject);

            Assert.IsTrue(response.id > 0);
        }

        [TestMethod]
        public void PurchaseBRL_ReturnsInsertedObject()
        {
            Purchase pObject = new Purchase()
            {
                userId = DateTime.Now.ToBinary().ToString(),
                amount = 1000,
                currency = "BRL"
            };

            var response = _request.Post<Purchase>($"{_baseUrl}api/exchange/purchase", pObject);

            Assert.IsTrue(response.id > 0);
        }

        [TestMethod]
        public void PurchaseBRLFailure_ShouldReturnANullValue()
        {
            Purchase pObject = new Purchase()
            {
                userId = DateTime.Now.ToBinary().ToString(),
                amount = 80000,
                currency = "BRL"
            };

            var response = _request.Post<Purchase>($"{_baseUrl}api/exchange/purchase", pObject);

            Assert.IsTrue(response == null);
        }

        [TestMethod]
        public void PurchaseUSDFailure_ShouldReturnANullValue()
        {
            Purchase pObject = new Purchase()
            {
                userId = DateTime.Now.ToBinary().ToString(),
                amount = 80000,
                currency = "USD"
            };

            var response = _request.Post<Purchase>($"{_baseUrl}api/exchange/purchase", pObject);

            Assert.IsTrue(response == null);
        }

        [TestMethod]
        public void MaxMonthExceededUsd_ShouldReturnANullValue()
        {
            Purchase pObject = new Purchase()
            {
                userId = "userABC",
                amount = 16000,
                currency = "USD"
            };

            var response = _request.Post<Purchase>($"{_baseUrl}api/exchange/purchase", pObject);
            response = _request.Post<Purchase>($"{_baseUrl}api/exchange/purchase", pObject);
            response = _request.Post<Purchase>($"{_baseUrl}api/exchange/purchase", pObject);
            response = _request.Post<Purchase>($"{_baseUrl}api/exchange/purchase", pObject);

            Assert.IsTrue(response == null);
        }

        [TestMethod]
        public void MaxMonthExceededBRL_ShouldReturnANullValue()
        {
            Purchase pObject = new Purchase()
            {
                userId = "userABC",
                amount = 32000,
                currency = "BRL"
            };

            var response = _request.Post<Purchase>($"{_baseUrl}api/exchange/purchase", pObject);
            response = _request.Post<Purchase>($"{_baseUrl}api/exchange/purchase", pObject);
            response = _request.Post<Purchase>($"{_baseUrl}api/exchange/purchase", pObject);
            response = _request.Post<Purchase>($"{_baseUrl}api/exchange/purchase", pObject);

            Assert.IsTrue(response == null);
        }
    }
}
