using CVbox.Common.CV;
using CVWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CVbox.Common
{
    public class CSImage: NotifyPropertyChanged
    {
        private WriteableBitmap bitMap;

        public int PixelWidth
        {
            get 
            {
                return bitMap.PixelWidth; 
            }
        }
        public int PixelHeight
        {
            get
            {
                return bitMap.PixelHeight;
            }
        }
        public int PixCount
        {
            get
            {
                return PixelWidth * PixelWidth;
            }
        }
        public PixelFormat PixelFormat
        {
            get
            {
                return bitMap.Format;
            }
        }
        public WriteableBitmap BitMap
        {
            get { return bitMap; }
            set
            {
                bitMap = value;
                NotifyOfPropertyChange(nameof(BitMap));
            }
        }

        public CSImage(Mat_CPP mat)
        {
            var pixelFormat = CVHelper.CVTypeConvert(mat.Type);
            BitMap = new WriteableBitmap(mat.Width, mat.Height, 96, 96, pixelFormat, null);
            unsafe
            {
                BitMap.Lock();
                Buffer.MemoryCopy(mat.Ptr.ToPointer(), BitMap.BackBuffer.ToPointer(), PixelWidth * PixelHeight, PixelWidth * PixelHeight);
                BitMap.AddDirtyRect(new Int32Rect(0, 0, BitMap.PixelWidth, BitMap.PixelHeight));
                BitMap.Unlock();
            }
        }
        public CSImage(WriteableBitmap Source)
        {
            BitMap = new WriteableBitmap(Source);
        }
        public CSImage(int pixWidth, int pixHeight, PixelFormat pixelFormat)
        {
            BitMap = new WriteableBitmap(pixWidth, pixHeight, 96, 96, pixelFormat, null);
        }

        public Mat_CPP ConvertToMatCPP()
        {
            if (this.BitMap == null)
                return null;
            Mat_CPP matSrc = new Mat_CPP();
            matSrc.Width = this.PixelWidth;
            matSrc.Height = this.PixelHeight;
            matSrc.Type = CVHelper.CVTypeConvert(this.PixelFormat);
            matSrc.Ptr = this.BitMap.BackBuffer;
            return matSrc;
        }
        public void CopyFrom(Mat_CPP mat)
        {
            if (BitMap == null)
            {
                var format = CVHelper.CVTypeConvert(mat.Type);
                BitMap = new WriteableBitmap(mat.Width, mat.Height, 96, 96, format, null);
            }
            int srcCount = mat.Width * mat.Height;
            int dstCount = PixelWidth * PixelHeight;
            int copyCount = srcCount > dstCount ? dstCount : srcCount;

            unsafe
            {
                BitMap.Lock();
                Buffer.MemoryCopy(mat.Ptr.ToPointer(), BitMap.BackBuffer.ToPointer(), copyCount, copyCount);
                BitMap.AddDirtyRect(new Int32Rect(0, 0, BitMap.PixelWidth, BitMap.PixelHeight));
                BitMap.Unlock();
            }
        }
    }
}
