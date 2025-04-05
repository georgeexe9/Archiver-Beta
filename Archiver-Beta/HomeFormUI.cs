using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Archiver_Beta

/// <summary>
/// ==========================:ModifyFormUI (ViewForm) source code:=========================
/// Receipt Archiver by George Lavchanski 217knr - KN1:
/// Written in C#
/// 5 Windows Forms
/// Icons pack  Source: https://icons8.com/icons/ultraviolet (UltraViolet icons pack)
/// georgeexe GitHub: https://github.com/georgeexe9
/// </summary>
{
    public partial class HomeForm : Form
    {



        public HomeForm()
        {
            //Извикваме си методите
            InitializeComponent();
            this.MaximizeBox = false;
            LoadDataListView();
            UpdateRowCountLabel();
            UpdateItemsSumLabel();
            GetSumOfReceipts();
            GetSumAvarageofReceipt();
        }


        public void HomeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string message = "Are you sure you want to exit?";
            string title = "Exit Receipt Archiver";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            MessageBoxIcon icon = MessageBoxIcon.Question;
            if (DialogResult.Yes == MessageBox.Show(message, title, buttons, icon))
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //отваряме си нова форма
            ViewUI v1 = new ViewUI();
            v1.ShowDialog();
        }

        private void RepotButton_Click(object sender, EventArgs e)
        {
            //отваряме си нова форма
            Reports reports = new Reports();
            reports.Enabled = true;
            reports.ShowDialog();
        }


        //Инициализираме си ListView, който ще визуализира данни от нашата база от данни
        public void LoadDataListView()
        {
            //Създаваме си колони и редове, със пропъртита за разстояние и подравняване
            ReceiptViewList.Columns.Add("Id", 50);
            ReceiptViewList.Columns.Add("ShopName", -2, HorizontalAlignment.Left);
            ReceiptViewList.Columns.Add("PurchaseType", -2, HorizontalAlignment.Left);
            ReceiptViewList.Columns.Add("Items", -2, HorizontalAlignment.Left);
            ReceiptViewList.Columns.Add("Amount", -2, HorizontalAlignment.Left);
            ReceiptViewList.Columns.Add("Payment", -2, HorizontalAlignment.Left);
            ReceiptViewList.Columns.Add("Date", -2, HorizontalAlignment.Left);

            //Пропъртита за цветове и дизайн и шрифт
            ReceiptViewList.ForeColor = Color.Black;
            //Background color 
            ReceiptViewList.BackColor = Color.WhiteSmoke;
            //Font, and font type
            ReceiptViewList.Font = new Font("Segoe UI", 8f, FontStyle.Regular);
            //Make grid lines visible
            ReceiptViewList.GridLines = true;
            //Loading receiptviewlist
            ReceiptViewList.View = View.Details;


            //Правим връзка към нашата база от данни, инициализираме си променливите
            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\george\Desktop\Archiver (2)\Archiver\Archiver-Beta\Archiver-Beta\Databases\Receipts.mdf"";Integrated Security=True");
            SqlCommand cmd;
            DataTable dt;
            SqlDataAdapter adapter;
            DataSet dataSet;
            //отваряме връзка
            //използваме sql скрипт, който ще визуализира цялата таблица от базата от данни
            connection.Open();
            cmd = new SqlCommand("SELECT * FROM [Table] ", connection);
            adapter = new SqlDataAdapter(cmd);
            //правим си DataSett адаптер, който пълним с информацията от таблицата
            dataSet = new DataSet();
            adapter.Fill(dataSet, "dt");
            connection.Close();

            //Започваме да пълним ListView с данните от базата от данни
            dt = dataSet.Tables["dt"];
            int i;
            for (i = 0; i < dt.Rows.Count; i++)
            {

                ListViewItem item = new ListViewItem(dt.Rows[i][0].ToString());
                item.SubItems.Add(dt.Rows[i][1].ToString());
                item.SubItems.Add(dt.Rows[i][2].ToString());
                item.SubItems.Add(dt.Rows[i][3].ToString());
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

        //Същото като предходния метод, но го използвам за рефреш
        private void RefreshListView()
        {
            
            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\george\Desktop\Archiver (2)\Archiver\Archiver-Beta\Archiver-Beta\Databases\Receipts.mdf"";Integrated Security=True");
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
            //Извикваме си методите
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


        //Визуализиря общият брой въведени артикули от всяка бележка„
        private int GetRowCount()
        {
            int rowCount = 0;
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\george\Desktop\Archiver (2)\Archiver\Archiver-Beta\Archiver-Beta\Databases\Receipts.mdf"";Integrated Security=True"))
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
                //Ако хване грешка при базата от данни, ще го хване в catch блок и ще извика messagebox с грешката 

                catch (Exception ex)
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
        //Създаваме променлива int rowCount и я защриховаме към private int GetRowCount,
        //присвояваме стойността от променливата към лейбъл контрола

        public void UpdateRowCountLabel()
        {
            int rowCount = GetRowCount();
            TotalRows.Text = $"{rowCount}";
        }

        //Визуализиря общият брой въведени артикули от всяка бележка
        private int GetItemsSum()
        {
            //създаваме променлива
            int itemSum = 0;
            //свързваме базата от данни 
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\george\Desktop\Archiver (2)\Archiver\Archiver-Beta\Archiver-Beta\Databases\Receipts.mdf"";Integrated Security=True"))
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
        //Защриховаме private decimal GetItemsSum към decimal променлива, след което
        //присвояваме стойността на decimal променливата към Лейбъл

        public void UpdateItemsSumLabel()
        {
            decimal itemsSum = GetItemsSum();
            TotalSum.Text = $"{itemsSum}";

        }
        //Визуализира общата сума от касовите бележки
        private decimal GetSumOfRows()
        {
            //създаваме си променлива
            decimal amountSum = 0;
            //свързваме нашата база от данни
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\george\Desktop\Archiver (2)\Archiver\Archiver-Beta\Archiver-Beta\Databases\Receipts.mdf"";Integrated Security=True"))
            {
                try
                {
                    //отваряме връзка
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

        //Защриховаме private decimal към decimal променлива и след това присвояваме тази стойност към лейбъл,
        //конвертираме decimal в бг валута - лв.

        private void GetSumOfReceipts()
        {
            decimal amountSum = GetSumOfRows();
            string formattedAmount = amountSum.ToString("C", new CultureInfo("bg-BG"));
            TotalAmountReceipts.Text = $"{formattedAmount}";

        }
        //Визуализира средна сума от касовите бележки
        private decimal GetSumAvarege()
        {
            //създаваме си променлива
            decimal amountAvarage = 0;
            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\george\Desktop\Archiver (2)\Archiver\Archiver-Beta\Archiver-Beta\Databases\Receipts.mdf"";Integrated Security=True"))
            {
                try
                {
                    //отваряме връзка
                    connection.Open();
                    //SQL скрипт, който пресмята средната стойност на сумата на всички бележките 
                    using (SqlCommand cmd = new SqlCommand("SELECT AVG(Amount) AS AvarageAmount FROM [TABLE]", connection))
                    {
                        var result = cmd.ExecuteScalar();
                        //Ако нямам въведени стойности в таблицата да изпише 0 в Label контролите, вместо да изписва грешката в catch
                        if (result != DBNull.Value)
                        {
                            amountAvarage = Convert.ToInt32(result);
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
        //Защриховаме private decimal към decimal променлива, след което присвояваме тази стойност в лейбъл и конвертираме към
        //бг лева

        public void GetSumAvarageofReceipt()
        {
            decimal amountAvarage = GetSumAvarege();
            string formattedAmount = amountAvarage.ToString("C", new CultureInfo("bg-BG"));
            AvarageSumReceipts.Text = $"{formattedAmount}";
        }



        private void HomeForm_Load(object sender, EventArgs e)
        {

        }
    }

}




