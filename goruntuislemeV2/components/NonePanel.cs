using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace goruntuislemeV2.components
{
    internal class NonePanel: OptionsPanel
    {
        public NonePanel() : base()
        {
        }

  

        internal async override Task<Bitmap> ApplyFilter()
        {
            return await Task.Run(() =>
            {
                return MainForm.originalImage;
            });
        }
    }


}
