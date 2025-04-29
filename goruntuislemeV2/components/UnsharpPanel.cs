using goruntuislemeV2.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace goruntuislemeV2.components
{
    internal class UnsharpPanel : OptionsPanel
    {
        NumericUpDown amountNUD = new NumericUpDown();
        internal override void InitializeComponents()
        {

            Label amountLabel = new Label();
            amountLabel.Text = "Amount";
            amountLabel.Location = new Point(10, 10);
            amountLabel.AutoSize = true;
            this.Controls.Add(amountLabel);

            amountNUD.Location = new Point(10, 30);
            amountNUD.Minimum = 0;
            amountNUD.Maximum = 100;
            amountNUD.Value = 1;
            amountNUD.Size = new Size(100, 20);
            this.Controls.Add(amountNUD);

        }



        internal override async Task<Bitmap> ApplyFilter()
        {
            float amount = (float)amountNUD.Value;
            return await Task.Run(() =>
            {
                
               return Filters.ApplyUnsharpMask(MainForm.originalImage, amount);
            });
            
        }
    }

}
