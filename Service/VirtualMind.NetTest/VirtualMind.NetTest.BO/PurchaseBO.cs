using System;
using System.Collections.Generic;
using System.Text;
using VirtualMind.NetTest.BO.Settings;
using VirtualMind.NetTest.Interfaces;
using Microsoft.Extensions.Options;
using System.Data;
using VirtualMind.NetTest.Arquitetura.Library.Util.Security;
using VirtualMind.NetTest.DAO;
using VirtualMind.NetTest.VO;
using System.Linq;
using System.Threading;
using System.Globalization;

namespace VirtualMind.NetTest.BO
{
    public class PurchaseBO : IPurchase
    {
        #region Local Variables
        private readonly ApiRequestSettings apiRequestSettings;
        private readonly PurchaseSettings purchaseSettings;

        private readonly IRequest request;

        protected PurchaseDAO purchaseDAO;

        #endregion

        #region Constructors

        public PurchaseBO(IOptions<ApiRequestSettings> apiRequestSettings, IOptions<PurchaseSettings> purchaseSettings, IOptions<DatabaseSettings> databaseSettings, IRequest request) : base()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

            this.apiRequestSettings = apiRequestSettings.Value;
            this.purchaseSettings = purchaseSettings.Value;
            this.request = request;
            this.purchaseDAO = new PurchaseDAO(ConnectionFactory.GetDbConnectionDefault(databaseSettings.Value.connectionString));
        }

        ~PurchaseBO()
        {
            purchaseDAO.Dispose();
        }

        #endregion

        #region Base Methods

        public Purchase Insert(Purchase pObject)
        {
            purchaseDAO.BeginTransaction();
            try
            {
                CheckCurrency(pObject.currency);

                var currencyQuote = GetCurrentQuotes();

                pObject.amount = pObject.amount / currencyQuote[pObject.currency];

                if(pObject.amount > purchaseSettings.limitByPurchase[pObject.currency])
                {
                    throw new ApplicationException($"The limit value per purchase with {pObject.currency} is {(purchaseSettings.limitByPurchase[pObject.currency] * currencyQuote[pObject.currency]).ToString("C")} pesos");
                }

                CheckLimit(pObject);

                Purchase purchase = purchaseDAO.InsertByStoredProcedure(pObject);

                pObject.id = purchase.id;

                purchaseDAO.CommitTransaction();
            }
            catch (Exception ex)
            {
                purchaseDAO.RollbackTransaction();
                throw ex;
            }
            return pObject;
        }

        public IList<Purchase> ListForLookup(Purchase pObject)
        {
            return purchaseDAO.ListForLookup(pObject);
        }

        public Dictionary<string, decimal> GetCurrencysQuote(string currency)
        {
            var currencys = GetCurrentQuotes();

            if (!string.IsNullOrEmpty(currency))
            {
                CheckCurrency(currency);

                return currencys.Where(p => p.Key == currency).ToDictionary(p => p.Key, p => p.Value);
            }

            return currencys;
        }

        #endregion

        #region Custom Methods

        private Dictionary<string, decimal> GetCurrentQuotes()
        {
            Dictionary<string, decimal> dt = new Dictionary<string, decimal>();

            Dictionary<string, decimal> currency = apiRequestSettings.currencys;

            decimal dolarPrice = GetDolarPrice();

            if (apiRequestSettings.realRateApi != "")
            {
                currency["BRL"] = request.Get<decimal>(apiRequestSettings.realRateApi);
            }

            if (currency?.Count > 0)
            {
                foreach (var item in currency)
                {
                    dt[item.Key] = dolarPrice / item.Value;
                }
            }

            return dt;
        }

        private decimal GetDolarPrice()
        {
            string[] currentDolar = request.Get<string[]>(apiRequestSettings.baseUrl);

            if (currentDolar != null && currentDolar.Length > 0) return Convert.ToDecimal(currentDolar[0]);

            return 0;
        }
        private void CheckCurrency(string currency)
        {
            if (!apiRequestSettings.currencys.ContainsKey(currency))
            {
                throw new ApplicationException("This currency is not yet enabled for purchases.");
            }
        }

        private void CheckLimit(Purchase pObject)
        {
            var currencyQuote = GetCurrentQuotes();

            var purchases = purchaseDAO.ListForLookup(pObject);

            decimal amount = purchases.Where(p => p.createdAt.Month == DateTime.Today.Month).Sum(p => p.amount);

            if (amount + pObject.amount > purchaseSettings.limitByMonth[pObject.currency])
            {
                throw new ApplicationException($"You have exceeded your purchase limit per month. The {pObject.currency} limit is {(purchaseSettings.limitByMonth[pObject.currency] * currencyQuote[pObject.currency]).ToString("C")} pesos");
            }

        }

        #endregion
    }
}
