using goruntuislemeV2.enums;
using goruntuislemeV2.components;

namespace goruntuislemeV2.form
{
    public class MenuHandler
    {
        OptionsPanel previousPanel;
        ISideEffectRemove sideEffectRemove;
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

        public bool SetOptionsMenu(Enum filterName, Panel panel)
        {
            bool success = true;

            panel.Controls.Clear();

            if (previousPanel != null)
            {
                previousPanel.Dispose();
                previousPanel = null;
            }

            if (sideEffectRemove != null)
            {
                sideEffectRemove.RemoveSideEffects();
                sideEffectRemove = null;
            }


            switch (filterName)
            {
                case FilterNames.None:
                    previousPanel = new NonePanel(); 
                    break;
                case FilterNames.Grayscale:
                    previousPanel = new GrayPanel();
                    break;
                case FilterNames.Binarize:
                    previousPanel = new BinarizePanel();
                    break;
               
                case FilterNames.Rotate:
                    previousPanel = new RotatePanel();
                    break;
                case FilterNames.ColorSpace:
                    previousPanel = new ColorSpacePanel();
                    break;
                case FilterNames.SaltAndPepperNoise:
                    previousPanel = new SaltPaperPanel();            
                    break;
                case FilterNames.Aritmatic:
                    previousPanel = new AritmaticPanel();
                    break;
                case FilterNames.Threshold:
                    previousPanel = new ThresholdPanel();
                   
                    break;
                case FilterNames.MeanConvolution:
                    previousPanel = new MeanConvolutionPanel();
                    
                    break;
                case FilterNames.Morphology:
                    previousPanel = new MorphologyPanel();
                    
                    break;

                case FilterNames.NoiseCleaner:
                    previousPanel = new NoiseCleanerPanel();
                   
                    break;
                case FilterNames.PrewittEdgeDetection:
                    previousPanel = new EdgeDetectionPanel();
                    break;
                case FilterNames.Histogram:
                    previousPanel = new HistogramPanel();
                    break;
                case FilterNames.Zoom:
                    previousPanel = new ZoomPanel();
                    break;
                case FilterNames.Cut:
                    if(MainForm.originalImage == null)
                    {
                        MessageBox.Show("Please select an image first.");
                        success = false;
                        break;
                    }
                    previousPanel = new CutPanel();
                    sideEffectRemove = (ISideEffectRemove)previousPanel;
                    break;   

                case FilterNames.Unsharp:
                    previousPanel = new UnsharpPanel();
                    break;

                case FilterNames.IncreaseContrast:
                    previousPanel = new ContrastPanel();
                    break;
            }

            if (previousPanel != null)
            {
                panel.Controls.Add(previousPanel);
            }

            return success;

        }

    }
}