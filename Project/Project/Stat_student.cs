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
    public partial class Stat_student : Form
    {
        public Stat_student()
        {
            InitializeComponent();
            Class1.Teachr_fio = "Могутов Мир Платонович";
        }

        OracleConnection ORACLE = new OracleConnection(constr);
        static string constr = "User Id=PHYSICAL_PROJECT; Password=1111;Data Source=127.0.0.1:1521/xe";
        OracleDataAdapter oraAdap = new OracleDataAdapter();

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            chart1.Series[0].LegendText = comboBox2.Text;

            List<double> arrX = new List<double>();
            List<string> arrY = new List<string>();

            //// Статистика по определенному нормативу, за определенный год ВСЕХ ИЛИ Одной группы

            string stat1 = "select value_norm from journal, date_normative, SP_NORMATIVE, st_ank1 where journal.DATE_LESSON = date_normative.DATE_NORMATIVE"+
            " and date_normative.ID_NORMATIVE = SP_NORMATIVE.ID_NORMATIVE and st_ank1.K_ST = journal.ID_STUDENT and"+
            " SP_NORMATIVE.TITLE_NORMATIVE = '"+comboBox3.Text+"' and st_ank1.STFAM || ' ' || STNAME || ' ' || STOT = '"+comboBox1.Text+"'";

            string stat2 = "select journal.date_lesson from journal, date_normative, SP_NORMATIVE, st_ank1 where journal.DATE_LESSON = date_normative.DATE_NORMATIVE" +
            " and date_normative.ID_NORMATIVE = SP_NORMATIVE.ID_NORMATIVE and st_ank1.K_ST = journal.ID_STUDENT and" +
            " SP_NORMATIVE.TITLE_NORMATIVE = '" + comboBox3.Text + "' and st_ank1.STFAM || ' ' || STNAME || ' ' || STOT = '" + comboBox1.Text + "'"; 



            OracleCommand oc = new OracleCommand(stat1, ORACLE);
            OracleDataReader odr = oc.ExecuteReader();
            //MessageBox.Show(odr.GetInt32(0).ToString);
            while (odr.Read())
            {
                arrX.Add(odr.GetInt32(0));
            }

            oc = new OracleCommand(stat2, ORACLE);
            odr = oc.ExecuteReader();
            while (odr.Read())
            {
                arrY.Add(odr.GetDateTime(0).ToShortDateString());
            }

            chart1.Series[0].Points.DataBindXY(arrY, arrX);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            (new Menu()).Show();
            this.Hide();
        }

        private void Load_List()
        {
            ORACLE.Open();
            oraAdap.SelectCommand = new OracleCommand();
            oraAdap.SelectCommand.CommandText = "select SP_ST_GROUP.TITLE from TEACH_GROUP, SP_TEACHERS, SP_ST_GROUP where SP_ST_GROUP.ID=TEACH_GROUP.ID_GROUP and SP_TEACHERS.ID_TEACHER=TEACH_GROUP.ID_TEACH and TRIM(SP_TEACHERS.FIO) ='" + Class1.Teachr_fio + "'";
            oraAdap.SelectCommand.Connection = ORACLE;
            OracleDataReader oraReader = oraAdap.SelectCommand.ExecuteReader();
            while (oraReader.Read())
            {
                object[] values = new object[oraReader.FieldCount];
                oraReader.GetValues(values);
                comboBox2.Items.Add(values[0].ToString());
            }

            oraAdap.SelectCommand = new OracleCommand();
            oraAdap.SelectCommand.CommandText = "Select * from SP_NORMATIVE ";
            oraAdap.SelectCommand.Connection = ORACLE;
            OracleDataReader oraReader1 = oraAdap.SelectCommand.ExecuteReader();
            while (oraReader1.Read())
            {
                object[] values = new object[oraReader1.FieldCount];
                oraReader1.GetValues(values);
                comboBox3.Items.Add(values[1].ToString());
            }
        }

        private void Stat_student_Load(object sender, EventArgs e)
        {
            Load_List();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            (new Choose()).Show();
            this.Hide();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            oraAdap.SelectCommand = new OracleCommand();
            oraAdap.SelectCommand.CommandText = "select STFAM||' '||STNAME||' '||STOT as FIO from st_ank1, SP_ST_GROUP where SP_ST_GROUP.ID = st_ank1.GROUP_ID and SP_ST_GROUP.TITLE = '"+comboBox2.Text+"' ";
            oraAdap.SelectCommand.Connection = ORACLE;
            OracleDataReader oraReader1 = oraAdap.SelectCommand.ExecuteReader();
            while (oraReader1.Read())
            {
                object[] values = new object[oraReader1.FieldCount];
                oraReader1.GetValues(values);
                comboBox1.Items.Add(values[0].ToString());
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
