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


namespace Archiver_Beta
{
    public partial class ViewUI : Form

    {

        //SqlConnection connection = new SqlConnection(@"Data Source=(LocaIDB)\MSSQLLocaIDB.C:\USERS\GEORGE\ONEDRIVE\PROJECTS\ARCHIVER\ARCHIVER-BETA\ARCHIVER-BETA\DATABASES\ARCHIVERDATABASE.MDF;Integrated Security=Frue;Connect Timeout=30");
        public ViewUI()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            LoadData();
            InitializeComponent();
            //Инициализираме си метода за зареждане на базата от данни в data grid формичката
            
        }


        private void AddButton_Click(object sender, EventArgs e)
        {
            AddFormUI addform = new AddFormUI();
            addform.ShowDialog();
        }

        private void CloseButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void LoadData()
        {
            //Отваряме нов конекшък към базата от данни - path
            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\george\OneDrive\Projects\Archiver\Archiver-Beta\Archiver-Beta\Databases\ArchiverDataBase.mdf;Integrated Security=True");
            connection.Open();
            DataTable receipts = new DataTable();

            //SQL скрипт
            SqlDataAdapter query = new SqlDataAdapter(@"SELECT * FROM [TABLE]", connection);
            query.Fill(receipts);

            ReceiptDataViewer.DataSource = receipts;
            connection.Close();
            LoadData();
        }
        






    }
}

