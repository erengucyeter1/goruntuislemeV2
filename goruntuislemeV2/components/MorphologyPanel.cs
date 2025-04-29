using goruntuislemeV2.utils;

namespace goruntuislemeV2.components
{
    internal class MorphologyPanel : OptionsPanel
    {
        ComboBox methodCB;
        ComboBox StructerElementCB;
        NumericUpDown KernelSizeNUD;

        public MorphologyPanel()
        {
            
        }

        internal override void InitializeComponents()
        {
            Label label = new Label();
            label.Text = "Method";
            label.Location = new System.Drawing.Point(10, 1);
            label.AutoSize = true;

            methodCB = new ComboBox();
            methodCB.Location = new System.Drawing.Point(10, 30);
            methodCB.Items.Add("Dilation");
            methodCB.Items.Add("Erosion");
            methodCB.Items.Add("Opening");
            methodCB.Items.Add("Closing");
            methodCB.SelectedIndex = 0;
            methodCB.Size = new Size(100, 20);
            this.Controls.Add(methodCB);
            this.Controls.Add(label);

            Label label2 = new Label();
            label2.Text = "Structer Element";
            label2.Location = new System.Drawing.Point(10, 63);
            label2.AutoSize = true;

            StructerElementCB = new ComboBox();
            StructerElementCB.Location = new System.Drawing.Point(10, 92);
            StructerElementCB.Items.Add("Square");
            StructerElementCB.Items.Add("Plus");
            StructerElementCB.Items.Add("Cross");
            StructerElementCB.Items.Add("Circle");
            StructerElementCB.SelectedIndex = 0;
            StructerElementCB.Size = new Size(100, 20);
            this.Controls.Add(StructerElementCB);
            this.Controls.Add(label2);

            Label label3 = new Label();
            label3.Text = "Kernel Size";
            label3.Location = new System.Drawing.Point(10, 125);
            label3.AutoSize = true;

            KernelSizeNUD = new NumericUpDown();
            KernelSizeNUD.Location = new System.Drawing.Point(10, 154);
            KernelSizeNUD.Minimum = 1;
            KernelSizeNUD.Maximum = 100;
            KernelSizeNUD.Value = 3;
            KernelSizeNUD.Size = new Size(100, 20);
            KernelSizeNUD.Increment = 2;
            KernelSizeNUD.DecimalPlaces = 0;
            KernelSizeNUD.ValueChanged += (s, e) =>
            {
                if (KernelSizeNUD.Value % 2 == 0)
                {
                    KernelSizeNUD.Value++;
                }
            };
            this.Controls.Add(KernelSizeNUD);
            this.Controls.Add(label3);
        }

        internal override async Task<Bitmap> ApplyFilter()
        {
            string method = methodCB.SelectedItem.ToString();
            string structerElement = StructerElementCB.SelectedItem.ToString();
            int kernelSize = (int)KernelSizeNUD.Value;


            switch (method)
            {
                case "Dilation":
                    return await Task.Run(() =>
                    {
                        return Filters.Dilation(MainForm.originalImage, structerElement, kernelSize);
                    });
                case "Erosion":
                    return await Task.Run(() =>
                    {
                        return Filters.Erosion(MainForm.originalImage, structerElement, kernelSize);
                    });
                case "Opening":
                    return await Task.Run(() =>
                    {
                        return Filters.Opening(MainForm.originalImage, structerElement, kernelSize);
                    });
                case "Closing":
                    return await Task.Run(() =>
                    {
                        return Filters.Closing(MainForm.originalImage, structerElement, kernelSize);
                    });
                default:
                    throw new NotImplementedException("Selected method is not implemented.");
                }
            }


    }
}
