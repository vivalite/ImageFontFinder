namespace ImageFontFinder
{
    sealed partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxOriginal = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanelResult = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxGenerated = new System.Windows.Forms.PictureBox();
            this.pictureBoxOriginalCrop = new System.Windows.Forms.PictureBox();
            this.labelFontInfo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelCurrentFontNum = new System.Windows.Forms.Label();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonTest = new System.Windows.Forms.Button();
            this.buttonLoadImage = new System.Windows.Forms.Button();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).BeginInit();
            this.tableLayoutPanelResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGenerated)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginalCrop)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.pictureBoxOriginal, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelResult, 0, 2);
            this.tableLayoutPanelMain.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(913, 624);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // pictureBoxOriginal
            // 
            this.pictureBoxOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxOriginal.Location = new System.Drawing.Point(3, 64);
            this.pictureBoxOriginal.Name = "pictureBoxOriginal";
            this.pictureBoxOriginal.Size = new System.Drawing.Size(907, 357);
            this.pictureBoxOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxOriginal.TabIndex = 1;
            this.pictureBoxOriginal.TabStop = false;
            this.pictureBoxOriginal.Click += new System.EventHandler(this.pictureBoxOriginal_Click);
            // 
            // tableLayoutPanelResult
            // 
            this.tableLayoutPanelResult.ColumnCount = 1;
            this.tableLayoutPanelResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelResult.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelResult.Controls.Add(this.pictureBoxGenerated, 0, 1);
            this.tableLayoutPanelResult.Controls.Add(this.pictureBoxOriginalCrop, 0, 0);
            this.tableLayoutPanelResult.Controls.Add(this.labelFontInfo, 0, 2);
            this.tableLayoutPanelResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelResult.Location = new System.Drawing.Point(3, 427);
            this.tableLayoutPanelResult.Name = "tableLayoutPanelResult";
            this.tableLayoutPanelResult.RowCount = 3;
            this.tableLayoutPanelResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanelResult.Size = new System.Drawing.Size(907, 194);
            this.tableLayoutPanelResult.TabIndex = 2;
            // 
            // pictureBoxGenerated
            // 
            this.pictureBoxGenerated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxGenerated.Location = new System.Drawing.Point(3, 80);
            this.pictureBoxGenerated.Name = "pictureBoxGenerated";
            this.pictureBoxGenerated.Size = new System.Drawing.Size(901, 71);
            this.pictureBoxGenerated.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxGenerated.TabIndex = 1;
            this.pictureBoxGenerated.TabStop = false;
            // 
            // pictureBoxOriginalCrop
            // 
            this.pictureBoxOriginalCrop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxOriginalCrop.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxOriginalCrop.Name = "pictureBoxOriginalCrop";
            this.pictureBoxOriginalCrop.Size = new System.Drawing.Size(901, 71);
            this.pictureBoxOriginalCrop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxOriginalCrop.TabIndex = 0;
            this.pictureBoxOriginalCrop.TabStop = false;
            // 
            // labelFontInfo
            // 
            this.labelFontInfo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelFontInfo.AutoSize = true;
            this.labelFontInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.labelFontInfo.Location = new System.Drawing.Point(453, 165);
            this.labelFontInfo.Name = "labelFontInfo";
            this.labelFontInfo.Size = new System.Drawing.Size(0, 18);
            this.labelFontInfo.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelCurrentFontNum);
            this.panel1.Controls.Add(this.buttonDown);
            this.panel1.Controls.Add(this.buttonUp);
            this.panel1.Controls.Add(this.buttonTest);
            this.panel1.Controls.Add(this.buttonLoadImage);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Margin = new System.Windows.Forms.Padding(10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(893, 41);
            this.panel1.TabIndex = 3;
            // 
            // labelCurrentFontNum
            // 
            this.labelCurrentFontNum.AutoSize = true;
            this.labelCurrentFontNum.Location = new System.Drawing.Point(605, 15);
            this.labelCurrentFontNum.Name = "labelCurrentFontNum";
            this.labelCurrentFontNum.Size = new System.Drawing.Size(13, 13);
            this.labelCurrentFontNum.TabIndex = 8;
            this.labelCurrentFontNum.Text = "0";
            // 
            // buttonDown
            // 
            this.buttonDown.Location = new System.Drawing.Point(649, 10);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(75, 23);
            this.buttonDown.TabIndex = 7;
            this.buttonDown.Text = "Down";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Location = new System.Drawing.Point(513, 10);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(75, 23);
            this.buttonUp.TabIndex = 6;
            this.buttonUp.Text = "Up";
            this.buttonUp.UseVisualStyleBackColor = true;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonTest
            // 
            this.buttonTest.Dock = System.Windows.Forms.DockStyle.Right;
            this.buttonTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.buttonTest.Location = new System.Drawing.Point(783, 0);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(110, 41);
            this.buttonTest.TabIndex = 5;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = true;
            this.buttonTest.Visible = false;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest_Click);
            // 
            // buttonLoadImage
            // 
            this.buttonLoadImage.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonLoadImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.buttonLoadImage.Image = global::ImageFontFinder.Properties.Resources.icons8_opened_folder_20;
            this.buttonLoadImage.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonLoadImage.Location = new System.Drawing.Point(0, 0);
            this.buttonLoadImage.Margin = new System.Windows.Forms.Padding(10);
            this.buttonLoadImage.Name = "buttonLoadImage";
            this.buttonLoadImage.Size = new System.Drawing.Size(176, 41);
            this.buttonLoadImage.TabIndex = 4;
            this.buttonLoadImage.Text = "加载并处理图片  ";
            this.buttonLoadImage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonLoadImage.UseVisualStyleBackColor = true;
            this.buttonLoadImage.Click += new System.EventHandler(this.buttonLoadImage_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 624);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.tableLayoutPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).EndInit();
            this.tableLayoutPanelResult.ResumeLayout(false);
            this.tableLayoutPanelResult.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGenerated)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginalCrop)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
		private System.Windows.Forms.PictureBox pictureBoxOriginal;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelResult;
        private System.Windows.Forms.PictureBox pictureBoxGenerated;
        private System.Windows.Forms.PictureBox pictureBoxOriginalCrop;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonTest;
        private System.Windows.Forms.Button buttonLoadImage;
        private System.Windows.Forms.Label labelFontInfo;
        private System.Windows.Forms.Label labelCurrentFontNum;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonUp;
    }
}

