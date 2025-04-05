using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Archiver_Beta
{
    public partial class Reports : Form
    {
        public Reports()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void CloseButton_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Reports_Load(object sender, EventArgs e)
        {

        }
    }
}
