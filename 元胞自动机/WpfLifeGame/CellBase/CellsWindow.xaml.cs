using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfLifeGame.CellBase
{
    /// <summary>
    /// CellsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CellsWindow:UserControl
    {
        public Cells ActionCells;
        public CellsWindow()
        {
            InitializeComponent();
            this.Loaded += Window_Loaded;
            this.Unloaded += Window_Unloaded;

            dg_pattern.SelectedCellsChanged += Dg_pattern_SelectedCellsChanged;
            btn_Save.Click += Btn_Save_Click;
            btn_Start.Click += Btn_Start_Click;
            btn_Reset.Click += Btn_Reset_Click;
            btn_Step.Click += Btn_Step_Click;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.IsVisible)
            {
                this.Loaded -= Window_Loaded;
                //窗口对齐
                double width = this.ActualWidth;
                double height = grid_board.ActualHeight;
                bord_grid.Width = height + 6;
                //初始化
                InitCells(50);
                InitBinding(50);
                //界面绑定
                dg_pattern.ItemsSource = null;
                dg_pattern.ItemsSource = CellsParameter.Instance;
                lab_count.DataContext = ActionCells;
            }
        }
        private void Window_Unloaded(object sender, EventArgs e)
        {
            ActionCells.Stop();
        }
        public void InitCells(int count)
        {
            //动态生成布局器
            for (int i = 0; i < count; i++)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = new GridLength();
                grid_board.RowDefinitions.Add(rd);
            }
            for (int i = 0; i < count; i++)
            {
                ColumnDefinition cd = new ColumnDefinition();
                cd.Width = new GridLength();
                grid_board.ColumnDefinitions.Add(cd);
            }
            //初始化cell
            ActionCells = new Cells(count);
        }
        public void InitBinding(int count)
        {
            double wid = grid_board.ActualHeight / count;
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    //动态绑定
                    Binding binding = new Binding();
                    binding.Source = ActionCells.cells[i][j];
                    binding.Path = new PropertyPath("LiveNow");
                    binding.Mode = BindingMode.TwoWay;

                    ToggleButton btn = new ToggleButton();
                    btn.Style = this.FindResource("GreenToggleButton") as Style;
                    btn.ApplyTemplate();
                    btn.Height = wid;
                    btn.Width = wid;
                    btn.Tag = i.ToString() + "_" + j.ToString();
                    btn.SetBinding(ToggleButton.IsCheckedProperty, binding);

                    grid_board.Children.Add(btn);
                    Grid.SetColumn(btn, j);
                    Grid.SetRow(btn, i);
                }
            }
        }

        private void Dg_pattern_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            int id = dg_pattern.SelectedIndex;
            if (dg_pattern.Items.Count >= 1)
            {
                ActionCells.SetPattern(CellsParameter.Instance[id]);
            }
        }
        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            ActionCells.Suspand();
            string saveName = txt_Name.Text;
            if (CellsParameter.Instance.IsExitPattern(saveName))
            {
                MessageBox.Show("样式已存在请重新命名！");
                return;
            }
            else if (saveName == "")
            {
                MessageBox.Show("请输入名称！");
                return;
            }

            CellsParameter.Instance.Add(new Pattern(saveName, ActionCells.cells));
            dg_pattern.ItemsSource = null;
            dg_pattern.ItemsSource = CellsParameter.Instance;
            dg_pattern.SelectedIndex = dg_pattern.Items.Count - 1;
        }
        private void Btn_Start_Click(object sender, RoutedEventArgs e)
        {
            String str = btn_Start.Content.ToString();
            if (btn_Start.IsChecked == true)
            {
                btn_Start.Content = "暂停";
                ActionCells.Start();
                dg_pattern.IsEnabled = false;
                btn_Save.IsEnabled = false;
                btn_Step.IsEnabled = false;
                btn_Reset.IsEnabled = false;
            }
            else
            {
                btn_Start.Content = "推演";
                ActionCells.Suspand();
                dg_pattern.IsEnabled = true;
                btn_Save.IsEnabled = true;
                btn_Step.IsEnabled = true;
                btn_Reset.IsEnabled = true;
            }
        }
        private void Btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            int id = dg_pattern.SelectedIndex;
            if (dg_pattern.Items.Count >= 1)
            {
                ActionCells.SetPattern(CellsParameter.Instance[id]);
            }
        }

        private void Btn_Step_Click(object sender, RoutedEventArgs e)
        {
            int step = 0;
            int.TryParse(txtStepBox.Text.ToString(), out step);
            if (step <= 0)
            {
                return;
            }
            ActionCells.StepTo(step);
        }
        private void MenuItem_Delete_Click(object sender, RoutedEventArgs e)
        {
            int id = dg_pattern.SelectedIndex;
            if (id >= 0 && CellsParameter.Instance.Count >= 1)
            {
                CellsParameter.Instance.RemoveAt(id);
                dg_pattern.ItemsSource = null;
                dg_pattern.ItemsSource = CellsParameter.Instance;
                if (id <= CellsParameter.Instance.Count - 1)
                {
                    dg_pattern.SelectedIndex = id;
                }
                else
                {
                    if (id - 1 >= 0)
                    {
                        dg_pattern.SelectedIndex = id - 1;
                    }
                }
            }
        }
        private void MenuItem_ReName_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
