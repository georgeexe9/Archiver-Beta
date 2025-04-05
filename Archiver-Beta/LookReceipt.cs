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
    public partial class LookReceipt : Form
    {
        //Методът `get` проверява дали обектът `look` е null и ако е така, създава нов обект от тип `LookReceipt`.
        //След това връща създадения обект. Това е полезно,
        //когато искаме да създадем обект само когато е необходимо, за да спестим ресурси. Ако имаш други въпроси или искаш да обсъдим нещо друго, съм тук!
        public static LookReceipt look;
        public static LookReceipt lookreceipt
        {
           get
            {
                if (look == null)
                {
                    look = new LookReceipt();
                }
                return look;
            } 
            
        }
        //
        public LookReceipt()
        {
            //задаваме централен стартъп на формата на екрана
            this.StartPosition = FormStartPosition.CenterParent;
            InitializeComponent();
            this.MaximizeBox = false;

        }

        private void LookReceipt_Load(object sender, EventArgs e)
        {
            //Зъпълваме текстовите полета с информацията предоставена от базата от данни
            labelID.Text = ViewUI.selectedrow.Cells[0].Value.ToString();
            ViewerShop.Text = ViewUI.selectedrow.Cells[1].Value.ToString();
            PurchaseViewer.Text = ViewUI.selectedrow.Cells[2].Value.ToString();
            ItemsViewer.Text = ViewUI.selectedrow.Cells[3].Value.ToString();
            AmountViewer.Text = ViewUI.selectedrow.Cells[4].Value.ToString();
            ViewerPayment.Text = ViewUI.selectedrow.Cells[5].Value.ToString();
            DateViewer.Text = ViewUI.selectedrow.Cells[6].Value.ToString();
        }

        private void ReceiptID_Click(object sender, EventArgs e)
        {

        }

        private void CloseButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
