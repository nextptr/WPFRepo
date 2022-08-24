using CVbox.Common;
using CVWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CVbox.Common.CV
{
    /*
    public bool DivideImageIntoRGBBytes(CameraData image, out IntPtr IntPtr_R, out IntPtr IntPtr_G, out IntPtr IntPtr_B)
    {

        IntPtr_R = IntPtr.Zero;
        IntPtr_G = IntPtr.Zero;
        IntPtr_B = IntPtr.Zero;

        byte[] imageRBytes = new byte[image.Width * image.Height];
        byte[] imageGBytes = new byte[image.Width * image.Height];
        byte[] imageBBytes = new byte[image.Width * image.Height];

        for (int i = 0; i < image.Height; i++)
        {
            for (int j = 0, k = 0; j < image.Width; j++, k += 3)
            {
                imageBBytes[image.Width * i + j] = image.BufferData[image.Width * i * 3 + k];
                imageGBytes[image.Width * i + j] = image.BufferData[image.Width * i * 3 + k + 1];
                imageRBytes[image.Width * i + j] = image.BufferData[image.Width * i * 3 + k + 2];
            }
        }
        IntPtr_R = ImageHelper.BytesToIntPtrMarshal(imageRBytes);
        IntPtr_G = ImageHelper.BytesToIntPtrMarshal(imageGBytes);
        IntPtr_B = ImageHelper.BytesToIntPtrMarshal(imageBBytes);
        return true;
    }
    */

    public class CVHelper
    {
        public static PixelFormat CVTypeConvert(ImgType_CPP tp)
        {
            PixelFormat pixelFormat = PixelFormats.Default;
            switch (tp)
            {
                case ImgType_CPP.Gray2:
                    pixelFormat = PixelFormats.Gray2;
                    break;
                case ImgType_CPP.Gray8:
                    pixelFormat = PixelFormats.Gray8;
                    break;
                case ImgType_CPP.Bgr32:
                    pixelFormat = PixelFormats.Bgr32;
                    break;
                case ImgType_CPP.Bgra32:
                    pixelFormat = PixelFormats.Bgra32;
                    break;
                default:
                    break;
            }
            return pixelFormat;
        }
        public static ImgType_CPP CVTypeConvert(PixelFormat tp)
        {
            if (tp == PixelFormats.Gray2)
            {
                return ImgType_CPP.Gray2;
            }
            else if (tp == PixelFormats.Gray8)
            {
                return ImgType_CPP.Gray8;
            }
            else if (tp == PixelFormats.Bgr32)
            {
                return ImgType_CPP.Bgr32;
            }
            else if (tp == PixelFormats.Bgra32)
            {
                return ImgType_CPP.Bgra32;
            }
            else
            {
                return ImgType_CPP.Bgr32;
            }
        }
        public static CSImage CV2GrayImg(CSImage oriImg)
        {
            Mat_CPP matSrc = new Mat_CPP();
            Mat_CPP matDst = oriImg.ConvertToMatCPP();
            CVproxy.CVproxyInstance.ToolBGR2Gray(matSrc, ref matDst);
            CSImage disBit = new CSImage(matDst);
            return disBit;
        }

        public static IntPtr BytesToIntPtrMarshal(byte[] bytes)
        {
            try
            {
                IntPtr ptr = Marshal.AllocHGlobal(bytes.Length);
                Marshal.Copy(bytes, 0, ptr, bytes.Length);
                return ptr;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
