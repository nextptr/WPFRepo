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

namespace WPFDrawing
{
    /// <summary>
    /// SimplePainter.xaml 的交互逻辑
    /// </summary>
    public partial class SimplePainter : UserControl
    {
        private SolidColorBrush color = new SolidColorBrush();
        private Point startPosition;    //绘图起点
        private Shape insertShape;      //插入图形
        private Shape shape;            //选中图形
        private double thickness = 1;   //线宽
        private double opacity = 1;     //透明度
        private bool drawFlag = false;  //是否画图动作中
        public SimplePainter()
        {
            InitializeComponent();
        }

        //工具条选项
        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            color = (SolidColorBrush)((Rectangle)(sender as RadioButton).Content).Fill;
        }
        private void ThicknessButton_Click(object sender, RoutedEventArgs e)
        {
            thickness = ((Ellipse)(sender as RadioButton).Content).Width;
        }
        private void ShapeButton_Click(object sender, RoutedEventArgs e)
        {
            shape = (Shape)(sender as RadioButton).Content;
        }
        private void Opacity_TextChanged(object sender, TextChangedEventArgs e)
        {
            double.TryParse((sender as TextBox).Text, out opacity);
        }

        //绘图动作
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            drawFlag = true;
            Canvas board = sender as Canvas;
            startPosition = e.GetPosition(board);
            insertShape = CreateShape();
            insertShape.Opacity = opacity / 2;
            Canvas.SetLeft(insertShape, e.GetPosition(board).X);
            Canvas.SetTop(insertShape, e.GetPosition(board).Y);
            board.Children.Add(insertShape);
        }
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Canvas board = sender as Canvas;
            if (drawFlag && insertShape != null)
            {
                if (insertShape is Line)
                {
                    (insertShape as Line).X1 = 0; (insertShape as Line).X2 = e.GetPosition(board).X - startPosition.X;
                    (insertShape as Line).Y1 = 0; (insertShape as Line).Y2 = e.GetPosition(board).Y - startPosition.Y;
                }
                else
                {
                    if (e.GetPosition(board).X > startPosition.X)
                    {
                        insertShape.Width = e.GetPosition(board).X - startPosition.X;
                    }
                    else
                    {
                        insertShape.Width = startPosition.X - e.GetPosition(board).X;
                        Canvas.SetLeft(insertShape, e.GetPosition(board).X);
                    }
                    if (e.GetPosition(board).Y > startPosition.Y)
                    {
                        insertShape.Height = e.GetPosition(board).Y - startPosition.Y;
                    }
                    else
                    {
                        insertShape.Height = startPosition.Y - e.GetPosition(board).Y;
                        Canvas.SetTop(insertShape, e.GetPosition(board).Y);
                    }
                }
            }
        }
        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            drawFlag = false;
            if (insertShape != null)
                insertShape.Opacity = opacity;
        }


        //创造不同图形对象
        private Shape CreateShape()
        {
            if (shape is Rectangle)
                return new Rectangle() { Fill = color, Stroke = Brushes.Black, StrokeThickness = thickness };
            else if (shape is Ellipse)
                return new Ellipse() { Fill = color, Stroke = Brushes.Black, StrokeThickness = thickness };
            else
                return new Line() { Stroke = (color.Color == Brushes.Transparent.Color ? Brushes.Black : color), StrokeThickness = thickness };
        }
    }
}
