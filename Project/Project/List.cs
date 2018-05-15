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
        DataSet ds = new DataSet();

        private void Load_List()
        {
            ORACLE.Open();
            oraAdap.SelectCommand = new OracleCommand();
            oraAdap.SelectCommand.CommandText = "Select * from SP_ST_GROUP ";
            oraAdap.SelectCommand.Connection = ORACLE;
            OracleDataReader oraReader = oraAdap.SelectCommand.ExecuteReader();
            while (oraReader.Read())
            {
                object[] values = new object[oraReader.FieldCount];
                oraReader.GetValues(values);
                listBox1.Items.Add(values[1].ToString());
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
            this.dataGridView2.Rows.Clear();  // удаление всех строк 
            int count = this.dataGridView2.Columns.Count; for (int i = 0; i < count; i++)     // цикл удаления всех столбцов 
            { this.dataGridView2.Columns.RemoveAt(0); }
            Update_Load();
            dataGridView2.Columns.Add(new DataGridViewTextBoxColumn() { Name = "БАЛЛ", HeaderText = "БАЛЛ", Width = 100 });
        }
        private void Update_Load()
        {
            //dataGridView2   = null;
           // dataGridView2.DataBindings.Clear();
            string group = "";
            if (listBox1.SelectedItems.Count > 0)
            {
                group = listBox1.SelectedItems[0].ToString();
            }
            oraAdap.SelectCommand.CommandText = "Select K_ST, SP_PHYSICAL_GROUP.TITLE as Группа_здоровья, stfam as Фамилия, stname as Имя, stot as Отчество from SP_PHYSICAL_GROUP, ST_ANK1, SP_ST_GROUP where SP_PHYSICAL_GROUP.Id=ST_ANK1.PHYSICAL_ID and ST_ANK1.group_id = SP_ST_GROUP.id and SP_ST_GROUP.title = '" + group + "'";
            //oraAdap.SelectCommand.CommandText = "SElect * from Journal";
            DataTable data = new DataTable();
            oraAdap.Fill(data);
            dataGridView2.DataSource = data;
            DataGridViewTextBoxColumn dgvAge;
            dgvAge = new DataGridViewTextBoxColumn();

            
            

            int index;
            //index = dataGridView2.Columns["БАЛЛ"].Index;
            
            //index = dataGridView2.Columns["БАЛЛ"].Index;

            //if (dataGridView2.Columns.Count-2 == index)
            //{
            //    dataGridView2.Columns.Remove("БАЛЛ");
            //    MessageBox.Show("!!!!!!!!!!");
            //}

           // dataGridView2.Columns.Add(new DataGridViewTextBoxColumn() { Name = "БАЛЛ", HeaderText = "БАЛЛ", Width = 100 });
            //if (dataGridView2.Columns[dataGridView2.Columns.Count - 1].Name == dataGridView2.Columns[dataGridView2.Columns.Count - 2].Name && dataGridView2.Columns[dataGridView2.Columns.Count - 1].Name == "БАЛЛ") dataGridView2.Columns.Remove(dataGridView2.Columns[dataGridView2.Columns.Count - 1]);
            //dataGridView2.Columns.Add();
            //dataGridView2.Columns[dataGridView2.Columns.Count].V;

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
            oraAdap.SelectCommand.CommandText = "Select * from test_view where " + comboBox1.Text; ////////////
            DataTable data = new DataTable();
            oraAdap.Fill(data);
            dataGridView2.DataSource = data;
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
            DataTable data = dataGridView2.DataSource as DataTable;
            OracleCommandBuilder builder = new OracleCommandBuilder(oraAdap);
            oraAdap.Update(data);
        }
    }
}
