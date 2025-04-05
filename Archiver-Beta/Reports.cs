using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace Archiver_Beta
{
    public partial class Reports : Form
    {
        public Reports()
        {
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CloseButton_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ExportButton_Click(object sender, EventArgs e)
        {
            ExporttoExcel();
        }

        private void ExporttoExcel()
        {
            DataTable dt = new DataTable();
            string connect = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\george\Desktop\Archiver (2)\Archiver\Archiver-Beta\Archiver-Beta\Databases\Receipts.mdf;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connect))
            {
                try
                {
                    con.Open();
                    string query = "SELECT * FROM [Table]"; 
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    dt.Load(reader);
                }
                catch (Exception ex)
                {
                    
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ToExcel(dt, saveFileDialog1.FileName);
                this.Close();
            }
        }

        private void ToExcel(DataTable dt, string filename)
        {
            Microsoft.Office.Interop.Excel.Application excel;
            Microsoft.Office.Interop.Excel.Workbook workbook;
            Microsoft.Office.Interop.Excel.Worksheet worksheet;

            try
            {
                excel = new Microsoft.Office.Interop.Excel.Application();
                excel.Visible = false;
                excel.DisplayAlerts = false;
                workbook = excel.Workbooks.Add(Type.Missing);
                worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Sheets["Sheet1"];
                worksheet.Name = "Receipts";

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    worksheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();
                    }
                }
                workbook.SaveAs(filename);
                workbook.Close();
                excel.Quit();
                MessageBox.Show("Export successful!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                workbook = null;
                worksheet = null;
            }
        }

        [Obsolete]
        private void ExportPDF_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string connect = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\george\Desktop\Archiver (2)\Archiver\Archiver-Beta\Archiver-Beta\Databases\Receipts.mdf;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connect))
            {
                try
                {
                    con.Open();
                    string query = "SELECT * FROM [Table]"; 
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    dt.Load(reader);
                }
                catch (Exception ex)
                {
                    
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ToPDF(dt, saveFileDialog1.FileName);
                this.Close();
            }
        }

        [Obsolete]
        private void ToPDF(DataTable dt, string fileName)
        {
            try
            {
                PdfDocument document = new PdfDocument();
                document.Info.Title = "Exported Data";
                PdfPage page = document.AddPage();
                page.Orientation = PdfSharp.PageOrientation.Landscape;
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont font = new XFont("Arial", 10); // Reduced font size

                gfx.DrawString("Exported Receipts", font, XBrushes.Black, new XRect(0, 0, page.Width, 50), XStringFormats.Center);

                double yPoint = 50;
                double xPoint = 0;
                double columnWidth = page.Width / dt.Columns.Count;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    gfx.DrawString(dt.Columns[i].ColumnName, font, XBrushes.Black, new XRect(xPoint, yPoint, columnWidth, 10), XStringFormats.TopLeft);
                    xPoint += columnWidth;
                }
                yPoint += 20;
                xPoint = 0;

                foreach (DataRow row in dt.Rows)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        gfx.DrawString(row[i].ToString(), font, XBrushes.Black, new XRect(xPoint, yPoint, columnWidth, 10), XStringFormats.TopLeft);
                        xPoint += columnWidth;
                    }
                    yPoint += 20;
                    xPoint = 0;
                }
                gfx.DrawString("Made by Receipt Archiver by George Lavchanski!", font, XBrushes.Black, new XRect(0, 0, page.Width, 50), XStringFormats.TopRight);

                document.Save(fileName + ".pdf");
                MessageBox.Show("Export successful!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExportWord();
        }

        private void ExportWord()
        {
            DataTable dt = new DataTable();
            string connect = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\george\Desktop\Archiver (2)\Archiver\Archiver-Beta\Archiver-Beta\Databases\Receipts.mdf;Integrated Security=True";
            using (SqlConnection con = new SqlConnection(connect))
            {
                try
                {
                    con.Open();
                    string query = "SELECT * FROM [Table]"; // Replace with your actual table name
                    SqlCommand cmd = new SqlCommand(query, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    dt.Load(reader);
                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., log it, show a message to the user, etc.)
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    con.Close();
                }
            }
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ToWord(dt, saveFileDialog1.FileName);
                this.Close();
            }
        }

        private void ToWord(DataTable dt, string filename)
        {
            Microsoft.Office.Interop.Word.Application wordApp;
            Microsoft.Office.Interop.Word.Document document;
            Microsoft.Office.Interop.Word.Table table;

            try
            {
                wordApp = new Microsoft.Office.Interop.Word.Application();
                wordApp.Visible = false;
                document = wordApp.Documents.Add();

                Microsoft.Office.Interop.Word.Paragraph titleParagraph = document.Content.Paragraphs.Add();
                titleParagraph.Range.Text = "Exported Receipts";
                titleParagraph.Range.Font.Bold = 1;
                titleParagraph.Format.SpaceAfter = 24;
                titleParagraph.Range.InsertParagraphAfter();

                table = document.Tables.Add(titleParagraph.Range, dt.Rows.Count + 1, dt.Columns.Count);
                table.Borders.Enable = 1;

                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    table.Cell(1, i + 1).Range.Text = dt.Columns[i].ColumnName;
                    table.Cell(1, i + 1).Range.Bold = 1;
                    table.Cell(1, i + 1).Range.Font.Name = "Arial";
                    table.Cell(1, i + 1).Range.Font.Size = 12;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        table.Cell(i + 2, j + 1).Range.Text = dt.Rows[i][j].ToString();
                        table.Cell(i + 2, j + 1).Range.Font.Name = "Arial";
                        table.Cell(i + 2, j + 1).Range.Font.Size = 12;
                    }
                }

                document.SaveAs2(filename);
                document.Close();
                wordApp.Quit();
                MessageBox.Show("Export successful!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                document = null;
                table = null;
            }
        }
    }
}



          