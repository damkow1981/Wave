using System;
using System.IO;
using System.Collections.Generic;
using WaveApi.Models;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;


namespace WaveApi.Services
{
    public static class ImageFilesProcessing
    {
        // Import user32.dll (containing the function we need) and define
        // the method corresponding to the native function.
        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int MessageBox(IntPtr hWnd, string lpText, string lpCaption, uint uType);

        public static int szerokoscObrazu = 1432;
        public static int wysokoscObrazu = 915;
        public static double SWratio = (double)szerokoscObrazu / (double)wysokoscObrazu;

        public static void CzekajNaPlik(string filename)
        {
            //This will lock the execution until the file is ready
            //TODO: Add some logic to make it async and cancelable
            while (!CzyPlikJestDostepny(filename)) { }
        }

        private static bool CzyPlikJestDostepny(string filename)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public static string NadajNumerPlikowi(HashSet<string> hashset)
        {
            //var seed = (int)DateTime.Now.Ticks;
            var rand = new Random();

            //dodaj losowo wybrany nr obrazu fali do hashlisty
            string NumerPliku;
            int maxLiczbaPlików = Math.Max(999999999 * hashset.Count, 999999999);
            while (!hashset.Add(NumerPliku = rand.Next(100000000, maxLiczbaPlików).ToString())) ;

            Console.WriteLine(string.Join(", ", hashset));
            Console.WriteLine("NumerPlikuAudio = " + NumerPliku);

            //Console.ReadKey();

            return NumerPliku;
        }
        
        public static void UtwórzObrazFali(ItemData data)
        {
            // Create a new image
            Bitmap img = new Bitmap(szerokoscObrazu, wysokoscObrazu);

            Graphics graphics = Graphics.FromImage(img);

            int x;
            const int y = 0;
            char[] cyfry = data.ID.ToCharArray();


            Image image_0 = (Bitmap)(new ImageConverter().ConvertFrom(Images.Images.Image_0_byte()));
            Image image_1 = (Bitmap)(new ImageConverter().ConvertFrom(Images.Images.Image_1_byte()));
            Image image_2 = (Bitmap)(new ImageConverter().ConvertFrom(Images.Images.Image_2_byte()));
            Image image_3 = (Bitmap)(new ImageConverter().ConvertFrom(Images.Images.Image_3_byte()));
            Image image_4 = (Bitmap)(new ImageConverter().ConvertFrom(Images.Images.Image_4_byte()));
            Image image_5 = (Bitmap)(new ImageConverter().ConvertFrom(Images.Images.Image_5_byte()));
            Image image_6 = (Bitmap)(new ImageConverter().ConvertFrom(Images.Images.Image_6_byte()));
            Image image_7 = (Bitmap)(new ImageConverter().ConvertFrom(Images.Images.Image_7_byte()));
            Image image_8 = (Bitmap)(new ImageConverter().ConvertFrom(Images.Images.Image_8_byte()));
            Image image_9 = (Bitmap)(new ImageConverter().ConvertFrom(Images.Images.Image_9_byte()));

            int szerokoscPoczatkuKonca = 50;
            int szerokoscObrazkaSkladowego = 148;

            graphics.DrawImage(image_4, new Point(0, y));
            graphics.DrawImage(image_4, new Point(szerokoscObrazu - szerokoscObrazkaSkladowego, y));

            for (int i = 0; i <= 8; i++)
            {
                x = 1 + szerokoscPoczatkuKonca + i * szerokoscObrazkaSkladowego;

                switch (cyfry[i])
                {
                    case '0':
                        graphics.DrawImage(image_0, new Point(x, y));
                        break;
                    case '1':
                        graphics.DrawImage(image_1, new Point(x, y));
                        break;
                    case '2':
                        graphics.DrawImage(image_2, new Point(x, y));
                        break;
                    case '3':
                        graphics.DrawImage(image_3, new Point(x, y));
                        break;
                    case '4':
                        graphics.DrawImage(image_4, new Point(x, y));
                        break;
                    case '5':
                        graphics.DrawImage(image_5, new Point(x, y));
                        break;
                    case '6':
                        graphics.DrawImage(image_6, new Point(x, y));
                        break;
                    case '7':
                        graphics.DrawImage(image_7, new Point(x, y));
                        break;
                    case '8':
                        graphics.DrawImage(image_8, new Point(x, y));
                        break;
                    case '9':
                        graphics.DrawImage(image_9, new Point(x, y));
                        break;
                    default:
                        Console.WriteLine("Default case");
                        break;
                }
            }

            graphics.Dispose();

            // Adjust the image's brightness.
            img = AdjustBrightness(img, (float)(0.8));

            //konwertuj do b&w
            img = img.Clone(new Rectangle(0, 0, img.Width, img.Height), PixelFormat.Format1bppIndexed);

            //wczytaj obraz fali do item            
            data.WavePicture = convertImageToByte(img);

            //zapisz obraz fali na dysku lokalnym
            string waveFilePath = Paths.wavesPath + "\\" + data.ID + ".jpg";
            img.Save(waveFilePath, ImageFormat.Jpeg);

            img.Dispose();

        }

