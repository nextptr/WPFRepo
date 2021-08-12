using DesignPatternDemo.common;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DesignPatternDemo.UItest
{
    public class CanvasTestViewModel : Screen, IPage
    {
        private string name = "Canvas后台访问";
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private int rowCount = 1;
        private int colCount = 1;
        private double boxWidth;
        private double boxHeight;
        private Canvas myCanvas = null;

        public int RowCount
        {
            get
            {
                return rowCount;
            }
            set
            {
                rowCount = value;
                OnPropertyChanged(nameof(RowCount));
                freshCanvas(RowCount, ColCount);
            }
        }
        public int ColCount
        {
            get
            {
                return colCount;
            }
            set
            {
                colCount = value;
                OnPropertyChanged(nameof(ColCount));
                freshCanvas(RowCount, ColCount);
            }
        }
        public double BoxWidth
        {
            get
            {
                return boxHeight;
            }
            set
            {
                boxHeight = value;
                OnPropertyChanged(nameof(BoxWidth));
            }
        }
        public double BoxHeight
        {
            get
            {
                return boxWidth;
            }
            set
            {
                boxWidth = value;
                OnPropertyChanged(nameof(BoxHeight));
            }
        }
        public Canvas MyCanvas
        {
            get
            {
                return myCanvas;
            }
            set
            {
                myCanvas = value;
                OnPropertyChanged(nameof(MyCanvas));
            }
        }
        private void freshCanvas(int row, int col)
        {
            if (row == -1 || col == -1)
            {
                return;
            }
            if (MyCanvas == null)
                return;

            MyCanvas.Children.Clear();
            double wid = MyCanvas.ActualWidth;
            double hei = MyCanvas.ActualHeight;
            double rowStep = hei / (row + 1);
            double colStep = wid / (col + 1);
            for (int i = 1; i < row + 1; i++)
            {
                var line = new Line() { X1 = 0, Y1 = i * rowStep, X2 = wid, Y2 = i * rowStep };
                line.Stroke = Brushes.Yellow;
                line.StrokeThickness = 2;
                MyCanvas.Children.Add(line);
            }
            for (int i = 1; i < col + 1; i++)
            {
                var line = new Line() { X1 = i * colStep, Y1 = 0, X2 = i * colStep, Y2 = hei };
                line.Stroke = Brushes.Yellow;
                line.StrokeThickness = 2;
                MyCanvas.Children.Add(line);
            }
        }

        bool flag = true;
        public void loadEvent(object sender, RoutedEventArgs e)
        {
            if (flag)
            {
                freshCanvas(RowCount, ColCount);
                flag = false;
            }
        }

        public void btnBackHome()
        {
            var router = IoC.Get<IRouter>();
            router.GoBack();
        }

        public CanvasTestViewModel()
        {
            myCanvas = new Canvas();
            myCanvas.Background = Brushes.Black;
        }
    }
}
