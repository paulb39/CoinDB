using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoinGecko.Clients;
using CoinGecko.Interfaces;

namespace CoinDB.Services
{
    public abstract class Coin
    {
        protected readonly ICoinGeckoClient _client;
        protected readonly string vsCurrencies = "usd";
        public string Ticker { get; set; }
        public string Name { get; set; }
        public bool Staked { get; set; }

        public Coin(string ticker, string name, bool staked)
        {
            _client = CoinGeckoClient.Instance;
            Ticker = ticker;
            Name = name;
            Staked = staked;
        }

        public abstract Task<float> GetPriceAsync();

    }
}
