using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace Project
{
    public partial class List : Form
    {
        public List()
        {
            InitializeComponent();
        }
        OracleConnection ORACLE = new OracleConnection(constr);
        static string constr = "User Id=PHYSICAL_PROJECT; Password=1111;Data Source=127.0.0.1:1521/xe";
        OracleDataAdapter oraAdap = new OracleDataAdapter();
        DataSet ds = new DataSet();

        private void Load_List()
        {
            ORACLE.Open();
            oraAdap.SelectCommand = new OracleCommand();
            oraAdap.SelectCommand.CommandText = "Select * from test_view ";
            oraAdap.SelectCommand.Connection = ORACLE;
            OracleDataReader oraReader = oraAdap.SelectCommand.ExecuteReader();
            while (oraReader.Read())
            {
                object[] values = new object[oraReader.FieldCount];
                oraReader.GetValues(values);
                listBox1.Items.Add(values[5].ToString());
            }
        }

        private void List_Load(object sender, EventArgs e)
        {
            Load_List();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            (new Menu()).Show();
            this.Hide();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Update_Load();
        }
        private void Update_Load()
        {
            string group = "";
            if (listBox1.SelectedItems.Count > 0)
            {
                group = listBox1.SelectedItems[0].ToString();
            }
            oraAdap.SelectCommand.CommandText = "Select * from test_view "; ////////////
            DataTable data = new DataTable();
            oraAdap.Fill(data);
            dataGridView2.DataSource = data;
        }
    }
}
