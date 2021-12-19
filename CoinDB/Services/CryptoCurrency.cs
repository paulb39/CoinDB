using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinDB.Models;
using CoinGecko.Clients;
using CoinGecko.Interfaces;

namespace CoinDB.Services
{
    public class CryptoCurrency
    {
        private readonly ICoinGeckoClient _client;
        private readonly string vsCurrencies = "usd";
        private readonly DBFactory _dbFactory;
        public string Ticker { get; set; }
        public string Name { get; set; }
        public double TotalQuantity { get; set; }
        public double CurrentPrice { get; set; }
        public bool Staked { get; set; }

        public double TotalSpent { get; set; }
        public double CostAverage { get; set; }
        public double TotalProfitLoss { get; set; }

        /*
            APY = (1 + r/n)n – 1
            APY= (1+0.5/12)^12-1= 5.116 percent
            r - the interest rate
            n - the number of times the interest is compounded per year
            12 for monthly
        */
        public double APY { get; set; }
        public double TotalStakedCoinEarnings { get; set; } //# of coins
        public double TotalStakedProfit { get; set; }


        public List<Transaction> Transactions { get; set; }

        public static async Task<CryptoCurrency> Create(string ticker, string name, bool staked, DBFactory dbFactory, double apy = 0d)
        {
            var cryptoCurrecy = new CryptoCurrency(ticker, name, staked, dbFactory);
            var currentPrice = await cryptoCurrecy.GetCurrentPrice();

            var quantity = cryptoCurrecy.GetAllTransactions().Sum(x => x.Quantity);
            var currentSell = currentPrice * quantity;
            var totalProfitLoss = currentSell - cryptoCurrecy.TotalSpent;
            cryptoCurrecy.TotalProfitLoss = totalProfitLoss;
            cryptoCurrecy.CurrentPrice = currentPrice;

            if (staked)
            {
                cryptoCurrecy.SetTotalCoinStakedEarnings(apy, currentPrice);
            }

            return cryptoCurrecy;
        }

        public CryptoCurrency(string ticker, string name, bool staked, DBFactory dbFactory)
        {
            _client = CoinGeckoClient.Instance;
            _dbFactory = dbFactory;
            this.Ticker = ticker;
            this.Name = name;
            this.Staked = staked;
            this.Transactions = null;
            SetTotalSpend();
            SetCostAverage();
            SetQuantity();
        }

        public async Task<float> GetCurrentPrice()
        {
            float price = 0.00f;
            var result = await _client.SimpleClient.GetSimplePrice(new[] { Name }, new[] { vsCurrencies });
            
            try
            {
                price = float.Parse(result[Name][vsCurrencies].ToString());
            } catch (Exception e)
            {
                Console.WriteLine("ERROR: Cannot parse " + Name + e.Message);
                return 0.00f;
            }

            return price;
        }

        private void SetTotalCoinStakedEarnings(double apy, double currentPrice)
        {
            DateTime today = DateTime.Now;
            double percentage = apy / 100; //20% = 0.2
            this.APY = Math.Pow((1 + percentage / 12), 12) - 1;
            double totalStakedCoinEarnings = 0d;

            foreach (var row in GetAllTransactions())
            {
                var oneYearEarned = row.Quantity * this.APY;
                var oneDayEarnings = oneYearEarned / 365;
                var coinEarnings = oneDayEarnings * (today - row.BuyDate).TotalDays;
                totalStakedCoinEarnings += coinEarnings;
            }

            TotalStakedCoinEarnings = totalStakedCoinEarnings;
            TotalStakedProfit = totalStakedCoinEarnings * currentPrice;
        }

        private void SetTotalSpend()
        {
            var totalSpend = GetAllTransactions().Sum(x => x.Total);
            this.TotalSpent = totalSpend;
        }

        private void SetCostAverage()
        {
            var quantity = GetAllTransactions().Sum(x => x.Quantity);
            var costAverage = this.TotalSpent / quantity;
            this.CostAverage = costAverage;
        }

        private void SetQuantity()
        {
            var quantity = GetAllTransactions().Sum(x => x.Quantity);
            this.TotalQuantity = quantity;
        }

        private List<Transaction> GetAllTransactions()
        {
            if (this.Transactions != null)
            {
                return this.Transactions;
            }

            var allTransactions = _dbFactory.GetTransactionDataFromTicker(this.Ticker);
            return allTransactions;
        }
       
    }
}
