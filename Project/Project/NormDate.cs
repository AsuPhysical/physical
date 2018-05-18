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
    public partial class NormDate : Form
    {
        public NormDate()
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
                comboBox1.Items.Add(values[1].ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            (new Menu()).Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text != "" && comboBox1.Text != "")
            { 
                string query = "INSERT INTO DATE_NORMATIVE (ID_NORMATIVE, DATE_NORMATIVE, ID_GROUP) values ((Select ID_NORMATIVE from  SP_NORMATIVE where TITLE_NORMATIVE= '" + comboBox1.Text +
                "'), '" + dateTimePicker1.Text + "' , (select ID from SP_ST_GROUP where TITLE= '" + comboBox2.Text + "'))";
                oraAdap.InsertCommand = new OracleCommand(query, ORACLE);
                oraAdap.InsertCommand.ExecuteNonQuery();
                MessageBox.Show("Норматив добавлен");
            }
            else
            {
                MessageBox.Show("Не удалось добавить дату сдачи норматива");
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void NormDate_Load(object sender, EventArgs e)
        {
            Load_List();
        }
    }
}
