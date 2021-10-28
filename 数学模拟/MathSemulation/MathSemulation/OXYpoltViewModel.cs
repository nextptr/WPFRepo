using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MathSemulation
{
    public class OXYpoltViewModel : Screen
    {
        private PlotModel _plotModel;
        public PlotModel PlotModel
        {
            get => _plotModel;
            set => SetAndNotify(ref _plotModel, value);
        }

        Dispatcher dis = null;
        private Timer timer;
        private LineSeries Lins=null;
        public OXYpoltViewModel()
        {
            dis = Dispatcher.CurrentDispatcher;
            var model = new PlotModel();
            model.Axes.Add(new LinearAxis()
            {
                Title = "面积",
                Position = AxisPosition.Left,
                MajorGridlineStyle = LineStyle.Dash
            });
            model.Axes.Add(new LinearAxis()
            {
                Title = "角度",
                Position = AxisPosition.Bottom,
                MajorGridlineStyle = LineStyle.Dash
            });
            PlotModel = model;

            Lins = new LineSeries();
            PlotModel.Series.Add(Lins);

            // 自动刷新曲线
            timer = new Timer(obj => {
                model.InvalidatePlot(true);
            }, null, 0, 500);
        }


        public void AddVal(double deg,double ar)
        {
            dis.BeginInvoke(new Action(() =>
            {
                Lins.Points.Add(new DataPoint(deg, ar));
            }));
        }
    }
}
