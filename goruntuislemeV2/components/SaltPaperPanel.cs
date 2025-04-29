using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using goruntuislemeV2.utils;

namespace goruntuislemeV2.components
{
    internal class SaltPaperPanel : OptionsPanel
    {
        public NumericUpDown NoiseRatioUpDown { get; set; } = new NumericUpDown();
        public SaltPaperPanel()
        {
            

        }

        internal override void InitializeComponents()
        {
            Label label = new Label();
            label.Text = "Noise Ratio";
            label.Location = new System.Drawing.Point(10, 8);
            label.AutoSize = true;


            NoiseRatioUpDown.Location = new System.Drawing.Point(10, 30);
            NoiseRatioUpDown.Minimum = 0;
            NoiseRatioUpDown.Maximum = 1;
            NoiseRatioUpDown.DecimalPlaces = 2;
            NoiseRatioUpDown.Increment = 0.01m;
            NoiseRatioUpDown.Value = 0.5m;
            NoiseRatioUpDown.Size = new Size(100, 20);
            this.Controls.Add(NoiseRatioUpDown);

        }



        internal async override Task<Bitmap> ApplyFilter()
        {
            float noiseRatio = (float)NoiseRatioUpDown.Value;
            return await Task.Run(() =>
            {
                return Filters.AddSaltAndPepperNoise(MainForm.originalImage, noiseRatio);
            });
        }

    }
}
