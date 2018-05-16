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
    }
}
