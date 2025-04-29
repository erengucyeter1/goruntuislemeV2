using goruntuislemeV2.utils;

namespace goruntuislemeV2.components
{
    internal class EdgeDetectionPanel : OptionsPanel
    {


        internal async override Task<Bitmap> ApplyFilter()
        {
            return await Task.Run(() =>
            {
                return Filters.PrewittEdgeDetection(MainForm.originalImage);
            });
        }
    }
}
