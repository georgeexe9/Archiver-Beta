using System;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;


namespace Archiver_Beta
{
    public partial class ViewUI : Form

    {
        //глобална променлива
        string payment;
        //int id;

        public ViewUI()
        {
            //тагваме текстовите полена, задаваме на формата да стартира от центъра на екрана на основната форма
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
            //Защриховаме събитията към текстовите полета
            this.ShopNameBox.Validating += new CancelEventHandler(TextBoxesEmpty);
            this.PurchaseBox.Validating += new CancelEventHandler(TextBoxesEmpty);
            this.AmountBox.Validating += new CancelEventHandler(TextBoxesEmpty);
            this.TotalBox.Validating += new CancelEventHandler(TextBoxesEmpty);



            //Извикваме метод
            LoadData();
        }
        //Валидираме бутона да е активен, само когато имаме въведени стойности в текстовите полета
        private void ValidateAddButton()
        {
            this.AddButton.Enabled =
                ((bool)this.ShopNameBox.Tag)
                && (bool)(this.PurchaseBox.Tag)
                && (bool)(this.AmountBox.Tag)
                && (bool)(this.TotalBox.Tag);

        }



        //Проверява дали текстовите полета имат въведена стойност, ако нямат , те се оцветяват в червено, ако не си остават в бяло и активираме бутона
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
        //Валидация за ShopNameBox - макс 50 символа за въвеждане, може контроли, само букви
        private void ShopNameBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            ShopNameBox.MaxLength = 50;
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        //Валидация на PurchaseBox - 25 макс символа само букви и контроли
        private void PurchaseBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            PurchaseBox.MaxLength = 25;
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }


        }
        //Валидацията на TotalBox само числа и контроли и без 0
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
        //Валидация на AmountBox - макс 20 символа, само десетични числа или нормално.
        //Десетичните числа се отделят само с точка - .
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

        }
        //Присвояваме стойност на радиобутона DebitCredit и стойност на Cash
        //използваме глобалната променлива
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

            //Проверява дали сме избрали един от двата радио бутона и ако сме избрали,
            ///записва бележката и рефрешва базата от данни, а ако не сме избрали, ни показва съобщение да изберем
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
        /// <summary>
        /// Методът InsertData() вмъква въведените стойности от текстовите полета директно в базата данни, използвайки
        /// SQL заявки.
        /// Използвам Connection String, който указва пътя до базата данни
        /// - вмъкнат в блок try/catch
        /// - Ако има грешка от страна на базата данни, отпечатва грешка, ако не, запазва въведените стойности, накрая
        /// затваряме връзката.

        /// </summary>
        public void InsertData()
        {

            using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\george\Desktop\Archiver (2)\Archiver\Archiver-Beta\Archiver-Beta\Databases\Receipts.mdf"";Integrated Security=True"))
            {
                //използаме нашите любимци try - catch - finally 
                //0_0
                try
                {
                    //Отваряме връзка
                    connection.Open();
                    //Използваме sql скрипт, който ще записва бележки към базата от данни
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO [Table] (ShopName, PurchaseType, Items, Amount, Payment, Date) VALUES (@ShopName, @PurchaseType, @Items, @Amount, @Payment, @Date)", connection))
                    {
                        //Запълва SHOPNAME.TEXT -> @SHOPNAME (КОЛОНА)
                        cmd.Parameters.AddWithValue("@ShopName", ShopNameBox.Text);
                        //Запълва PurchaseType.TEXT -> @PurchaseType (КОЛОНА)
                        cmd.Parameters.AddWithValue("@PurchaseType", PurchaseBox.Text);
                        //Запълва Items.TEXT -> @Items (КОЛОНА)
                        cmd.Parameters.AddWithValue("@Items", TotalBox.Text);
                        //Запълва Amount.TEXT -> @Amount (КОЛОНА)
                        cmd.Parameters.AddWithValue("@Amount", decimal.Parse(AmountBox.Text));
                        //Запълва Payment.TEXT -> @Payment (КОЛОНА)
                        cmd.Parameters.AddWithValue("@Payment", payment);
                        //Запълва DateTimePicker1.Value -> @Date (КОЛОНА)
                        cmd.Parameters.AddWithValue("@Date", dateTimePicker1.Value);
                        cmd.ExecuteNonQuery();

                    }
                    MessageBox.Show("The receipt has been added!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);


                }
                //Ако има грешка някъде по кода, ще хване грешките тук
                catch (SqlException ex)
                {

                    MessageBox.Show($"The receipt has not been added! There is an error with the connection! {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    //затваряме връзката
                    connection.Close();
                }
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //Форматираме си формата на датата 
            dateTimePicker1.CustomFormat = "dd/mm/yyyy";
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back)
            {
                dateTimePicker1.CustomFormat = "";

            }
        }
        //Зареждаме данните от базата данни към DataViewer
        public void LoadData()
        {
            //Loading data from sql
            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\george\Desktop\Archiver (2)\Archiver\Archiver-Beta\Archiver-Beta\Databases\Receipts.mdf"";Integrated Security=True");
            connection.Open();
            DataTable receipts = new DataTable();

            //Запълваме Viewer с помощта на скрипт
            SqlDataAdapter query = new SqlDataAdapter(@"SELECT * FROM [TABLE]", connection);
            query.Fill(receipts);

            ModifyDataViewer.DataSource = receipts;
            //Конвертираме Сумата в бг лева - лев
            ModifyDataViewer.Columns["Amount"].DefaultCellStyle.Format = "C";

            ModifyDataViewer.Columns["Amount"].DefaultCellStyle.FormatProvider = new CultureInfo("bg-BG");
            connection.Close();

        }
        //инстанции
        public static DataGridViewRow selectedrow;
        private HomeForm homeform;

        private void CheckAndCleanTextBoxes()
        {
            // Ако всички текстови полета са попълнени, можем да изтрием стойностите им, ако са празни, не можем да ги изтрием, защото няма данни
            // проверява дали всички текстови полета са попълнени и дали е избран един от радиобутоните

            if (!string.IsNullOrEmpty(ShopNameBox.Text) || !string.IsNullOrEmpty(PurchaseBox.Text) || !string.IsNullOrEmpty(AmountBox.Text) || !string.IsNullOrEmpty(TotalBox.Text) || !string.IsNullOrEmpty(IdBox.Text) || CashRadioButton.Checked || DebitCreditRadioButton.Checked)
            {
                ShopNameBox.Clear();
                PurchaseBox.Clear();
                AmountBox.Clear();
                TotalBox.Clear();
                IdBox.Clear();
                DebitCreditRadioButton.Checked = false;
                CashRadioButton.Checked = false;

            }
            else
            {
                MessageBox.Show("There are not data to be cleaned", "Infomation", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void ClearButton_Click_1(object sender, EventArgs e)
        {
            //Извикваме си метода
            CheckAndCleanTextBoxes();
        }
        //Метод, който актуализира бележките от базата от данни
        private void UpdateData()
        {
            //Трябва да сме селектирали ред от таблицата
            if (ModifyDataViewer.SelectedRows.Count > 0)
            {
                //Задаваме стойност на радиобутоните пак
                if (DebitCreditRadioButton.Checked == true)
                {
                    payment = "Debit/Credit card";
                }
                else if (CashRadioButton.Checked == true)
                {
                    payment = "Cash";
                }
                //Правим връзка, работи аналогично като първия метод за вмъкванена да данни
                using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\george\Desktop\Archiver (2)\Archiver\Archiver-Beta\Archiver-Beta\Databases\Receipts.mdf"";Integrated Security=True"))
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
            else
            {
                MessageBox.Show("There are no changes!");
            }
        }

        private void ModifyDataViewer_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //При селектиране на ред от таблицата, взима данните от там и ги разпределя по правилните текстови полета
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
            //За да извикаме метода Updata трябва да сме селектирали бележката, която желаем да редактираме
            if (ModifyDataViewer.SelectedRows.Count > 0)
            {
                //Зареждаме медотите си 
                UpdateData();
                //След запазваме на бележката, актуализираме наново базата от данни
                LoadData();

                ModifyDataViewer.ClearSelection();
                //Изчистваме всичко след това 
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
        //Метод за изтриване на бележка
        private void DeleteRecord()
        {
            //Пита потребителя дали е съгласен да изтрие бележка, като извиква използваме MessageBox.
            //Само ако сме селектирали вече ред /бележка/ от касовата бележка
            if (ModifyDataViewer.SelectedRows.Count > 0)
            {
                string message = "Are you sure you want to delete the receipt?";
                string title = "Delete Receipt";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                MessageBoxIcon icon = MessageBoxIcon.Question;
                //Ако цъкнем Yes, ще се установи нова връзка към базата от данни, ще използва Script , който ще изтрие дадения ред от базата от данни
                if (DialogResult.Yes == MessageBox.Show(message, title, buttons, icon))
                {
                    SqlConnection connecton = null;
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\george\Desktop\Archiver (2)\Archiver\Archiver-Beta\Archiver-Beta\Databases\Receipts.mdf"";Integrated Security=True"))
                        {
                            connection.Open();
                            //скрипта
                            string query = "DELETE FROM [TABLE] WHERE Id=@Id";
                            using (SqlCommand cmd = new SqlCommand(query, connection))
                            {

                                cmd.Parameters.AddWithValue("@Id", int.Parse(IdBox.Text));
                                cmd.ExecuteNonQuery();

                            }
                        }
                        //След изтриването, актуализираме базата от данни
                        LoadData();
                    }
                    //SQLException - за грешки
                    catch (SqlException ex)
                    {
                        //Ако настъпи проблем, ще покаже грешка
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
        //Изтриване на цялата база от данни
        //Работи на същия принцип като горния метод
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
                    connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\george\Desktop\Archiver (2)\Archiver\Archiver-Beta\Archiver-Beta\Databases\Receipts.mdf"";Integrated Security=True");
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
            //Извикваме си бетода
            ClearAllData();
        }
        //При наличието на селектиран ред (бележка) тогава можем да я изтрием (ще извика метода за Delete Record), ако не ще изпише MessageBox
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
            //При стартиране на формата, всички контроли за въвеждане на данни ще са празни!
            //Изчистваме избрания ред от таблицата
            ModifyDataViewer.ClearSelection();
            ShopNameBox.Text = string.Empty;
            PurchaseBox.Text = string.Empty;
            TotalBox.Text = string.Empty;
            AmountBox.Text = string.Empty;
            DebitCreditRadioButton.Checked = false;
            CashRadioButton.Checked = false;
            //изчистваме дататаа
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
            LookReceipt lookReceipt = new LookReceipt();

        }

        private void ViewerReceipButton_Click(object sender, EventArgs e)
        {
            //Ако сме избрали касова бележка (ред) от таблицата, бележката ще се отвори в нова форма и ще покажа нейната информация, а ако не сме 
            //избрали , ще се покаже грешка, която да ни накара да изберем ред
            if (ModifyDataViewer.SelectedRows.Count > 0)
            {
                //инициализираме selectedrow и отваряме нова форма
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
            //Създава bitmap изображение на базата от дани
            Bitmap imagebmp = new Bitmap(ModifyDataViewer.Width, ModifyDataViewer.Height);
            ModifyDataViewer.DrawToBitmap(imagebmp, new Rectangle(0, 0, ModifyDataViewer.Width, ModifyDataViewer.Height));
            e.Graphics.DrawImage(imagebmp, 120, 20);
            //Размери на графиката
        }

        private void PrintrDatabase_Click(object sender, EventArgs e)
        {
            //Принтираме изображението
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.PrintPreviewControl.Zoom = 1;
            printPreviewDialog1.ShowDialog();

        }


        private void FillHelp()
        {
            //Този метод запълва текстовите полета с примерна помощна информация за потребителя
            string shophelp = ":Кауфланд :";
            string purchasehelp = ":Храна:";
            string totalitems = ":5:";
            string amounthelp = ":12.98:";
            CashRadioButton.Checked = true;

            //Защриховаме
            this.ShopNameBox.Text = shophelp;
            this.PurchaseBox.Text = purchasehelp;
            this.AmountBox.Text = amounthelp;
            this.TotalBox.Text = totalitems;
        }

        private void RefreshDatabase()
        {//Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename="C:\Users\george\Desktop\Archiver (2)\Archiver\Archiver-Beta\Archiver-Beta\Databases\Receipts.mdf";Integrated Security=True
            //Рефреш на базата от данни, логиката е същата като при акута
            string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=""C:\Users\george\Desktop\Archiver (2)\Archiver\Archiver-Beta\Archiver-Beta\Databases\Receipts.mdf"";Integrated Security=True";
            string query = "SELECT * FROM [Table]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlDataAdapter dataAdapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                ModifyDataViewer.DataSource = dataTable;
            }
        }

        private void DashboardButton_Click_1(object sender, EventArgs e)
        {
            //Отваряме формата
            Dashboard d1 = new Dashboard();
            d1.ShowDialog();
        }

        private void HelpFillButton_Click(object sender, EventArgs e)
        {
            //Извикваме метода
            FillHelp();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            //Извикваме метода
            RefreshDatabase();
        }
    }
}