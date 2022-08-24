using CVbox.Common;
using CVbox.Panel.ProjPage.View;
using Stylet;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CVWrapper;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Threading;
using System.Threading.Tasks;

namespace CVbox.Panel.ProjPage.ViewModel
{
    public class ImgCvtTestViewModel : Screen,IPage
    {
        public string Name { get; set; } = "BGR转灰度测试";

        private CSImage srcImg;
        public CSImage SrcImg
        {
            get { return srcImg; }
            set
            {
                srcImg = value;
                NotifyOfPropertyChange(nameof(SrcImg));
            }
        }

        private CSImage dstImg;
        public CSImage DstImg
        {
            get { return dstImg; }
            set
            {
                dstImg = value;
                NotifyOfPropertyChange(nameof(DstImg));
            }
        }

        Random rd;
        Dispatcher dic;
        System.Timers.Timer tm;
        public ImgCvtTestViewModel()
        {
            dic = App.Current.Dispatcher;
            rd = new Random(DateTime.Now.Millisecond);
            tm = new System.Timers.Timer(200);
            SrcImg = new CSImage(400, 400, PixelFormats.Bgr32);

            tm.AutoReset = true;
            tm.Elapsed += Tm_Elapsed;
            init();
        }

        private void init()
        {
            //int strid;
            //Int32Rect Rect;
            //for (int x = 0; x < SrcImg.PixelWidth; x++)
            //{
            //    for (int y = 0; y < SrcImg.PixelHeight; y++)
            //    {
            //        byte[] colorData = { 0, 0, 0, 255 };
            //        Rect = new Int32Rect(x, y, 1, 1);
            //        strid = SrcImg.PixelWidth * SrcImg.BitMap.Format.BitsPerPixel / 8;
            //        SrcImg.BitMap.WritePixels(Rect, colorData, strid, 0);
            //    }
            //}

            Int32Rect Rect;
            for (int x = 0; x < SrcImg.PixelWidth; x++)
            {
                for (int y = 0; y < SrcImg.PixelHeight; y++)
                {
                    byte[] colorData = { 255, 255, 255, 255 };
                    Rect = new Int32Rect(x, y, 1, 1);
                    SrcImg.BitMap.WritePixels(Rect, colorData, SrcImg.BitMap.BackBufferStride, 0);
                }
            }
            //for (int x = 0; x < SrcImg.PixelWidth; x += 4)
            //{
            //    for (int y = 0; y < SrcImg.PixelHeight; y += 4)
            //    {
            //        for (int a = x; a < x+2; a++)
            //        {
            //            for (int b = y; b < y+2; b++)
            //            {
            //                byte[] colorData = { 0, 0, 0, 255 };
            //                Rect = new Int32Rect(a, b, 1, 1);
            //                SrcImg.BitMap.WritePixels(Rect, colorData, SrcImg.BitMap.BackBufferStride, 0);
            //            }
            //        }
            //    }
            //}

            //for (int x = 0; x < SrcImg.PixelWidth; x += 4)
            //{
            //    for (int y = 0; y < SrcImg.PixelHeight; y++)
            //    {
            //        for (int a = x; a < x + 2; a++)
            //        {
            //            byte[] colorData = { 0, 0, 0, 255 };
            //            Rect = new Int32Rect(a, y, 1, 1);
            //            SrcImg.BitMap.WritePixels(Rect, colorData, SrcImg.BitMap.BackBufferStride, 0);
            //        }
            //    }
            //}

            for (int x = 0; x < SrcImg.PixelWidth; x++)
            {
                for (int y = 0; y < SrcImg.PixelHeight; y+=4)
                {
                    for (int a = y; a < y + 2; a++)
                    {
                        byte[] colorData = { 0, 0, 0, 255 };
                        Rect = new Int32Rect(x, a, 1, 1);
                        SrcImg.BitMap.WritePixels(Rect, colorData, SrcImg.BitMap.BackBufferStride, 0);
                    }
                }
            }
        }

        

