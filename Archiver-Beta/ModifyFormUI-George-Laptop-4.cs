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

        string payment;
        //int id;

        public ViewUI()
        {
            InitializeComponent();
            
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            AddButton.Enabled = false;
            this.ShopNameBox.Tag = false;
            this.PurchaseBox.Tag = false;
            this.AmountBox.Tag = false;
            this.TotalBox.Tag = false;
            //DebitCreditRadioButton.Tag = false;
            //CashRadioButton.Tag = false;
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
            ShopNameBox.MaxLength = 50;
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
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
            AmountBox.MaxLength = 20;
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
        //This method Inserts data into our database
        public void InsertData()
        {

            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\george\OneDrive\Projects\Archiver\Archiver-Beta\Archiver-Beta\Databases\ArchiverDataBase.mdf;Integrated Security=True"))
            {
                //again everything in try-catch-finally---:)
                try
                {
                   
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand("INSERT INTO [Table] (ShopName, PurchaseType, Items, Amount, Payment, Date) VALUES (@ShopName, @PurchaseType, @Items, @Amount, @Payment, @Date)", connection))
                    {
                        //Insert data from ShopNameBox (Textbox) to ShopName
                        //same with the rest of them
                        cmd.Parameters.AddWithValue("@ShopName", ShopNameBox.Text);
                        cmd.Parameters.AddWithValue("@PurchaseType", PurchaseBox.Text);

                        cmd.Parameters.AddWithValue("@Items", TotalBox.Text);
                        cmd.Parameters.AddWithValue("@Amount", decimal.Parse(AmountBox.Text));
                        cmd.Parameters.AddWithValue("@Payment", payment);
                        cmd.Parameters.AddWithValue("@Date", dateTimePicker1.Value);
                        cmd.ExecuteNonQuery();

                    }
                    MessageBox.Show("The receipt has been added!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);


                }
                catch (SqlException ex)
                {

                    MessageBox.Show($"The receipt has not been added! There is an erroи with the connection! {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

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
            //Loading data from sql
            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\george\OneDrive\Projects\Archiver\Archiver-Beta\Archiver-Beta\Databases\ArchiverDataBase.mdf;Integrated Security=True");
            connection.Open();
            //making datatable instance
            DataTable receipts = new DataTable();

            //using sql query to fill our table
            SqlDataAdapter query = new SqlDataAdapter(@"SELECT * FROM [TABLE]", connection);
            query.Fill(receipts);
            //защриховам си modifyviewer.datasource с receipts
            ModifyDataViewer.DataSource = receipts;
            //Conver decimal number to BG values
            ModifyDataViewer.Columns["Amount"].DefaultCellStyle.Format = "C";

            ModifyDataViewer.Columns["Amount"].DefaultCellStyle.FormatProvider = new CultureInfo("bg-BG");
            connection.Close();

        }
        public static DataGridViewRow selectedrow;

        private void CheckAndCleanTextBoxes()
        {
            //If all textboxes are full, we can delete their value, if they are empty we cannot delete them, because there are no data
            //checks if all textboxes are full and if one of the radiobuttons are checked
            if (!string.IsNullOrEmpty(ShopNameBox.Text) || !string.IsNullOrEmpty(PurchaseBox.Text) || !string.IsNullOrEmpty(AmountBox.Text) || !string.IsNullOrEmpty(TotalBox.Text) || !string.IsNullOrEmpty(IdBox.Text) || CashRadioButton.Checked || DebitCreditRadioButton.Checked )
            {
                ShopNameBox.Clear();
                PurchaseBox.Clear();
                AmountBox.Clear();
                TotalBox.Clear();
                IdBox.Clear();
                DebitCreditRadioButton.Checked = false;
                CashRadioButton.Checked = false;
                dateTimePicker1.Value = DateTime.Now.AddDays(-1);

            }
           else
            {
                MessageBox.Show("There are not data to be cleaned", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void ClearButton_Click_1(object sender, EventArgs e)
        {
            CheckAndCleanTextBoxes();
        }
        //This method updates our data, when user make changes
        private void UpdataData()
        {
            //IF ONLY USER SELECT A ROW FROM THE TABLE
            if (ModifyDataViewer.SelectedRows.Count > 0)
            {
                //OUR GLOBAL PAYMENT WILL BE DEBIT-CREDIT CARD OR CASH
                if (DebitCreditRadioButton.Checked == true)
                {
                    payment = "Debit/Credit card";
                }
                else if (CashRadioButton.Checked == true)
                {
                    payment = "Cash";
                }
                //Uploading changes 
                using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\george\OneDrive\Projects\Archiver\Archiver-Beta\Archiver-Beta\Databases\ArchiverDataBase.mdf;Integrated Security=True"))
                {

                    try
                    {
                        connection.Open();

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
                    catch (SqlException ex)
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
            //if a user 
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
                UpdataData();
                LoadData();
                ModifyDataViewer.ClearSelection();
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

            else
            {
                MessageBox.Show("There are no selected receipt! Please, select a receipt to edit!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        //This method deletes a selected record 
        private void DeleteRecord()
        {
            //BIG IF
            //if there is selected row,  the method will perform
            if (ModifyDataViewer.SelectedRows.Count > 0)
            {
                //another way to put a messagebox
                string message = "Are you sure you want to delete the receipt?";
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
                            //openning connection
                            connection.Open();
                            //deleting record with  sql query/script
                            string query = "DELETE FROM [TABLE] WHERE Id=@Id";
                            using (SqlCommand cmd = new SqlCommand(query, connection))
                            {
                                cmd.Parameters.AddWithValue("@Id", int.Parse(IdBox.Text));
                                cmd.ExecuteNonQuery();

                            }
                        }
                        //refreshing database
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"There is an error with the connection {ex.Message}");
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
                MessageBox.Show("There are not selected receipt to delete. Please, select a receipt to remove!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }


        }
        //Cleaning all data into the database
        private void ClearAllData()
        {
            string message = "If you delete all saved receipts, you will not be able to recover them! Are you sure you want to clear?";
            string title = "Clear All";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;

            //Before doing what i will do, a messagebox will ask the user if she/him is sure they want to delete all database
            //initializing message
            //--title
            //--icons
            //--buttons 
            //well A LOT 
            if (DialogResult.Yes == MessageBox.Show(message, title, buttons, icon))
            {
                SqlConnection connection = null;
                try
                {
                    //open connection
                    connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\george\OneDrive\Projects\Archiver\Archiver-Beta\Archiver-Beta\Databases\ArchiverDataBase.mdf;Integrated Security=True");
                    connection.Open();
                    //using detele script for deleting
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM [Table]", connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("All records have been deleted!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadData(); // Refresh the DataGridView
                    //Automaticlly, load database again
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    //ako the connection is open, close it
                    if (connection != null && connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
            }
        }

        private void ClearDatabase_Click(object sender, EventArgs e)
        {
            ClearAllData();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {

            if (ModifyDataViewer.SelectedRows.Count > 0)
            {
                DeleteRecord();
            }
            else
            {
                MessageBox.Show("Please, select a receipt form the viewer to delete receipt", "Warning - Delete receipt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }


        private void ModifyDataViewer_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ModifyDataViewer.ClearSelection();
        }


        private void ViewUI_Load(object sender, EventArgs e)
        {
            //when the form starts, all controls need to be empty
            ModifyDataViewer.ClearSelection();
            ShopNameBox.Text = string.Empty;
            PurchaseBox.Text = string.Empty;
            TotalBox.Text = string.Empty;
            AmountBox.Text = string.Empty;
            DebitCreditRadioButton.Checked = false;
            CashRadioButton.Checked = false;
            dateTimePicker1.Value = DateTime.Now.AddDays(-1);
            IdBox.Text = string.Empty;
        }

        private void ModifyDataViewer_DoubleClick(object sender, EventArgs e)
        {
            //another way to clean textboxes
            ModifyDataViewer.ClearSelection();
            ShopNameBox.Text = string.Empty;
            PurchaseBox.Text = string.Empty;
            TotalBox.Text = string.Empty;
            AmountBox.Text = string.Empty;
            payment = string.Empty;
            DebitCreditRadioButton.Checked = false;
            CashRadioButton.Checked = false;
            

            dateTimePicker1.Value = DateTime.Now.AddDays(-1);
            IdBox.Text = string.Empty;
        }
        private void ModifyDataViewer_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //when i double clich, a new form with info will be opened
            LookReceipt lookReceipt = new LookReceipt();

        }

        private void ViewReceiptWITHBUTTON()
        {
            //same, but with the button
            LookReceipt lookreceipt = new LookReceipt();

        }

        private void ViewerReceipButton_Click(object sender, EventArgs e)
        {

            if (ModifyDataViewer.SelectedRows.Count > 0)
            {
                selectedrow = ModifyDataViewer.SelectedRows[0];
                LookReceipt.lookreceipt.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please, select a receipt from the viewer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //making a bitmap image, later to be printed
            //screenshot of our database
            Bitmap imagebmp = new Bitmap(ModifyDataViewer.Width, ModifyDataViewer.Height);
            ModifyDataViewer.DrawToBitmap(imagebmp, new Rectangle(0, 0, ModifyDataViewer.Width, ModifyDataViewer.Height));
            e.Graphics.DrawImage(imagebmp, 120, 20);
        }

        private void PrintrDatabase_Click(object sender, EventArgs e)
        {
            ///printing the pitmap image - 
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.PrintPreviewControl.Zoom = 1;
            printPreviewDialog1.ShowDialog();
        }

        private void FillHelp()
        {
            //fill texboxes with examples
            string shophelp = ":Кауфланд :";
            string purchasehelp = ":Храна:";
            string totalitems = ":5:";
            string amounthelp = ":12.98:";
            CashRadioButton.Checked = true;

            this.ShopNameBox.Text = shophelp;
            this.PurchaseBox.Text = purchasehelp;
            this.AmountBox.Text = amounthelp;
            this.TotalBox.Text = totalitems;
        }

        private void HelpFillButton_Click(object sender, EventArgs e)
        {
            //Call the method
           FillHelp();
        } 

        private void DashboardButton_Click(object sender, EventArgs e)
        {
            Dashboard d1 = new Dashboard();
            d1.ShowDialog();  
        }

        private void RefreshDatabase()
        {
            //refresh database 
            //using directory when i store the database
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\george\OneDrive\Projects\Archiver\Archiver-Beta\Archiver-Beta\Databases\ArchiverDataBase.mdf;Integrated Security=True";
            //poredniq sql script select
            
            string query = "SELECT * FROM [Table]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //again we fill the databaseviewer with fresh info
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                ModifyDataViewer.DataSource = dataTable;
            }
        }


        private void RefreshButton_Click(object sender, EventArgs e)
        {
            //Call the method
            RefreshDatabase();
        }

        private void ViewUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

       
    }
    }

    


