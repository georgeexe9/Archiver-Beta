﻿using System;
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
    public partial class ViewUI : Form
    {
        public ViewUI()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
;        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            AddFormUI addformui = new AddFormUI();
            addformui.ShowDialog();
        }
    }
}
