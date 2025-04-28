using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace goruntuislemeV2.components
{
    internal class MorphologyPanel :OptionsPanel
    {

        public MorphologyPanel()
        {
            
        }

        /// <summary>
        /// Morfolojik genişleme (dilation) işlemi uygular
        /// </summary>
        /// <param name="sourceImage">Kaynak görüntü</param>
        /// <param name="structureSize">Yapısal eleman boyutu (3x3, 5x5, vb.)</param>
        /// <returns>Genişleme uygulanmış görüntü</returns>
        public Bitmap Dilation(Bitmap sourceImage, int structureSize = 3)
        {
            return ApplyMorphologicalOperation(sourceImage, structureSize, true);
        }

        /// <summary>
        /// Morfolojik aşınma (erosion) işlemi uygular
        /// </summary>
        /// <param name="sourceImage">Kaynak görüntü</param>
        /// <param name="structureSize">Yapısal eleman boyutu (3x3, 5x5, vb.)</param>
        /// <returns>Aşınma uygulanmış görüntü</returns>
        public Bitmap Erosion(Bitmap sourceImage, int structureSize = 3)
        {
            return ApplyMorphologicalOperation(sourceImage, structureSize, false);
        }

        /// <summary>
        /// Morfolojik açma (opening) işlemi uygular (önce aşınma, sonra genişleme)
        /// </summary>
        /// <param name="sourceImage">Kaynak görüntü</param>
        /// <param name="structureSize">Yapısal eleman boyutu (3x3, 5x5, vb.)</param>
        /// <returns>Açma uygulanmış görüntü</returns>
        public Bitmap Opening(Bitmap sourceImage, int structureSize = 3)
        {
            // Önce aşınma, sonra genişleme uygulanır
            Bitmap erodedImage = Erosion(sourceImage, structureSize);
            Bitmap result = Dilation(erodedImage, structureSize);

            // Ara görüntüyü temizle
            erodedImage.Dispose();

            return result;
        }

        /// <summary>
        /// Morfolojik kapama (closing) işlemi uygular (önce genişleme, sonra aşınma)
        /// </summary>
        /// <param name="sourceImage">Kaynak görüntü</param>
        /// <param name="structureSize">Yapısal eleman boyutu (3x3, 5x5, vb.)</param>
        /// <returns>Kapama uygulanmış görüntü</returns>
        public Bitmap Closing(Bitmap sourceImage, int structureSize = 3)
        {
            // Önce genişleme, sonra aşınma uygulanır
            Bitmap dilatedImage = Dilation(sourceImage, structureSize);
            Bitmap result = Erosion(dilatedImage, structureSize);

            // Ara görüntüyü temizle
            dilatedImage.Dispose();

            return result;
        }

        /// <summary>
        /// Temel morfolojik işlemi uygular (genişleme veya aşınma)
        /// </summary>
        /// <param name="sourceImage">Kaynak görüntü</param>
        /// <param name="structureSize">Yapısal eleman boyutu</param>
        /// <param name="isDilation">Genişleme mi, aşınma mı?</param>
        /// <returns>İşlem uygulanmış görüntü</returns>
        private Bitmap ApplyMorphologicalOperation(Bitmap sourceImage, int structureSize, bool isDilation)
        {
            // Görüntü boyutlarını al
            int width = sourceImage.Width;
            int height = sourceImage.Height;

            // Sonuç görüntüsünü oluştur
            Bitmap resultImage = new Bitmap(width, height);

            // Yapısal elemanın yarıçapı
            int radius = structureSize / 2;

            // Görüntüleri kilitle
            BitmapData sourceData = sourceImage.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            BitmapData resultData = resultImage.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);

            // Piksel verisi için tampon dizileri oluştur
            int bytesPerPixel = 4; // 32bpp için
            int stride = sourceData.Stride;
            int totalBytes = stride * height;

            byte[] sourcePixels = new byte[totalBytes];
            byte[] resultPixels = new byte[totalBytes];

            // Verileri byte dizilerine kopyala
            Marshal.Copy(sourceData.Scan0, sourcePixels, 0, totalBytes);

            // Her piksel için işlem yap
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    // Her kanal için en büyük veya en küçük değeri saklayacak değişkenler
                    byte maxB = 0, maxG = 0, maxR = 0, maxA = 0;
                    byte minB = 255, minG = 255, minR = 255, minA = 255;

                    // Yapısal eleman içindeki pozisyonları dolaş
                    for (int ky = -radius; ky <= radius; ky++)
                    {
                        int py = y + ky;

                        // Görüntü sınırlarını kontrol et
                        if (py < 0) py = 0;
                        if (py >= height) py = height - 1;

                        for (int kx = -radius; kx <= radius; kx++)
                        {
                            int px = x + kx;

                            // Görüntü sınırlarını kontrol et
                            if (px < 0) px = 0;
                            if (px >= width) px = width - 1;

                            // Piksel indeksini hesapla
                            int pixelIndex = py * stride + px * bytesPerPixel;

                            // Piksel değerlerini kontrol et
                            // Genişleme için maksimum, aşınma için minimum değeri bul
                            byte b = sourcePixels[pixelIndex];     // Blue
                            byte g = sourcePixels[pixelIndex + 1]; // Green
                            byte r = sourcePixels[pixelIndex + 2]; // Red
                            byte a = sourcePixels[pixelIndex + 3]; // Alpha

                            // Maksimum değerleri güncelle
                            if (b > maxB) maxB = b;
                            if (g > maxG) maxG = g;
                            if (r > maxR) maxR = r;
                            if (a > maxA) maxA = a;

                            // Minimum değerleri güncelle
                            if (b < minB) minB = b;
                            if (g < minG) minG = g;
                            if (r < minR) minR = r;
                            if (a < minA) minA = a;
                        }
                    }

                    // Sonuç piksel değerini belirle
                    int resultIndex = y * stride + x * bytesPerPixel;

                    if (isDilation)
                    {
                        // Genişleme için maksimum değerleri kullan
                        resultPixels[resultIndex] = maxB;     // Blue
                        resultPixels[resultIndex + 1] = maxG; // Green
                        resultPixels[resultIndex + 2] = maxR; // Red
                        resultPixels[resultIndex + 3] = maxA; // Alpha
                    }
                    else
                    {
                        // Aşınma için minimum değerleri kullan
                        resultPixels[resultIndex] = minB;     // Blue
                        resultPixels[resultIndex + 1] = minG; // Green
                        resultPixels[resultIndex + 2] = minR; // Red
                        resultPixels[resultIndex + 3] = minA; // Alpha
                    }
                }
            }

            // İşlem sonucunu hedef görüntüye kopyala
            Marshal.Copy(resultPixels, 0, resultData.Scan0, totalBytes);

            // Görüntüleri serbest bırak
            sourceImage.UnlockBits(sourceData);
            resultImage.UnlockBits(resultData);

            return resultImage;
        }

        /// <summary>
        /// İkili (binary) görüntüye dönüştürme işlemi (siyah-beyaz)
        /// </summary>
        /// <param name="sourceImage">Kaynak görüntü</param>
        /// <param name="threshold">Eşik değeri (0-255)</param>
        /// <returns>İkili görüntü</returns>
        public Bitmap ToBinary(Bitmap sourceImage, int threshold = 128)
        {
            int width = sourceImage.Width;
            int height = sourceImage.Height;

            // Sonuç görüntüsünü oluştur
            Bitmap resultImage = new Bitmap(width, height);

            // Görüntüleri kilitle
            BitmapData sourceData = sourceImage.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            BitmapData resultData = resultImage.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);

            // Piksel verisi için tampon dizileri oluştur
            int bytesPerPixel = 4; // 32bpp için
            int stride = sourceData.Stride;
            int totalBytes = stride * height;

            byte[] sourcePixels = new byte[totalBytes];
            byte[] resultPixels = new byte[totalBytes];

            // Verileri byte dizilerine kopyala
            Marshal.Copy(sourceData.Scan0, sourcePixels, 0, totalBytes);

            // Her piksel için işlem yap
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int pixelIndex = y * stride + x * bytesPerPixel;

                    // RGB değerlerinin ortalamasını al
                    byte b = sourcePixels[pixelIndex];     // Blue
                    byte g = sourcePixels[pixelIndex + 1]; // Green
                    byte r = sourcePixels[pixelIndex + 2]; // Red
                    byte a = sourcePixels[pixelIndex + 3]; // Alpha

                    // Grilik değerini hesapla (basit ortalama)
                    byte grayValue = (byte)((r + g + b) / 3);

                    // Eşik değerine göre siyah veya beyaz yap
                    byte binaryValue = (byte)(grayValue >= threshold ? 255 : 0);

                    // Sonuç piksele ata
                    resultPixels[pixelIndex] = binaryValue;     // Blue
                    resultPixels[pixelIndex + 1] = binaryValue; // Green
                    resultPixels[pixelIndex + 2] = binaryValue; // Red
                    resultPixels[pixelIndex + 3] = a;           // Alpha değerini koru
                }
            }

            // İşlem sonucunu hedef görüntüye kopyala
            Marshal.Copy(resultPixels, 0, resultData.Scan0, totalBytes);

            // Görüntüleri serbest bırak
            sourceImage.UnlockBits(sourceData);
            resultImage.UnlockBits(resultData);

            return resultImage;
        }

        /// <summary>
        /// Skeleton (iskelet) çıkarma işlemi - temel morfolojik işlemlerin birleşimi
        /// </summary>
        /// <param name="sourceImage">Kaynak görüntü</param>
        /// <param name="iterations">Maksimum iterasyon sayısı</param>
        /// <returns>İskelet çıkarılmış görüntü</returns>
        public Bitmap Skeletonize(Bitmap sourceImage, int iterations = 50)
        {
            // Önce ikili görüntüye dönüştür
            Bitmap binaryImage = ToBinary(sourceImage);
            Bitmap result = new Bitmap(binaryImage);
            Bitmap temp;

            // İskelet çıkarma algoritması
            for (int i = 0; i < iterations; i++)
            {
                // Bir önceki görüntüyü sakla
                Bitmap previous = new Bitmap(result);

                // Aşınma ve açma işlemleri
                temp = Erosion(result, 3);
                Bitmap opened = Opening(temp, 3);
                temp.Dispose();

                // Fark görüntüsünü hesapla
                Bitmap difference = SubtractImages(previous, opened);

                // Sonuç görüntüsünü güncelle
                temp = result;
                result = difference;
                temp.Dispose();
                opened.Dispose();
                previous.Dispose();

                // Eğer görüntü tamamen boş hale geldiyse durabilirsiniz
                if (IsImageEmpty(result))
                    break;
            }

            binaryImage.Dispose();
            return result;
        }

        /// <summary>
        /// İki görüntü arasındaki farkı hesaplar
        /// </summary>
        private Bitmap SubtractImages(Bitmap image1, Bitmap image2)
        {
            int width = image1.Width;
            int height = image1.Height;

            Bitmap resultImage = new Bitmap(width, height);

            // Görüntüleri kilitle
            BitmapData data1 = image1.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            BitmapData data2 = image2.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            BitmapData resultData = resultImage.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.WriteOnly,
                PixelFormat.Format32bppArgb);

            // Piksel verisi için tampon dizileri oluştur
            int bytesPerPixel = 4; // 32bpp için
            int stride = data1.Stride;
            int totalBytes = stride * height;

            byte[] pixels1 = new byte[totalBytes];
            byte[] pixels2 = new byte[totalBytes];
            byte[] resultPixels = new byte[totalBytes];

            // Verileri byte dizilerine kopyala
            Marshal.Copy(data1.Scan0, pixels1, 0, totalBytes);
            Marshal.Copy(data2.Scan0, pixels2, 0, totalBytes);

            // Her piksel için işlem yap
            for (int i = 0; i < totalBytes; i += bytesPerPixel)
            {
                // RGB kanalları için fark hesapla (basit çıkarma)
                for (int j = 0; j < 3; j++)
                {
                    int diff = pixels1[i + j] - pixels2[i + j];
                    resultPixels[i + j] = (byte)(diff > 0 ? diff : 0);
                }

                // Alpha kanalını koru
                resultPixels[i + 3] = pixels1[i + 3];
            }

            // İşlem sonucunu hedef görüntüye kopyala
            Marshal.Copy(resultPixels, 0, resultData.Scan0, totalBytes);

            // Görüntüleri serbest bırak
            image1.UnlockBits(data1);
            image2.UnlockBits(data2);
            resultImage.UnlockBits(resultData);

            return resultImage;
        }

        /// <summary>
        /// Görüntünün boş olup olmadığını kontrol eder
        /// </summary>
        private bool IsImageEmpty(Bitmap image)
        {
            int width = image.Width;
            int height = image.Height;

            // Görüntüyü kilitle
            BitmapData data = image.LockBits(
                new Rectangle(0, 0, width, height),
                ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            // Piksel verisi için tampon dizi oluştur
            int bytesPerPixel = 4; // 32bpp için
            int stride = data.Stride;
            int totalBytes = stride * height;

            byte[] pixels = new byte[totalBytes];

            // Verileri byte dizisine kopyala
            Marshal.Copy(data.Scan0, pixels, 0, totalBytes);

            // Görüntüyü serbest bırak
            image.UnlockBits(data);

            // Tüm pikselleri kontrol et
            for (int i = 0; i < totalBytes; i += bytesPerPixel)
            {
                // Herhangi bir RGB değeri 0'dan büyükse görüntü boş değildir
                if (pixels[i] > 0 || pixels[i + 1] > 0 || pixels[i + 2] > 0)
                    return false;
            }

            return true;
        }

        internal override Bitmap ApplyFilter()
        {
            // Uygulamak istediğiniz morfolojik işlemi burada belirleyin
            // Örneğin, genişleme işlemi
            return Dilation(MainForm.originalImage);
        }


    }
}
