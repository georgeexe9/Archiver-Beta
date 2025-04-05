using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Web;
using System.Globalization;
using System.Reflection.Emit;
using System.Diagnostics.Eventing.Reader;


namespace Archiver_Beta
{
    public partial class ViewUI : Form

    {
        private TextBox textbox;
        string payment;
        int id;
        public ViewUI()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            AddButton.Enabled = false;
            this.ShopNameBox.Tag = false;
            this.PurchaseBox.Tag = false;
            this.AmountBox.Tag = false;
            this.TotalBox.Tag = false;
            DebitCreditRadioButton.Checked = false;
            CashRadioButton.Checked = false;
            ModifyDataViewer.RowHeadersVisible = false;
            this.ShopNameBox.Validating += new CancelEventHandler(TextBoxesEmpty);
            this.PurchaseBox.Validating += new CancelEventHandler(TextBoxesEmpty);
            this.AmountBox.Validating += new CancelEventHandler(TextBoxesEmpty);
            this.TotalBox.Validating += new CancelEventHandler(TextBoxesEmpty);
            //ModifyDataViewer.DataBindingComplete += new DataGridViewBindingCompleteEventHandler(ModifyDataViewer_DataBindingComplete);
            //ModifyDataViewer.CellClick += new DataGridViewCellEventHandler(ModifyDataViewer_CellClick);
            LoadData();
        }
        //Валидиране на бутона OK - ако изискванията на TextBoxes са изпълнени, то бутона ще е активен за кликане
        private void ValidateAddButton()
        {
            this.AddButton.Enabled =
                ((bool)this.ShopNameBox.Tag)
                && (bool)(this.PurchaseBox.Tag)
                && (bool)(this.AmountBox.Tag)
                && (bool)(this.TotalBox.Tag);

        }

