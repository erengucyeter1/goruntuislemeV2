using goruntuislemeV2.utils;

namespace goruntuislemeV2.components
{
    internal class BinarizePanel:OptionsPanel
    {

        public BinarizePanel():base() { }   
        internal override void InitializeComponents()
        {
            
        }
        internal async override Task<Bitmap> ApplyFilter()
        {
            return await Task.Run(() =>
            {
                return Filters.Binarize(MainForm.originalImage);
            });

        }
    }
}
