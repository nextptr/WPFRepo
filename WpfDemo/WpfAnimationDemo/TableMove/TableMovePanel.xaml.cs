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
using WpfAnimationDemo.Attach;

namespace WpfAnimationDemo.TableMove
{
    /// <summary>
    /// TableMovePanel.xaml 的交互逻辑
    /// </summary>
    public partial class TableMovePanel : UserControl
    {
        private Storyboard storyboard = null;
        public TableMovePanel()
        {
            InitializeComponent();
        }
        //开始
        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            MotionControl.Instance.SetFrameWork(tb1, new Point(10, 10), new Point(400, 600));
            MotionControl.Instance.Start1();
            tb2.Start(new Point(10, 10), new Point(500, 600), 5000);
        }

        //暂停
        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            MotionControl.Instance.Pause();

            tb2.RePlase(new Point(500, 600), new Point(30, 200));
        }

        //继续
        private void Button_Click3(object sender, RoutedEventArgs e)
        {
            tb2.SetPose(new Point(200, 30));
        }

        //结束
        private void Button_Click4(object sender, RoutedEventArgs e)
        {
            MotionControl.Instance.Stop();
        }

        //多动画执行
        private void Button_Click5(object sender, RoutedEventArgs e)
        {
            tb1.toPos(new Point(500, 100), 1000, true);
            tb1.toPos(new Point(500, 500), 1000, true);
            tb1.toPos(new Point(100, 500), 1000, true);
        }

        private void Button_Click6(object sender, RoutedEventArgs e)
        {
            tb1.toPos(new Point(500, 100), 1000, true);
            tb1.toPos(new Point(500, 500), 1000, true);
            tb1.toPos(new Point(100, 500), 1000, true);
        }

        private void Button_Click7(object sender, RoutedEventArgs e)
        {
            tb1.toPos(new Point(500, 100), 1000, true);
            tb1.toPos(new Point(500, 500), 1000, true);
            tb1.toPos(new Point(100, 500), 1000, true);
        }

        //table

        private void Table_Click1(object sender, RoutedEventArgs e)
        {
            tbt.ToPoint(new Point(200, 200), 1000);
        }

        ///ttttttttttttttt
        private Storyboard _storyboard;
        public string LocationString
        {
            get { return (string)GetValue(LocationStringProperty); }
            set { SetValue(LocationStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LocationString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LocationStringProperty =
            DependencyProperty.Register("LocationString", typeof(string), typeof(MainWindow), new PropertyMetadata("0,0"));



        private void Start_Click(object sender, RoutedEventArgs e)
        {
            var animation = new PointAnimation()
            {
                From = new Point(10, 100),
                To = new Point(510, 100),
                Duration = new Duration(TimeSpan.FromSeconds(10))
            };
            //Storyboard.SetTargetName(animation, "Left");
            Storyboard.SetTargetProperty(animation, new PropertyPath(LocationAttach.LocationProperty));
            _storyboard = new Storyboard();
            _storyboard.Children.Add(animation);
            this.RegisterName("Location", Rect0);
            _storyboard.Begin(Rect0, true);
        }

        private bool pauseflag;
        private void Pause_Click(object sendr, RoutedEventArgs e)
        {
            var state = _storyboard?.GetCurrentState(Rect0);
            if (!state.Equals(ClockState.Active))
            {
                return;
            }
            if (!pauseflag)
            {
                _storyboard.Pause(Rect0);
            }
            else
            {
                _storyboard.Resume(Rect0);
            }
            pauseflag = !pauseflag;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            _storyboard.Stop(Rect0);
            pauseflag = false;
        }

        private void ToPoint_Click(object sender, RoutedEventArgs e)
        {
            var animation = _storyboard?.Children.FirstOrDefault() as PointAnimation;
            if (animation != null)
            {
                try
                {
                    var newLoc = Point.Parse(LocationString);
                    animation.From = (Point)Rect0.GetValue(LocationAttach.LocationProperty);
                    animation.To = newLoc;
                    animation.Duration = new Duration(TimeSpan.FromSeconds(3));
                    _storyboard.Begin(Rect0, true);
                    pauseflag = false;
                }
                catch
                {
                    MessageBox.Show("坐标点格式错误！");
                }
            }
        }
    }
}