        public void btnCvt(string arg)
        {
            switch (arg)
            {
                case "1":
                    RandomRgb();
                    break;
                case "2":
                    RgbCvtGray();
                    break;
                default:
                    break;
            }
        }


        private void Tm_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            int strid;
            Int32Rect Rect;
            dic.Invoke(() =>
            {
                byte[] colorData = { 0, 0, 125, 0 };
                int x = rd.Next(0, 400);
                int y = rd.Next(0, 400);
                Rect = new Int32Rect(x, y, 1, 1);
                strid = SrcImg.BitMap.PixelWidth * SrcImg.BitMap.Format.BitsPerPixel / 8;
                SrcImg.BitMap.WritePixels(Rect, colorData, strid, 0);
            });
        }
        static bool rdmFlg = false;
        private void RandomRgb()
        {
            rdmFlg = !rdmFlg;
            if (rdmFlg)
            {
                tm.Start();
            }
            else
            {
                tm.Stop();
            }
        }
        private void RgbCvtGray()
        {
            //生成图片
            WriteableBitmap writBit = new WriteableBitmap(400, 400, 96, 96, PixelFormats.Bgr32, null);
            byte blue;
            byte green;
            byte red;
            byte alpha;
            int strid;
            Int32Rect Rect;
            Random rd;
            rd = new Random();
            for (int x = 0; x < writBit.PixelWidth; x++)
            {
                for (int y = 0; y < writBit.PixelHeight; y++)
                {
                    blue = (byte)rd.Next(0, 256);
                    green = (byte)rd.Next(0, 256);
                    red = (byte)rd.Next(0, 256);
                    alpha = (byte)rd.Next(0, 256);
                    byte[] colorData = { blue, green, red, alpha };
                    Rect = new Int32Rect(x, y, 1, 1);
                    strid = writBit.PixelWidth * writBit.Format.BitsPerPixel / 8;
                    SrcImg.BitMap.WritePixels(Rect, colorData, strid, 0);
                }
            }
            for (int x = 100; x < 110; x++)
            {
                for (int y = 0; y < writBit.PixelHeight; y++)
                {
                    blue = 0;
                    green = 0;
                    red = 0;
                    alpha = 0;
                    byte[] colorData = { blue, green, red, alpha };
                    Rect = new Int32Rect(x, y, 1, 1);
                    strid = writBit.PixelWidth * writBit.Format.BitsPerPixel / 8;
                    SrcImg.BitMap.WritePixels(Rect, colorData, strid, 0);
                }
            }
            for (int x = 0; x < writBit.PixelWidth; x++)
            {
                for (int y = 100; y < 110; y++)
                {
                    blue = (byte)21;
                    green = (byte)215;
                    red = (byte)128;
                    alpha = (byte)111;
                    byte[] colorData = { blue, green, red, alpha };
                    Rect = new Int32Rect(x, y, 1, 1);
                    strid = writBit.PixelWidth * writBit.Format.BitsPerPixel / 8;
                    SrcImg.BitMap.WritePixels(Rect, colorData, strid, 0);
                }
            }

            Mat_CPP matSrc = new Mat_CPP();
            matSrc.Width = SrcImg.PixelWidth;
            matSrc.Height = SrcImg.PixelHeight;
            matSrc.Type = ImgType_CPP.Bgr32;
            matSrc.Ptr = SrcImg.BitMap.BackBuffer;

            Mat_CPP matDst = new Mat_CPP();
            //转化为灰度图
            CVproxy.CVproxyInstance.ToolBGR2Gray(matSrc, ref matDst);
            DstImg =new CSImage(matDst);

            //WriteableBitmap disBit = new WriteableBitmap(400, 400, 96, 96, PixelFormats.Gray8, null);
            //unsafe
            //{
            //    disBit.Lock();
            //    Buffer.MemoryCopy(matDst.Ptr.ToPointer(), disBit.BackBuffer.ToPointer(), 400 * 400, 400 * 400);
            //    disBit.AddDirtyRect(new Int32Rect(0, 0, 400, 400));
            //    disBit.Unlock();
            //}
            //DstImg.BitMap = disBit;
        }
    }
}
