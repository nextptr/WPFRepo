using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace FanBlade
{
    /// <summary>
    /// FanBladeControl.xaml 的交互逻辑
    /// </summary>
    public partial class FanBladeControl : UserControl
    {
        private Storyboard storyboard = null;
        MemoryStream stream = null;

        public FanBladeControl()
        {
            InitializeComponent();
            this.SizeChanged += FanBlade_SizeChanged;
        }
        private void FanBlade_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (IsVisible == true)
            {
                double width = this.ActualWidth;
                double height = this.ActualHeight;
                if (width > height)
                {
                    Scale = height / 130;
                }
                else
                {
                    Scale = width / 130;
                }
            }
        }
        public bool StartRolling()
        {
            try
            {
                if (storyboard == null)
                {
                    //InitImgSource(image);
                    InitImg(image);
                    storyboard = new Storyboard();
                    DoubleAnimationUsingKeyFrames frams = new DoubleAnimationUsingKeyFrames();//关键帧
                    KeyTime staTim = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0)); //第一帧的时间
                    KeyTime endTim = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1000));//第二帧的时间
                    frams.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = staTim, Value = 0 });    //第一帧的值
                    frams.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = endTim, Value = 360 });  //第二帧的值
                    storyboard.Children.Add(frams);

                    Storyboard.SetTargetProperty(frams, new PropertyPath("(UIElement.RenderTransform).(TransformGroup.Children)[2].(RotateTransform.Angle)"));
                    Storyboard.SetTargetName(frams, "image");
                    storyboard.RepeatBehavior = RepeatBehavior.Forever;
                }
                storyboard.Resume(image);
                storyboard.Begin(image, true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        public bool PauseRolling()
        {
            if (storyboard != null)
            {
                storyboard.Pause(image);
                return true;
            }
            return false;
        }
        public bool ResumeRolling()
        {
            if (storyboard != null)
            {
                storyboard.Resume(image);
                return true;
            }
            return false;
        }
        public bool StopRolling()
        {
            if (storyboard != null)
            {
                storyboard.Stop(image);
                return true;
            }
            return false;
        }
        public void Clear()
        {
            if (storyboard != null)
            {
                storyboard.Children.Clear();
                storyboard.Remove(image);
                image.Source = null;
                image.UpdateLayout();
                image = null;
                stream.Dispose();
                stream.Close();
                stream = null;
            }
        }

        /// <summary>
        /// CycleCountEachSecond 每秒钟圈数
        /// 不能小于零
        /// 大于60是可以的，但是人眼无法分辨没有意义
        /// </summary>
        public double CycleCountEachSecond
        {
            get
            {
                return storyboard.SpeedRatio;
            }
            set
            {
                if (value <= 0)
                {
                    return;
                }
                storyboard.SetSpeedRatio(image, value);
            }
        }
        public double Scale
        {
            set
            {
                if (value <= 0)
                {
                    return;
                }
                transform_scale.ScaleX = value;
                transform_scale.ScaleY = value;
                if (value < 1)
                {
                    double mag = -1 * (130 * (1 - value)) / 2;
                    image.Margin = new Thickness(mag);
                }
                else
                {
                    image.Margin = new Thickness(0);
                }
            }
            get
            {
                return transform_scale.ScaleX;
            }
        }
        private void InitImgSource(System.Windows.Controls.Image img)
        {
            var bi = new BitmapImage { CacheOption = BitmapCacheOption.OnLoad, CreateOptions = BitmapCreateOptions.DelayCreation | BitmapCreateOptions.IgnoreImageCache };
            bi.BeginInit();
            bi.UriSource = new Uri(@"fan.bmp", UriKind.Relative);
            bi.EndInit();
            img.Source = bi;
        }

        private void InitImg(System.Windows.Controls.Image img)
        {
            Stream stm = Assembly.GetExecutingAssembly().GetManifestResourceStream(Assembly.GetExecutingAssembly().GetName().Name + ".fan.bmp");
            stm.Position = 0;
            BinaryReader br = new BinaryReader(stm);
            byte[] FacePicture = br.ReadBytes((int)stm.Length);
            br.Close();
            stm.Close();

            ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
            stream = new MemoryStream(FacePicture);
            BitmapFrame source = imageSourceConverter.ConvertFrom(stream) as BitmapFrame;
            img.Source = source;
        }
    }
}
