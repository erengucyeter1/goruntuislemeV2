using goruntuislemeV2.utils;

namespace goruntuislemeV2.components
{
    internal class ContrastPanel: OptionsPanel
    {
        NumericUpDown factorNUD;
        internal override void InitializeComponents()
        {
            Label label = new Label();
            label.Text = "Factor";
            label.Location = new System.Drawing.Point(10, 8);
            label.AutoSize = true;

            factorNUD = new NumericUpDown();
            factorNUD.Location = new System.Drawing.Point(10, 40);
            factorNUD.Size = new Size(100, 20);
            factorNUD.Minimum = 0;
            factorNUD.Maximum = 10;
            factorNUD.Value = 1;
            factorNUD.Increment = 0.1M;
            factorNUD.DecimalPlaces = 2;
            this.Controls.Add(factorNUD);
            this.Controls.Add(label);
            Label infoLabel = new Label();

        }

        internal override Task<Bitmap> ApplyFilter()
        {
            float factor = (float)factorNUD.Value;

            return Task.Run(() =>
            {
                return Filters.IncreaseContrast(MainForm.originalImage, factor);
            });
        }
    }
}
