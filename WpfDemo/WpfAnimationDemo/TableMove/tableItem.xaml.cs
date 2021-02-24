using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace WpfAnimationDemo.TableMove
{
    public partial class tableItem : UserControl
    {
        public string Title { get; set; }
        private Storyboard storyboard = null;
        private FrameworkElement _element = null;
        private DoubleAnimation anim_x = null;
        private DoubleAnimation anim_y = null;

        public delegate void InposEventHandle();
        public event InposEventHandle InposEvent;

        AutoResetEvent animLockEvent = new AutoResetEvent(true);

        public tableItem()
        {
            InitializeComponent();
            //动画初始化
            _element = this;
            storyboard = new Storyboard();
            anim_x = new DoubleAnimation();
            anim_y = new DoubleAnimation();

            storyboard = new Storyboard();
            //创建X轴方向动画 
            Storyboard.SetTarget(anim_x, _element);
            Storyboard.SetTargetProperty(anim_x, new PropertyPath("(Canvas.Left)"));
            //创建Y轴方向动画 
            Storyboard.SetTarget(anim_y, _element);
            Storyboard.SetTargetProperty(anim_y, new PropertyPath("(Canvas.Top)"));

            anim_x.From = 0;
            anim_x.To = 0;
            anim_x.Duration = new Duration(TimeSpan.FromMilliseconds(1));
            anim_y.From = 0;
            anim_y.To = 0;
            anim_y.Duration = new Duration(TimeSpan.FromMilliseconds(1));

            storyboard.Children.Add(anim_x);
            storyboard.Children.Add(anim_y);
            storyboard.Completed += Storyboard_Completed;
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            InposEvent?.Invoke();
            animLockEvent.Set();
        }

        public void PointToPoint(Point pos_sta, Point pos_end, int time)
        {
            anim_x.From = pos_sta.X;
            anim_x.To = pos_end.X;
            anim_x.Duration = new Duration(TimeSpan.FromMilliseconds(time));

            anim_y.From = pos_sta.Y;
            anim_y.To = pos_end.Y;
            anim_y.Duration = new Duration(TimeSpan.FromMilliseconds(time));

            storyboard.Children[0] = anim_x;
            storyboard.Children[1] = anim_y;
            storyboard.Begin();
        }

        public void ToPoint(Point pos_end, int time)
        {
            if (animLockEvent.WaitOne(1000))
            {
                anim_x.From = Canvas.GetLeft(this);
                anim_x.To = pos_end.X;
                anim_x.Duration = new Duration(TimeSpan.FromMilliseconds(time));

                anim_y.From = Canvas.GetTop(this);
                anim_y.To = pos_end.Y;
                anim_y.Duration = new Duration(TimeSpan.FromMilliseconds(time));

                storyboard.Children[0] = anim_x;
                storyboard.Children[1] = anim_y;
                storyboard.Begin();
            }
        }

        public void SetPose(Point pos)
        {
            SetValue(Canvas.LeftProperty, pos.X);
            SetValue(Canvas.TopProperty, pos.Y);
        }

        public void CellIn()
        {
        }
    }
}
