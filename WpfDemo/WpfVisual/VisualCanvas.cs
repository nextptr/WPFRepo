using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfVisual
{
    public class VisualCanvas : Canvas
    {
        private List<Visual> visuals = new List<Visual>();
        private List<DrawingVisual> hits = new List<DrawingVisual>();
        protected override int VisualChildrenCount
        {
            get
            {
                return visuals.Count;
            }
        }
        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }
        public void AddVisual(Visual visual)
        {
            visuals.Add(visual);
            base.AddVisualChild(visual);
            base.AddLogicalChild(visual);
        }
        public void DeleteVisual(Visual visual)
        {
            visuals.Remove(visual);
            base.RemoveVisualChild(visual);
            base.RemoveLogicalChild(visual);
        }
        public DrawingVisual GetVisual(Point point)
        {
            HitTestResult hitTestResult = VisualTreeHelper.HitTest(this, point);
            return hitTestResult.VisualHit as DrawingVisual;
        }
        public Visual GetVisualTest(Point point)
        {
            HitTestResult hitTestResult = VisualTreeHelper.HitTest(this, point);
            return hitTestResult.VisualHit as Visual;
        }
        public List<DrawingVisual> GetVisuals(Geometry region)
        {
            hits.Clear();
            GeometryHitTestParameters parameters = new GeometryHitTestParameters(region);
            HitTestResultCallback callback = new HitTestResultCallback(this.HitTestResultCallback);
            VisualTreeHelper.HitTest(this, null, callback, parameters);
            return hits;
        }
        private HitTestResultBehavior HitTestResultCallback(HitTestResult result)
        {
            GeometryHitTestResult testResult = (GeometryHitTestResult)result;
            DrawingVisual visual = result.VisualHit as DrawingVisual;
            if (visual != null && testResult.IntersectionDetail == IntersectionDetail.FullyInside)
            {
                hits.Add(visual);
            }
            return HitTestResultBehavior.Continue;
        }
    }
}
