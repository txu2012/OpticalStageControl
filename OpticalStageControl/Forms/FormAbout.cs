using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OpticalStageControl
{
    public partial class FormAbout : Form
    {
        public FormAbout(double version_num)
        {
            InitializeComponent();
            lbVers.Text = String.Format("{0:0.00}", version_num);
        }
    }
}
