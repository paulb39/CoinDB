using CoinDB.Models;
using CoinDB.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoinDB
{
    public partial class Form1 : Form
    {
        private TabControl _tabControl { get; set; }
        private double ExchangeStakingRemoval = 0.15d; // Kraken takes 15% of staking rewards

        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(1700, 900);
            this._tabControl = new TabControl();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            var coinList = new List<CryptoCurrency>();
            var _dbFactory = new DBFactory();

            //test API works using name first
            //https://www.coingecko.com/en/api/documentation
            //e.g https://api.coingecko.com/api/v3/simple/price?ids=litecoin&vs_currencies=us
            var loopring = await CryptoCurrency.Create("lrc", "loopring", false, _dbFactory);
            var kava = await CryptoCurrency.Create("kava", "kava", true, _dbFactory, 20d);
            var cosmos = await CryptoCurrency.Create("atom", "cosmos", true, _dbFactory, 7.5d);
            var algo = await CryptoCurrency.Create("algo", "algorand", true, _dbFactory, 4.75d);
            var tezos = await CryptoCurrency.Create("xtz", "tezos", true, _dbFactory, 4.7d);
            var stellar = await CryptoCurrency.Create("xlm", "stellar", false, _dbFactory);
            var ripple = await CryptoCurrency.Create("xrp", "ripple", false, _dbFactory);
            var litecoin = await CryptoCurrency.Create("ltc", "litecoin", false, _dbFactory);
            var ethereum = await CryptoCurrency.Create("eth", "ethereum", false, _dbFactory);
            var polkadot = await CryptoCurrency.Create("dot", "polkadot", true, _dbFactory, 12d);
            var bitcoinCash = await CryptoCurrency.Create("bch", "bitcoin-cash", false, _dbFactory);
            var bitcoin = await CryptoCurrency.Create("btc", "bitcoin", false, _dbFactory);
            var zcash = await CryptoCurrency.Create("zec", "zcash", false, _dbFactory);
            var monero = await CryptoCurrency.Create("xmr", "monero", false, _dbFactory);

            coinList.Add(loopring);
            coinList.Add(kava);
            coinList.Add(cosmos);
            coinList.Add(algo);
            coinList.Add(tezos);
            coinList.Add(stellar);
            coinList.Add(ripple);
            coinList.Add(litecoin);
            coinList.Add(ethereum);
            coinList.Add(polkadot);
            coinList.Add(bitcoinCash);
            coinList.Add(bitcoin);
            coinList.Add(zcash);
            coinList.Add(monero);

            int i = 0;
            foreach (var coin in coinList)
            {
                var tabPage = this.CreateTabPage(coin, i);
                _tabControl.Controls.Add(tabPage);
                i++;
            }

            var masterTabPage = CreateMasterTabPage(coinList, i);
            _tabControl.Controls.Add(masterTabPage);

            _tabControl.Location = new System.Drawing.Point(16, 16);
            _tabControl.Size = new System.Drawing.Size(1500, 750);
            _tabControl.SelectedIndex = 0;
            _tabControl.TabIndex = 0;
            this.Controls.Add(this._tabControl);

        }

        private TabPage CreateMasterTabPage(List<CryptoCurrency> coins, int tabIndex)
        {
            var allProfitLoss = coins.Sum(x => x.TotalProfitLoss);
            var allSpend = coins.Sum(y => y.TotalSpent);
            var allStakeProfit = coins.Sum(z => z.TotalStakedProfit);

            var masterTabPage = new TabPage();
            masterTabPage.Text = "All";
            masterTabPage.Size = this.Size;
            masterTabPage.TabIndex = tabIndex;

            var richTextBox = new RichTextBox();
            richTextBox.ReadOnly = true;
            richTextBox.BorderStyle = BorderStyle.None;
            richTextBox.Dock = DockStyle.Fill;

            var sb = new StringBuilder();
            sb.Append(@"{\rtf1\ansi");
            sb.Append(@"{\colortbl;\red0\green0\blue0;\red0\green0\blue255;\red0\green255\blue255;\red0\green255\blue0;\red255\green0\blue255;\red255\green0\blue0;\red255\green255\blue0;\red255\green255\blue255;\red0\green0\blue128;\red0\green128\blue128;\red0\green128\blue0;\red128\green0\blue128;\red128\green0\blue0;\red128\green128\blue0;\red128\green128\blue128;\red192\green192\blue192;\red201\green33\blue30;}");

            foreach (var coin in coins)
            {
                sb.Append(@" \line ");
                sb.Append(@"\b " + coin.Name);
                sb.Append(@"\b0 : ");
                sb.Append(GetProfitLabel(coin.TotalProfitLoss));
                sb.Append(@" \line \line ");
            }

            sb.Append(@"\b Total Spend: \b0 ");
            sb.Append(allSpend.ToString());
            sb.Append(@" \line \line ");
            sb.Append(@"\b All profit or loss: \b0 ");
            sb.Append(GetProfitLabel(allProfitLoss));
            sb.Append(@" \line \line ");
            sb.Append(@"\b Staked Earned: \b0 ");
            sb.Append((allStakeProfit - (allStakeProfit * ExchangeStakingRemoval)).ToString());
            sb.Append(@"}");
            richTextBox.Rtf = sb.ToString();
            masterTabPage.Controls.Add(richTextBox);

            return masterTabPage;
        }

        private string GetProfitLabel(double profit)
        {
            //cf6 = red 
            //cf4 = green
            //cf1 = black

            if (profit >= 0) //green baby!
            {
                return @"\cf4 " + profit.ToString() + @"\cf1";
            }

            return @"\cf6 " + profit.ToString() + @"\cf1"; // red :(
        }

        private TabPage CreateTabPage(CryptoCurrency cryptoCurrency, int tabIndex)
        {
            var tabPage = new TabPage();
            tabPage.Text = cryptoCurrency.Ticker;
            tabPage.Size = this.Size;
            tabPage.TabIndex = tabIndex;

            var richTextBox = new RichTextBox();
            richTextBox.ReadOnly = true;
            richTextBox.BorderStyle = BorderStyle.None;
            richTextBox.Dock = DockStyle.Fill;

            var sb = new StringBuilder();
            sb.Append(@"{\rtf1\ansi");
            sb.Append(@"{\colortbl;\red0\green0\blue0;\red0\green0\blue255;\red0\green255\blue255;\red0\green255\blue0;\red255\green0\blue255;\red255\green0\blue0;\red255\green255\blue0;\red255\green255\blue255;\red0\green0\blue128;\red0\green128\blue128;\red0\green128\blue0;\red128\green0\blue128;\red128\green0\blue0;\red128\green128\blue0;\red128\green128\blue128;\red192\green192\blue192;\red201\green33\blue30;}");
            sb.Append(@"\b Total Spent: \b0 ");
            sb.Append(cryptoCurrency.TotalSpent.ToString());
            sb.Append(@" \line \line ");
            sb.Append(@"\b Have: \b0 ");
            sb.Append(cryptoCurrency.TotalQuantity.ToString());
            sb.Append(@" \line \line ");
            sb.Append(@"\b Current Price: \b0 ");
            sb.Append(cryptoCurrency.CurrentPrice.ToString());
            sb.Append(@" \line \line ");
            sb.Append(@"\b Cost Average: \b0 ");
            sb.Append(cryptoCurrency.CostAverage.ToString());
            sb.Append(@" \line \line ");
            sb.Append(@"\b Profit or Loss: \b0 ");
            sb.Append(GetProfitLabel(cryptoCurrency.TotalProfitLoss));
            
            if (cryptoCurrency.Staked)
            {
                string stakingMessage = cryptoCurrency.TotalStakedCoinEarnings.ToString() + " / " + cryptoCurrency.TotalStakedProfit;
                sb.Append(@" \line \line ");
                sb.Append(@"\b Staked Earned: \b0 ");
                sb.Append(stakingMessage);
                sb.Append(@" \line \line ");
            }

            sb.Append(@"}");
            richTextBox.Rtf = sb.ToString();

            tabPage.Controls.Add(richTextBox);

            return tabPage;
        }
    }
}
