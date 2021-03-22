using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ScreenSnap
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BitmapSource imageData = Tool.ScreenSnapshot1(130,343,1200,585);
                if (imageData == null)
                {
                    return;
                }

                SaveImageToFile(imageData, RootPath.G_PicturePath + "\\" + DateTime.Now.ToString("hhmmssfff") + ".png");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveImageToFile(BitmapSource image, string filePath)
        {
            BitmapEncoder encoder = GetBitmapEncoder(filePath);
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                encoder.Save(stream);
            }
        }

        /// <summary>
        /// 根据文件扩展名获取图片编码器
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>图片编码器</returns>
        private BitmapEncoder GetBitmapEncoder(string filePath)
        {
            var extName = System.IO.Path.GetExtension(filePath).ToLower();
            if (extName.Equals(".png"))
            {
                return new PngBitmapEncoder();
            }
            else
            {
                return new JpegBitmapEncoder();
            }
        }

    }
    public class RootPath
    {
        public static string Root
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        private static string _G_PicturePath;
        public static string G_PicturePath
        {
            set
            {
                _G_PicturePath = value;
            }
            get
            {
                _G_PicturePath = System.IO.Path.Combine(RootPath.Root, "Picture");
                return _G_PicturePath;
            }
        }
    }
}
