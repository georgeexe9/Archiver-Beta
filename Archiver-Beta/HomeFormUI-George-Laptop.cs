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
    public partial class HomeForm : Form
    {
        public HomeForm()
        {
            InitializeComponent();
        }

        
        //Извиква диалогов прозорец, който пита потребителя дали желае да затвори приложението.
        //Ако Потребителят кликне върху Yes, се задейства if проверка - Yes - ще се затвори
        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            string message = "Are you sure, you want to exit?";
            string title = "Exit - Receipt Archiver";
            //Извикваме си бутони - yes/no
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            //Извикваме иконка - въпросителна
            MessageBoxIcon icon = MessageBoxIcon.Question;
            if (DialogResult.Yes == MessageBox.Show(message, title, buttons, icon))
            {
                Application.Exit();
            }


        }
        private void AddButton_Click(object sender, EventArgs e)
        {
            AddFormUI addformui = new AddFormUI();
            addformui.ShowDialog();
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            ViewUI a2 = new ViewUI();
            a2.ShowDialog();
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {

        }

        private void HomeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string message1 = "Are you sure you want to exit?";
            string message = "Exit";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;
            if (DialogResult.Yes == MessageBox.Show(message1, message, buttons, icon));
            {
                Application.Exit();
            }
        }

       
    }
}
