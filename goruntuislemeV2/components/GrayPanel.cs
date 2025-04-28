using goruntuislemeV2.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace goruntuislemeV2.components
{
    internal class GrayPanel: OptionsPanel
    {
        public GrayPanel():base()
        {
            
        }

        internal override Bitmap ApplyFilter()
        {
            return Filters.GrayScale(MainForm.originalImage);
           
        }
    }
}
