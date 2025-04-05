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
using System.CodeDom;


namespace Archiver_Beta
{
    public partial class ViewUI : Form

    {
        //global fields/variables
        private TextBox textbox;
        string payment;
        int id;

        public ViewUI()
        {
            InitializeComponent();
            //disable maximize button because we dont plant to make it bigger 
            this.MaximizeBox = false;
            //Setting centralparent position of the form
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
            
            LoadData();
        }
        //Validate AddButton to be active when all texboxes are full with infomation
        private void ValidateAddButton()
        {
            this.AddButton.Enabled =
                ((bool)this.ShopNameBox.Tag)
                && (bool)(this.PurchaseBox.Tag)
                && (bool)(this.AmountBox.Tag)
                && (bool)(this.TotalBox.Tag);

        }

        //If texboxes have zero length, then they will be in blue color, if not, will be the same
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
            else if (e.KeyChar == ',' && ((TextBox)sender).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
            //else if ()
        }
        //Radiobutton validations if / if one of the radiobutton is checked, the user can insert data and upload date into the table
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

            //If one of the radibutton is checked, the methods below will be excited, if not, it will thown a messagebox
            if (DebitCreditRadioButton.Checked || CashRadioButton.Checked)
            {
                InsertData();
                LoadData();
            }
            else
            {           
              MessageBox.Show("Please, select a payment option!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            
        }
        //Establising connection with the sql local db.
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
            //Loading data into the modifydataviewer
            //Open new sql connection for retriving data
            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\george\OneDrive\Projects\Archiver\Archiver-Beta\Archiver-Beta\Databases\ArchiverDataBase.mdf;Integrated Security=True");
            connection.Open();
            DataTable receipts = new DataTable();

            //SQL скрипт
            SqlDataAdapter query = new SqlDataAdapter(@"SELECT * FROM [TABLE]", connection);
            query.Fill(receipts);

            ModifyDataViewer.DataSource = receipts;
            ModifyDataViewer.Columns["Amount"].DefaultCellStyle.Format = "C";
            //format the amount values into bulgarian lev
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
                //payment - global variable
                //checking the radiobuttons
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
                            cmd.Parameters.AddWithValue("@Items", TotalBox.Text);
                            cmd.Parameters.AddWithValue("@Amount", decimal.Parse(AmountBox.Text));
                            cmd.Parameters.AddWithValue("@Payment", payment);
                            cmd.Parameters.AddWithValue("@Date", dateTimePicker1.Value);
                            cmd.Parameters.AddWithValue("@Id", int.Parse(IdBox.Text));
                            cmd.ExecuteNonQuery();
                        }
                        MessageBox.Show("The receipt has been updated!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                      

                    }
                    catch (Exception ex)
                    {
                         
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
                //Update the edited receipt
                UpdataData();
                //Update the modifydataview with the edited items
                LoadData();
                //clear selected row
                ModifyDataViewer.ClearSelection();
                //clear all controls
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
                    SqlConnection connecton = null;
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

                            }
                        }
                        LoadData();
                    }
                    catch (SqlException sqlexceptionerror)
                    {
                        MessageBox.Show($"There is an error with the connection {sqlexceptionerror.Message}");
                    }
                    finally
                    {
                        if (connecton != null && connecton.State == ConnectionState.Open)
                        {
                            connecton.Close();
                        }
                    }

                }
                
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

            if (DialogResult.Yes == MessageBox.Show(message, title, buttons, icon))
            {
                SqlConnection connection = null;
                try
                {
                    connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\george\OneDrive\Projects\Archiver\Archiver-Beta\Archiver-Beta\Databases\ArchiverDataBase.mdf;Integrated Security=True");
                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM [Table]", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("All records have been deleted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData(); // Refresh the DataGridView
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (connection != null && connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void ClearDatabase_Click(object sender, EventArgs e)
        {
            //calling method to clean all data from the controls
            ClearAllData();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            //if we have a selected row from the modifydataview will can delete, if it is not selected then will be a error
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
            //by loading form, all controls must be empty and data picker will be with one day behind
            ModifyDataViewer.ClearSelection();
            ShopNameBox.Text = string.Empty;
            PurchaseBox.Text = string.Empty;
            TotalBox.Text = string.Empty;
            AmountBox.Text = string.Empty;
            DebitCreditRadioButton.Checked = false;
            CashRadioButton.Checked = false;
            //payment = string.Empty;
            dateTimePicker1.Value = DateTime.Now.AddDays(-1);
            IdBox.Text = string.Empty;
        }

        private void ModifyDataViewer_DoubleClick(object sender, EventArgs e)
        {
            //Clear all controls when I double click over the selected row in the modifydataviewer
            ModifyDataViewer.ClearSelection();
            ShopNameBox.Text = string.Empty;
            PurchaseBox.Text = string.Empty;
            TotalBox.Text = string.Empty;
            AmountBox.Text = string.Empty;
            payment = string.Empty;
            DebitCreditRadioButton.Checked = false;
            CashRadioButton.Checked = false;
            //payment = string.Empty;
            dateTimePicker1.Value = DateTime.Now.AddDays(-1);
            IdBox.Text = string.Empty;
        }
        private void ModifyDataViewer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            LookReceipt lookReceipt = new LookReceipt();
           
        }

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


