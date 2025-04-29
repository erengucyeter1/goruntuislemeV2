using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using goruntuislemeV2.utils;

namespace goruntuislemeV2.components
{
    internal class NoiseCleanerPanel:OptionsPanel
    {
        public NumericUpDown KernelSizeUpDown { get; set; } = new NumericUpDown();
        public ComboBox FilterTypeCB { get; set; } = new ComboBox();
        public NoiseCleanerPanel()
        {
            
        }

        internal override void InitializeComponents()
        {
            Label label = new Label();
            label.Text = "Kernel Size";
            label.Location = new System.Drawing.Point(10, 8);
            label.AutoSize = true;
            KernelSizeUpDown.Location = new System.Drawing.Point(10, 30);
            KernelSizeUpDown.Size = new Size(100, 20);
            KernelSizeUpDown.Minimum = 1;
            KernelSizeUpDown.Maximum = 100;
            KernelSizeUpDown.Value = 3;
            KernelSizeUpDown.Increment = 2;
            KernelSizeUpDown.ValueChanged += new EventHandler((sender, e) =>
            {
                if (KernelSizeUpDown.Value % 2 == 0)
                {
                    KernelSizeUpDown.Value++;
                }
            });

            this.Controls.Add(KernelSizeUpDown);
            this.Controls.Add(label);

            Label label2 = new Label();
            label2.Text = "Filter Type";
            label2.Location = new System.Drawing.Point(120, 8);
            label2.AutoSize = true;
            FilterTypeCB.Location = new System.Drawing.Point(120, 30);
            FilterTypeCB.Size = new Size(100, 20);
            FilterTypeCB.Items.Add("Median");
            FilterTypeCB.Items.Add("Mean");

            FilterTypeCB.SelectedIndex = 0;
            this.Controls.Add(FilterTypeCB);
            this.Controls.Add(label2);


        }

        internal async override Task<Bitmap> ApplyFilter()
        {
            string filterType = FilterTypeCB.SelectedItem.ToString();

            return await Task.Run(() =>
            {
                switch (filterType)
                {
                    case "Median":
                        return Filters.applyMedianFilter(MainForm.originalImage, (int)KernelSizeUpDown.Value);
                    case "Mean":
                        return Filters.applyMeanFilter(MainForm.originalImage, (int)KernelSizeUpDown.Value);
                    default:
                        throw new NotImplementedException("Filter type not implemented");
                }
            });

            
        }



    }
}