        //Метод TextBoxesEmpy сигнализира със син цвят, ако не е вкарана информация в текстовите полета
        private void TextBoxesEmpty(object sender, CancelEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text.Length == 0)
            {
                tb.BackColor = Color.Red;
                tb.Tag = false;
            }
            else
            {
                tb.BackColor = System.Drawing.SystemColors.Window;
                tb.Tag = true;
            }
            ValidateAddButton();
        }
        private void ShopNameBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            ShopNameBox.MaxLength = 25;
            if (!char.IsControl(e.KeyChar) && !char.IsLetterOrDigit
            (e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void PurchaseBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            PurchaseBox.MaxLength = 25;
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }


        }

        private void TotalBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) 
                {
                    e.Handled = true;
                }
            if (e.KeyChar == '0')
            {
                e.Handled = true;
            }

        }

        private void AmountBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            AmountBox.MaxLength = 7;
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '.' && !
            char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            if (e.KeyChar == ',' && ((TextBox)sender).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
            //Да не се приема нулева стойност в amount box 
            
            //e.Handled = !char.IsDigit(e.KeyChar) && e.KeyChar != (char)8;
            //if(e.KeyChar ==(char)13)
            //{
            //    AmountBox.Text = string.Format(CultureInfo.CreateSpecificCulture("bg-BG"),"{0:C}",decimal.Parse(AmountBox.Text));
            //}
        }
        //При натискане на AddButton:
        //If конструкция, която проверява какви стойности са зададени за радиобутоните
        //Зарежда методите - InsertData и LoadData
        private void AddButton_Click(object sender, EventArgs e)
        {
            if (DebitCreditRadioButton.Checked == true)
            {
                payment = "Debit/Credit card";
            }
            else if (CashRadioButton.Checked == true)
            {
                payment = "Cash";
            }

            InsertData();
            LoadData();
        }
        //InsertData създава връзка към локалната база от данни, отваря връзка със sql скрипт, който пълни въведената информация от потребителя (текстовите полета) и
        //ги запаметява в таблицата [TABLE]
        //Цялата връзка към базата от данни е включена в try & catch блок
        public void InsertData()
        {
           
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
                        cmd.Parameters.AddWithValue("@Items", TotalBox.Text);
                        cmd.Parameters.AddWithValue("@Amount", decimal.Parse(AmountBox.Text));
                        cmd.Parameters.AddWithValue("@Payment", payment);
                        cmd.Parameters.AddWithValue("@Date", dateTimePicker1.Value);
                        cmd.ExecuteNonQuery();
                        
                    }
                    MessageBox.Show("The receipt has been added!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // След получения резултат, AddForm се затваря!

                }
                catch (Exception ex)
                {
                    //Извиква се MessageBox ако настъпи някаква грешка в кода 
                    MessageBox.Show($"The receipt has not been added! There is an erroи with the connection! {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close() ;
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

        public void LoadData()
        {
            //Да го сложа в TRY/CATCH
            //Отваряме нов конекшък към базата от данни - path
            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\george\OneDrive\Projects\Archiver\Archiver-Beta\Archiver-Beta\Databases\ArchiverDataBase.mdf;Integrated Security=True");
            connection.Open();
            DataTable receipts = new DataTable();

            //SQL скрипт
            SqlDataAdapter query = new SqlDataAdapter(@"SELECT * FROM [TABLE]", connection);
            query.Fill(receipts);

            ModifyDataViewer.DataSource = receipts;
            ModifyDataViewer.Columns["Amount"].DefaultCellStyle.Format = "C";
            ModifyDataViewer.Columns["Amount"].DefaultCellStyle.FormatProvider = new CultureInfo("bg-BG");
            connection.Close();
            
        }
        public static DataGridViewRow selectedrow;

        private void CheckAndCleanTextBoxes()
        {
            
            ShopNameBox.Clear();
            PurchaseBox.Clear();
            AmountBox.Clear();
            TotalBox.Clear();
            IdBox.Clear();
            DebitCreditRadioButton.Checked = false;
            CashRadioButton.Checked = false;

        }

        private void ClearButton_Click_1(object sender, EventArgs e)
        {
            CheckAndCleanTextBoxes();
        }
           
        private void UpdataData()
        {

            if (ModifyDataViewer.SelectedRows.Count > 0)
            {
                //payment - глобална променлива
                //Проверяваме стойността на radio buttons
                if (DebitCreditRadioButton.Checked == true)
                {
                    payment = "Debit/Credit card";
                }
                else if (CashRadioButton.Checked == true)
                {
                    payment = "Cash";
                }
                
                using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\george\OneDrive\Projects\Archiver\Archiver-Beta\Archiver-Beta\Databases\ArchiverDataBase.mdf;Integrated Security=True"))
                {

                    try
                    {
                        connection.Open();
                        // Пишем sql скрип, който ще вземе информацията от текстовите полета и ще ги запише в таблицата
                        using (SqlCommand cmd = new SqlCommand("UPDATE [Table] SET ShopName = @ShopName, PurchaseType = @PurchaseType, Items = @Items, Amount = @Amount, Payment = @Payment, Date = @Date WHERE Id = @Id", connection))
                        {

                            cmd.Parameters.AddWithValue("@ShopName", ShopNameBox.Text);
                            cmd.Parameters.AddWithValue("@PurchaseType", PurchaseBox.Text);
                            // Парсваме информацията в int и отдолу в double:
                            cmd.Parameters.AddWithValue("@Items", TotalBox.Text);
                            cmd.Parameters.AddWithValue("@Amount", decimal.Parse(AmountBox.Text));
                            cmd.Parameters.AddWithValue("@Payment", payment);
                            cmd.Parameters.AddWithValue("@Date", dateTimePicker1.Value);
                            cmd.Parameters.AddWithValue("@Id", int.Parse(IdBox.Text));
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("The receipt has been updated!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        // След получения резултат, AddForm се затваря!

                    }
                    catch (Exception ex)
                    {
                        //Извиква се MessageBox ако настъпи някаква грешка в кода 
                        MessageBox.Show($"The receipt has not been added! There is an error with the connection! {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }

            }
        }

        private void ModifyDataViewer_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                IdBox.Text = ModifyDataViewer.CurrentRow.Cells[0].Value.ToString();
                ShopNameBox.Text = ModifyDataViewer.CurrentRow.Cells[1].Value.ToString();
                PurchaseBox.Text = ModifyDataViewer.CurrentRow.Cells[2].Value.ToString();
                TotalBox.Text = ModifyDataViewer.CurrentRow.Cells[3].Value.ToString();
                AmountBox.Text = ModifyDataViewer.CurrentRow.Cells[4].Value.ToString();
                string payment = ModifyDataViewer.CurrentRow.Cells[5].Value.ToString();
                dateTimePicker1.Text = ModifyDataViewer.CurrentRow.Cells[6].Value.ToString();
                if (payment == "Debit/Credit card")
                {
                    DebitCreditRadioButton.Checked = true;
                }
                else if (payment == "Cash")
                {
                    CashRadioButton.Checked = true;
                } 
            }

        }
       


        private void EditButton_Click(object sender, EventArgs e)
        {
            if (ModifyDataViewer.SelectedRows.Count > 0)
            {
                //Актуализира новите данни в редактираната бележка
                UpdataData();
                //Актуализира цялата таблица с бележки
                LoadData();
                //Изчиства избран ред от таблицата
                ModifyDataViewer.ClearSelection();
                //Изчиста всичките контроли
                ShopNameBox.Text = string.Empty;
                PurchaseBox.Text = string.Empty;
                TotalBox.Text = string.Empty;
                AmountBox.Text = string.Empty;
                payment = string.Empty;
                this.DebitCreditRadioButton.Checked = false;
                this.CashRadioButton.Checked = false;
                dateTimePicker1.Value = DateTime.Now.AddDays(-1);
                IdBox.Text = string.Empty;
                
            }
            //Ако няма избран ред в таблицата, изписва тва:
            else
            {
                MessageBox.Show("There are no selected receipt! Please, select a receipt to edit!", "Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        private void DeleteRecord()
        {
            if (ModifyDataViewer.SelectedRows.Count > 0) 
            {
                string message = "Are you sure, you want to delete the receipt?";
                string title = "Delete Receipt";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Question;
                if (DialogResult.Yes == MessageBox.Show(message, title, buttons, icon))
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\george\OneDrive\Projects\Archiver\Archiver-Beta\Archiver-Beta\Databases\ArchiverDataBase.mdf;Integrated Security=True"))
                        {
                            connection.Open();
                            string query = "DELETE FROM [TABLE] WHERE Id=@Id";
                            using (SqlCommand cmd = new SqlCommand(query, connection))
                            {
                                cmd.Parameters.AddWithValue("@Id", int.Parse(IdBox.Text));
                                cmd.ExecuteNonQuery();
                                connection.Close();

                            }
                        }
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"There is an error with the connection {ex.Message}");
                    }

                }
                LoadData();
            }
            else
            {
                MessageBox.Show("There are not selected receipt to delete. Please, select a receipt to remove!","Warning",MessageBoxButtons.OK,MessageBoxIcon.Warning );
            }
              
        }


        private void ClearAllData()
        {
            string message = "If you delete all saved receipts, you will not be able to recover them! Are you sure you want to clear?";
            string title = "Clear All";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;
            //Нещата по-горе = MessageBox.Show(message, title, buttons, icon)
            if (DialogResult.Yes == MessageBox.Show(message, title, buttons, icon))
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\george\OneDrive\Projects\Archiver\Archiver-Beta\Archiver-Beta\Databases\ArchiverDataBase.mdf;Integrated Security=True"))
                    {
                        connection.Open();
                        using (SqlCommand cmd = new SqlCommand("DELETE FROM [Table]", connection))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                    MessageBox.Show("All records have been deleted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData();// Refresh the DataGridView
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
        }


        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ClearAllData();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            //IF конструкция, която проверява колко са селектираните редове и ако са повече от 0 , изтрива записи, ако са 0 , дава грешка
            if (ModifyDataViewer.SelectedRows.Count > 0)
            {
                DeleteRecord();
            }
           else
            {
                MessageBox.Show("Please, select a receipt form the viewer to delete receipt", "Warning - Delete receipt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void CloseButton_Click(object sender, EventArgs e)
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

        private void ModifyDataViewer_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ModifyDataViewer.ClearSelection();
        }


        private void ViewUI_Load(object sender, EventArgs e)
        {
            ModifyDataViewer.ClearSelection();
            ShopNameBox.Text = string.Empty;
            PurchaseBox.Text = string.Empty;
            TotalBox.Text = string.Empty;
            AmountBox.Text = string.Empty;
            payment = string.Empty;
            dateTimePicker1.Value = DateTime.Now.AddDays(-1);
            IdBox.Text = string.Empty;
        }

        private void ModifyDataViewer_DoubleClick(object sender, EventArgs e)
        {
            ModifyDataViewer.ClearSelection();
            ShopNameBox.Text = string.Empty;
            PurchaseBox.Text = string.Empty;
            TotalBox.Text = string.Empty;
            AmountBox.Text = string.Empty;
            payment = string.Empty;
            dateTimePicker1.Value = DateTime.Now.AddDays(-1);
            IdBox.Text = string.Empty;
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
        }

        private void ModifyDataViewer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            LookReceipt lookReceipt = new LookReceipt();
           
        }

        //private void ModifyDataViewer_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    if (e.RowIndex >=0)
        //    {
        //        selectedrow = ModifyDataViewer.Rows[e.RowIndex];
        //        LookReceipt.lookreceipt.ShowDialog();
        //    }
        //}

        private void ViewReceiptWITHBUTTON()
        {
            LookReceipt lookreceipt = new LookReceipt();

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {  
            if (ModifyDataViewer.SelectedRows.Count > 0)
            {
                selectedrow = ModifyDataViewer.SelectedRows[0] ;
                LookReceipt.lookreceipt.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please, select a receipt from the viewer.","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ModifyDataViewer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}


