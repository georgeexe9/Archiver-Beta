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
using System.Threading;
using System.Xml.Schema;
using System.Globalization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Archiver_Beta

/// <summary>
/// ==========================:ModifyFormUI (ViewForm) source code:=========================
/// Receipt Archiver by George Lavchanski 217knr - KN1:
/// Written in C# 0:
/// Dedicated to Slav
/// 7 Windows Forms
/// Icons pack  Source: https://icons8.com/icons/ultraviolet (UltraViolet icons pack)
/// georgeexe GitHub: https://github.com/georgeexe9
/// </summary>
{
    public partial class HomeForm : Form
    {

        public HomeForm()
        {
           
            InitializeComponent();
            this.MaximizeBox = false;
            //Loading data into ListView
            LoadDataListView();
            //Loading information into lables (Methods)
            UpdateRowCountLabel();
            UpdateItemsSumLabel();
            GetSumOfReceipts();
            GetSumAvarageofReceipt();
            //DashBoardViewer();

        }
        private void HomeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //will change it later
            string message = "Are you sure you want to exit?";
            string title = "Exit Receipt Archiver";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;
            if (DialogResult.Yes == MessageBox.Show(message, title, buttons, icon))
            {
                //the app will close
                e.Cancel = false;
            }
            else
            {
                //the app will not close
                e.Cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.Hide();
            ViewUI viewUI = new ViewUI();
            viewUI.ShowDialog();

        }
        private void RepotButton_Click(object sender, EventArgs e)
        {
            Reports reports = new Reports();
            reports.Enabled = true;
            reports.ShowDialog();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            SearchReceipts searchReceipts = new SearchReceipts();
            searchReceipts.ShowDialog();
        }

        //ListView maker method
        public void LoadDataListView()
        {
            // Create columns in ListView with titles, character limit (length), and left-aligned text
            // Autosize columns based on cell values
            ReceiptViewList.Columns.Add("Id", 50);
            ReceiptViewList.Columns.Add("ShopName", -2, HorizontalAlignment.Left);
            ReceiptViewList.Columns.Add("PurchaseType", -2, HorizontalAlignment.Left);
            ReceiptViewList.Columns.Add("Items", -2, HorizontalAlignment.Left);
            ReceiptViewList.Columns.Add("Amount", -2, HorizontalAlignment.Left);
            ReceiptViewList.Columns.Add("Payment", -2, HorizontalAlignment.Left);
            ReceiptViewList.Columns.Add("Date", -2, HorizontalAlignment.Left);
            
            ReceiptViewList.ForeColor = Color.Black;
            //Background color 
            ReceiptViewList.BackColor = Color.WhiteSmoke;
            //Font, and font type
            ReceiptViewList.Font = new Font("Segoe UI", 8f, FontStyle.Regular);
            //Make grid lines visible
            ReceiptViewList.GridLines = true;
            //Loading receiptviewlist
            ReceiptViewList.View = View.Details;


            //Създаваме връзка към нашата база от данни
            //може би ще го сложа в try and catch ;)
            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\george\OneDrive\Projects\Archiver\Archiver-Beta\Archiver-Beta\Databases\ArchiverDataBase.mdf;Integrated Security=True");
            SqlCommand cmd;
            DataTable dt;
            SqlDataAdapter adapter;
            DataSet dataSet;

            connection.Open();
            cmd = new SqlCommand("SELECT * FROM [Table] ", connection);
            adapter = new SqlDataAdapter(cmd);
            dataSet = new DataSet();
            adapter.Fill(dataSet, "dt");
            connection.Close();

            //Пълним съдържанието на листа с информация от базата от данни (елементи)
            dt = dataSet.Tables["dt"];
            int i; //i - id на всеки запис
            for (i = 0; i < dt.Rows.Count; i++)
            {

                ListViewItem item = new ListViewItem(dt.Rows[i][0].ToString()); //Визуализираме id
                item.SubItems.Add(dt.Rows[i][1].ToString());  //Визуализираме Shopname
                item.SubItems.Add(dt.Rows[i][2].ToString()); //Визуализираме PurchaseType
                item.SubItems.Add(dt.Rows[i][3].ToString()); //Визуализиаме TotalItems
                decimal amount = Convert.ToDecimal(dt.Rows[i].ItemArray[4]); //Визуализираме Amount в български лев
                string formattedAmount = amount.ToString("C", new CultureInfo("bg-BG")); // форматираме стригна в бг лев
                item.SubItems.Add(formattedAmount);
                item.SubItems.Add(dt.Rows[i].ItemArray[5].ToString()); //Визуализираме Payment
                item.SubItems.Add(dt.Rows[i].ItemArray[6].ToString()); //визуализираме Date

                //Добавяме всеки елемент от по-горе изброените в list view
                ReceiptViewList.Items.Add(item);
                ReceiptViewList.Columns[0].Width = -2;
                
            }

        }

        public void RefreshListView()
        {
            //Създаваме връзка към нашата база от данни
            //може би ще го сложа в try and catch ;)
            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\george\OneDrive\Projects\Archiver\Archiver-Beta\Archiver-Beta\Databases\ArchiverDataBase.mdf;Integrated Security=True");
            SqlCommand cmd;
            DataTable dt;
            SqlDataAdapter adapter;
            DataSet dataSet;

            connection.Open();
            cmd = new SqlCommand("SELECT * FROM [Table] ", connection);
            adapter = new SqlDataAdapter(cmd);
            dataSet = new DataSet();
            adapter.Fill(dataSet, "dt");
            ReceiptViewList.Items.Clear();
            connection.Close();

            //Пълним съдържанието на листа с информация от базата от данни (елементи)
            dt = dataSet.Tables["dt"];
            int i; //i - id на всеки запис
            for (i = 0; i < dt.Rows.Count; i++)
            {

                ListViewItem item = new ListViewItem(dt.Rows[i][0].ToString()); //Визуализираме id
                item.SubItems.Add(dt.Rows[i][1].ToString());  //Визуализираме Shopname
                item.SubItems.Add(dt.Rows[i][2].ToString()); //Визуализираме PurchaseType
                item.SubItems.Add(dt.Rows[i][3].ToString()); //Визуализиаме TotalItems
                decimal amount = Convert.ToDecimal(dt.Rows[i].ItemArray[4]); //Визуализираме Amount в български лев
                string formattedAmount = amount.ToString("C", new CultureInfo("bg-BG")); // форматираме стригна в бг лев
                item.SubItems.Add(formattedAmount);
                item.SubItems.Add(dt.Rows[i].ItemArray[5].ToString()); //Визуализираме Payment
                item.SubItems.Add(dt.Rows[i].ItemArray[6].ToString()); //визуализираме Date


                ReceiptViewList.Items.Add(item); //Добавяме всеки елемент от по-горе изброените в list view
            }

        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {

            RefreshListView();
            UpdateRowCountLabel();
            UpdateItemsSumLabel();
            GetSumOfReceipts();
            GetSumAvarageofReceipt();
        }
        ///<summary>
        /// ВИЗУАЛИЗАЦИЯ НА (DASHBOARD във формата)
        /// Общ брой касови бележки - Rows
        /// Общ брой въвеведи артикули от касовите бележки
        /// Обща сума от всички въведени касови бележки
        /// Обща средна стойност от всички въведени касови бележки
        ///</summary>
        // Визуализира общият брой на Касовите бележки в таблицата - rows 

        private int GetRowCount()
        {
            int rowCount = 0;
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\george\OneDrive\Projects\Archiver\Archiver-Beta\Archiver-Beta\Databases\ArchiverDataBase.mdf;Integrated Security=True"))
            {
                try
                {
                    connection.Open();
                    //SQL скрипт, който показва общия брой на редовете в таблицата (Касовите бележки)
                    using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM [TABLE]", connection))
                    {
                        var result = cmd.ExecuteScalar();
                        //Ако нямам въведени стойности в таблицата да изпише 0 в Label контролите, вместо да изписва грешката в catch
                        if (result != DBNull.Value) 
                        {
                            rowCount = Convert.ToInt32(result);
                        }
                        else
                        {
                            rowCount = 0;
                        }
                       
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"An error occurred while retrieving the receipts count: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
            return rowCount;
        }

        private void UpdateRowCountLabel()
        {
            int rowCount = GetRowCount();
            TotalRows.Text = $"{rowCount}"; 
        }

        //Визуализиря общият брой въведени артикули от всяка бележка
        private int GetItemsSum()
        {
            int itemSum = 0;
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\george\OneDrive\Projects\Archiver\Archiver-Beta\Archiver-Beta\Databases\ArchiverDataBase.mdf;Integrated Security=True"))
            {
                try
                {
                    connection.Open();
                    //SQL скрипт, който събира абсолютно всички въведени артикули (items) в таблицата
                    using (SqlCommand cmd = new SqlCommand("SELECT SUM(Items) AS TotalSum FROM [TABLE]", connection))
                    {
                        var result = cmd.ExecuteScalar();
                        //Ако нямам въведени стойности в таблицата да изпише 0 в Label контролите, вместо да изписва грешката в catch
                        if (result != DBNull.Value)
                        {
                            itemSum = Convert.ToInt32(result);
                        }
                        else
                        {
                            itemSum = 0;
                        }
                    }
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while retrieving the number of total items: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
            return itemSum;
        }

        private void UpdateItemsSumLabel()
        {
            decimal itemsSum = GetItemsSum();
            TotalSum.Text = $"{itemsSum}"; 

        }
        //Визуализира общата сума от касовите бележки
        private decimal GetSumOfRows()
        {
            decimal amountSum = 0;
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\george\OneDrive\Projects\Archiver\Archiver-Beta\Archiver-Beta\Databases\ArchiverDataBase.mdf;Integrated Security=True"))
            {
                try
                {
                    connection.Open();
                    //SQL скрипт, който събира сумите на всичките бележки
                    using (SqlCommand cmd = new SqlCommand("SELECT SUM(Amount) AS TotalSum FROM [TABLE]", connection))
                    {
                        var result = cmd.ExecuteScalar();
                        //Ако нямам въведени стойности в таблицата да изпише 0 в Label контролите, вместо да изписва грешката в catch
                        if (result != DBNull.Value) 
                        {
                            amountSum = Convert.ToInt32(result);
                        }
                        else
                        {
                            amountSum = 0;
                        }

                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while retrieving the total sum: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        return amountSum;
        }
        
        private void GetSumOfReceipts()
        {
            decimal amountSum = GetSumOfRows();
            string formattedAmount = amountSum.ToString("C", new CultureInfo("bg-BG"));
             TotalAmountReceipts.Text = $"{formattedAmount}";

        }
        //Визуализира средна сума от касовите бележки
        private decimal GetSumAvarege()
        {
            decimal amountAvarage = 0;
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\george\OneDrive\Projects\Archiver\Archiver-Beta\Archiver-Beta\Databases\ArchiverDataBase.mdf;Integrated Security=True"))
            {
                try
                {
                    connection.Open();
                    //SQL скрипт, който пресмята средната стойност на сумата на всички бележките 
                    using (SqlCommand cmd = new SqlCommand("SELECT AVG(Amount) AS AvarageAmount FROM [TABLE]", connection))
                    {
                        var result = cmd.ExecuteScalar();
                        //Ако нямам въведени стойности в таблицата да изпише 0 в Label контролите, вместо да изписва грешката в catch
                        if (result != DBNull.Value)
                        {
                            amountAvarage = Convert.ToInt32( result);
                        }
                        else
                        {
                            amountAvarage = 0;
                        }
                      
                    }
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred while retrieving the total avarage sum : {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
            return amountAvarage;
        }
        private void GetSumAvarageofReceipt()
        {
            //Присвоявам на променливата получените стойности от връзката с база от данни
            decimal amountAvarage = GetSumAvarege();
            //Форматираме променливата от decimal в string, за да визуализира валутата
            string formattedAmount = amountAvarage.ToString("C", new CultureInfo("bg-BG"));
            AvarageSumReceipts.Text = $"{formattedAmount}";
        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshListView();
        }
    }
    
}




