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

namespace ImageEdit
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            flgClick1 = false;
            flgClick2 = false;
            flgClick3 = false;
            Line1Sta = new Point(0, 0);
            lin1.X1 = 0;
            lin1.Y1 = 0;
            lin1.X2 = 0;
            lin1.Y2 = 0;
            lin2.X1 = 0;
            lin2.Y1 = 0;
            lin2.X2 = 0;
            lin2.Y2 = 0;
            lin3.X1 = 0;
            lin3.Y1 = 0;
            lin3.X2 = 0;
            lin3.Y2 = 0;
        }

        Point Line1Sta = new Point(0, 0);

        static bool flgClick1 = false;
        static bool flgClick2 = false;
        static bool flgClick3 = false;
        double k1 , b1 = 0;
        double k2 , b2 = 0;
        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (flgClick1 == false)
            {
                flgClick1 = true;
                Point pt = Mouse.GetPosition(BlackBoard);
                Line1Sta = pt;
            }
            else if (flgClick2 == false)
            {
                flgClick2 = true;
                Point pt = Mouse.GetPosition(BlackBoard);
                lin1.X1 = Line1Sta.X;
                lin1.Y1 = Line1Sta.Y;
                lin1.X2 = pt.X;
                lin1.Y2 = pt.Y;
                k1 = (lin1.Y1 - lin1.Y2) / (lin1.X1 - lin1.X2);
                b1 = lin1.Y1 - k1 * lin1.X1;
                k2=-1/k1;
            }
            else if (flgClick3 == false)
            {
                flgClick3 = true;
                double difX = lin2.X2 - lin2.X1;
                double difY = lin2.Y2 - lin2.Y1;
                lin3.X1 = lin1.X1 + difX;
                lin3.X2 = lin1.X2 + difX;
                lin3.Y1 = lin1.Y1 + difY;
                lin3.Y2 = lin1.Y2 + difY;
            }
        }

        //y = kx + b;
        //y1=kx1+b
        //y2=kx2+b
        //y1-kx1=y2-kx2
        //y1-y2=kx1-kx2
        //k=(y1-y2)/(x1-x2)

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (flgClick1 == true && flgClick2 == false)
            {
                Point pt = e.GetPosition(BlackBoard);
                lin1.X1 = Line1Sta.X;
                lin1.Y1 = Line1Sta.Y;
                lin1.X2 = pt.X;
                lin1.Y2 = pt.Y;
            }
            else if (flgClick2 == true && flgClick3 == false)
            {
                Point pt = e.GetPosition(BlackBoard);
                b2 = pt.Y - k2 * pt.X;

                //k1x+b1=k2x+b2
                //(k1-k2)x=b2-b1;
                double x = (b2 - b1) / (k1 - k2);
                double y = k2 * x + b2;
                lin2.X1 = x;
                lin2.Y1 = y;
                lin2.X2 = pt.X;
                lin2.Y2 = pt.Y;
            }
        }
    }
}
