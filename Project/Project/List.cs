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
using System.Data.OleDb;

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
        string month = DateTime.Now.ToString("MM");

        private void Load_List()
        {
            ORACLE.Open();
            oraAdap.SelectCommand = new OracleCommand();
            //oraAdap.SelectCommand.CommandText = "Select * from SP_ST_GROUP ";
            oraAdap.SelectCommand.CommandText = "select SP_ST_GROUP.TITLE from TEACH_GROUP, SP_TEACHERS, SP_ST_GROUP where SP_ST_GROUP.ID=TEACH_GROUP.ID_GROUP and SP_TEACHERS.ID_TEACHER=TEACH_GROUP.ID_TEACH and TRIM(SP_TEACHERS.FIO) ='" + Class1.Teachr_fio + "'";

            oraAdap.SelectCommand.Connection = ORACLE;
            OracleDataReader oraReader = oraAdap.SelectCommand.ExecuteReader();
            while (oraReader.Read())
            {
                object[] values = new object[oraReader.FieldCount];
                oraReader.GetValues(values);
                listBox1.Items.Add(values[0].ToString());
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
            ORACLE.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int count = this.dataGridView2.Columns.Count; for (int i = 0; i < count; i++) // цикл удаления всех столбцов 
            { this.dataGridView2.Columns.RemoveAt(0); }
            Update_Load();
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn() { Name = "БАЛЛ", HeaderText = "БАЛЛ", Width = 100 });
        }
        private void Update_Load()
        {
            string group = "";
            if (listBox1.SelectedItems.Count > 0)
            {
                group = listBox1.SelectedItems[0].ToString();
            }
            oraAdap.SelectCommand.CommandText = "Select * from journal where Substr(DATE_LESSON,4,2) = '"+ month + "'";
            

            DataTable data = new DataTable();
            oraAdap.Fill(data);
            dataGridView2.DataSource = data;
            DataGridViewTextBoxColumn dgvAge;
            dgvAge = new DataGridViewTextBoxColumn();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
            
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                for (int i = 0; i < dataGridView2.RowCount; i++)
                {
                    dataGridView2.Rows[i].Selected = false;
                    for (int j = 0; j < dataGridView2.ColumnCount; j++)
                        if (dataGridView2.Rows[i].Cells[j].Value != null)
                            if (dataGridView2.Rows[i].Cells[j].Value.ToString().Contains(textBox1.Text))
                            {
                                dataGridView2.Rows[i].Selected = true;
                                break;
                            }
                }
            }
            else
            {
                dataGridView2.ClearSelection();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            month = Convert.ToDateTime(comboBox1.Text + "/01/2000").ToString("MM");
            Update_Load();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //int n, nrow, sum = 0;
            //Double result, result1;
            //String s;
            //nrow = dataGridView2.Columns.Count;
            //for (int i = 0; i < nrow; i++)
            //{
            //    s = dataGridView2.Rows[i].Cells[3].Value.ToString();
            //    n = int.Parse(s);
            //    sum += n;
            //    result = sum;
            //    result1 = Math.Round(result / 3, 2);
            //    datagridView2.Rows[i].Cells[6].Value = result1;

            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable data = dataGridView2.DataSource as DataTable;
                OracleCommandBuilder builder = new OracleCommandBuilder(oraAdap);
                oraAdap.Update(data);
                MessageBox.Show("Данные сохранены");
            }
            catch
            {
                MessageBox.Show("Ошибка при вводе данных");
            }
            
        }
    }
}
