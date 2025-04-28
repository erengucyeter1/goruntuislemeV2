using goruntuislemeV2.utils;

namespace goruntuislemeV2.components
{
    internal class BinarizePanel:OptionsPanel
    {

        public BinarizePanel():base() { }   
        internal override void InitializeComponents()
        {
            
        }
        internal override Bitmap ApplyFilter()
        {
            return Filters.Binarize(MainForm.originalImage);
            

        }
    }
}
