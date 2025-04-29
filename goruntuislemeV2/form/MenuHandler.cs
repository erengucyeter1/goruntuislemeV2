using goruntuislemeV2.enums;
using goruntuislemeV2.components;

namespace goruntuislemeV2.form
{
    internal class MenuHandler
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

        public void SetOptionsMenu(Enum filterName, Panel panel)
        {

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
                    previousPanel = new CutPanel();
                    sideEffectRemove = (ISideEffectRemove)previousPanel;
                    break;   
            }

            if (previousPanel != null)
            {
                previousPanel.InitializeComponents();
                panel.Controls.Add(previousPanel);
            }
            
        }

    }
}