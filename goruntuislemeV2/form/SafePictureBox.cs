

namespace goruntuislemeV2.form
{
    public class SafePictureBox : PictureBox
    {
        public Bitmap OriginalResolutionImage { get; set; }

        public bool isSelected { get; set; } = false;

        public SafePictureBox()
        {
            
        }

    }
}
