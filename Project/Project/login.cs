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
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }
        OracleConnection ORACLE = new OracleConnection(constr);
        static string constr = "User Id=PHYSICAL_PROJECT; Password=1111;Data Source=127.0.0.1:1521/xe";
        OracleDataAdapter oraAdap = new OracleDataAdapter();
        AutoCompleteStringCollection source = new AutoCompleteStringCollection();
        TextBox textBox = new TextBox();

        private bool Load_List()
        {
            bool flag = false;
            foreach (string x in source)
            {
                if (textBox.Text == x)
                {
                    flag = true;
                }
            }
            return flag;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (Load_List())
                {
                    (new Menu()).Show();
                    this.Hide();
                    ORACLE.Close();
                }
                else { MessageBox.Show("Неверно ввели данные"); }
            }
            catch (Exception) { MessageBox.Show("Неверно ввели данные"); }

        }
        

        private void login_Load(object sender, EventArgs e)
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
                source.Add(values[5].ToString() );
            }
            textBox.AutoCompleteCustomSource = source;

            textBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBox.Width = 200;
            textBox.Location = new Point(43, 71);
            Controls.Add(textBox);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
