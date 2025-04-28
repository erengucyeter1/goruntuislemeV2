using goruntuislemeV2.utils;

namespace goruntuislemeV2.components
{
    internal class EdgeDetectionPanel : OptionsPanel
    {


        internal override Bitmap ApplyFilter()
        {
            return Filters.PrewittEdgeDetection(MainForm.originalImage);
        }
    }
}
