using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfAnimationDemo.TableMove
{
    /// <summary>
    /// TransferTableItem.xaml 的交互逻辑
    /// </summary>
    public partial class TransferTableItem : UserControl
    {
        public string Title { get; set; }

        private Storyboard storyboard = null;
        private FrameworkElement _element = null;
        private DoubleAnimation anim_x = null;
        private DoubleAnimation anim_y = null;

        private Queue<animItem> animationList = new Queue<animItem>();

        public TransferTableItem()
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
            storyboard.Completed += Storyboard_Completed;
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            toNextPos();
        }

        public void Start(Point pos_sta, Point pos_end,int time)
        {
            anim_x.From = pos_sta.X;
            anim_x.To = pos_end.X;
            anim_x.Duration = new Duration(TimeSpan.FromMilliseconds(time));

            anim_y.From = pos_sta.Y;
            anim_y.To = pos_end.Y;
            anim_y.Duration = new Duration(TimeSpan.FromMilliseconds(time));

            storyboard.Children.Add(anim_x);
            storyboard.Children.Add(anim_y);
            storyboard.Begin();

            //storyboard = new Storyboard();
            ////创建X轴方向动画 
            //DoubleAnimation doubleAnimation = new DoubleAnimation(
            //                                  pos_sta.X,
            //                                  pos_end.X,
            //                                  new Duration(TimeSpan.FromMilliseconds(lxspeed))
            //                                 );
            //Storyboard.SetTarget(doubleAnimation, _element);
            //Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Left)"));
            //storyboard.Children.Add(doubleAnimation);
            ////创建Y轴方向动画 
            //doubleAnimation = new DoubleAnimation(
            //                  pos_sta.Y,
            //                  pos_end.Y,
            //                  new Duration(TimeSpan.FromMilliseconds(lyspeed))
            //                 );
            //Storyboard.SetTarget(doubleAnimation, _element);
            //Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("(Canvas.Top)"));
            //storyboard.Children.Add(doubleAnimation);
            //动画播放 
            //storyboard.Begin();
        }

        private void toNextPos()
        {
            animItem item = animationList.Dequeue();
            anim_x.From = Canvas.GetLeft(this);
            anim_x.To = item.pos.X;
            anim_x.Duration = new Duration(TimeSpan.FromMilliseconds(item.Tim));

            anim_y.From = Canvas.GetTop(this);
            anim_y.To = item.pos.Y;
            anim_y.Duration = new Duration(TimeSpan.FromMilliseconds(item.Tim));

            storyboard.Children[0] = anim_x;
            storyboard.Children[1] = anim_y;
            storyboard.Begin();
        }
        public void toPos(Point pos_end, int time,bool isWait)
        {
            animationList.Enqueue(new animItem(pos_end, time));
            if (animationList.Count == 0||isWait==false)
            {
                toNextPos();
            }
        }


        public void RePlase( Point pos1,Point pos2 )
        {
            anim_x.From = pos1.X;
            anim_x.To = pos2.X;

            anim_y.From = pos1.Y;
            anim_y.To = pos2.Y;

            storyboard.Children[0] = anim_x;
            storyboard.Children[1] = anim_y;
            storyboard.Begin();
        }

        public void SetPose(Point pos)
        {
            SetValue(Canvas.LeftProperty, pos.X);
            SetValue(Canvas.TopProperty, pos.Y);
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


        public void Lock(int tim)
        {
            Thread.Sleep(tim);
        }
    }

    public class animItem
    {
        public Point pos = new Point();
        public int Tim = 0;

        public animItem()
        {
        }

        public animItem(Point ps,int tm)
        {
            pos.X = ps.X;
            pos.Y = ps.Y;
            Tim = tm;
        }
    }

}
