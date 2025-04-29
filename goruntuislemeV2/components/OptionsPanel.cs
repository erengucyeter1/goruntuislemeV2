using goruntuislemeV2.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace goruntuislemeV2.components
{
    internal abstract class OptionsPanel : Panel
    {
        public Button ApplyButton { get; private set; } = new();

        internal bool ImageRequired { get; set; } = true;
        public OptionsPanel()
        {
            this.Size = new Size(896, 200);
            this.Location = new Point(0, 0);
            this.BorderStyle = BorderStyle.FixedSingle;

            CreateApplyButton();

            InitializeComponents();
        }
        internal virtual void InitializeComponents()
        {
            this.AutoSize = true;

        }
        private void CreateApplyButton()
        {
            ApplyButton.Text = "Apply";
            ApplyButton.Location = new Point(790, 155);
            ApplyButton.Size = new Size(100, 40);
            ApplyButton.BackColor = Color.LightGray;
            ApplyButton.Click += new EventHandler(Apply!);
            this.Controls.Add(ApplyButton);
        }

        internal async void  Apply(object? sender, EventArgs e)
        {
            
            if (ImageRequired && MainForm.originalImage == null)
            {
                MessageBox.Show("Lütfen bir resim seçin.");
                return;
            }

            MainForm.selectedPictureBox.Image = Filters.StringToBitmap("\n\n\n      Processing...", 32);

            Bitmap processed = await ApplyFilter();

            DisplayImage(processed);
        }

        private void DisplayImage(Bitmap img)
        {
            if( img == null)
            {
                MessageBox.Show("Resim işlenemedi.");
                return;
            }
            if (MainForm.selectedPictureBox.SizeMode == PictureBoxSizeMode.Normal)
            {
                MainForm.selectedPictureBox.Size = img.Size;
            }

            if (MainForm.pictureBox1.isSelected)
            {
                MainForm.pictureBox1.OriginalResolutionImage = img;
            }
            else
            {
                MainForm.pictureBox2.OriginalResolutionImage = img;
            }

            


            MainForm.selectedPictureBox.Image = img;
        }

        internal abstract Task<Bitmap> ApplyFilter();



    }
}
