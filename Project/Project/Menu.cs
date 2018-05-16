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
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        List list;
        private void button1_Click(object sender, EventArgs e)
        {
            list = new List();
            list.Show();
            this.Hide();
        }
        Choose statistics;
        private void button2_Click(object sender, EventArgs e)
        {
            statistics = new Choose();
            statistics.Show();
            this.Hide();
        }
        NormDate normDate;
        private void button3_Click(object sender, EventArgs e)
        {
            normDate = new NormDate();
            normDate.Show();
            this.Hide();
        }
        login loggin;
        private void button4_Click(object sender, EventArgs e)
        {
            loggin = new login();
            loggin.Show();
            this.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }

    //public class Students
    //{
    //    private bool study;
    //    public bool Study
    //    {
    //        get { return study; }
    //        set { study = value; }
    //    }

    //    private int skip;
    //    public int Skip
    //    {
    //        get { return skip; }
    //        set { skip = value; }
    //    }

    //    private char group;
    //    public char Group
    //    {
    //        get { return group; }
    //        set { group = value; }
    //    }

    //    private char health_group;
    //    public char Health_group
    //    {
    //        get { return health_group; }
    //        set { health_group = value; }
    //    }
    //}

    //public class Normative
    //{

    //}
}
