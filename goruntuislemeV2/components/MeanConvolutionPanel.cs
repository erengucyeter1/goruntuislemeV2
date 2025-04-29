using goruntuislemeV2.utils;

namespace goruntuislemeV2.components
{
    internal class MeanConvolutionPanel : OptionsPanel
    {
        NumericUpDown kernelSizeUpDown = new NumericUpDown();


        internal override void InitializeComponents()
        {
            Label label = new Label();
            label.Text = "Kernel Size";
            label.Location = new System.Drawing.Point(10, 8);
            label.AutoSize = true;
            kernelSizeUpDown.Location = new System.Drawing.Point(10, 30);
            kernelSizeUpDown.Minimum = 1;
            kernelSizeUpDown.Maximum = 100;
            kernelSizeUpDown.Value = 3;
            kernelSizeUpDown.Size = new Size(100, 20);
            this.Controls.Add(kernelSizeUpDown);
            this.Controls.Add(label);
        }

        internal override async Task<Bitmap> ApplyFilter()
        {
            int kernelSize = (int)kernelSizeUpDown.Value;

            return await Task.Run(() =>
            {
                return Filters.MeanConvolution(MainForm.originalImage, kernelSize);
            });
        }
    }
}