        public static Bitmap ZapiszZdjęcie(Bitmap img)
        {
            // Adjust the image's brightness.
            img = AdjustBrightness(img, (float)(2.5));

            //konwertuj do b&w
            img = img.Clone(new Rectangle(0, 0, img.Width, img.Height), PixelFormat.Format1bppIndexed);

            //obróć zdjęcie
            img = RotateBitmap(img, RotationAngle(img));

            //przytnij zdjęcie
            try
            {
                img = CropBitmap(img);
            }
            catch (Exception)
            {
                //throw new Exception("Can't crop!");
            }

            Image img_resized = resizeImage(img, szerokoscObrazu, wysokoscObrazu);

            //zapisz zdjęcie na dysku lokalnym
            img_resized.Save(Paths.photoPath + "photo.jpg", ImageFormat.Jpeg);

            img.Dispose();

            return (Bitmap)(img_resized);
        }

        public static Bitmap AdjustBrightness(Image image, float brightness)
        {
            // Make the ColorMatrix.
            float b = brightness;
            ColorMatrix cm = new ColorMatrix(new float[][]
                {
                    new float[] {b, 0, 0, 0, 0},
                    new float[] {0, b, 0, 0, 0},
                    new float[] {0, 0, b, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1},
                });
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(cm);

            // Draw the image onto the new bitmap while applying the new ColorMatrix.
            Point[] points =
            {
                new Point(0, 0),
                new Point(image.Width, 0),
                new Point(0, image.Height),
            };
            Rectangle rect = new Rectangle(0, 0, image.Width, image.Height);

            // Make the result bitmap.
            Bitmap bm = new Bitmap(image.Width, image.Height);
            using (Graphics gr = Graphics.FromImage(bm))
            {
                gr.DrawImage(image, points, rect, GraphicsUnit.Pixel, attributes);
            }

            // Return the result.
            return bm;
        }

        static float RotationAngle(Bitmap bm)
        {
            double angle = 0;
            int argb, Xpoczatku, Ypoczatku = 0, Xkonca, Ykonca = 0;

            //znajduje współrzędne początku fali
            for (Xpoczatku = 0; Xpoczatku < bm.Width; Xpoczatku++)
            {
                for (Ypoczatku = 0; Ypoczatku < bm.Height; Ypoczatku++)
                {
                    argb = bm.GetPixel(Xpoczatku, Ypoczatku).ToArgb();

                    if (argb != -1)
                    {
                        goto Wyjdz_1;
                    }
                }
            }

        Wyjdz_1:

            //znajduje współrzędne końca fali
            for (Xkonca = bm.Width - 1; Xkonca > 0; Xkonca--)
            {
                for (Ykonca = bm.Height - 1; Ykonca > 0; Ykonca--)
                {
                    argb = bm.GetPixel(Xkonca, Ykonca).ToArgb();

                    if (argb != -1)
                    {
                        goto Wyjdz_2;
                    }
                }
            }

        Wyjdz_2:

            angle = Math.Atan2(Ypoczatku - Ykonca, Xkonca - Xpoczatku) * (180.0 / Math.PI);

            return (float)angle;
        }

