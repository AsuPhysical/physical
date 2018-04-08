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
    }

    public class Students
    {
        private bool study;
        public bool Study
        {
            get { return study; }
            set { study = value; }
        }

        private int skip;
        public int Skip
        {
            get { return skip; }
            set { skip = value; }
        }

        private char group;
        public char Group
        {
            get { return group; }
            set { group = value; }
        }

        private char health_group;
        public char Health_group
        {
            get { return health_group; }
            set { health_group = value; }
        }
    }

    public class Normative
    {

    }
}
