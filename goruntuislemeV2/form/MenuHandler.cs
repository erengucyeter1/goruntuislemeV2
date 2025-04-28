using goruntuislemeV2.enums;
using goruntuislemeV2.components;

namespace goruntuislemeV2.form
{
    internal class MenuHandler
    {
        public void FillComboBox(ComboBox cb, Type source)
        {
            cb.DataSource = Enum.GetValues(source);
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
        }

        public Bitmap GetImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return new Bitmap(openFileDialog.FileName);
            }
            return null;
        }

        public void SetOptionsMenu(Enum filterName, Panel panel)
        {

            foreach(Control control in panel.Controls)
            {
                if (control is IMenuChanger menuChanger)
                {
                    menuChanger.UpdateMainMenu();
                    
                }
            }

            panel.Controls.Clear();



            switch (filterName)
            {
                case FilterNames.None:
                    panel.Controls.Add(new NonePanel());
                    break;
                case FilterNames.Grayscale:
                    panel.Controls.Add(new GrayPanel());
                    break;
                case FilterNames.Binarize:
                    panel.Controls.Add(new BinarizePanel());
                    break;
               
                case FilterNames.Rotate:
                    panel.Controls.Add(new RotatePanel());
                    break;
                case FilterNames.ColorSpace:
                    panel.Controls.Add(new ColorSpacePanel());
                    break;
                case FilterNames.SaltAndPepperNoise:
                    panel.Controls.Add(new SaltPaperPanel());
                    break;
                case FilterNames.Aritmatic:
                    panel.Controls.Add(new AritmaticPanel());
                    break;
                case FilterNames.Threshold:
                    panel.Controls.Add(new ThresholdPanel());
                    break;
                case FilterNames.Morphology:
                    panel.Controls.Add(new MorphologyPanel());
                    break;
                case FilterNames.NoiseCleaner:
                    panel.Controls.Add(new NoiseCleanerPanel());
                    break;
                case FilterNames.PrewittEdgeDetection:
                    panel.Controls.Add(new EdgeDetectionPanel());
                    break;
                case FilterNames.Histogram:
                    panel.Controls.Add(new HistogramPanel());
                    break;
                case FilterNames.Zoom:
                    panel.Controls.Add(new ZoomPanel());
                    break;

                default:
                    break;
            }
        }

    }
}