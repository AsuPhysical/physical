﻿using Oracle.ManagedDataAccess.Client;
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
            if (comboBox1.Text=="1")
            {
                (new Statistics()).Show();
                 this.Hide();
            }
            if (comboBox1.Text == "2")
            {
                (new Stat_student()).Show();
                this.Hide();
            }
            //if (comboBox1.Text == "3")
            //{
            //    (new Statistics()).Show();
            //    this.Hide();
            //}

        }

        private void Choose_Load(object sender, EventArgs e)
        {
            ORACLE.Open();
            chart1.Series[0].Points.Clear();
            //chart1.Series[0].LegendText = comboBox2.Text;

            List<string> arrX = new List<string>();
            List<string> arrY = new List<string>();

            //// Статистика по определенному нормативу, за определенный год ВСЕХ ИЛИ Одной группы

            string stat1 = "select STFAM from st_ank1, SP_PHYSICAL_GROUP where SP_PHYSICAL_GROUP.ID = st_ank1.GROUP_ID";

            string stat2 = "select SP_PHYSICAL_GROUP.TITLE from st_ank1, SP_PHYSICAL_GROUP where SP_PHYSICAL_GROUP.ID = st_ank1.GROUP_ID";


            OracleCommand oc = new OracleCommand(stat1, ORACLE);
            OracleDataReader odr = oc.ExecuteReader();
            //MessageBox.Show(odr.GetInt32(0).ToString);
            while (odr.Read())
            {
                arrX.Add(odr.GetString(0));
            }

            oc = new OracleCommand(stat2, ORACLE);
            odr = oc.ExecuteReader();
            while (odr.Read())
            {
                arrY.Add(odr.GetString(0));
            }

            chart1.Series[0].Points.DataBindXY(arrY, arrX);

        }
    }
}
