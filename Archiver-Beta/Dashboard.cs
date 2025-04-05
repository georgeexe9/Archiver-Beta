using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;
using System;


namespace Archiver_Beta
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;


        }



        private void Dashboard_Load(object sender, EventArgs e)
        {


            UpdateRowCountLabel();
            UpdateItemsSumLabel();
            GetSumAvarageofReceipt();
            GetSumOfReceipts();
        }
        private void CloseButton3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private int GetRowCount()
        {
            //Визуализиря общият брой въведени артикули от всяка бележка
            int rowCount = 0;
            using (SqlConnection connection = SQLconnection.GetConnection())
            {
                try
                {
                    //openning connection
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
            using (SqlConnection connection = SQLconnection.GetConnection())
            {
                try
                {
                    //отваряме връзка
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
            decimal amountSum = 0;
            //свързваме нашата база от данни
            using (SqlConnection connection = SQLconnection.GetConnection())
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
                //хващаме грешките в catch блока
                catch (SqlException ex)
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
            //създаваме променлива, използваме връзка към нашата база от данни
            decimal amountAvarage = 0;
            using (SqlConnection connection = SQLconnection.GetConnection())
            {
                try
                {
                    //отваряме връзка към базата от данни
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
    }
}
