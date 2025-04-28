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
            label1 = new Label();
            thumbnailTemp = new PictureBox();
            optionsPanel = new Panel();
            filtersComboBox = new ComboBox();
            thumbnailPictureBox = new PictureBox();
            displayPanel = new Panel();
            pictureBox2 = new PictureBox();
            pictureBox1 = new PictureBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)thumbnailTemp).BeginInit();
            ((System.ComponentModel.ISupportInitialize)thumbnailPictureBox).BeginInit();
            displayPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Gainsboro;
            panel1.Controls.Add(label1);
            panel1.Controls.Add(thumbnailTemp);
            panel1.Controls.Add(optionsPanel);
            panel1.Controls.Add(filtersComboBox);
            panel1.Controls.Add(thumbnailPictureBox);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(1300, 210);
            panel1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(211, 47);
            label1.Name = "label1";
            label1.Size = new Size(54, 25);
            label1.TabIndex = 4;
            label1.Text = "temp";
            // 
            // thumbnailTemp
            // 
            thumbnailTemp.Location = new Point(211, 75);
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
            // displayPanel
            // 
            displayPanel.BackColor = Color.Gainsboro;
            displayPanel.Controls.Add(pictureBox2);
            displayPanel.Controls.Add(pictureBox1);
            displayPanel.Location = new Point(12, 228);
            displayPanel.Name = "displayPanel";
            displayPanel.Size = new Size(1300, 635);
            displayPanel.TabIndex = 1;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.White;
            pictureBox2.Location = new Point(655, 10);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(635, 635);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            pictureBox2.DoubleClick += selectPictureBox;
            pictureBox2.MouseDown += PictureBox_swipe_up;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.White;
            pictureBox1.Location = new Point(10, 10);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(635, 635);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.DoubleClick += selectPictureBox;
            pictureBox1.MouseDown += PictureBox_swipe_up;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1321, 877);
            Controls.Add(displayPanel);
            Controls.Add(panel1);
            Name = "MainForm";
            Text = "Home";
            Load += MainForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)thumbnailTemp).EndInit();
            ((System.ComponentModel.ISupportInitialize)thumbnailPictureBox).EndInit();
            displayPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private ComboBox filtersComboBox;
        private PictureBox thumbnailPictureBox;
        private Panel optionsPanel;
        private Panel displayPanel;
        private PictureBox pictureBox2;
        private PictureBox pictureBox1;
        private PictureBox thumbnailTemp;
        private Label label1;
    }
}
