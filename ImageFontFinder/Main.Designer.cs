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
			this.tableLayoutPanelMain.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).BeginInit();
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
			this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
			this.tableLayoutPanelMain.RowCount = 3;
			this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 61F));
			this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.tableLayoutPanelMain.Size = new System.Drawing.Size(776, 464);
			this.tableLayoutPanelMain.TabIndex = 1;
			// 
			// pictureBoxOriginal
			// 
			this.pictureBoxOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBoxOriginal.Location = new System.Drawing.Point(3, 64);
			this.pictureBoxOriginal.Name = "pictureBoxOriginal";
			this.pictureBoxOriginal.Size = new System.Drawing.Size(770, 297);
			this.pictureBoxOriginal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBoxOriginal.TabIndex = 1;
			this.pictureBoxOriginal.TabStop = false;
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(776, 464);
			this.Controls.Add(this.tableLayoutPanelMain);
			this.Name = "Main";
			this.Text = "Image Font Finder";
			this.tableLayoutPanelMain.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxOriginal)).EndInit();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonLoadImage;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
		private System.Windows.Forms.PictureBox pictureBoxOriginal;
	}
}

