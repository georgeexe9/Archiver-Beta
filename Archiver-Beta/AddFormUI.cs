using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Archiver_Beta
{
    public partial class AddFormUI : Form
    {

        public string payment;
        public AddFormUI()
        {
            InitializeComponent();
            //Стартира формата в средата на основната форма от приложението - HomeFormUI!
            this.StartPosition = FormStartPosition.CenterParent;
            this.AddButton.Enabled = false;
            this.ShopNameBox.Tag = false;
            this.PurchaseBox.Tag = false;
            this.AmountBox.Tag = false;
            this.TotalBox.Tag = false;


            this.ShopNameBox.Validating += new CancelEventHandler(TextBoxesEmpty);
            this.PurchaseBox.Validating += new CancelEventHandler(TextBoxesEmpty);
            this.AmountBox.Validating += new CancelEventHandler(TextBoxesEmpty);
            this.TotalBox.Validating += new CancelEventHandler(TextBoxesEmpty);
        }

        
        //Валидира бутона: Когато потребителя е въвел данните правилно - бутона е активен за кликване,
        //иначе не е!
        private void ValidateAddButton()
        {
            this.AddButton.Enabled =
                ((bool)this.ShopNameBox.Tag)
                && (bool)(this.PurchaseBox.Tag)
                && (bool)(this.AmountBox.Tag)
                && (bool)(this.TotalBox.Tag);
        }

        
        private void TextBoxesEmpty(object sender, CancelEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text.Length == 0)
            {
                tb.BackColor = Color.Blue;
                tb.Tag = false;
            }
            else
            {
                tb.BackColor = System.Drawing.SystemColors.Window;
                tb.Tag = true;
            }
            ValidateAddButton();
        }

        //Валидиране на ShopNameBox да приема максимално 25 символа, контроли + само букви.
        private void ShopNameBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            ShopNameBox.MaxLength = 25;
            if (!char.IsControl(e.KeyChar) && !char.IsLetterOrDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        //Валидиране на PurchaseBox да приема максимално 10 символа, контроли и само числа
        private void PurchaseBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            PurchaseBox.MaxLength = 25;
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        //Валидиране на AmountBox да приема само контроли, *,* + само цифри.
        private void AmountBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            AmountBox.MaxLength = 7;
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && ((TextBox)sender).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }

        }

        //Примерна помощ какви данни трябва да се въведат в текстовите полета
        private void HelpButtonAdd_Click(object sender, EventArgs e)
        {
            //стрингове - съединяваме ги с текстовите полета по-долу.
            string shophelp = ":Кауфланд :";
            string purchasehelp = ":Храна:";
            string totalitems = ":5:";
            string amounthelp = ":12.98:";

            this.ShopNameBox.Text = shophelp;
            this.PurchaseBox.Text = purchasehelp;
            this.AmountBox.Text = amounthelp;
            this.TotalBox.Text = totalitems;
        }
        //Изчиства вече попълнените текстови полета/радио бутони и дата валидатор
        private void ClearButton_Click(object sender, EventArgs e)
        {
            ShopNameBox.Clear();
            PurchaseBox.Clear();
            AmountBox.Clear();
            TotalBox.Clear();
            DebitCreditRadioButton.Checked = false;
            CashRadioButton.Checked = false;
           

        }

        private void Close_Click(object sender, EventArgs e)
        {
            Close();
        }

        //Валидираме radiobuttons със стойности. Предварително задаваме глобална променлива
        //в началото на кода - string payment
        private void AddButton_Click(object sender, EventArgs e)
        {
            if (DebitCreditRadioButton.Checked == true)
            {
                payment = "Debit/Credit card";
            }
            if (CashRadioButton.Checked == true)
            {
                payment = "Cash";
            }

            //If проверка да проверява дали въведената стойност е int или decimal/float. Ако не е INT ще даде грешка!
            if (!int.TryParse(TotalBox.Text, out int totaltype))
            {
                MessageBox.Show("Please enter, a valid integer number! Items can be only whole numbers", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Отваряме нов конекшък към базата от данни с try and catch блок
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\george\OneDrive\Projects\Archiver\Archiver-Beta\Archiver-Beta\Databases\ArchiverDataBase.mdf;Integrated Security=True"))
            {
                try
                {
                    connection.Open();
                    // Пишем sql скрип, който ще вземе информацията от текстовите полета и ще ги запише в таблицата
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO [Table] (ShopName, PurchaseType, Items, Amount, Payment, Date) VALUES (@ShopName, @PurchaseType, @Items, @Amount, @Payment, @Date)", connection))
                    {
                        
                        cmd.Parameters.AddWithValue("@ShopName", ShopNameBox.Text);
                        cmd.Parameters.AddWithValue("@PurchaseType", PurchaseBox.Text);
                        // Парсваме информацията в int и отдолу в double:
                        cmd.Parameters.AddWithValue("@Items", totaltype);
                        cmd.Parameters.AddWithValue("@Amount", decimal.Parse(AmountBox.Text));
                        cmd.Parameters.AddWithValue("@Payment", payment);
                        cmd.Parameters.AddWithValue("@Date", dateTimePicker1.Value);
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                    MessageBox.Show("The receipt has been added!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // След получения резултат, AddForm се затваря!
                    Close();
                }
                catch (Exception ex)
                {
                    //Извиква се MessageBox ако настъпи някаква грешка в кода 
                    MessageBox.Show($"The receipt has not been added! There is an erroи with the connection! {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //Валидираме datatimepicker да показва - "dd/mm/yyyy"
            dateTimePicker1.CustomFormat = "dd/mm/yyyy";
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                dateTimePicker1.CustomFormat = "";

            }
        }
    }
}
