using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class Statistics : Form
    {
        public Statistics()
        {
            InitializeComponent();
            chart1.Series[0].LegendText = " ";
        }

        OracleConnection ORACLE = new OracleConnection(constr);
        static string constr = "User Id=PHYSICAL_PROJECT; Password=1111;Data Source=127.0.0.1:1521/xe";
        OracleDataAdapter oraAdap = new OracleDataAdapter();
        DataSet ds = new DataSet();

        private void button2_Click(object sender, EventArgs e)
        {
            (new Menu()).Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[0].LegendText = "ИТ";

        }
    }
}
