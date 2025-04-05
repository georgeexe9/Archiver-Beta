namespace Archiver_Beta
{
    partial class LookReceipt
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LookReceipt));
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("Coming soon, I promise :)");
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.labelID = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.ReceiptID = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ViewerShop = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.PurchaseViewer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ItemsViewer = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.AmountViewer = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ListItems = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CloseButton3 = new System.Windows.Forms.Button();
            this.DateLabel = new System.Windows.Forms.Label();
            this.DateViewer = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ViewerPayment = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.pictureBox6);
            this.panel1.Controls.Add(this.labelID);
            this.panel1.Controls.Add(this.label21);
            this.panel1.Controls.Add(this.ReceiptID);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(-1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(592, 55);
            this.panel1.TabIndex = 23;
            // 
            // pictureBox6
            // 
            this.pictureBox6.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox6.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox6.Image")));
            this.pictureBox6.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBox6.InitialImage")));
            this.pictureBox6.Location = new System.Drawing.Point(3, 3);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(63, 48);
            this.pictureBox6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox6.TabIndex = 28;
            this.pictureBox6.TabStop = false;
            // 
            // labelID
            // 
            this.labelID.AutoSize = true;
            this.labelID.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.labelID.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelID.ForeColor = System.Drawing.Color.Transparent;
            this.labelID.Location = new System.Drawing.Point(397, 12);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(35, 30);
            this.labelID.TabIndex = 25;
            this.labelID.Text = "ID";
            this.labelID.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.BackColor = System.Drawing.Color.Transparent;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(956, 62);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(49, 9);
            this.label21.TabIndex = 15;
            this.label21.Text = "Version 1.0.0";
            // 
            // ReceiptID
            // 
            this.ReceiptID.AutoSize = true;
            this.ReceiptID.BackColor = System.Drawing.Color.Transparent;
            this.ReceiptID.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ReceiptID.ForeColor = System.Drawing.Color.Transparent;
            this.ReceiptID.Location = new System.Drawing.Point(72, 10);
            this.ReceiptID.Name = "ReceiptID";
            this.ReceiptID.Size = new System.Drawing.Size(319, 32);
            this.ReceiptID.TabIndex = 24;
            this.ReceiptID.Text = "Information about receipt:";
            this.ReceiptID.Click += new System.EventHandler(this.ReceiptID_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(857, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 13);
            this.label3.TabIndex = 2;
            // 
            // ViewerShop
            // 
            this.ViewerShop.HideSelection = false;
            this.ViewerShop.Location = new System.Drawing.Point(113, 94);
            this.ViewerShop.Multiline = true;
            this.ViewerShop.Name = "ViewerShop";
            this.ViewerShop.ReadOnly = true;
            this.ViewerShop.Size = new System.Drawing.Size(166, 26);
            this.ViewerShop.TabIndex = 25;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(35, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 26);
            this.label4.TabIndex = 24;
            this.label4.Text = "Shop name:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PurchaseViewer
            // 
            this.PurchaseViewer.HideSelection = false;
            this.PurchaseViewer.Location = new System.Drawing.Point(113, 128);
            this.PurchaseViewer.Multiline = true;
            this.PurchaseViewer.Name = "PurchaseViewer";
            this.PurchaseViewer.ReadOnly = true;
            this.PurchaseViewer.Size = new System.Drawing.Size(166, 26);
            this.PurchaseViewer.TabIndex = 27;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(12, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 26);
            this.label2.TabIndex = 26;
            this.label2.Text = "Purchase Type:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ItemsViewer
            // 
            this.ItemsViewer.HideSelection = false;
            this.ItemsViewer.Location = new System.Drawing.Point(113, 165);
            this.ItemsViewer.Multiline = true;
            this.ItemsViewer.Name = "ItemsViewer";
            this.ItemsViewer.ReadOnly = true;
            this.ItemsViewer.Size = new System.Drawing.Size(166, 26);
            this.ItemsViewer.TabIndex = 29;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(35, 161);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 26);
            this.label5.TabIndex = 28;
            this.label5.Text = "Total Items:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AmountViewer
            // 
            this.AmountViewer.HideSelection = false;
            this.AmountViewer.Location = new System.Drawing.Point(113, 197);
            this.AmountViewer.Multiline = true;
            this.AmountViewer.Name = "AmountViewer";
            this.AmountViewer.ReadOnly = true;
            this.AmountViewer.Size = new System.Drawing.Size(119, 26);
            this.AmountViewer.TabIndex = 30;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(46, 197);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 26);
            this.label6.TabIndex = 31;
            this.label6.Text = "Amount:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ListItems
            // 
            this.ListItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.ListItems.HideSelection = false;
            this.ListItems.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.ListItems.Location = new System.Drawing.Point(15, 19);
            this.ListItems.Name = "ListItems";
            this.ListItems.Size = new System.Drawing.Size(223, 137);
            this.ListItems.TabIndex = 32;
            this.ListItems.UseCompatibleStateImageBehavior = false;
            // 
            // CloseButton3
            // 
            this.CloseButton3.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.CloseButton3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CloseButton3.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.CloseButton3.FlatAppearance.BorderSize = 0;
            this.CloseButton3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CloseButton3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CloseButton3.ForeColor = System.Drawing.Color.White;
            this.CloseButton3.Location = new System.Drawing.Point(479, 243);
            this.CloseButton3.Name = "CloseButton3";
            this.CloseButton3.Size = new System.Drawing.Size(83, 28);
            this.CloseButton3.TabIndex = 33;
            this.CloseButton3.Text = "Close";
            this.CloseButton3.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.CloseButton3.UseVisualStyleBackColor = false;
            this.CloseButton3.Click += new System.EventHandler(this.CloseButton3_Click);
            // 
            // DateLabel
            // 
            this.DateLabel.AutoSize = true;
            this.DateLabel.BackColor = System.Drawing.Color.Transparent;
            this.DateLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DateLabel.ForeColor = System.Drawing.Color.White;
            this.DateLabel.Location = new System.Drawing.Point(12, 67);
            this.DateLabel.Name = "DateLabel";
            this.DateLabel.Size = new System.Drawing.Size(81, 15);
            this.DateLabel.TabIndex = 35;
            this.DateLabel.Text = "Receipt date:";
            // 
            // DateViewer
            // 
            this.DateViewer.AutoSize = true;
            this.DateViewer.BackColor = System.Drawing.Color.Transparent;
            this.DateViewer.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DateViewer.ForeColor = System.Drawing.Color.White;
            this.DateViewer.Location = new System.Drawing.Point(110, 65);
            this.DateViewer.Name = "DateViewer";
            this.DateViewer.Size = new System.Drawing.Size(36, 17);
            this.DateViewer.TabIndex = 36;
            this.DateViewer.Text = "Date";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(46, 236);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 26);
            this.label1.TabIndex = 37;
            this.label1.Text = "Payment:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ViewerPayment
            // 
            this.ViewerPayment.HideSelection = false;
            this.ViewerPayment.Location = new System.Drawing.Point(113, 233);
            this.ViewerPayment.Multiline = true;
            this.ViewerPayment.Name = "ViewerPayment";
            this.ViewerPayment.ReadOnly = true;
            this.ViewerPayment.Size = new System.Drawing.Size(119, 26);
            this.ViewerPayment.TabIndex = 38;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Transparent;
            this.groupBox1.Controls.Add(this.ListItems);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(307, 67);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(255, 167);
            this.groupBox1.TabIndex = 39;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Items: (Beta)";
            // 
            // LookReceipt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(574, 283);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.CloseButton3);
            this.Controls.Add(this.ViewerPayment);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DateViewer);
            this.Controls.Add(this.DateLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.AmountViewer);
            this.Controls.Add(this.ItemsViewer);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.PurchaseViewer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ViewerShop);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(590, 322);
            this.MinimumSize = new System.Drawing.Size(590, 322);
            this.Name = "LookReceipt";
            this.Text = "Receipt(Beta)";
            this.Load += new System.EventHandler(this.LookReceipt_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label ReceiptID;
        private System.Windows.Forms.Label labelID;
        public System.Windows.Forms.TextBox ViewerShop;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox PurchaseViewer;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox ItemsViewer;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox AmountViewer;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.ListView ListItems;
        private System.Windows.Forms.Button CloseButton3;
        private System.Windows.Forms.Label DateLabel;
        private System.Windows.Forms.Label DateViewer;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox ViewerPayment;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
    }
}