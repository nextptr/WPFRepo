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

namespace WpfVisual
{
    /// <summary>
    /// VisualSelection.xaml 的交互逻辑
    /// </summary>
    public partial class VisualSelectionPanel : UserControl
    {
        private Brush drawingBrush = Brushes.AliceBlue;
        private Brush selectedDrawingBrush = Brushes.LightGoldenrodYellow;
        private Pen drawingPen = new Pen(Brushes.SteelBlue, 3);
        private Size squareSize = new Size(30, 30);

        private bool isDragging = false;
        private DrawingVisual selectedVisual;
        private Vector clickOffset;

        private DrawingVisual selectionSquare;
        private bool isMutiSelecting = false;
        private Point selectionSquareTopLeft;

        private Brush selectionSquareBrush = Brushes.Transparent;
        private Pen selectionSquarePen = new Pen(Brushes.Black, 2);
        public VisualSelectionPanel()
        {
            InitializeComponent();
        }
        protected void DrawSquary(DrawingVisual vis, Point point, bool flag)
        {
            using (DrawingContext dc = vis.RenderOpen())
            {
                Brush brush = drawingBrush;
                if (flag)
                {
                    brush = selectedDrawingBrush;
                }
                dc.DrawRectangle(brush, drawingPen, new Rect(point, squareSize));
            }
        }

        protected void DrawRedSquary(DrawingVisual vis, Point point, bool flag)
        {
            using (DrawingContext dc = vis.RenderOpen())
            {
                Brush brush = drawingBrush;
                if (flag)
                {
                    brush = Brushes.Red;
                }
                dc.DrawRectangle(brush, drawingPen, new Rect(point, squareSize));
            }
        }
        protected void DrawSelectionSquare(Point p1, Point p2)
        {
            selectionSquarePen.DashStyle = DashStyles.Dash;
            using (DrawingContext dc = selectionSquare.RenderOpen())
            {
                dc.DrawRectangle(selectionSquareBrush, selectionSquarePen, new Rect(p1, p2));
            }
        }
        protected void ClearSelection()
        {
            Point topLeftCorner = new Point(selectedVisual.ContentBounds.TopLeft.X + drawingPen.Thickness / 2, selectedVisual.ContentBounds.TopLeft.Y + drawingPen.Thickness / 2);
            DrawSquary(selectedVisual, topLeftCorner, false);
            selectedVisual = null;
        }

        protected void RedFlash(List<DrawingVisual> drawingVisuals, bool flag)
        {
            foreach (var tmp in drawingVisuals)
            {
                Point point = new Point(tmp.ContentBounds.TopLeft.X + drawingPen.Thickness / 2, tmp.ContentBounds.TopLeft.Y + drawingPen.Thickness / 2);
                DrawRedSquary(tmp, point, flag);
            }
        }

        private void visualSurface_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point pointClick = e.GetPosition(visualSurface);
            if (true == btnAdd.IsChecked)
            {
                DrawingVisual drawingVisual = new DrawingVisual();
                DrawSquary(drawingVisual, pointClick, false);
                visualSurface.AddVisual(drawingVisual);
            }
            else if (true == btnDelete.IsChecked)
            {
                DrawingVisual visual = visualSurface.GetVisual(pointClick);
                if (visual != null)
                {
                    visualSurface.DeleteVisual(visual);
                }
            }
            else if (true == btnSelect.IsChecked)
            {
                DrawingVisual visual = visualSurface.GetVisual(pointClick);
                if (visual != null)
                {
                    DrawingVisual drawing = visual as DrawingVisual;
                    if (drawing != null)
                    {
                        labtxtx.Content = drawing.ContentBounds.X;
                        labtxty.Content = drawing.ContentBounds.Y;
                    }

                    Point topLeftCorner = new Point(visual.ContentBounds.TopLeft.X + drawingPen.Thickness / 2, visual.ContentBounds.TopLeft.Y + drawingPen.Thickness / 2);
                    DrawSquary(visual, topLeftCorner, true);
                    clickOffset = topLeftCorner - pointClick;
                    isDragging = true;
                    if (selectedVisual != null && selectedVisual != visual)
                    {
                        ClearSelection();
                    }
                    selectedVisual = visual;
                }
            }
            else if (true == btnChecked.IsChecked)
            {
                selectionSquare = new DrawingVisual();
                visualSurface.AddVisual(selectionSquare);
                selectionSquareTopLeft = pointClick;
                isMutiSelecting = true;
                visualSurface.CaptureMouse();
            }
        }

        private void visualSurface_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            if (isMutiSelecting)
            {
                RectangleGeometry geometry = new RectangleGeometry(new Rect(selectionSquareTopLeft, e.GetPosition(visualSurface)));
                List<DrawingVisual> visualss = visualSurface.GetVisuals(geometry);
                RedFlash(visualss, true);
                MessageBox.Show(string.Format("you selection {0} square(s).", visualss.Count));
                RedFlash(visualss, false);
                isMutiSelecting = false;
                visualSurface.DeleteVisual(selectionSquare);
                visualSurface.ReleaseMouseCapture();
            }
        }

        private void visualSurface_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point pointDragged = e.GetPosition(visualSurface) + clickOffset;
                DrawSquary(selectedVisual, pointDragged, true);
            }
            else if (isMutiSelecting)
            {
                Point poinDragged = e.GetPosition(visualSurface);
                DrawSelectionSquare(selectionSquareTopLeft, poinDragged);
            }
        }
    }
}
