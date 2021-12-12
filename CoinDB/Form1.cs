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
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            var _dbFactory = new DBFactory();

            var bitcoin = await CryptoCurrency.Create("btc", "bitcoin", false, _dbFactory);
            Console.WriteLine(bitcoin.TotalProfitLoss);
            var x = "test";

        }
    }
}
