namespace goruntuislemeV2
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();

            thumbnailTemp = new PictureBox();
            optionsPanel = new Panel();
            filtersComboBox = new ComboBox();
            thumbnailPictureBox = new PictureBox();
    
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)thumbnailTemp).BeginInit();
            ((System.ComponentModel.ISupportInitialize)thumbnailPictureBox).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Gainsboro;

            panel1.Controls.Add(thumbnailTemp);
            panel1.Controls.Add(optionsPanel);
            panel1.Controls.Add(filtersComboBox);
            panel1.Controls.Add(thumbnailPictureBox);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(1300, 210);
            panel1.TabIndex = 0;
            // 
            // rbNormal
            // 
            
            // 
            // rbStratch
            // 
            
            // 
            // thumbnailTemp
            // 
            thumbnailTemp.Location = new Point(211, 42);
            thumbnailTemp.Name = "thumbnailTemp";
            thumbnailTemp.Size = new Size(130, 130);
            thumbnailTemp.SizeMode = PictureBoxSizeMode.StretchImage;
            thumbnailTemp.TabIndex = 3;
            thumbnailTemp.TabStop = false;
            thumbnailTemp.DoubleClick += toggleTemp;
            // 
            // optionsPanel
            // 
            optionsPanel.Location = new Point(399, 5);
            optionsPanel.Name = "optionsPanel";
            optionsPanel.Size = new Size(896, 200);
            optionsPanel.TabIndex = 2;
            // 
            // filtersComboBox
            // 
            filtersComboBox.FormattingEnabled = true;
            filtersComboBox.Location = new Point(211, 5);
            filtersComboBox.Name = "filtersComboBox";
            filtersComboBox.Size = new Size(182, 33);
            filtersComboBox.TabIndex = 1;
            filtersComboBox.SelectedIndexChanged += filtersComboBox_SelectedIndexChanged;
            // 
            // thumbnailPictureBox
            // 
            thumbnailPictureBox.Location = new Point(5, 5);
            thumbnailPictureBox.Name = "thumbnailPictureBox";
            thumbnailPictureBox.Size = new Size(200, 200);
            thumbnailPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            thumbnailPictureBox.TabIndex = 0;
            thumbnailPictureBox.TabStop = false;
            thumbnailPictureBox.Paint += pictureBox1_Paint;
            thumbnailPictureBox.DoubleClick += thumbnailPictureBox_DoubleClick;
            // 
            // pictureBox2
            // 
            
            // 
            // pictureBox1
            // 
            
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1321, 877);
            Controls.Add(panel1);
            Name = "MainForm";
            Text = "Home";
            Load += MainForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)thumbnailTemp).EndInit();
            ((System.ComponentModel.ISupportInitialize)thumbnailPictureBox).EndInit();
      
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private ComboBox filtersComboBox;
        private PictureBox thumbnailPictureBox;
        private Panel optionsPanel;
        
        private PictureBox thumbnailTemp;

        
    }
}
