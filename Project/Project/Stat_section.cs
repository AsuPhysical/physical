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
    public partial class Stat_section : Form
    {
        public Stat_section()
        {
            InitializeComponent();
        }

        OracleConnection ORACLE = new OracleConnection(constr);
        static string constr = "User Id=PHYSICAL_PROJECT; Password=1111;Data Source=127.0.0.1:1521/xe";
        OracleDataAdapter oraAdap = new OracleDataAdapter();

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            //chart1.Series[0].LegendText = comboBox2.Text;

            List<double> arrX = new List<double>();
            List<string> arrY = new List<string>();

            string stat1 = "select (Sum(value_norm)/count(id_student)) from journal, date_normative, SP_NORMATIVE, st_ank1, ST_SPORT_SECTIONS, SP_SPORT_SECTIONS " + 
            "where journal.DATE_LESSON = date_normative.DATE_NORMATIVE and date_normative.ID_NORMATIVE = SP_NORMATIVE.ID_NORMATIVE and "+ 
            "st_ank1.K_ST = journal.ID_STUDENT and journal.ID_STUDENT = ST_SPORT_SECTIONS.K_ST and SP_SPORT_SECTIONS.ID_SECTION = ST_SPORT_SECTIONS.SECTION_ID and " + 
            "SP_NORMATIVE.TITLE_NORMATIVE = '"+ comboBox3.Text +"' group by SP_SPORT_SECTIONS.TITLE_SECTION";

            string stat2 = "select TITLE_SECTION from journal, date_normative, SP_NORMATIVE, st_ank1, ST_SPORT_SECTIONS, SP_SPORT_SECTIONS " +
            "where journal.DATE_LESSON = date_normative.DATE_NORMATIVE and date_normative.ID_NORMATIVE = SP_NORMATIVE.ID_NORMATIVE and " +
            "st_ank1.K_ST = journal.ID_STUDENT and journal.ID_STUDENT = ST_SPORT_SECTIONS.K_ST and SP_SPORT_SECTIONS.ID_SECTION = ST_SPORT_SECTIONS.SECTION_ID and " +
            "SP_NORMATIVE.TITLE_NORMATIVE = '" + comboBox3.Text + "' group by SP_SPORT_SECTIONS.TITLE_SECTION";

            OracleCommand oc = new OracleCommand(stat1, ORACLE);
            OracleDataReader odr = oc.ExecuteReader();
            while (odr.Read())
            {
                arrX.Add(odr.GetInt32(0));
            }

            oc = new OracleCommand(stat2, ORACLE);
            odr = oc.ExecuteReader();
            while (odr.Read())
            {
                arrY.Add(odr.GetString(0));
            }

            chart1.Series[0].Points.DataBindXY(arrY, arrX);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            (new Choose()).Show();
            this.Hide();
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

        private void Stat_section_Load(object sender, EventArgs e)
        {
            Load_List();
        }
    }
}
