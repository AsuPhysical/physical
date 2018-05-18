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
    public partial class LFK : Form
    {
        public LFK()
        {
            InitializeComponent();
        }

        OracleConnection ORACLE = new OracleConnection(constr);
        static string constr = "User Id=PHYSICAL_PROJECT; Password=1111;Data Source=127.0.0.1:1521/xe";
        OracleDataAdapter oraAdap = new OracleDataAdapter();

        private void Load_List()
        {
            ORACLE.Open();
            oraAdap.SelectCommand = new OracleCommand();
            //oraAdap.SelectCommand.CommandText = "Select * from SP_ST_GROUP ";
            oraAdap.SelectCommand.CommandText = "select SP_ST_GROUP.TITLE from TEACH_GROUP, SP_TEACHERS, SP_ST_GROUP where SP_ST_GROUP.ID=TEACH_GROUP.ID_GROUP and SP_TEACHERS.ID_TEACHER=TEACH_GROUP.ID_TEACH and TRIM(SP_TEACHERS.FIO) ='" + Class1.Teachr_fio.Trim() + "'";

            oraAdap.SelectCommand.Connection = ORACLE;
            OracleDataReader oraReader = oraAdap.SelectCommand.ExecuteReader();
            while (oraReader.Read())
            {
                object[] values = new object[oraReader.FieldCount];
                oraReader.GetValues(values);
                listBox1.Items.Add(values[0].ToString());
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            (new Menu()).Show();
            this.Hide();
            ORACLE.Close();
        }

        private void LFK_Load(object sender, EventArgs e)
        {
            Load_List();
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
            oraAdap.SelectCommand.CommandText = "select STFAM||' '||STNAME||' '||STOT as ФИО, month1 as Устный_ответ_1, month2 as Устный_ответ_2, month3 as Устный_ответ_3, month4 as Реферат from lfk, st_ank1, sp_st_group where lfk.K_ST = st_ank1.K_ST and " +
                "st_ank1.GROUP_ID = sp_st_group.ID and sp_st_group.TITLE = '" + listBox1.SelectedItem + "' and semestr = '" + comboBox1.Text + "'";
            try
            {
                DataTable data = new DataTable();
                oraAdap.Fill(data);
                dataGridView2.DataSource = data;
                DataGridViewTextBoxColumn dgvAge;
                dgvAge = new DataGridViewTextBoxColumn();
            }
            catch (SyntaxErrorException)
            {
                MessageBox.Show("Упс");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable grid = dataGridView2.DataSource as DataTable;
            oraAdap.SelectCommand.CommandText = "select STFAM||' '||STNAME||' '||STOT as FIO, lfk.k_st, SEMESTR, month1,month2,month3, month4  from st_ank1, lfk where st_ank1.K_ST = lfk.K_ST";
            DataTable id_stud = new DataTable();
            oraAdap.Fill(id_stud);

            oraAdap.SelectCommand.CommandText = "select * from lfk";
            DataTable data = new DataTable();
            oraAdap.Fill(data);
            
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    for (int j = 0; j < id_stud.Rows.Count; j++)
                    {
                        if (data.Rows[i][1].ToString() == id_stud.Rows[j][1].ToString() && data.Rows[i][2].ToString() == comboBox1.Text)
                        {
                            for (int h = 0; h < grid.Rows.Count; h++)
                            {

                                if (id_stud.Rows[j][0].ToString() == grid.Rows[h][0].ToString())
                                {
                                    //Console.WriteLine(id_stud.Rows[j][0].ToString() + " " + grid.Rows[h][0].ToString());
                                    data.Rows[i][3] = grid.Rows[h][1];
                                    data.Rows[i][4] = grid.Rows[h][2];
                                    data.Rows[i][5] = grid.Rows[h][3];
                                    data.Rows[i][6] = grid.Rows[h][4];
                                }
                            }
                        }
                    }
                }

                try
                {
                    OracleCommandBuilder builder = new OracleCommandBuilder(oraAdap);
                    oraAdap.Update(data);
                    MessageBox.Show("Данные сохранены");
                }
                catch
                {
                    MessageBox.Show("Ошибка при вводе данных");
                }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            oraAdap.SelectCommand.CommandText = "select(TRIM(stfam) || ' ' || TRIM(stname) || ' ' || TRIM(stot)) ФИО, (month1+month2+month3+month4) Балл from lfk, st_ank1, sp_st_group " +
                "where st_ank1.GROUP_ID = sp_st_group.ID and lfk.k_st = st_ank1.k_st and sp_st_group.TITLE = '" + listBox1.SelectedItem + "'  and semestr = '"+comboBox1.Text+"'";
            DataTable data = new DataTable();
            oraAdap.Fill(data);
            dataGridView2.DataSource = data;
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
            Update_Load();
        }
    }
}
