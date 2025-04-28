using goruntuislemeV2.utils;

namespace goruntuislemeV2.components
{
    internal class ZoomPanel : OptionsPanel, IMenuChanger
    {
        ComboBox interpolationMethodCB = new ComboBox();
        NumericUpDown zoomFactorNUD = new NumericUpDown();

        // pictureboxların doldurma yöntemleri değiştirilip geri düzeltilmeşi

        public ZoomPanel()
        {
            foreach(PictureBox pictureBox in MainForm.pictureBoxes)
            {
                pictureBox.SizeMode = PictureBoxSizeMode.Normal;


            }
            
        }



        public void UpdateMainMenu()
        {
            foreach (PictureBox pictureBox in MainForm.pictureBoxes)
            {
                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        internal override void InitializeComponents()
        {
           interpolationMethodCB.Size = new Size(100, 20);
            interpolationMethodCB.Location = new Point(10, 30);
            interpolationMethodCB.DropDownStyle = ComboBoxStyle.DropDownList;
            interpolationMethodCB.Items.Add("Nearest");
            interpolationMethodCB.Items.Add("Bilinear");
            interpolationMethodCB.Items.Add("Bicubic");
            interpolationMethodCB.SelectedIndex = 0;
            this.Controls.Add(interpolationMethodCB);
            Label label = new Label();
            label.Text = "Interpolation Method";
            label.Location = new Point(10, 8);
            label.AutoSize = true;
            this.Controls.Add(label);

            label = new Label();
            label.Text = "Zoom Factor";
            label.Location = new Point(120, 8);

            label.AutoSize = true;
            this.Controls.Add(label);

            zoomFactorNUD.Location = new Point(120, 30);
            zoomFactorNUD.Size = new Size(100, 20);
            zoomFactorNUD.Minimum = 1;
            zoomFactorNUD.Maximum = 10;
            zoomFactorNUD.Value = 2;
            this.Controls.Add(zoomFactorNUD);

        }

        internal override Bitmap ApplyFilter()
        {
            string interpolationMethod = interpolationMethodCB.SelectedItem.ToString();
            decimal zoomFactor = zoomFactorNUD.Value;


            switch (interpolationMethod)
            {
                case "Nearest":
                    return Filters.NearestNeighborInterpolation(MainForm.originalImage, (int)(MainForm.originalImage.Width * zoomFactor), (int)(MainForm.originalImage.Height * zoomFactor));
                case "Bilinear":
                    return null;
                case "Bicubic":
                    return null;
                default:
                    throw new ArgumentException("Invalid interpolation method");

            }
        }

        


    }
}
