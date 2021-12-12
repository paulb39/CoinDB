using CoinDB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinDB.Coins
{
    public class Bitcoin : Coin
    {
        public Bitcoin(string ticker, string name, bool staked) : base(ticker, name, false)
        {

        }

        public override async Task<float> GetPriceAsync()
        {
            var result = await this._client.SimpleClient.GetSimplePrice(new[] { Name }, new[] { this.vsCurrencies });

            return 0.0f;
        }
    }
}
