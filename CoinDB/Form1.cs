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

            var bitcoin = await CryptoCurrency.Create("btc", "bitcoin", false, _dbFactory);
            var litecoin = await CryptoCurrency.Create("ltc", "litecoin", false, _dbFactory);

            coinList.Add(bitcoin);
            coinList.Add(litecoin);

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
                return @"\cf4" + profit.ToString() + @"\cf1";
            }

            return @"\cf6" + profit.ToString() + @"\cf1"; // red :(
        }

        private TabPage CreateTabPage(CryptoCurrency cryptoCurrency, int tabIndex)
        {
            //TODO staking
            //TODO font / $ / red / green

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
            sb.Append(@"}");
            richTextBox.Rtf = sb.ToString();

            tabPage.Controls.Add(richTextBox);

            return tabPage;
        }
    }
}
