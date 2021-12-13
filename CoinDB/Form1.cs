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

            _tabControl.Location = new System.Drawing.Point(16, 16);
            _tabControl.Size = new System.Drawing.Size(1500, 750);
            _tabControl.SelectedIndex = 0;
            _tabControl.TabIndex = 0;
            this.Controls.Add(this._tabControl);

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
            sb.Append(cryptoCurrency.TotalProfitLoss.ToString());
            sb.Append(@"}");
            richTextBox.Rtf = sb.ToString();

            tabPage.Controls.Add(richTextBox);

            return tabPage;
        }
    }
}
