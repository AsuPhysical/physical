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
using Oracle.ManagedDataAccess.Types;
using System.Xml;

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
        string month = DateTime.Now.ToString("MM/yy");//.Replace('/','.');

        private void Load_List()
        {
            ORACLE.Open();
            oraAdap.SelectCommand = new OracleCommand();
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

        private void List_Load(object sender, EventArgs e)
        {
            Load_List();
            //oraAdap.SelectCommand.CommandText = "select * from SP_PHYSICAL_GROUP";
            //DataTable dt = new DataTable();
            //oraAdap.Fill(dt);
            //dataGridView2.DataSource = dt;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            (new Menu()).Show();
            this.Hide();
            ORACLE.Close();
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
            oraAdap.SelectCommand.CommandText = "Select distinct date_lesson from journal, sp_st_group, st_ank1 where st_ank1.K_ST = journal.ID_STUDENT and st_ank1.GROUP_ID = sp_st_group.ID " +
                "and sp_st_group.TITLE = '" + listBox1.SelectedItem + "' and Substr(DATE_LESSON,4, 5) = '" + month + "'";
            DataTable date_lesson = new DataTable();
            oraAdap.Fill(date_lesson);
            if (date_lesson.Rows.Count == 0) return;

            oraAdap.SelectCommand.CommandText = "Select STFAM||' '||STNAME||' '||STOT as FIO, date_lesson, value_norm, attendance from journal, sp_st_group, st_ank1 where st_ank1.K_ST = journal.ID_STUDENT " +
                " and st_ank1.GROUP_ID = sp_st_group.ID and sp_st_group.TITLE = '" + listBox1.SelectedItem + "' and Substr(DATE_LESSON,4, 5) = '" + month + "' order by FIO";
            DataTable journal = new DataTable();
            oraAdap.Fill(journal);

            oraAdap.SelectCommand.CommandText = "select DATE_NORMATIVE from date_normative";
            DataTable normative = new DataTable();
            oraAdap.Fill(normative);
            
            DataTable grid = new DataTable();
            grid.Columns.Add("ФИО");

            
            for (int i = 0; i < date_lesson.Rows.Count; i++)
            {
                string flag = "\n\rПрисутствие";
                for (int j = 0; j < normative.Rows.Count; j++)
                {
                    if (date_lesson.Rows[i][0].ToString() == normative.Rows[j][0].ToString())
                    {
                        flag = "\n\rНорматив";
                    }
                }
                grid.Columns.Add(date_lesson.Rows[i][0].ToString().Substring(0, 10) + flag);
            }

            for (int i = 0; i <= journal.Rows.Count-1; i++)
            {
                DataRow newRow = grid.NewRow();
                newRow[0] = journal.Rows[(date_lesson.Rows.Count)*(i+1)-1][0];
                for (int j = 0; j <= journal.Rows.Count-1; j++)
                {
                    for (int h = 1; h < grid.Columns.Count; h++)
                    {
                        if (journal.Rows[j][1].ToString().Substring(0, 10).ToString() == grid.Columns[h].ColumnName.Substring(0, 10).ToString() && grid.Columns[h].ColumnName.Substring(10) == "\n\rПрисутствие" && newRow[0].ToString() == journal.Rows[j][0].ToString())
                        {
                            if (journal.Rows[j][3].ToString() == "1") newRow[h] = "Б";
                            if (journal.Rows[j][3].ToString() == "0") newRow[h] = "Н";
                        }
                        else if (journal.Rows[j][1].ToString().Substring(0, 10).ToString() == grid.Columns[h].ColumnName.Substring(0, 10).ToString() && grid.Columns[h].ColumnName.Substring(10) == "\n\rНорматив" && newRow[0].ToString() == journal.Rows[j][0].ToString())
                        {
                            if(journal.Rows[j][2]==null) newRow[h] = 0;
                            else newRow[h] = journal.Rows[j][2];
                        }
                    }
                }
                if ((date_lesson.Rows.Count) *(i + 1) % date_lesson.Rows.Count == 0)
                {
                    grid.Rows.Add(newRow);
                }
                if ((date_lesson.Rows.Count) * (i + 1) == journal.Rows.Count) break;
            }

            dataGridView2.DataSource = grid;
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
            month = Convert.ToDateTime(comboBox1.Text + "/01/"+DateTime.Now.ToString("yy")).ToString("MM/yy");
            Update_Load();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string dat = DateTime.Now.ToString("MM");
            string dat1 = DateTime.Now.ToString("yyyy");
            oraAdap.SelectCommand.CommandText = "select trunc(avg(vn),0) kk, FIO from (select value_norm vn,STFAM||' '||STNAME||' '||STOT as FIO from journal,st_ank1, " +
            "(select dat, dat1, DATE_NORMATIVE dnn, ID_GROUP from(select(EXTRACT(MONTH FROM dd.DATE_NORMATIVE)) dat, " +
            "(EXTRACT(year FROM dd.DATE_NORMATIVE))dat1, DATE_NORMATIVE, ID_GROUP from DATE_NORMATIVe dd, sp_st_group " +
            "where ID_GROUP = sp_st_group.ID and sp_st_group.TITLE = '" + listBox1.SelectedItem + "') where "+ dat + " <= 6 and "+ dat + " >= 1 and "+dat1+" = 2018) ddd " +
            "where DATE_lesson = dnn and st_ank1.k_st = journal.id_student and st_ank1.group_id = ddd.id_group) group by FIO";
            DataTable data = new DataTable();
            oraAdap.Fill(data);
            dataGridView2.DataSource = data;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DataTable data = dataGridView2.DataSource as DataTable;
            DataTable bd = new DataTable();
            bd.Columns.Add("FIO");
            bd.Columns.Add("DATE_LESSON");
            bd.Columns.Add("VALUE_NORM");
            bd.Columns.Add("ATTENDANCE");
            oraAdap.SelectCommand.CommandText = "Select distinct STFAM||' '||STNAME||' '||STOT as FIO, k_st from journal, sp_st_group, st_ank1 where st_ank1.K_ST = journal.ID_STUDENT and st_ank1.GROUP_ID = sp_st_group.ID " +
                "and sp_st_group.TITLE = '"+listBox1.SelectedItem+"' and Substr(DATE_LESSON,4, 5) = '"+month+"'";
            DataTable id_stud = new DataTable();
            oraAdap.Fill(id_stud);

            for (int i = 0; i < data.Rows.Count; i++)
            {
                for (int j = 1; j < data.Columns.Count; j++)
                {
                    DataRow newRow = bd.NewRow();
                    for (int h = 0; h < id_stud.Rows.Count; h++)
                    {
                        if (id_stud.Rows[h][0].ToString() == data.Rows[i][0].ToString())
                        {
                            newRow[0] = id_stud.Rows[h][1];
                        }
                    }
                    newRow[1] = data.Columns[j].ColumnName.Substring(0,10);
                    if (data.Columns[j].ColumnName.Substring(10) == "\n\rПрисутствие")
                    {
                        if(data.Rows[i][j].ToString() == "Б") newRow[3] = "1";
                        if(data.Rows[i][j].ToString() == "Н") newRow[3] = "0";
                        newRow[2] = "0";
                    }  
                    if (data.Columns[j].ColumnName.Substring(10) == "\n\rНорматив")
                    {
                        newRow[3] = "1";
                        newRow[2] = data.Rows[i][j];
                    }
                    bd.Rows.Add(newRow);
                }
            }
            
            oraAdap.SelectCommand.CommandText = "select * from journal";
            DataTable journal = new DataTable();
            oraAdap.Fill(journal);
            for (int i = 0; i < journal.Rows.Count; i++)
            {
                for (int j = 0; j < bd.Rows.Count; j++)
                {
                    if (journal.Rows[i][0].ToString() == bd.Rows[j][0].ToString() && journal.Rows[i][1].ToString().Substring(0,10) == bd.Rows[j][1].ToString())
                    {
                        journal.Rows[i][2] = bd.Rows[j][2];
                        journal.Rows[i][3] = bd.Rows[j][3];
                    }
                }
               
            }

            try
            {
                OracleCommandBuilder builder = new OracleCommandBuilder(oraAdap);
                oraAdap.Update(journal);
                MessageBox.Show("Данные сохранены");
            }
            catch
            {
                MessageBox.Show("Ошибка при вводе данных");
            }
            
        }
    }
}
