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

namespace WpfDemo.WPF动画.FanBlade
{
    /// <summary>
    /// FanBladePanel.xaml 的交互逻辑
    /// </summary>
    public partial class FanBladePanel : UserControl
    {
        List<FanBladeControl> ls_fan = new List<FanBladeControl>();
        int test_count = 30;
        public FanBladePanel()
        {
            InitializeComponent();
            txt_speed.TextChanged += Txt_speed_TextChanged;
            btn_start.Click += Btn_start_Click;
            btn_stop.Click += Btn_stop_Click;
            btn_pause.Click += Btn_pause_Click;
            btn_resume.Click += Btn_resume_Click;
            btn_restart.Click += Btn_restart_Click;
            this.Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.IsVisible == true)
            {
                initGrid(test_count);
                initFans(test_count);
                this.Loaded -= MainWindow_Loaded;
            }
        }

        private void initGrid(int count)
        {
            grid_board.RowDefinitions.Clear();
            grid_board.ColumnDefinitions.Clear();
            grid_board.Children.Clear();
            //动态生成布局器
            for (int i = 0; i < count; i++)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength(1, GridUnitType.Star);
                grid_board.RowDefinitions.Add(rd);
            }
            for (int i = 0; i < count; i++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = new GridLength(1, GridUnitType.Star);
                grid_board.ColumnDefinitions.Add(cd);
            }
        }
        private void initFans(int count)
        {
            //初始化fan
            ls_fan.Clear();
            ls_fan = new List<FanBladeControl>();
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    FanBladeControl fan = new FanBladeControl();
                    ls_fan.Add(fan);
                    grid_board.Children.Add(fan);
                    Grid.SetColumn(fan, j);
                    Grid.SetRow(fan, i);
                }
            }
        }

        private void Txt_speed_TextChanged(object sender, TextChangedEventArgs e)
        {
            double speed = -1;
            double.TryParse(txt_speed.Text, out speed);
            foreach (var fan in ls_fan)
            {
                fan.CycleCountEachSecond = speed;
            }
        }

        private void Btn_restart_Click(object sender, RoutedEventArgs e)
        {
            grid_board.Children.Clear();
            foreach (FanBladeControl fan in ls_fan)
            {
                fan.Clear();
            }
            ls_fan.Clear();
            initFans(test_count);
            foreach (var fan in ls_fan)
            {
                fan.StartRolling();
            }
        }

        private void Btn_start_Click(object sender, RoutedEventArgs e)
        {
            foreach (var fan in ls_fan)
            {
                fan.StartRolling();
            }
        }

        private void Btn_stop_Click(object sender, RoutedEventArgs e)
        {
            foreach (var fan in ls_fan)
            {
                fan.StopRolling();
            }

        }

        private void Btn_pause_Click(object sender, RoutedEventArgs e)
        {
            foreach (var fan in ls_fan)
            {
                fan.PauseRolling();
            }
        }

        private void Btn_resume_Click(object sender, RoutedEventArgs e)
        {
            foreach (var fan in ls_fan)
            {
                fan.ResumeRolling();
            }
        }
    }
}
