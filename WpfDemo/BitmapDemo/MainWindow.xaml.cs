using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace BitmapDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        enum ImageFormat
        {
            JPG,
            BMP,
            PNG,
            GIF,
            TIF
        }
        string path = "";

        private Bitmap bot;

        public MainWindow()
        {
            InitializeComponent();
            btnStart.Click += BtnStart_Click;
            btnSave.Click += BtnSave_Click;
            btnGrph.Click += BtnGrph_Click;
            path = Directory.GetCurrentDirectory() + "test.BMP";
            if (!File.Exists(path))
            {
                File.Create(path);
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

            FileStream sWr = new FileStream(path, FileMode.Create);
            GenerateImage((BitmapSource)imgPanel.Source, ImageFormat.BMP, sWr);
            sWr.Close();
        }

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            WriteableBitmap wb = new WriteableBitmap((int)this.ActualWidth, (int)this.ActualHeight, 96, 96, PixelFormats.Bgr32, null);
            byte blue;
            byte green;
            byte red;
            byte alpha;
            int strid;
            Int32Rect Rect;
            Random rd;
            rd = new Random();
            for (int x = 0; x < wb.PixelWidth; x++)
            {
                for (int y = 0; y < wb.PixelHeight; y++)
                {
                    blue = (byte)rd.Next(0, 256);
                    green = (byte)rd.Next(0, 256);
                    red = (byte)rd.Next(0, 256);
                    alpha = (byte)rd.Next(0, 256);
                    byte[] colorData = { blue, green, red, alpha };
                    Rect = new Int32Rect(x, y, 1, 1);
                    strid = wb.PixelWidth * wb.Format.BitsPerPixel / 8;
                    wb.WritePixels(Rect, colorData, strid, 0);
                }
            }
            for (int x = 100; x < 110; x++)
            {
                for (int y = 0; y < wb.PixelHeight; y++)
                {
                    //blue  = (byte)21;
                    //green = (byte)215;
                    //red   = (byte)128;
                    //alpha = (byte)111;
                    blue = 0;
                    green = 0;
                    red = 0;
                    alpha = 0;

                    byte[] colorData = { blue, green, red, alpha };
                    Rect = new Int32Rect(x, y, 1, 1);
                    strid = wb.PixelWidth * wb.Format.BitsPerPixel / 8;
                    wb.WritePixels(Rect, colorData, strid, 0);
                }
            }
            for (int x = 0; x < wb.PixelWidth; x++)
            {
                for (int y = 100; y < 110; y++)
                {
                    blue = (byte)21;
                    green = (byte)215;
                    red = (byte)128;
                    alpha = (byte)111;
                    byte[] colorData = { blue, green, red, alpha };
                    Rect = new Int32Rect(x, y, 1, 1);
                    strid = wb.PixelWidth * wb.Format.BitsPerPixel / 8;
                    wb.WritePixels(Rect, colorData, strid, 0);
                }
            }
            imgPanel.Source = wb;

            FileStream sWr = new FileStream(path, FileMode.Create);
            GenerateImage(wb, ImageFormat.BMP, sWr);
            sWr.Close();
        }

        private void GenerateImage(BitmapSource bitmap, ImageFormat format, Stream destStream)
        {
            BitmapEncoder encoder = null;

            switch (format)
            {
                case ImageFormat.BMP:
                    encoder = new BmpBitmapEncoder();
                    break;
                case ImageFormat.GIF:
                    encoder = new GifBitmapEncoder();
                    break;
                case ImageFormat.JPG:
                    encoder = new JpegBitmapEncoder();
                    break;
                case ImageFormat.PNG:
                    encoder = new PngBitmapEncoder();
                    break;
                case ImageFormat.TIF:
                    encoder = new TiffBitmapEncoder();
                    break;
                default:
                    throw new InvalidOperationException();
            }

            encoder.Frames.Add(BitmapFrame.Create(bitmap));
            encoder.Save(destStream);
        }


        //https://blog.csdn.net/jiuzaizuotian2014/article/details/81279423
        //利用不安全代码拷贝
        private void BtnGrph_Click(object sender, RoutedEventArgs e)
        {
            Bitmap bot = new Bitmap(400, 400);
            Graphics gs = Graphics.FromImage(bot);
            System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Red);
            int num1 = 50;
            int num2 = 50;
            int num3 = 50;
            gs.DrawLine(pen, num1, num2 + num3, num1, num2 - num3);
            gs.DrawLine(pen, num1 - num3, num2, num1 + num3, num2);
            gs.DrawRectangle(pen, 12, 12, 6, 6);

            int bw = bot.Width, bh = bot.Height;
            WriteableBitmap writeableBitmap = new WriteableBitmap(400, 400, 96, 96, PixelFormats.Bgra32, null);
            int rPixelBytes = 400 * 400 * 4;
            BitmapData data = bot.LockBits(new System.Drawing.Rectangle(0, 0, bw, bh), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            writeableBitmap.Lock();
            unsafe
            {
                Buffer.MemoryCopy(data.Scan0.ToPointer(), writeableBitmap.BackBuffer.ToPointer(), rPixelBytes, rPixelBytes);
            }
            writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, bw, bh));
            writeableBitmap.Unlock();
            bot.UnlockBits(data);

            imgPanel.Source = writeableBitmap;
            bot.Dispose();
            pen.Dispose();
        }
    }
}
