using goruntuislemeV2.utils;

namespace goruntuislemeV2.components
{
    internal class ZoomPanel : OptionsPanel
    {
        ComboBox interpolationMethodCB = new ComboBox();
        NumericUpDown zoomFactorNUD = new NumericUpDown();

        public ZoomPanel()
        {
            if (!MainForm.rbSetNormal.Checked)
            {
                MainForm.rbSetNormal.Checked = true;
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
                    return Filters.BilinearInterpolation(MainForm.originalImage, (int)(MainForm.originalImage.Width * zoomFactor), (int)(MainForm.originalImage.Height * zoomFactor));
                case "Bicubic":
                    return Filters.BicubicInterpolation(MainForm.originalImage, (int)(MainForm.originalImage.Width * zoomFactor), (int)(MainForm.originalImage.Height * zoomFactor));
                default:
                    throw new ArgumentException("Invalid interpolation method");

            }
        }
        



    }
}
