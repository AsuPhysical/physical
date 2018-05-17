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
    public partial class Choose : Form
    {
        public Choose()
        {
            InitializeComponent();
        }
        // ДОПИЛИТЬ ДИАГРАММУ БОЛЬНЫХ/ЗДОРОВЫХ
        OracleConnection ORACLE = new OracleConnection(constr);
        static string constr = "User Id=PHYSICAL_PROJECT; Password=1111;Data Source=127.0.0.1:1521/xe";
        OracleDataAdapter oraAdap = new OracleDataAdapter();

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text== "Статистика в конкретной группе по определенному нормативу")
            {
                (new Statistics()).Show();
                 this.Hide();
            }
            if (comboBox1.Text == "Статистика данных по студенту по определенному нормативу за все время обучения")
            {
                (new Stat_student()).Show();
                this.Hide();
            }
            if (comboBox1.Text == "Сравнение данных по определенному нормативу в разных спортивных секциях")
            {
                (new Stat_section()).Show();
                this.Hide();
            }
            if (comboBox1.Text == "4")
            {
                (new Stat_group()).Show();
                this.Hide();
            }

        }

        private void Choose_Load(object sender, EventArgs e)
        {
            ORACLE.Open();
            chart1.Series[0].Points.Clear();
            //chart1.Series[0].LegendText = comboBox2.Text;

            List<int> arrX = new List<int>();
            List<string> arrY = new List<string>();

            //// Статистика по определенному нормативу, за определенный год ВСЕХ ИЛИ Одной группы

            string stat1 = "select Count(STFAM) from st_ank1, SP_PHYSICAL_GROUP where SP_PHYSICAL_GROUP.ID = st_ank1.PHYSICAL_ID group by SP_PHYSICAL_GROUP.TITLE";

            string stat2 = "select SP_PHYSICAL_GROUP.TITLE from st_ank1, SP_PHYSICAL_GROUP where SP_PHYSICAL_GROUP.ID = st_ank1.PHYSICAL_ID group by SP_PHYSICAL_GROUP.TITLE";


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
                arrY.Add(odr.GetString(0).Trim());
            }
            chart1.Series[0].Points.DataBindXY(arrY, arrX);

        }



        private void button2_Click(object sender, EventArgs e)
        {
            (new Menu()).Show();
            this.Hide();
        }
    }
}
