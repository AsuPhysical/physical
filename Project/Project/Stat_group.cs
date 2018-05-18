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
using System.Windows.Forms.DataVisualization.Charting;

namespace Project
{
    public partial class Stat_group : Form
    {
        public Stat_group()
        {
            InitializeComponent();
        }

        OracleConnection ORACLE = new OracleConnection(constr);
        static string constr = "User Id=PHYSICAL_PROJECT; Password=1111;Data Source=127.0.0.1:1521/xe";
        OracleDataAdapter oraAdap = new OracleDataAdapter();
        ToolTip toolTip1 = new ToolTip();

        private void button1_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            chart1.GetToolTipText += chart1_GetToolTipText;
            //chart1.Series[0].LegendText = comboBox2.Text;

            List<double> arrX = new List<double>();
            List<string> arrY = new List<string>();
            string stat1 = "";
            string stat2 = "";

            if (checkBox1.Checked)
            {
               stat1 = "select value_norm from journal, date_normative, SP_NORMATIVE, st_ank1, SP_ST_GROUP, TEACH_GROUP, SP_TEACHERS " +
               " where journal.DATE_LESSON = date_normative.DATE_NORMATIVE and date_normative.ID_NORMATIVE = SP_NORMATIVE.ID_NORMATIVE and st_ank1.K_ST = journal.ID_STUDENT and  " +
               "SP_ST_GROUP.ID = st_ank1.GROUP_ID and SP_NORMATIVE.TITLE_NORMATIVE = '" + comboBox3.Text + "' and SUBSTR(SP_ST_GROUP.TITLE, 4, 1) = '" + comboBox1.Text + "' and " +
               "TEACH_GROUP.ID_GROUP = st_ank1.GROUP_ID and SP_TEACHERS.ID_TEACHER = TEACH_GROUP.ID_TEACH and TRIM(SP_TEACHERS.FIO) = '" + Class1.Teachr_fio + "'";

                stat2 = "select STFAM as FIO from journal, date_normative, SP_NORMATIVE, st_ank1, SP_ST_GROUP, TEACH_GROUP, SP_TEACHERS " +
               " where journal.DATE_LESSON = date_normative.DATE_NORMATIVE and date_normative.ID_NORMATIVE = SP_NORMATIVE.ID_NORMATIVE and st_ank1.K_ST = journal.ID_STUDENT and  " +
               "SP_ST_GROUP.ID = st_ank1.GROUP_ID and SP_NORMATIVE.TITLE_NORMATIVE = '" + comboBox3.Text + "' and SUBSTR(SP_ST_GROUP.TITLE, 4, 1) = '" + comboBox1.Text + "' and " +
               "TEACH_GROUP.ID_GROUP = st_ank1.GROUP_ID and SP_TEACHERS.ID_TEACHER = TEACH_GROUP.ID_TEACH and TRIM(SP_TEACHERS.FIO) = '" + Class1.Teachr_fio + "'";
            }
            else
            {
               stat1 = "select value_norm from journal, date_normative, SP_NORMATIVE, st_ank1, SP_ST_GROUP where " +
                "journal.DATE_LESSON = date_normative.DATE_NORMATIVE and date_normative.ID_NORMATIVE = SP_NORMATIVE.ID_NORMATIVE and " +
                "st_ank1.K_ST = journal.ID_STUDENT and SP_ST_GROUP.ID = st_ank1.GROUP_ID and SP_NORMATIVE.TITLE_NORMATIVE = '" + comboBox3.Text + "' and " +
                "SUBSTR(SP_ST_GROUP.TITLE, 4, 1) = '" + comboBox1.Text + "'";

                stat2 = "select STFAM as FIO from journal, date_normative, SP_NORMATIVE, st_ank1, SP_ST_GROUP where " +
               "journal.DATE_LESSON = date_normative.DATE_NORMATIVE and date_normative.ID_NORMATIVE = SP_NORMATIVE.ID_NORMATIVE and " +
               " st_ank1.K_ST = journal.ID_STUDENT and SP_ST_GROUP.ID = st_ank1.GROUP_ID and SP_NORMATIVE.TITLE_NORMATIVE = '" + comboBox3.Text + "' and " +
               "SUBSTR(SP_ST_GROUP.TITLE, 4, 1) = '" + comboBox1.Text + "'";
            }

            OracleCommand oc = new OracleCommand(stat1, ORACLE);
            OracleDataReader odr = oc.ExecuteReader();

            while (odr.Read())
            {
                try
                {
                    arrX.Add(odr.GetInt32(0));
                }
                catch (InvalidCastException)
                {
                    arrX.Add(0);
                }
            }

            oc = new OracleCommand(stat2, ORACLE);
            odr = oc.ExecuteReader();
            while (odr.Read())
            {
                try
                {
                    arrY.Add(odr.GetString(0));
                }
                catch (InvalidCastException)
                {
                    arrY.Add("");
                }
            }
            
           

            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
            //chart1.ChartAreas[0].AxisX = ChartValueType.String;
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

        private void Stat_group_Load(object sender, EventArgs e)
        {
            Load_List();
        }

        private void chart1_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            //chart1.Series[0].ToolTip = "X = #VALX, Y = #VAL";
        }
    }
}
