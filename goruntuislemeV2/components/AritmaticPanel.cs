using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using goruntuislemeV2.utils;

namespace goruntuislemeV2.components
{
    internal class AritmaticPanel : OptionsPanel
    {

        ComboBox operationComboBox = new ComboBox();
        PictureBox pictureBox = new PictureBox();
        public AritmaticPanel()
        {
                
        }

        internal override void InitializeComponents()
        {
            Label label = new Label();
            label.Text = "Operation";
            label.Location = new System.Drawing.Point(10, 8);
            label.AutoSize = true;
            operationComboBox.Location = new System.Drawing.Point(10, 30);
            operationComboBox.Size = new Size(100, 20);
            operationComboBox.Items.Add("Add");
            operationComboBox.Items.Add("Subtract");
            operationComboBox.Items.Add("Multiply");
            operationComboBox.Items.Add("Divide");
            operationComboBox.SelectedIndex = 0;
            this.Controls.Add(operationComboBox);


            label = new Label();
            label.Text = "Operation Image";
            label.Location = new System.Drawing.Point(120, 8);
            label.AutoSize = true;
            this.Controls.Add(label);

            pictureBox.DoubleClick += new EventHandler((sender, e) =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pictureBox.Image = Image.FromFile(openFileDialog.FileName);
                }
            });
            pictureBox.Location = new System.Drawing.Point(120, 30);
            pictureBox.Size = new Size(100, 100);
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Controls.Add(pictureBox);

        }

        

           internal override Bitmap ApplyFilter()
             {
            string operation = operationComboBox.SelectedItem.ToString();
            Bitmap image = pictureBox.Image as Bitmap;

            if (image == null)
            {
                throw new InvalidOperationException("The selected image is not a valid bitmap.");
            }

            // Resize the image if its dimensions do not match the original image
            if (image.Width != MainForm.originalImage.Width || image.Height != MainForm.originalImage.Height)
            {
                Bitmap resizedImage = new Bitmap(MainForm.originalImage.Width, MainForm.originalImage.Height);
                using (Graphics g = Graphics.FromImage(resizedImage))
                {
                    g.DrawImage(image, 0, 0, resizedImage.Width, resizedImage.Height);
                }
                image = resizedImage;
            }

            switch (operation)
            {
                case "Add":
                    return Filters.addImage(MainForm.originalImage, image);
                case "Subtract":
                    return Filters.substractImage(MainForm.originalImage, image);
                case "Multiply":
                    return Filters.multiplyImage(MainForm.originalImage, image);
                case "Divide":
                    return Filters.divideImage(MainForm.originalImage, image);
                default:
                    throw new NotImplementedException("Operation not implemented");
            }
        }

    }

}
