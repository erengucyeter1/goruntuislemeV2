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
        public static PictureBox selectedPictureBox;
        public static PictureBox[] pictureBoxes = new PictureBox[2];

        public MainForm()
        {
            InitializeComponent();
            pictureBoxes[0] = pictureBox1;
            pictureBoxes[1] = pictureBox2;
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
            if(e.Control && e.KeyCode == Keys.F)
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
            Form fullResolutionForm = new Form
            {
                Text = "Full Resolution Image",
                AutoScroll = true,
                Size = new Size(800, 600) // Default size, can be adjusted
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
                        MainForm.originalImage.Save(saveFileDialog.FileName, format);

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
            PictureBox pictureBox = (PictureBox)sender;

            if (selectedPictureBox != null)
            {
                selectedPictureBox.BorderStyle = BorderStyle.None;
            }
            selectedPictureBox = pictureBox;
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
    }
}