        // Return a bitmap rotated around its center.
        // positive angle rotates clockwise
        private static Bitmap RotateBitmap(Bitmap bm, float angle)
        {
            // Make a Matrix to represent rotation by this angle.
            Matrix rotate_at_origin = new Matrix();
            rotate_at_origin.Rotate(angle);

            // Rotate the image's corners to see how big
            // it will be after rotation.
            PointF[] points =
            {
                new PointF(0, 0),
                new PointF(bm.Width, 0),
                new PointF(bm.Width, bm.Height),
                new PointF(0, bm.Height),
            };
            rotate_at_origin.TransformPoints(points);
            float xmin, xmax, ymin, ymax;
            GetPointBounds(points, out xmin, out xmax, out ymin, out ymax);

            // Make a bitmap to hold the rotated result.
            int wid = (int)Math.Round(xmax - xmin);
            int hgt = (int)Math.Round(ymax - ymin);
            Bitmap result = new Bitmap(wid, hgt);

            // Create the real rotation transformation.
            Matrix rotate_at_center = new Matrix();
            rotate_at_center.RotateAt(angle,
                new PointF(wid / 2f, hgt / 2f));

            // Draw the image onto the new bitmap rotated.
            using (Graphics gr = Graphics.FromImage(result))
            {
                // Use smooth image interpolation.
                gr.InterpolationMode = InterpolationMode.High;

                // Clear with the color in the image's upper left corner.
                gr.Clear(bm.GetPixel(0, 0));

                //// For debugging. (Makes it easier to see the background.)
                //gr.Clear(Color.LightBlue);

                // Set up the transformation to rotate.
                gr.Transform = rotate_at_center;

                // Draw the image centered on the bitmap.
                int x = (wid - bm.Width) / 2;
                int y = (hgt - bm.Height) / 2;
                gr.DrawImage(bm, x, y);
            }

            // Return the result bitmap.
            return result;
        }

        // Find the bounding rectangle for an array of points.
        private static void GetPointBounds(PointF[] points, out float xmin, out float xmax, out float ymin, out float ymax)
        {
            xmin = points[0].X;
            xmax = xmin;
            ymin = points[0].Y;
            ymax = ymin;
            foreach (PointF point in points)
            {
                if (xmin > point.X) xmin = point.X;
                if (xmax < point.X) xmax = point.X;
                if (ymin > point.Y) ymin = point.Y;
                if (ymax < point.Y) ymax = point.Y;
            }
        }

        static Bitmap CropBitmap(Bitmap bm)
        {
            int argb, Xpoczatku = 0, Ypoczatku = 0, Xkonca, Ykonca = 0;

            //znajduje współrzędne początku fali
            for (Xpoczatku = 0; Xpoczatku < bm.Width - 1; Xpoczatku++)
            {
                for (Ypoczatku = 0; Ypoczatku < bm.Height - 1; Ypoczatku++)
                {
                    argb = bm.GetPixel(Xpoczatku, Ypoczatku).ToArgb();

                    if (argb != -1)
                    {
                        goto Wyjdz_1;
                    }
                }
            }

        Wyjdz_1:

            //znajduje współrzędne końca fali
            for (Xkonca = bm.Width - 1; Xkonca > 0; Xkonca--)
            {
                for (Ykonca = bm.Height - 1; Ykonca > 0; Ykonca--)
                {
                    argb = bm.GetPixel(Xkonca, Ykonca).ToArgb();

                    if (argb != -1)
                    {
                        goto Wyjdz_2;
                    }
                }
            }

        Wyjdz_2:

            int szerokosc = Xkonca - Xpoczatku;
            double wysokosc = szerokosc / SWratio;
            int gruboscFaliCiszy = 12;
            Ypoczatku = Ypoczatku + gruboscFaliCiszy / 2 - (int)wysokosc / 2;

            if (Ypoczatku < 0)
            {
                wysokosc = 2 * (wysokosc / 2 + Ypoczatku);
                Ypoczatku = 0;
            }

            Bitmap bmCropped = bm.Clone(new Rectangle(Xpoczatku, Ypoczatku, szerokosc, (int)wysokosc), bm.PixelFormat);

            bm.Dispose();

            return bmCropped;
        }

        private static Image resizeImage(Image imgToResize, int destWidth, int destHeight)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return (Image)b;
        }

        public static bool CompareImages(Bitmap img1, Bitmap img2)
        {
            double limit = 0.15 * szerokoscObrazu * wysokoscObrazu;
            int differentPixels = 0;

            if (img1.Width == img2.Width && img1.Height == img2.Height)
            {
                for (int kolumna = 0; kolumna < img1.Width; kolumna++)
                {
                    for (int wiersz = 0; wiersz < img1.Height; wiersz++)
                    {
                        if (img1.GetPixel(kolumna, wiersz).ToArgb() != img2.GetPixel(kolumna, wiersz).ToArgb())
                        {
                            differentPixels++;
                        }
                    }

                    if (differentPixels > limit)
                        return false;
                }
            }

            else
                MessageBox(IntPtr.Zero, "Obrazy mają różne rozmiary", "Info", 0);

            return true;
        }

        public static byte[] convertImageToByte(Image image)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] imageByte = (byte[])_imageConverter.ConvertTo(image, typeof(byte[]));
            return imageByte;
        }

    }
}