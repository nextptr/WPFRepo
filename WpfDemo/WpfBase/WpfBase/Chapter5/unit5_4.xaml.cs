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

namespace WpfBase.Chapter5
{
    /// <summary>
    /// unit5_4.xaml 的交互逻辑
    /// </summary>
    public partial class unit5_4 : UserControl
    {
        public unit5_4()
        {
            InitializeComponent();
            this.Tag = "鼠标输入";
            this.MouseMove += Unit5_4_MouseMove;
            btn_clear.Click += Btn_clear_Click;
            PreviewMouseDown += Unit5_4_MouseClick;
            PreviewMouseUp += Unit5_4_MouseClick;
            MouseDown += Unit5_4_MouseClick;
            MouseUp += Unit5_4_MouseClick;

            MouseEnter += Unit5_4_MouseEnter;
            MouseLeave += Unit5_4_MouseEnter;
            InitDrag();
            InitMove();
        }



        //鼠标事件
        private void Unit5_4_MouseEnter(object sender, MouseEventArgs e)
        {
            string msg = $"Event:{e.RoutedEvent}";
            list_event.Items.Add(msg);
        }

        private void Unit5_4_MouseClick(object sender, MouseButtonEventArgs e)
        {
            string msg = $"Event:{e.RoutedEvent}";
            list_event.Items.Add(msg);
        }


        private void Unit5_4_MouseMove(object sender, MouseEventArgs e)
        {
            Point pos = e.GetPosition(canv_board);
            lab_pos.Content = $"Canves坐标({pos.X},{pos.Y})";
        }
        private void Btn_clear_Click(object sender, RoutedEventArgs e)
        {
            list_event.Items.Clear();
        }


        //鼠标拖放
        private int count = 0;
        private void InitDrag()
        {
            lab_source.MouseDown += Lab_source_MouseDown;

            lab_target.DragEnter += dragEnter;
            txt_target.DragEnter += dragEnter;

            lab_target.Drop += Lab_target_Drop;
            txt_target.Drop += Txt_target_Drop;
        }
        private void Lab_source_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Label lab = (Label)lab_source;
            DragDrop.DoDragDrop(lab, lab.Content, DragDropEffects.Copy);
        }

        private void dragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
                e.Effects = DragDropEffects.Copy;
            else
                e.Effects = DragDropEffects.None;
        }

        private void Txt_target_Drop(object sender, DragEventArgs e)
        {
            txt_target.Text = e.Data.GetData(DataFormats.Text).ToString();
        }
        private void Lab_target_Drop(object sender, DragEventArgs e)
        {
            count++;
            ((Label)sender).Content = count.ToString()+ e.Data.GetData(DataFormats.Text);
        }

        //鼠标拖动
        double ori_x = 0.0;
        double ori_y = 0.0;
        bool enableMove = false;
        private void InitMove()
        {
            lab_move.MouseLeftButtonDown += Lab_move_MouseLeftButtonDown;
            lab_move.MouseMove += Lab_move_MouseMove;
            lab_move.MouseLeftButtonUp += Lab_move_MouseLeftButtonUp;
        }

        private void Lab_move_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //创建鼠标捕捉
            Mouse.Capture(lab_move);
            enableMove = true;
            ori_x = e.GetPosition(lab_move).X;
            ori_y = e.GetPosition(lab_move).Y;
        }
        private void Lab_move_MouseMove(object sender, MouseEventArgs e)
        {
            //移动中实时更新位置
            if (enableMove)
            {
                var pos_x = e.GetPosition(canv_move).X - ori_x;
                var pos_y = e.GetPosition(canv_move).Y - ori_y;
                Canvas.SetLeft(lab_move, pos_x);
                Canvas.SetTop(lab_move, pos_y);
            }
        }
        private void Lab_move_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //释放鼠标捕捉
            lab_move.ReleaseMouseCapture();
            enableMove = false;
        }
    }
}
