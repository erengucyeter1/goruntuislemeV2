using goruntuislemeV2.utils;

namespace goruntuislemeV2.components
{
    internal class ThresholdPanel : OptionsPanel
    {
        private int ThresoldVal { get; set; } = 128;

        internal override void InitializeComponents()
        {
            Label thresholdLabel = new Label();
            thresholdLabel.Text = "Threshold Value";
            thresholdLabel.Location = new Point(10, 10);
            thresholdLabel.Size = new Size(100, 30);
            this.Controls.Add(thresholdLabel);


            NumericUpDown thresholdInput = new NumericUpDown();
            thresholdInput.Minimum = 0;
            thresholdInput.Maximum = 255;
            thresholdInput.DecimalPlaces = 0;
            thresholdInput.Increment = 1;
            thresholdInput.Value = ThresoldVal;
            thresholdInput.Location = new Point(10, 40);
            thresholdInput.Size = new Size(100, 30);
            thresholdInput.ValueChanged += (s, e) =>
            {
                ThresoldVal = (int)thresholdInput.Value;
            };
            this.Controls.Add(thresholdInput);
            
           

        }
        internal override Bitmap ApplyFilter()
        {
            return Filters.Binarize(MainForm.originalImage, ThresoldVal); // 128 eşik değeri örnek
        }
    }
}
