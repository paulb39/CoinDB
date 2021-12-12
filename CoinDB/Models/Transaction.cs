using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinDB.Models
{
    public class Transaction
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("ticker")]
        public string ticker { get; set; }

        [JsonProperty("buy_date")]
        public DateTime BuyDate { get; set; }

        [JsonProperty("quantity")]
        public double Quantity { get; set; }

        [JsonProperty("price")]
        public double Price { get; set; }

        [JsonProperty("total")]
        public double Total { get; set; }

        [JsonProperty("staked")]
        public bool Staked { get; set; }

        [JsonProperty("sold")]
        public bool Sold { get; set; }

        [JsonProperty("sell_date")]
        public DateTime SellDate { get; set; }

    }
}
