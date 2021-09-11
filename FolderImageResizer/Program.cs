using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;

namespace reziser
{
    class Program
    {
        static void Main(string[] args)
        {
        start:
            Console.WriteLine("Folder path: ");
            string folder = Console.ReadLine();

            if (!Directory.Exists(folder))
            {
                Console.WriteLine("Path \"" + folder + "\" Doesn't exist");
                goto start; //yes i use goto fuck you
            }

            Console.WriteLine("Image type: ");
            string type = Console.ReadLine();

            string[] files;
            files = Directory.GetFiles(folder, "*." + type);

            foreach (var file in files)
            {
                Console.WriteLine(file);
            }

            var ka = files.Length;

            Console.WriteLine(ka);

            if (ka == 0)
            {
                Console.WriteLine("Found no \"" + type + "\" images in \"" + folder + "\"");
                goto start;
            }
            else
            {
                Console.WriteLine("\nWidth: ");
                string leveys = Console.ReadLine();
                int leveys2;
                string[] lx;

                Console.WriteLine("\nHeight: ");
                string korkeus = Console.ReadLine();
                int korkeus2;
                string[] kx;

                DirectoryInfo di = Directory.CreateDirectory(folder + "/resized");

                foreach (var file in files)
                {
                    string filenameX = Path.GetFileName(file);

                    Image image = Image.FromFile(file);

                    if (leveys.Contains('x', StringComparison.OrdinalIgnoreCase))
                    {
                        //lx = leveys.Split('x');
                        lx = Regex.Split(leveys, "x", RegexOptions.IgnoreCase);
                        leveys2 = image.Width * Int32.Parse(lx[0]);
                    }
                    else
                    {
                        leveys2 = Int32.Parse(leveys);
                    }

                    if (korkeus.Contains('x', StringComparison.OrdinalIgnoreCase))
                    {
                        //kx = korkeus.Split('x');
                        kx = Regex.Split(korkeus, "x", RegexOptions.IgnoreCase);
                        korkeus2 = image.Height * Int32.Parse(kx[0]);
                    }
                    else
                    {
                        korkeus2 = Int32.Parse(korkeus);
                    }

                    var destinationRect = new Rectangle(0, 0, leveys2, korkeus2);
                    var destinationImage = new Bitmap(leveys2, korkeus2);

                    destinationImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                    using (var graphics = Graphics.FromImage(destinationImage))
                    {
                        graphics.CompositingMode = CompositingMode.SourceCopy;
                        graphics.CompositingQuality = CompositingQuality.HighQuality;
                        graphics.SmoothingMode = SmoothingMode.None;
                        graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                        using (var wrapMode = new ImageAttributes())
                        {
                            wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                            graphics.DrawImage(image, destinationRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                        }
                    }

                    destinationImage.Save(folder + "/resized/" + filenameX);
                    Console.WriteLine("Image: " + filenameX + "\n" + image.Width + "x" + image.Height + " --> " + leveys2 + "x" + korkeus2);

                }

                Console.WriteLine("Done!");
                Console.ReadLine();
            }
        }
    }
}
