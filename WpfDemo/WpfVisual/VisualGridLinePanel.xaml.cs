using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace WpfVisual
{
    /// <summary>
    /// VisualGridLinePanel.xaml 的交互逻辑
    /// </summary>
    public partial class VisualGridLinePanel : UserControl
    {
        public VisualGridLinePanel()
        {
            InitializeComponent();
            btnAutoSet.Click += BtnAutoSet_Click;
            btnangle0.Click += Btnangle0_Click;
            btnangle90.Click += Btnangle90_Click;

            btnreset.Click += Btnreset_Click;
            btnclear.Click += Btnclear_Click;
            btnaddShortLine.Click += BtnaddShortLine_Click;
            btnaddLongLine.Click += BtnaddLongLine_Click;

            btnselect_short.Click += Btnselect_short_Click;
            btnUnselect_short.Click += BtnUnselect_short_Click;
            btnDeleteLine_short.Click += BtnDeleteLine_short_Click;
            btnselect_long.Click += Btnselect_short_Click;
            btnUnselect_long.Click += BtnUnselect_short_Click;
            btnDeleteLine_long.Click += BtnDeleteLine_short_Click;

            btnclean.Click += Btnclean_Click;
            btnStatus.Click += BtnStatus_Click;
        }
        private void BtnAutoSet_Click(object sender, RoutedEventArgs e)
        {
            double w = 1600;
            double h = 1000;
            gridline.ResetViewerRate(w, h);
            double wid = (w - 150) / 16;
            double hei = (h - 50) / 6;
            double tmp = -10;
            for (int i = 0; i < 16; i++)
            {
                tmp = tmp + 10;
                gridline.AddShortLine(tmp, Brushes.White);
                tmp = tmp + wid;
                gridline.AddShortLine(tmp, Brushes.White);
            }
            tmp = -10;
            for (int j = 0; j < 6; j++)
            {
                tmp = tmp + 10;
                gridline.AddLongLine(tmp, Brushes.White);
                tmp = tmp + hei;
                gridline.AddLongLine(tmp, Brushes.White);
            }
        }
        private void Btnangle0_Click(object sender, RoutedEventArgs e)
        {
            gridline.RotateViewer(0);
        }
        private void Btnangle90_Click(object sender, RoutedEventArgs e)
        {
            gridline.RotateViewer(90);
        }
        private void Btnreset_Click(object sender, RoutedEventArgs e)
        {
            double w = int.Parse(txtw.Text);
            double h = int.Parse(txth.Text);
            gridline.ResetViewerRate(w, h);
        }
        private void Btnclear_Click(object sender, RoutedEventArgs e)
        {
            gridline.ClearLines();
        }

        private void BtnaddShortLine_Click(object sender, RoutedEventArgs e)
        {
            double length = double.Parse(txtShortLine.Text);
            Brush bsh;
            int colorid = cmbShortLinecolor.SelectedIndex;
            switch (colorid)
            {
                case 0:
                    {
                        bsh = Brushes.White;
                    }
                    break;
                case 1:
                    {
                        bsh = Brushes.Red;
                    }
                    break;
                case 2:
                    {
                        bsh = Brushes.Green;
                    }
                    break;
                case 3:
                    {
                        bsh = Brushes.Blue;
                    }
                    break;
                case 4:
                    {
                        bsh = Brushes.Yellow;
                    }
                    break;
                default:
                    {
                        bsh = Brushes.White;
                    }
                    break;
            }
            gridline.AddShortLine(length, bsh);
        }
        private void BtnaddLongLine_Click(object sender, RoutedEventArgs e)
        {

            double length = double.Parse(txtlongLine.Text);
            Brush bsh;
            int colorid = cmbLongLinecolor.SelectedIndex;
            switch (colorid)
            {
                case 0:
                    {
                        bsh = Brushes.White;
                    }
                    break;
                case 1:
                    {
                        bsh = Brushes.Red;
                    }
                    break;
                case 2:
                    {
                        bsh = Brushes.Green;
                    }
                    break;
                case 3:
                    {
                        bsh = Brushes.Blue;
                    }
                    break;
                case 4:
                    {
                        bsh = Brushes.Yellow;
                    }
                    break;
                default:
                    {
                        bsh = Brushes.White;
                    }
                    break;
            }
            gridline.AddLongLine(length, bsh);
        }

        private void Btnselect_short_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btnselect_short")
            {
                int index = int.Parse(txtselect_short.Text);
                gridline.SelectLine(LineLength.ShortLine, index);
            }
            else
            {
                int index = int.Parse(txtselect_long.Text);
                gridline.SelectLine(LineLength.LongLine, index);
            }

        }
        private void BtnUnselect_short_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btnUnselect_short")
            {
                int index = int.Parse(txtselect_short.Text);
                gridline.UnselectLine(LineLength.ShortLine, index);
            }
            else
            {
                int index = int.Parse(txtselect_long.Text);
                gridline.UnselectLine(LineLength.LongLine, index);
            }
        }
        private void BtnDeleteLine_short_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "btnDeleteLine_short")
            {
                int index = int.Parse(txtselect_short.Text);
                gridline.DeleteLine(LineLength.ShortLine, index);
            }
            else
            {
                int index = int.Parse(txtselect_long.Text);
                gridline.DeleteLine(LineLength.LongLine, index);
            }
        }


        private void Btnclean_Click(object sender, RoutedEventArgs e)
        {
            txtboard.Items.Clear();
        }
        private void BtnStatus_Click(object sender, RoutedEventArgs e)
        {
            //txtboard.Items.Clear();
            //{
            //    ShowMsg("/////表层/////");
            //    ShowMsg("/////short/////");
            //    foreach (var tmp in gridline.ShortCuttingLines)
            //    {
            //        ShowMsg("key:" + tmp.Key + " id:" + tmp.Index + " brush:" + tmp.Value.brush);
            //    }
            //    ShowMsg("/////Long/////");
            //    foreach (var tmp in gridline.LongCuttingLines)
            //    {
            //        ShowMsg("key:" + tmp.Key + " id:" + tmp.Value.Index + " brush:" + tmp.Value.brush);
            //    }
            //}
            //{
            //    ShowMsg("/////里层/////");
            //    ShowMsg("/////LisVisuals/////");
            //    for (int i = 0; i < gridline.LisVisuals.Count; i++)
            //    {
            //        ShowMsg("id:" + i);
            //    }
            //    ShowMsg("/////DicVisuals/////");
            //    foreach (var tmp in gridline.DicVisuals)
            //    {
            //        ShowMsg("key:" + tmp.Key);
            //    }
            //}
        }


        protected void ShowMsg(string str)
        {
            ListBoxItem item = new ListBoxItem();
            item.Content = str;
            item.Foreground = Brushes.Red;
            // txtboard.Items.Insert(0, item);
            txtboard.Items.Add(item);
        }
    }
}
