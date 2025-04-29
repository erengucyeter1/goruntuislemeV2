

using goruntuislemeV2.enums;
using goruntuislemeV2.utils;
using System.Reflection;

namespace goruntuislemeV2.components
{


    internal class ColorSpacePanel : OptionsPanel
    {
        delegate Bitmap ColorSpaceConversion();

        Panel InputPanel = new Panel();
        ComboBox sourceColorSpaceCB;
        ComboBox destinationColorSpaceCB;


        internal override void InitializeComponents()
        {
            ImageRequired = false;

            InputPanel.Location = new Point(230,5);
            InputPanel.Size = new Size(550, 190);
            this.Controls.Add(InputPanel);
            // dest

            Label destinationLabel = new Label();
            destinationLabel.Text = "Destination Color Space";
            destinationLabel.Location = new Point(10, 60);
            destinationLabel.AutoSize = true;
            this.Controls.Add(destinationLabel);

            destinationColorSpaceCB = new ComboBox();
            destinationColorSpaceCB.Location = new Point(10, 80);
            destinationColorSpaceCB.Size = new Size(200, 20);
            destinationColorSpaceCB.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(destinationColorSpaceCB);


            // source
            Label sourceLabel = new Label();
            sourceLabel.Text = "Source Color Space";
            sourceLabel.Location = new Point(10, 10);
            sourceLabel.AutoSize = true;
            this.Controls.Add(sourceLabel);

            sourceColorSpaceCB = new ComboBox();
            sourceColorSpaceCB.Location = new Point(10, 30);
            sourceColorSpaceCB.Size = new Size(200, 20);
            sourceColorSpaceCB.DropDownStyle = ComboBoxStyle.DropDownList;

            sourceColorSpaceCB.SelectedIndexChanged += (s, e) =>
            {
                ColorSpaces selectedColorSpace = (ColorSpaces)sourceColorSpaceCB.SelectedItem;
                destinationColorSpaceCB.Items.Clear();
                InitializeColorSpaceComboBox(destinationColorSpaceCB, selectedColorSpace);
                destinationColorSpaceCB.SelectedIndex = 0;
            };

            sourceColorSpaceCB.SelectedIndexChanged += (s, e) =>
            {
                ColorSpaces selectedColorSpace = (ColorSpaces)sourceColorSpaceCB.SelectedItem;
                InitializeInputPanel(selectedColorSpace);
            };


            InitializeColorSpaceComboBox(sourceColorSpaceCB);

            this.Controls.Add(sourceColorSpaceCB);


        }

        private void SourceColorSpace_SelectedIndexChanged(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        internal async override Task<Bitmap> ApplyFilter()
        {
            string sourceColorSpace = this.sourceColorSpaceCB.SelectedItem.ToString();
            string destinationColorSpace = this.destinationColorSpaceCB.SelectedItem.ToString();

            string methodName = sourceColorSpace + "_to_" + destinationColorSpace;

            // Use reflection to invoke the static method from the Filters class  
            Type filtersType = typeof(Filters);
            MethodInfo? method = filtersType.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public);

            if (method == null)
            {
                throw new InvalidOperationException($"The method '{methodName}' does not exist in the Filters class.");
            }

            // Prepare parameters for the method  
            object[] parameters =
            [
               InputPanel.Controls.Cast<Control>().ToArray()
            ];

            // Invoke the method and cast the result to Bitmap  

            return await Task.Run(() =>
            {
                Bitmap result = (Bitmap)method.Invoke(null, parameters);

                return result;
            });
        }



        private void InitializeInputPanel(ColorSpaces sourceSpace)
        {
            InputPanel.Controls.Clear();
            switch (sourceSpace)
            {
                case ColorSpaces.RGB:
                    InitInputPanelRGB();
                    break;
                case ColorSpaces.CMYK:
                    InitInputPanelCMYK();
                    break;
                case ColorSpaces.YUV:
                    InitInputPanelYUV();
                    break;
            }
        }




        private void InitInputPanelRGB()
        {
            string[] labels = { "R:", "G:", "B:" };
            for (int i = 0; i < labels.Length; i++)
            {
                Label label = new Label
                {
                    Text = labels[i],
                    Location = new Point(10, 10 + i * 40),
                    AutoSize = true
                };
                InputPanel.Controls.Add(label);

                NumericUpDown numeric = new NumericUpDown
                {
                    Name = $"numericRGB{i}",
                    Location = new Point(38, 10 + i * 40),
                    Size = new Size(100, 30),
                    Minimum = 0,
                    Maximum = 255
                };
                InputPanel.Controls.Add(numeric);
            }
        }


        private void InitInputPanelCMYK()
        {
            string[] labels = { "C:", "M:", "Y:", "K:" };
            for (int i = 0; i < labels.Length; i++)
            {
                Label label = new Label
                {
                    Text = labels[i],
                    Location = new Point(10, 10 + i * 40),
                    AutoSize = true
                };
                InputPanel.Controls.Add(label);

                NumericUpDown numeric = new NumericUpDown
                {
                    Name = $"numericCMYK{i}",
                    Location = new Point(42, 10 + i * 40),
                    Size = new Size(100, 30),
                    Minimum = 0,
                    Maximum = 1,
                    DecimalPlaces = 2,
                    Increment = 0.01M
                };
                InputPanel.Controls.Add(numeric);
            }
        }

        private void InitInputPanelYUV()
        {
            string[] labels = { "Y:", "U:", "V:" };
            for (int i = 0; i < labels.Length; i++)
            {
                Label label = new Label
                {
                    Text = labels[i],
                    Location = new Point(10, 10 + i * 40),
                    AutoSize = true
                };
                InputPanel.Controls.Add(label);

                NumericUpDown numeric = new NumericUpDown
                {
                    Name = $"numericYUV{i}",
                    Location = new Point(38, 10 + i * 40),
                    Size = new Size(100, 30),
                    Minimum = 0,
                    Maximum = 255,
                    DecimalPlaces = 2,
                    Increment = 0.1M
                };
                InputPanel.Controls.Add(numeric);
            }
        }

        private void InitializeColorSpaceComboBox(ComboBox comboBox, ColorSpaces? forbidden = null)
        {
            foreach (ColorSpaces colorSpace in Enum.GetValues(typeof(ColorSpaces)))
            {
                if (forbidden == null || colorSpace != forbidden)
                {
                    comboBox.Items.Add(colorSpace);
                }
            }

           
            comboBox.SelectedIndex = 0;
        }
    }
}
