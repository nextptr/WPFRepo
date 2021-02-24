using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace WpfAnimationDemo.TableMove
{
    public class MotionControl
    {
        private static MotionControl _instance;
        public static MotionControl Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MotionControl();
                }
                return _instance;
            }
        }
        private MotionControl() { }

        private Storyboard storyboard = null;
        private FrameworkElement _element = null;
        Point pos_sta = new Point();
        Point pos_end = new Point();

        public void SetFrameWork(FrameworkElement element,Point pos1,Point pos2)
        {
            _element = element;
            pos_sta = pos1;
            pos_end = pos2;
        }

        public bool Start()
        {
            try
            {
                if (storyboard == null)
                {
                    storyboard = new Storyboard();
                    DoubleAnimationUsingKeyFrames frams = new DoubleAnimationUsingKeyFrames();//关键帧
                    KeyTime staTim = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0)); //第一帧的时间
                    KeyTime endTim = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(1000));//第二帧的时间
                    frams.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = staTim, Value = 0 });    //第一帧的值
                    frams.KeyFrames.Add(new EasingDoubleKeyFrame() { KeyTime = endTim, Value = 360 });  //第二帧的值
                    storyboard.Children.Add(frams);

                    Storyboard.SetTargetProperty(frams, new PropertyPath("(Canvas.Left)"));
                    Storyboard.SetTargetName(frams, "image");
                    storyboard.RepeatBehavior = RepeatBehavior.Forever;
                }
                storyboard.Resume(_element);
                storyboard.Begin(_element, true);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void Start1()
        {
            Point curPoint = new Point();
            curPoint.X = pos_sta.X;
            curPoint.Y = pos_sta.Y;

            double lxspeed = 1000, lyspeed = 1000; //设置X方向 / Y方向 移动时间片

            storyboard = new Storyboard();
            //创建X轴方向动画 
            DoubleAnimation doubleAnimation = new DoubleAnimation(
                                              pos_sta.X,
                                              pos_end.X,
                                              new Duration(TimeSpan.FromMilliseconds(lxspeed))
                                             );
            Storyboard.SetTarget(doubleAnimation, _element);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));
            storyboard.Children.Add(doubleAnimation);
            //创建Y轴方向动画 
            doubleAnimation = new DoubleAnimation(
                              pos_sta.Y,
                              pos_end.Y,
                              new Duration(TimeSpan.FromMilliseconds(lyspeed))
                             );
            Storyboard.SetTarget(doubleAnimation, _element);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Top)"));
            storyboard.Children.Add(doubleAnimation);
            //动画播放 
            storyboard.Begin();
        }


        public bool Pause()
        {
            if (storyboard != null)
            {
                storyboard.Pause(_element);
                return true;
            }
            return false;
        }
        public bool Resume()
        {
            if (storyboard != null)
            {
                storyboard.Resume(_element);
                return true;
            }
            return false;
        }
        public bool Stop()
        {
            if (storyboard != null)
            {
                storyboard.Stop(_element);
                return true;
            }
            return false;
        }

    }
}
