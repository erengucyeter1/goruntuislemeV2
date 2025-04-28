using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace goruntuislemeV2.utils
{
    internal static class Filters
    {
        public static Bitmap GrayScale(Bitmap image)
        {
            Bitmap grayScaleImage = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
            BitmapData bmData = grayScaleImage.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                         ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            BitmapData srcData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                 ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            int bytes = Math.Abs(bmData.Stride) * image.Height;
            byte[] buffer = new byte[bytes];
            byte[] srcBuffer = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, srcBuffer, 0, bytes);
            image.UnlockBits(srcData);

            for (int i = 0; i < srcBuffer.Length; i += 4)
            {
                int gray = (int)(0.3 * srcBuffer[i + 2] + 0.59 * srcBuffer[i + 1] + 0.11 * srcBuffer[i]);
                buffer[i] = buffer[i + 1] = buffer[i + 2] = (byte)gray; // R=G=B=Gray
                buffer[i + 3] = srcBuffer[i + 3]; // Alpha değeri korunur
            }

            System.Runtime.InteropServices.Marshal.Copy(buffer, 0, bmData.Scan0, bytes);
            grayScaleImage.UnlockBits(bmData);

            return grayScaleImage;
        }


        public static Bitmap Binarize(Bitmap image, int thresoldVal = 127)
        {
            image = GrayScale(image);
            Bitmap grayScaleImage = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
            BitmapData bmData = grayScaleImage.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                         ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            BitmapData srcData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                 ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            int bytes = Math.Abs(bmData.Stride) * image.Height;
            byte[] buffer = new byte[bytes];
            byte[] srcBuffer = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, srcBuffer, 0, bytes);
            image.UnlockBits(srcData);

            for (int i = 0; i < srcBuffer.Length; i += 4)
            {
                int binarized = srcBuffer[i + 2] > thresoldVal ? 255 : 0;

                buffer[i] = buffer[i + 1] = buffer[i + 2] = (byte)binarized; // R=G=B=Gray

                buffer[i + 3] = srcBuffer[i + 3]; // Alpha değeri korunur
            }

            System.Runtime.InteropServices.Marshal.Copy(buffer, 0, bmData.Scan0, bytes);
            grayScaleImage.UnlockBits(bmData);

            return grayScaleImage;
        }

        public static Bitmap RotateImage(Bitmap image, int radius = 45)
        {
            radius *= -1;
            Bitmap rotated = new Bitmap(image.Width, image.Height, PixelFormat.Format32bppArgb);
            BitmapData bmData = rotated.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                         ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            BitmapData srcData = image.LockBits(new Rectangle(0, 0, image.Width, image.Height),
                                                 ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            int bytes = Math.Abs(bmData.Stride) * image.Height;
            byte[] buffer = new byte[bytes];
            byte[] srcBuffer = new byte[bytes];

            System.Runtime.InteropServices.Marshal.Copy(srcData.Scan0, srcBuffer, 0, bytes);
            image.UnlockBits(srcData);

            double angle = radius * Math.PI / 180;
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            int centerX = image.Width / 2;
            int centerY = image.Height / 2;

            for (int i = 0; i < srcBuffer.Length; i += 4)
            {
                int x = i / 4 % image.Width;
                int y = i / 4 / image.Width;
                int newX = (int)(cos * (x - centerX) - sin * (y - centerY) + centerX);
                int newY = (int)(sin * (x - centerX) + cos * (y - centerY) + centerY);
                if (newX >= 0 && newX < image.Width && newY >= 0 && newY < image.Height)
                {
                    int newIndex = (newY * image.Width + newX) * 4;
                    buffer[i] = srcBuffer[newIndex];
                    buffer[i + 1] = srcBuffer[newIndex + 1];
                    buffer[i + 2] = srcBuffer[newIndex + 2];
                    buffer[i + 3] = srcBuffer[newIndex + 3];
                }
            }

            System.Runtime.InteropServices.Marshal.Copy(buffer, 0, bmData.Scan0, bytes);
            rotated.UnlockBits(bmData);

            return rotated;
        }







        // NEED FİX. WRONG CONVERTION

        // COLOR SPACE CONVERSION

        public static Bitmap RGB_to_YUV(Control[] controls)
        {
            int R, G, B = 0;
            (R, G, B) = GetRGBValues(controls);

            double Y, U, V = 0.0;
            (Y, U, V) = rgbToYuv(R, G, B);
            string yuv = $"Y: {Y:F2}\nU: {U:F2}\nV: {V:F2}";

            return StringToBitmap(yuv);
            
        }
        private static (double, double, double) rgbToYuv(int R, int G, int B)
        {
            double Y = (0.257 * R) + (0.504 * G) + (0.098 * B) + 16;
            double U = -(0.148 * R) - (0.291 * G) + (0.439 * B) + 128;
            double V = (0.439 * R) - (0.368 * G) - (0.071 * B) + 128;
            return (Y,U,V);
        }


        private static (int, int, int) GetRGBValues(Control[] controls)
        {
            int[] values = new int[3];
            int index = 0;

            foreach (Control control in controls)
            {
                if (control is NumericUpDown && control.Name.StartsWith("numericRGB"))
                {
                    values[index] = (int)((NumericUpDown)control).Value;
                    index++;
                }
            }

            return (values[0], values[1], values[2]);
        }


        private static (double, double, double) GetYUVValues(Control[] controls)
        {
            double[] values = new double[3];
            int index = 0;

            foreach (Control control in controls)
            {
                if (control is NumericUpDown && control.Name.StartsWith("numericYUV"))
                {
                    values[index] = (double)((NumericUpDown)control).Value;
                    index++;
                }
            }

            return (values[0], values[1], values[2]);
        }


        private static (double, double, double, double) GetCMYKValues(Control[] controls)
        {
            double[] values = new double[4];
            int index = 0;

            foreach (Control control in controls)
            {
                if (control is NumericUpDown && control.Name.StartsWith("numericCMYK"))
                {
                    values[index] = (double)((NumericUpDown)control).Value;
                    index++;
                }
            }

            return (values[0], values[1], values[2], values[3]);
        }





        public static Bitmap YUV_to_RGB(Control[] controls)
        {
            
            double Y, U, V = 0.0;
            (Y, U, V) = GetYUVValues(controls);

            int R, G, B = 0;

            (R, G, B) = yuvToRgb(Y, U, V);



            string rgb = $"R: {R}\nG: {G}\nB: {B}";

            return StringToBitmap(rgb);
        }

        private static (int,int,int) yuvToRgb(double Y,double U,double V)
        {
            int R = (int)((1.164 * (Y - 16)) + 1.595 * ((V - 128)));
            int G = (int)((1.164 * (Y - 16)) - (0.813 * (V - 128)) - (0.391 * (U - 128)));
            int B = (int)((1.164 * (Y - 16)) + (2.018 * (U - 128)));

            return (R, G, B);
        }

        public static Bitmap CMYK_to_RGB(Control[] controls)
        {

            double C, M, Y, K = 0;
            (C, M, Y, K) = GetCMYKValues(controls);

            int R, G, B = 0;

            (R, G, B) = cmykToRgb(C, M, Y, K);

            string rgb = $"R: {R}\nG: {G}\nB: {B}";
            return StringToBitmap(rgb);
        }

        private static (int R, int G, int B) cmykToRgb(double C, double M, double Y, double K)
        {
            int R = (int)Math.Round(255 * (1 - C) * (1 - K));
            int G = (int)Math.Round(255 * (1 - M) * (1 - K));
            int B = (int)Math.Round(255 * (1 - Y) * (1 - K));

            R = Math.Clamp(R, 0, 255);
            G = Math.Clamp(G, 0, 255);
            B = Math.Clamp(B, 0, 255);

            return (R, G, B);
        }

        public static Bitmap RGB_to_CMYK(Control[] controls)
        {
            int R, G, B = 0;
            (R, G, B) = GetRGBValues(controls);


            double C, M, Y, K = 0.0;

            (C, M, Y, K) = rgbToCmyk(R, G, B);

            string cmyk = $"C: {C:F2}\nM: {M:F2}\nY: {Y:F2}\nK: {K:F2}";
            return StringToBitmap(cmyk);
        }
        private static (double C, double M, double Y, double K) rgbToCmyk(int R, int G, int B)
        {
            double r = R / 255.0;
            double g = G / 255.0;
            double b = B / 255.0;

            double K = 1 - Math.Max(r, Math.Max(g, b));
            if (K >= 1.0) // tamamen siyah
                return (0, 0, 0, 1);

            double C = (1 - r - K) / (1 - K);
            double M = (1 - g - K) / (1 - K);
            double Y = (1 - b - K) / (1 - K);

            return (C, M, Y, K);
        }

        

        public static Bitmap YUV_to_CMYK(Control[] controls)
        {
            double Y, U, V = 0.0;
            (Y, U, V) = GetYUVValues(controls);
            int R, G, B = 0;
            (R, G, B) = yuvToRgb(Y, U, V);
            double C, M, Y1, K = 0.0;
            (C, M, Y1, K) = rgbToCmyk(R, G, B);
            string cmyk = $"C: {C:F2}\nM: {M:F2}\nY: {Y1:F2}\nK: {K:F2}";
            return StringToBitmap(cmyk);

        }

        public static Bitmap CMYK_to_YUV(Control[] controls)
        {
            double C, M, Y, K = 0;
            (C, M, Y, K) = GetCMYKValues(controls);
            int R, G, B = 0;
            (R, G, B) = cmykToRgb(C, M, Y, K);
            double Y1, U, V = 0.0;
            (Y1, U, V) = rgbToYuv(R, G, B);
            string yuv = $"Y: {Y1:F2}\nU: {U:F2}\nV: {V:F2}";
            return StringToBitmap(yuv);


        }



        //

        private static Bitmap StringToBitmap(string text)
        {
            // Create a bitmap to write the string
            Bitmap bitmap = new Bitmap(600, 600); // Adjust size as needed
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.White); // Set background color
                using (Font font = new Font("Arial", 52))
                {
                    g.DrawString(text, font, Brushes.Black, new PointF(10, 10));
                }
            }
            return bitmap;
        }


        public static Bitmap AddSaltAndPepperNoise(Bitmap image, double noiseRatio)
        {
            if (noiseRatio < 0 || noiseRatio > 1)
            {
                throw new ArgumentOutOfRangeException("noiseRatio", "Noise ratio must be between 0 and 1.");
            }

            Bitmap noisyImage = new Bitmap(image.Width, image.Height);
            Random random = new Random();

            for (int y = 0; y < image.Height; y++) //0.02
            {
                for (int x = 0; x < image.Width; x++)
                {
                    double randomValue = random.NextDouble();


                    if (randomValue < noiseRatio / 2) //0.01 
                    {
                        noisyImage.SetPixel(x, y, Color.Black);
                    }
                    else if (randomValue > 1 - noiseRatio / 2)
                    {
                        noisyImage.SetPixel(x, y, Color.White);
                    }
                    else
                    {
                        noisyImage.SetPixel(x, y, image.GetPixel(x, y));
                    }
                }
            }
            return noisyImage;
        }


        // aritmatic

        public static Bitmap addImage(Bitmap image1, Bitmap image2, double scaleFactor = 1.0)//ScaleFactor = Sonucu ölçeklendirme faktörü
        {
            //Goruntuler ayni boyutta olmali aksi takdirde islem yapilamamakta dolayisiyla kontrol mekanizmasi.
            if (image1.Width != image2.Width || image1.Height != image2.Height)
            {
                throw new ArgumentException("Goruntuler ayni boyutta olmalidir.");
            }

            int width = image1.Width;
            int height = image1.Height;

            Bitmap addResultImage = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color color1 = image1.GetPixel(x, y);
                    Color color2 = image2.GetPixel(x, y);

                    int r = (int)Math.Min(255, Math.Max(0, (color1.R + color2.R) * scaleFactor));
                    int g = (int)Math.Min(255, Math.Max(0, (color1.G + color2.G) * scaleFactor));
                    int b = (int)Math.Min(255, Math.Max(0, (color1.B + color2.B) * scaleFactor));


                    addResultImage.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            return addResultImage;
        }

        public static Bitmap substractImage(Bitmap image1, Bitmap image2, int offset = 0) //offset=  cikarma islemine eklencek offset degeri
        {
            //Goruntuler ayni boyutta olmali aksi takdirde islem yapilamamakta dolayisiyla kontrol mekanizmasi.
            if (image1.Width != image2.Width || image1.Height != image2.Height)
            {
                throw new ArgumentException("Goruntuler ayni boyutta olmalidir.");
            }

            int width = image1.Width;
            int height = image1.Height;

            Bitmap subtractResultImage = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color color1 = image1.GetPixel(x, y);
                    Color color2 = image2.GetPixel(x, y);

                    int r = (int)Math.Min(255, Math.Max(0, (color1.R - color2.R) + offset));
                    int g = (int)Math.Min(255, Math.Max(0, (color1.G - color2.G) + offset));
                    int b = (int)Math.Min(255, Math.Max(0, (color1.B - color2.B) + offset));

                    subtractResultImage.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            return subtractResultImage;
        }

        public static Bitmap multiplyImage(Bitmap image1, Bitmap image2, double scaleFactor = 1.0 / 255.0)
        {
            //Goruntuler ayni boyutta olmali aksi takdirde islem yapilamamakta dolayisiyla kontrol mekanizmasi.
            if (image1.Width != image2.Width || image1.Height != image2.Height)
            {
                throw new ArgumentException("Goruntuler ayni boyutta olmalidir.");
            }

            int width = image1.Width;
            int heigth = image1.Height;

            Bitmap multipliedImageResult = new Bitmap(width, heigth);

            for (int y = 0; y < heigth; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color color1 = image1.GetPixel(x, y);
                    Color color2 = image2.GetPixel(x, y);

                    int r = (int)Math.Min(255, Math.Max(0, (color1.R * color2.R) * scaleFactor));
                    int g = (int)Math.Min(255, Math.Max(0, (color1.G * color2.G) * scaleFactor));
                    int b = (int)Math.Min(255, Math.Max(0, (color1.B * color2.B) * scaleFactor));

                    multipliedImageResult.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            return multipliedImageResult;
        }

        public static Bitmap divideImage(Bitmap image1, Bitmap image2, double scaleFactor = 1.0)
        {
            //Goruntuler ayni boyutta olmali aksi takdirde islem yapilamamakta dolayisiyla kontrol mekanizmasi.
            if (image1.Width != image2.Width || image1.Height != image2.Height)
            {
                throw new ArgumentException("Goruntuler ayni boyutta olmalidir.");
            }

            int width = image1.Width;
            int heigth = image1.Height;

            Bitmap dividedImageResult = new Bitmap(width, heigth);

            for (int y = 0; y < heigth; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color color1 = image1.GetPixel(x, y);
                    Color color2 = image2.GetPixel(x, y);

                    int r = 0;
                    int g = 0;
                    int b = 0;

                    if (color2.R != 0)
                        r = (int)Math.Min(255, Math.Max(0, (color1.R / (double)color2.R) * scaleFactor));
                    if (color2.G != 0)
                        g = (int)Math.Min(255, Math.Max(0, (color1.G / (double)color2.G) * scaleFactor));
                    if (color2.B != 0)
                        b = (int)Math.Min(255, Math.Max(0, (color1.B / (double)color2.B) * scaleFactor));

                    dividedImageResult.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            return dividedImageResult;
        }



        // gürültü giderme

        /*
        public static Bitmap applyMeanFilter(Bitmap image, int kernelSize)
        {
            int width = image.Width;
            int height = image.Height;
            Bitmap meanFilterAppliedImage = new Bitmap(width, height);

            int radius = kernelSize / 2;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int totalR = 0;
                    int totalG = 0;
                    int totalB = 0;

                    int pixelCount = 0;

                    for (int kx = -radius; kx <= radius; kx++)
                    {
                        for (int ky = -radius; ky <= radius; ky++)
                        {
                            int pixelX = x + kx;
                            int pixelY = y + ky;

                            if (pixelX >= 0 && pixelX < width && pixelY >= 0 && pixelY < height)
                            {
                                Color pixelColor = image.GetPixel(pixelX, pixelY);
                                totalR += pixelColor.R;
                                totalG += pixelColor.G;
                                totalB += pixelColor.B;

                                pixelCount++;
                            }
                        }
                    }

                    byte avgR = (byte)(totalR / pixelCount);
                    byte avgG = (byte)(totalG / pixelCount);
                    byte avgB = (byte)(totalB / pixelCount);

                    meanFilterAppliedImage.SetPixel(x, y, Color.FromArgb(avgR, avgG, avgB));
                }
            }

            return meanFilterAppliedImage;
        }*/
        public static Bitmap applyMeanFilter(Bitmap image, int kernelSize)
        {
            int width = image.Width;
            int height = image.Height;
            Bitmap result = new Bitmap(width, height);

            int radius = kernelSize / 2;

            Rectangle rect = new Rectangle(0, 0, width, height);
            BitmapData srcData = image.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            BitmapData dstData = result.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

            int stride = srcData.Stride;
            int bytes = stride * height;
            byte[] pixelBuffer = new byte[bytes];
            byte[] resultBuffer = new byte[bytes];

            // Bellekten veriyi oku
            Marshal.Copy(srcData.Scan0, pixelBuffer, 0, bytes);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int totalR = 0, totalG = 0, totalB = 0;
                    int pixelCount = 0;

                    for (int ky = -radius; ky <= radius; ky++)
                    {
                        for (int kx = -radius; kx <= radius; kx++)
                        {
                            int px = x + kx;
                            int py = y + ky;

                            if (px >= 0 && px < width && py >= 0 && py < height)
                            {
                                int offset = py * stride + px * 3;
                                totalB += pixelBuffer[offset];
                                totalG += pixelBuffer[offset + 1];
                                totalR += pixelBuffer[offset + 2];
                                pixelCount++;
                            }
                        }
                    }

                    int currentOffset = y * stride + x * 3;
                    resultBuffer[currentOffset] = (byte)(totalB / pixelCount);
                    resultBuffer[currentOffset + 1] = (byte)(totalG / pixelCount);
                    resultBuffer[currentOffset + 2] = (byte)(totalR / pixelCount);
                }
            }

            // Sonuçları belleğe yaz
            Marshal.Copy(resultBuffer, 0, dstData.Scan0, bytes);
            image.UnlockBits(srcData);
            result.UnlockBits(dstData);

            return result;
        }


        public static Bitmap applyMedianFilter(Bitmap image, int kernelSize)
        {
            int width = image.Width;
            int height = image.Height;

            Bitmap medianFilterAppliedImage = new Bitmap(width, height);

            int radius = kernelSize / 2;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    List<int> redValues = new List<int>();
                    List<int> greenValues = new List<int>();
                    List<int> blueValues = new List<int>();

                    for (int ky = -radius; ky <= radius; ky++)
                    {
                        for (int kx = -radius; kx <= radius; kx++)
                        {
                            int pixelX = x + kx;
                            int pixelY = y + ky;

                            if (pixelX >= 0 && pixelX < width && pixelY >= 0 && pixelY < height)
                            {
                                Color pixelColor = image.GetPixel(pixelX, pixelY);
                                redValues.Add(pixelColor.R);
                                greenValues.Add(pixelColor.G);
                                blueValues.Add(pixelColor.B);
                            }
                        }
                    }

                    redValues.Sort();
                    greenValues.Sort();
                    blueValues.Sort();

                    byte medianR = 0;
                    byte medianG = 0;
                    byte medianB = 0;

                    if (kernelSize % 2 == 0)
                    {
                        medianR = (byte)((redValues[redValues.Count / 2] + redValues[(redValues.Count / 2) - 1]) / 2);
                        medianG = (byte)((greenValues[greenValues.Count / 2] + greenValues[(greenValues.Count / 2) - 1]) / 2);
                        medianB = (byte)((blueValues[blueValues.Count / 2] + blueValues[(blueValues.Count / 2) - 1]) / 2);
                    }
                    else
                    {
                        medianR = (byte)redValues[redValues.Count / 2];
                        medianG = (byte)greenValues[greenValues.Count / 2];
                        medianB = (byte)blueValues[blueValues.Count / 2];
                    }

                    medianFilterAppliedImage.SetPixel(x, y, Color.FromArgb(medianR, medianG, medianB));
                }
            }
            return medianFilterAppliedImage;
        }




        /// prewit edge detection
        /// 
        public static Bitmap PrewittEdgeDetection(Bitmap original)
        {
            Bitmap grayImage = GrayScale(original); // Zaten grayscale hale getirildiğini varsayıyoruz
            Bitmap result = new Bitmap(original.Width, original.Height, PixelFormat.Format32bppArgb);

            int[,] Gx = new int[,]
            {
        { -1, 0, 1 },
        { -1, 0, 1 },
        { -1, 0, 1 }
            };

            int[,] Gy = new int[,]
            {
        { 1, 1, 1 },
        { 0, 0, 0 },
        { -1, -1, -1 }
            };

            Rectangle rect = new Rectangle(0, 0, grayImage.Width, grayImage.Height);
            BitmapData grayData = grayImage.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            BitmapData resultData = result.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            int stride = grayData.Stride;
            int bytes = stride * grayImage.Height;
            byte[] grayBuffer = new byte[bytes];
            byte[] resultBuffer = new byte[bytes];

            Marshal.Copy(grayData.Scan0, grayBuffer, 0, bytes);

            for (int y = 1; y < grayImage.Height - 1; y++)
            {
                for (int x = 1; x < grayImage.Width - 1; x++)
                {
                    int gx = 0, gy = 0;

                    for (int j = -1; j <= 1; j++)
                    {
                        for (int i = -1; i <= 1; i++)
                        {
                            int offset = ((y + j) * stride) + ((x + i) * 4);
                            int intensity = grayBuffer[offset + 2]; // R kanalından al, çünkü gri tonlamada hepsi eşit

                            gx += intensity * Gx[j + 1, i + 1];
                            gy += intensity * Gy[j + 1, i + 1];
                        }
                    }

                    int g = (int)Math.Sqrt(gx * gx + gy * gy);
                    int clamped = g > 135 ? 255 : 0;

                    int resultOffset = (y * stride) + (x * 4);
                    resultBuffer[resultOffset + 0] = (byte)clamped; // B
                    resultBuffer[resultOffset + 1] = (byte)clamped; // G
                    resultBuffer[resultOffset + 2] = (byte)clamped; // R
                    resultBuffer[resultOffset + 3] = 255;           // A
                }
            }

            Marshal.Copy(resultBuffer, 0, resultData.Scan0, bytes);
            grayImage.UnlockBits(grayData);
            result.UnlockBits(resultData);

            return result;
        }

        // histogram

        public static Bitmap HistogramGerme(this Bitmap original, int inputMin, int inputMax, int outputMin, int outputMax)
        {
            int width = original.Width;
            int height = original.Height;

            Bitmap grayImg = GrayScale(original); // Gri tonlamalı hale getir

            BitmapData bitmapData = grayImg.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);

            int stride = bitmapData.Stride;
            int bytes = stride * height;
            byte[] pixelBuffer = new byte[bytes];

            // Veriyi belleğe al
            Marshal.Copy(bitmapData.Scan0, pixelBuffer, 0, bytes);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int offset = y * stride + x * 3;

                    int pixelValue = pixelBuffer[offset]; // Gri görüntüde R=G=B, biri yeter

                    // Histogram germe işlemi (float ile daha hassas)
                    float stretched = (float)(pixelValue - inputMin) / (inputMax - inputMin);
                    int newPixel = (int)(stretched * (outputMax - outputMin) + outputMin);

                    // Clamp
                    newPixel = Math.Max(0, Math.Min(255, newPixel));
                    byte finalVal = (byte)newPixel;

                    // R = G = B
                    pixelBuffer[offset] = finalVal;     // Blue
                    pixelBuffer[offset + 1] = finalVal; // Green
                    pixelBuffer[offset + 2] = finalVal; // Red
                }
            }

            // Veriyi geri aktar
            System.Runtime.InteropServices.Marshal.Copy(pixelBuffer, 0, bitmapData.Scan0, bytes);
            grayImg.UnlockBits(bitmapData);

            return grayImg;
        }

        public static Bitmap HistogramGenisletme(this Bitmap original)
        {
            int width = original.Width;
            int height = original.Height;

            Bitmap grayImg = GrayScale(original); // Gri tonlamalı hale çevir

            BitmapData bitmapData = grayImg.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadWrite,
                PixelFormat.Format24bppRgb);

            int stride = bitmapData.Stride;
            int bytes = stride * height;
            byte[] pixelBuffer = new byte[bytes];

            // Veriyi RAM'e kopyala
            System.Runtime.InteropServices.Marshal.Copy(bitmapData.Scan0, pixelBuffer, 0, bytes);

            int min = 255;
            int max = 0;

            // Min & Max bul
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int offset = y * stride + x * 3;
                    int pixelVal = pixelBuffer[offset]; // Gri tonlamada R=G=B

                    if (pixelVal < min) min = pixelVal;
                    if (pixelVal > max) max = pixelVal;
                }
            }

            // Histogram genişletme işlemi
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int offset = y * stride + x * 3;
                    int pixelVal = pixelBuffer[offset];

                    int newVal = (255 * (pixelVal - min)) / Math.Max(1, (max - min)); // Bölme hatasını önle

                    byte finalVal = (byte)Math.Clamp(newVal, 0, 255);

                    pixelBuffer[offset] = finalVal;     // Blue
                    pixelBuffer[offset + 1] = finalVal; // Green
                    pixelBuffer[offset + 2] = finalVal; // Red
                }
            }

            // RAM'deki veriyi geri aktar
            System.Runtime.InteropServices.Marshal.Copy(pixelBuffer, 0, bitmapData.Scan0, bytes);
            grayImg.UnlockBits(bitmapData);

            return grayImg;
        }



        // Zoom 

        public static  Bitmap NearestNeighborInterpolation(Bitmap originalImage, int newWidth, int newHeight)
        {
            // Create a new bitmap with the desired dimensions
            Bitmap resizedImage = new Bitmap(newWidth, newHeight);

            // Calculate scaling factors
            float xRatio = (float)originalImage.Width / newWidth;
            float yRatio = (float)originalImage.Height / newHeight;

            // Get source bitmap data
            System.Drawing.Imaging.BitmapData srcData = originalImage.LockBits(
                new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                originalImage.PixelFormat);

            // Get destination bitmap data
            System.Drawing.Imaging.BitmapData destData = resizedImage.LockBits(
                new Rectangle(0, 0, newWidth, newHeight),
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                originalImage.PixelFormat);

            // Get bytes per pixel
            int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(originalImage.PixelFormat) / 8;

            // Stride is width * bytes per pixel, aligned on 4-byte boundary
            int srcStride = srcData.Stride;
            int destStride = destData.Stride;

            // Pointers to the data
            IntPtr srcScan0 = srcData.Scan0;
            IntPtr destScan0 = destData.Scan0;

            // Create buffers for source and destination data
            byte[] srcBuffer = new byte[srcStride * originalImage.Height];
            byte[] destBuffer = new byte[destStride * newHeight];

            // Copy source data to buffer
            System.Runtime.InteropServices.Marshal.Copy(srcScan0, srcBuffer, 0, srcBuffer.Length);

            // Process each pixel in the destination image
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    // Find nearest source pixel
                    int srcX = (int)(x * xRatio);
                    int srcY = (int)(y * yRatio);

                    // Ensure source coordinates are within bounds
                    srcX = Math.Min(srcX, originalImage.Width - 1);
                    srcY = Math.Min(srcY, originalImage.Height - 1);

                    // Calculate offsets in buffers
                    int destOffset = y * destStride + x * bytesPerPixel;
                    int srcOffset = srcY * srcStride + srcX * bytesPerPixel;

                    // Copy pixel data (B, G, R, A if present)
                    for (int i = 0; i < bytesPerPixel; i++)
                    {
                        destBuffer[destOffset + i] = srcBuffer[srcOffset + i];
                    }
                }
            }

            // Copy processed data back to destination bitmap
            System.Runtime.InteropServices.Marshal.Copy(destBuffer, 0, destScan0, destBuffer.Length);

            // Unlock bitmaps
            originalImage.UnlockBits(srcData);
            resizedImage.UnlockBits(destData);

            return resizedImage;
        }


        public static Bitmap BilinearInterpolation (Bitmap originalImage, int newWidth, int newHeight)
        {
            // Create a new bitmap with the desired dimensions
            Bitmap resizedImage = new Bitmap(newWidth, newHeight);

            // Calculate scaling factors
            float xRatio = ((float)(originalImage.Width - 1)) / newWidth;
            float yRatio = ((float)(originalImage.Height - 1)) / newHeight;

            // Get source bitmap data
            System.Drawing.Imaging.BitmapData srcData = originalImage.LockBits(
                new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                originalImage.PixelFormat);

            // Get destination bitmap data
            System.Drawing.Imaging.BitmapData destData = resizedImage.LockBits(
                new Rectangle(0, 0, newWidth, newHeight),
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                originalImage.PixelFormat);

            // Get bytes per pixel
            int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(originalImage.PixelFormat) / 8;

            // Stride is width * bytes per pixel, aligned on 4-byte boundary
            int srcStride = srcData.Stride;
            int destStride = destData.Stride;

            // Pointers to the data
            IntPtr srcScan0 = srcData.Scan0;
            IntPtr destScan0 = destData.Scan0;

            // Create buffers for source and destination data
            byte[] srcBuffer = new byte[srcStride * originalImage.Height];
            byte[] destBuffer = new byte[destStride * newHeight];

            // Copy source data to buffer
            System.Runtime.InteropServices.Marshal.Copy(srcScan0, srcBuffer, 0, srcBuffer.Length);

            // Process each pixel in the destination image
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    // Calculate source position with floating point
                    float srcX = x * xRatio;
                    float srcY = y * yRatio;

                    // Get the four neighboring pixels
                    int x1 = (int)Math.Floor(srcX);
                    int y1 = (int)Math.Floor(srcY);
                    int x2 = Math.Min(x1 + 1, originalImage.Width - 1);
                    int y2 = Math.Min(y1 + 1, originalImage.Height - 1);

                    // Calculate fractional parts
                    float xFrac = srcX - x1;
                    float yFrac = srcY - y1;

                    // Get the offsets for the four pixels in the source buffer
                    int topLeftOffset = y1 * srcStride + x1 * bytesPerPixel;
                    int topRightOffset = y1 * srcStride + x2 * bytesPerPixel;
                    int bottomLeftOffset = y2 * srcStride + x1 * bytesPerPixel;
                    int bottomRightOffset = y2 * srcStride + x2 * bytesPerPixel;

                    // Destination pixel offset
                    int destOffset = y * destStride + x * bytesPerPixel;

                    // Bilinear interpolation for each channel (B, G, R, A if present)
                    for (int i = 0; i < bytesPerPixel; i++)
                    {
                        // Get values of the four pixels for this channel
                        byte c1 = srcBuffer[topLeftOffset + i];
                        byte c2 = srcBuffer[topRightOffset + i];
                        byte c3 = srcBuffer[bottomLeftOffset + i];
                        byte c4 = srcBuffer[bottomRightOffset + i];

                        // Bilinear interpolation (first horizontal, then vertical)
                        float horizontalTop = c1 * (1 - xFrac) + c2 * xFrac;
                        float horizontalBottom = c3 * (1 - xFrac) + c4 * xFrac;
                        byte result = (byte)(horizontalTop * (1 - yFrac) + horizontalBottom * yFrac);

                        // Set the interpolated value to the destination buffer
                        destBuffer[destOffset + i] = result;
                    }
                }
            }

            // Copy processed data back to destination bitmap
            System.Runtime.InteropServices.Marshal.Copy(destBuffer, 0, destScan0, destBuffer.Length);

            // Unlock bitmaps
            originalImage.UnlockBits(srcData);
            resizedImage.UnlockBits(destData);

            return resizedImage;
        }


        public static Bitmap BicubicInterpolation(Bitmap originalImage, int newWidth, int newHeight)
        {
            // Create a new bitmap with the desired dimensions
            Bitmap resizedImage = new Bitmap(newWidth, newHeight);

            // Calculate scaling factors
            float xRatio = ((float)(originalImage.Width - 1)) / newWidth;
            float yRatio = ((float)(originalImage.Height - 1)) / newHeight;

            // Get source bitmap data
            System.Drawing.Imaging.BitmapData srcData = originalImage.LockBits(
                new Rectangle(0, 0, originalImage.Width, originalImage.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly,
                originalImage.PixelFormat);

            // Get destination bitmap data
            System.Drawing.Imaging.BitmapData destData = resizedImage.LockBits(
                new Rectangle(0, 0, newWidth, newHeight),
                System.Drawing.Imaging.ImageLockMode.WriteOnly,
                originalImage.PixelFormat);

            // Get bytes per pixel
            int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(originalImage.PixelFormat) / 8;

            // Stride is width * bytes per pixel, aligned on 4-byte boundary
            int srcStride = srcData.Stride;
            int destStride = destData.Stride;

            // Pointers to the data
            IntPtr srcScan0 = srcData.Scan0;
            IntPtr destScan0 = destData.Scan0;

            // Create buffers for source and destination data
            byte[] srcBuffer = new byte[srcStride * originalImage.Height];
            byte[] destBuffer = new byte[destStride * newHeight];

            // Copy source data to buffer
            System.Runtime.InteropServices.Marshal.Copy(srcScan0, srcBuffer, 0, srcBuffer.Length);

            // Process each pixel in the destination image
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    // Calculate source position with floating point
                    float srcX = x * xRatio;
                    float srcY = y * yRatio;

                    // Integer parts of source coordinates
                    int xInt = (int)srcX;
                    int yInt = (int)srcY;

                    // Fractional parts of source coordinates
                    float xFrac = srcX - xInt;
                    float yFrac = srcY - yInt;

                    // Destination pixel offset
                    int destOffset = y * destStride + x * bytesPerPixel;

                    // For each color channel
                    for (int channel = 0; channel < bytesPerPixel; channel++)
                    {
                        // Get 16 surrounding pixels (4x4 grid) for bicubic interpolation
                        byte[] samples = new byte[16];
                        int idx = 0;

                        for (int j = -1; j <= 2; j++)
                        {
                            for (int i = -1; i <= 2; i++)
                            {
                                // Ensure coordinates are within bounds
                                int px = Math.Max(0, Math.Min(xInt + i, originalImage.Width - 1));
                                int py = Math.Max(0, Math.Min(yInt + j, originalImage.Height - 1));

                                // Calculate offset in source buffer
                                int srcOffset = py * srcStride + px * bytesPerPixel + channel;
                                samples[idx++] = srcBuffer[srcOffset];
                            }
                        }

                        // Apply bicubic interpolation
                        float result = BicubicInterpolate(samples, xFrac, yFrac);

                        // Clamp result to valid byte range
                        destBuffer[destOffset + channel] = (byte)Math.Max(0, Math.Min(255, result));
                    }
                }
            }

            // Copy processed data back to destination bitmap
            System.Runtime.InteropServices.Marshal.Copy(destBuffer, 0, destScan0, destBuffer.Length);

            // Unlock bitmaps
            originalImage.UnlockBits(srcData);
            resizedImage.UnlockBits(destData);

            return resizedImage;
        }

        // Bicubic interpolation helper function
        private static float BicubicInterpolate(byte[] p, float x, float y)
        {
            // Cubic kernel function
            Func<float, float> cubic = (t) =>
            {
                float a = -0.5f; // Adjustable parameter, typically between -0.5 and -0.75

                if (t < 0.0f) t = -t;

                if (t <= 1.0f)
                    return ((a + 2.0f) * t - (a + 3.0f)) * t * t + 1.0f;
                else if (t < 2.0f)
                    return ((a * t - 5.0f * a) * t + 8.0f * a) * t - 4.0f * a;
                else
                    return 0.0f;
            };

            // Calculate interpolation weights
            float[] wx = new float[4];
            float[] wy = new float[4];

            for (int i = 0; i < 4; i++)
            {
                wx[i] = cubic(x + 1.0f - i);
                wy[i] = cubic(y + 1.0f - i);
            }

            // Apply bicubic interpolation
            float result = 0.0f;

            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    result += p[j * 4 + i] * wx[i] * wy[j];
                }
            }

            return result;
        }

    }
}
