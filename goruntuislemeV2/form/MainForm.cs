using goruntuislemeV2.form;
using goruntuislemeV2.enums;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace goruntuislemeV2
{
    public partial class MainForm : Form
    {
        readonly MenuHandler menuHandler = new();
        public static Bitmap originalImage;
        public static Bitmap tempImage;
        public static FilterNames selectedFilter;
        public static SafePictureBox selectedPictureBox;
        public static Panel displayPanel;
        public static SafePictureBox pictureBox1;
        public static SafePictureBox pictureBox2;
        public static RadioButton rbSetNormal;
        public static RadioButton rbSetStratch;



        public MainForm()
        {
            InitializeComponent();
            this.AutoSize = true;
            AddHelpMenu();

            this.Controls.Add(displayPanel);
            initPictureBoxes();
            initRadioButtons();

        }
        static MainForm()
        {
            initDisplaypanel();
         


        }


        

        private void AddHelpMenu()
        {
            MenuStrip menuStrip = new MenuStrip();
            ToolStripMenuItem helpMenuItem = new ToolStripMenuItem("Help");

            helpMenuItem.Click += (s, e) =>
            {
                HelpForm helpForm = new HelpForm();
                helpForm.ShowDialog();
            };

            menuStrip.Items.Add(helpMenuItem);
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);
        }


        private static void initDisplaypanel()
        {
            displayPanel = new Panel();
            displayPanel.SuspendLayout();
            displayPanel.BackColor = Color.Gainsboro;
            displayPanel.Location = new Point(12, 253);
            displayPanel.Name = "displayPanel";
            displayPanel.Size = new Size(1300, 655);
            displayPanel.TabIndex = 1;
            displayPanel.ResumeLayout(false);
        }

        private void initRadioButtons()
        {
            rbSetNormal = new RadioButton();
            rbSetNormal.AutoSize = true;
            rbSetNormal.Location = new Point(302, 178);
            rbSetNormal.Name = "rbNormal";
            rbSetNormal.Size = new Size(96, 29);
            rbSetNormal.TabIndex = 6;
            rbSetNormal.Text = "Normal";
            rbSetNormal.UseVisualStyleBackColor = true;

            rbSetStratch = new RadioButton();
            rbSetStratch.AutoSize = true;
            rbSetStratch.Checked = true;
            rbSetStratch.Location = new Point(211, 178);
            rbSetStratch.Name = "rbStratch";
            rbSetStratch.Size = new Size(91, 29);
            rbSetStratch.TabIndex = 5;
            rbSetStratch.TabStop = true;
            rbSetStratch.Text = "Stratch";
            rbSetStratch.UseVisualStyleBackColor = true;
            rbSetStratch.CheckedChanged += rbStratch_CheckedChanged;

            panel1.Controls.Add(rbSetNormal);
            panel1.Controls.Add(rbSetStratch);

        }
        private void initPictureBoxes()
        {
            pictureBox1 = new SafePictureBox();
            pictureBox2 = new SafePictureBox();

            pictureBox1.BackColor = Color.White;
            pictureBox1.Location = new Point(10, 10);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(635, 635);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            pictureBox1.DoubleClick += selectPictureBox;
            pictureBox1.MouseDown += PictureBox_swipe_up;

            pictureBox2.BackColor = Color.White;
            pictureBox2.Location = new Point(655, 10);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(635, 635);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            pictureBox2.DoubleClick += selectPictureBox;
            pictureBox2.MouseDown += PictureBox_swipe_up;

            MainForm.displayPanel.Controls.Add(pictureBox1);
            MainForm.displayPanel.Controls.Add(pictureBox2);

        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.KeyPreview = true; // Klavye olaylarýný dinlemek için
            this.KeyDown += MainForm_KeyDown;
        }

        private void MainForm_KeyDown(object? sender, KeyEventArgs e)
        {
            // Ctrl + S kýsayolunu kontrol et
            if (e.Control && e.KeyCode == Keys.S)
            {
                SaveImage();
            }
            if (e.Control && e.KeyCode == Keys.F)
            {
                ShowFullResolutionImage();
            }
        }

        private void ShowFullResolutionImage()
        {
            if (selectedPictureBox.Image == null)
            {
                MessageBox.Show("No image selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Create a new form to display the image
            Size size;
            if (MainForm.selectedPictureBox.OriginalResolutionImage != null)
            {
                size = MainForm.selectedPictureBox.OriginalResolutionImage.Size;

            }
            else
            {
                size = new Size(100, 100);
            }

            Form fullResolutionForm = new Form
            {
                Text = "Full Resolution Image",
                AutoScroll = true,
                Size = size

                
            };

            PictureBox fullResolutionPictureBox = new PictureBox
            {
                Image = selectedPictureBox.Image,
                SizeMode = PictureBoxSizeMode.AutoSize // Ensures the image is displayed in full resolution
            };

            fullResolutionForm.Controls.Add(fullResolutionPictureBox);
            fullResolutionForm.Show();
        }
        private void SaveImage()
        {
            if (MainForm.originalImage == null)
            {
                MessageBox.Show("No image to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
                saveFileDialog.Title = "Save Image";
                saveFileDialog.FileName = "image";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Resmi seçilen formatta kaydet
                    string fileExtension = Path.GetExtension(saveFileDialog.FileName).ToLower();
                    ImageFormat format = fileExtension switch
                    {
                        ".jpg" => ImageFormat.Jpeg,
                        ".bmp" => ImageFormat.Bmp,
                        _ => ImageFormat.Png, // Varsayýlan PNG
                    };

                    try
                    {
                       MainForm.selectedPictureBox.OriginalResolutionImage.Save(saveFileDialog.FileName, format);

                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Image couldn't saved!");
                    }
                    MessageBox.Show("Image saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }


        private void MainForm_Load(object sender, EventArgs e)
        {
            menuHandler.FillComboBox(this.filtersComboBox, typeof(FilterNames));

            selectPictureBox(pictureBox1, new EventArgs());


        }

        private void thumbnailPictureBox_DoubleClick(object sender, EventArgs e)
        {
            originalImage = menuHandler.GetImage();
            this.thumbnailPictureBox.Image = originalImage;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (thumbnailPictureBox.Image == null)
            {
                string yazi = "Double-click\n  to open\n   a new\n   image";
                Font font = new Font("Arial", 15, FontStyle.Italic);
                SizeF yaziBoyutu = e.Graphics.MeasureString(yazi, font);

                // Center the text  
                float x = (thumbnailPictureBox.Width - yaziBoyutu.Width) / 2;
                float y = (thumbnailPictureBox.Height - yaziBoyutu.Height) / 2;

                e.Graphics.DrawString(yazi, font, Brushes.Gray, x, y);
            }
        }


        private void PictureBox_swipe_up(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                PictureBox pictureBox = (PictureBox)sender;
                tempImage = originalImage;
                if (pictureBox.Image != null)
                {
                    Bitmap image = new Bitmap(pictureBox.Image);
                    originalImage = image;
                    thumbnailPictureBox.Image = image;
                    thumbnailTemp.Image = tempImage;
                }
            }
        }

        private void filtersComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            selectedFilter = (FilterNames)filtersComboBox.SelectedItem;
            menuHandler.SetOptionsMenu(selectedFilter, optionsPanel);

        }

        private void selectPictureBox(object sender, EventArgs e)
        {
            SafePictureBox pictureBox = (SafePictureBox)sender;

            if (selectedPictureBox != null)
            {
                selectedPictureBox.BorderStyle = BorderStyle.None;
                selectedPictureBox.isSelected = false;
            }
           
            selectedPictureBox = pictureBox;
            selectedPictureBox.isSelected = true;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
        }

        private void toggleTemp(object sender, EventArgs e)
        {
            Bitmap tmp = tempImage;
            tempImage = originalImage;
            originalImage = tmp;

            thumbnailPictureBox.Image = originalImage;
            thumbnailTemp.Image = tempImage;

        }


        public static void ResetPictureBoxes()
        {
            pictureBox1.Location = new Point(10, 10);
            pictureBox1.Size = new Size(635, 635);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.Location = new Point(655, 10);
            pictureBox2.Size = new Size(635, 635);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        public static void SetStratch()
        {
            MainForm.displayPanel.Controls.Clear();

            MainForm.displayPanel.Controls.Add(pictureBox1);
            MainForm.displayPanel.Controls.Add(pictureBox2);


            ResetPictureBoxes();

        }


        public static void SetNormal()
        {
            

            MainForm.displayPanel.Controls.Clear();

            SafePictureBox[] pictureBoxes = {pictureBox1, pictureBox2 };

            foreach (SafePictureBox pictureBox in pictureBoxes)
            {
                Panel scrollPanel = new Panel();
                scrollPanel.AutoScroll = true;
                scrollPanel.Size = pictureBox.Size;
                scrollPanel.Location = pictureBox.Location;
                pictureBox.Location = new Point(0, 0);
                scrollPanel.BorderStyle = BorderStyle.FixedSingle;
                pictureBox.SizeMode = PictureBoxSizeMode.Normal;

                if(pictureBox.OriginalResolutionImage != null)
                {
                    pictureBox.Size = pictureBox.OriginalResolutionImage.Size;
                    pictureBox.Image = pictureBox.OriginalResolutionImage;
                }
                

                scrollPanel.Controls.Add(pictureBox);
                MainForm.displayPanel.Controls.Add(scrollPanel);
            }




        }

        private static void rbStratch_CheckedChanged(object sender, EventArgs e)
        {
            if(rbSetStratch.Checked)
            {
                SetStratch();
             
                
            }
            else if (rbSetNormal.Checked)
            {
                SetNormal();
            }
        }
    }
}
