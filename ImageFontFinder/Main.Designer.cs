namespace ImageFontFinder
{
    partial class Main
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
            this.buttonLoadImage = new System.Windows.Forms.Button();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxOriginal = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanelResult = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBoxOriginalCrop = new System.Windows.Forms.PictureBox();
            this.pictureBoxGenerated = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).BeginInit();
            this.tableLayoutPanelResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginalCrop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGenerated)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonLoadImage
            // 
            this.buttonLoadImage.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonLoadImage.Location = new System.Drawing.Point(10, 11);
            this.buttonLoadImage.Margin = new System.Windows.Forms.Padding(10);
            this.buttonLoadImage.Name = "buttonLoadImage";
            this.buttonLoadImage.Size = new System.Drawing.Size(139, 38);
            this.buttonLoadImage.TabIndex = 0;
            this.buttonLoadImage.Text = "Load Image";
            this.buttonLoadImage.UseVisualStyleBackColor = true;
            this.buttonLoadImage.Click += new System.EventHandler(this.buttonLoadImage_Click);
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelMain.Controls.Add(this.buttonLoadImage, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.pictureBoxOriginal, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.tableLayoutPanelResult, 0, 2);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(776, 624);
            this.tableLayoutPanelMain.TabIndex = 1;
            // 
            // pictureBoxOriginal
            // 
            this.pictureBoxOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxOriginal.Location = new System.Drawing.Point(3, 64);
            this.pictureBoxOriginal.Name = "pictureBoxOriginal";
            this.pictureBoxOriginal.Size = new System.Drawing.Size(770, 357);
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
            this.tableLayoutPanelResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelResult.Location = new System.Drawing.Point(3, 427);
            this.tableLayoutPanelResult.Name = "tableLayoutPanelResult";
            this.tableLayoutPanelResult.RowCount = 2;
            this.tableLayoutPanelResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelResult.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelResult.Size = new System.Drawing.Size(770, 194);
            this.tableLayoutPanelResult.TabIndex = 2;
            // 
            // pictureBoxOriginalCrop
            // 
            this.pictureBoxOriginalCrop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxOriginalCrop.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxOriginalCrop.Name = "pictureBoxOriginalCrop";
            this.pictureBoxOriginalCrop.Size = new System.Drawing.Size(764, 91);
            this.pictureBoxOriginalCrop.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxOriginalCrop.TabIndex = 0;
            this.pictureBoxOriginalCrop.TabStop = false;
            // 
            // pictureBoxGenerated
            // 
            this.pictureBoxGenerated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxGenerated.Location = new System.Drawing.Point(3, 100);
            this.pictureBoxGenerated.Name = "pictureBoxGenerated";
            this.pictureBoxGenerated.Size = new System.Drawing.Size(764, 91);
            this.pictureBoxGenerated.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxGenerated.TabIndex = 1;
            this.pictureBoxGenerated.TabStop = false;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 624);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Name = "Main";
            this.Text = "Image Font Finder";
            this.tableLayoutPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).EndInit();
            this.tableLayoutPanelResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginalCrop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGenerated)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonLoadImage;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
		private System.Windows.Forms.PictureBox pictureBoxOriginal;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelResult;
        private System.Windows.Forms.PictureBox pictureBoxGenerated;
        private System.Windows.Forms.PictureBox pictureBoxOriginalCrop;
    }
}

