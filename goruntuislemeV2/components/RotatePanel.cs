
using goruntuislemeV2.utils;
using System.Drawing.Imaging;
using goruntuislemeV2.form;

namespace goruntuislemeV2.components
{
    internal class RotatePanel : OptionsPanel
    {
        private NumericUpDown angleNumericUpDown;
        private Label angleLabel;
        public RotatePanel():base()
        { 
        }

        internal override void InitializeComponents()
        {
            this.AutoSize = true;
            this.angleNumericUpDown = new NumericUpDown();
            this.angleLabel = new Label();
            // 
            // angleNumericUpDown
            // 
            this.angleNumericUpDown.Location = new System.Drawing.Point(10, 30);
            this.angleNumericUpDown.Minimum = -360;
            this.angleNumericUpDown.Maximum = 360;
            this.angleNumericUpDown.Value = 0;
            this.angleNumericUpDown.DecimalPlaces = 0;
            // 
            // angleLabel
            // 
            this.angleLabel.Text = "Angle:";
            this.angleLabel.Location = new System.Drawing.Point(10, 8);
            // 
            // RotateOptionsPanel
            // 
            this.Controls.Add(this.angleNumericUpDown);
            this.Controls.Add(this.angleLabel);

           


        }

        internal  async override Task<Bitmap> ApplyFilter()
        {
            
            int angle = (int)angleNumericUpDown.Value;

            return await Task.Run(() =>
            {
                return Filters.RotateImage(MainForm.originalImage, angle);
            });
        }


        
    }
}
