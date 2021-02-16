using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
namespace WpfVisual
{
    public class VisualGridLine : Canvas
    {
        public List<Visual> LisVisuals = new List<Visual>();

        private List<DrawingVisual> Longvisuals = new List<DrawingVisual>();
        private List<DrawingVisual> Shortvisuals = new List<DrawingVisual>();

        protected override int VisualChildrenCount
        {
            get
            {
                return LisVisuals.Count;
            }
        }
        protected override Visual GetVisualChild(int index)
        {
            return LisVisuals[index];
        }
        public bool AddShortVisual(DrawingVisual visual)
        {
            if (Shortvisuals.Count == 0)
            {
                Shortvisuals.Add(visual);
                LisVisuals.Insert(0, visual);
                base.AddVisualChild(visual);
                base.AddLogicalChild(visual);
                return true;
            }
            else
            {
                int init = 0;
                DrawingVisual drawing;
                bool flag = false;
                double source;
                double newitem;
                for (; init < Shortvisuals.Count;)
                {
                    drawing = Shortvisuals[init];
                    source = (drawing.ContentBounds.Left + drawing.ContentBounds.Right) / 2;
                    newitem = (visual.ContentBounds.Left + visual.ContentBounds.Right) / 2;
                    if (newitem > source)
                    {
                        init++;
                    }
                    else if (source == newitem)
                    {//相同位置不能添加
                        break;
                    }
                    else
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag || (init == Shortvisuals.Count))
                {
                    Shortvisuals.Insert(init, visual);
                    LisVisuals.Insert(init, visual);
                    base.AddVisualChild(visual);
                    base.AddLogicalChild(visual);
                    flag = true;
                }
                return flag;
            }
        }
        public bool AddLongVisual(DrawingVisual visual)
        {
            if (Longvisuals.Count == 0)
            {
                Longvisuals.Add(visual);
                LisVisuals.Insert(Shortvisuals.Count, visual);
                base.AddVisualChild(visual);
                base.AddLogicalChild(visual);
                return true;
            }
            else
            {
                int init = 0;
                DrawingVisual drawing;
                bool flag = false;
                double source;
                double newitem;
                for (; init < Longvisuals.Count;)
                {
                    drawing = Longvisuals[init];
                    source = (drawing.ContentBounds.Top + drawing.ContentBounds.Bottom) / 2;
                    newitem = (visual.ContentBounds.Top + visual.ContentBounds.Bottom) / 2;
                    if (newitem > source)
                    {
                        init++;
                    }
                    else if (source == newitem)
                    {
                        break;
                    }
                    else
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag || (init == Longvisuals.Count))
                {
                    Longvisuals.Insert(init, visual);
                    LisVisuals.Insert(Shortvisuals.Count + init, visual);
                    base.AddVisualChild(visual);
                    base.AddLogicalChild(visual);
                    flag = true;
                }
                return flag;
            }
        }
        public void DeleteVisual(int index)
        {
            if (index < 0 || VisualChildrenCount < 0 || VisualChildrenCount <= index)
            {
                return;
            }

            if (index < Shortvisuals.Count)
            {
                Visual visual = LisVisuals[index];
                Shortvisuals.RemoveAt(index);
                base.RemoveVisualChild(visual);
                base.RemoveLogicalChild(visual);
                LisVisuals.Remove(visual);
            }
            else if (index < Longvisuals.Count)
            {
                Visual visual = LisVisuals[index];
                Longvisuals.RemoveAt(index - Shortvisuals.Count);
                base.RemoveVisualChild(visual);
                base.RemoveLogicalChild(visual);
                LisVisuals.Remove(visual);
            }
        }
        public Visual GetVisual(int index)
        {
            if (index < 0 || index >= VisualChildrenCount)
            {
                return null;
            }
            return LisVisuals[index];
        }
        public void ClearVisuals()
        {
            for (int i = VisualChildrenCount - 1; i >= 0; i--)
            {
                base.RemoveVisualChild(LisVisuals[i]);
                base.RemoveLogicalChild(LisVisuals[i]);
            }
            LisVisuals.Clear();
            Longvisuals.Clear();
            Shortvisuals.Clear();
        }
    }
}
