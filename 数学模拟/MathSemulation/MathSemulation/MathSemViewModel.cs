using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace MathSemulation
{

    public delegate void AreaEvent(double deg, double ar);
        

    public class MathSemViewModel:Screen
    {
        private string linesA = "";
        public string LinesA
        {
            get { return linesA; }
            set
            {
                linesA = value;
                NotifyOfPropertyChange(nameof(LinesA));
            }
        }

        private string linesB = "";
        public string LinesB
        {
            get { return linesB; }
            set
            {
                linesB = value;
                NotifyOfPropertyChange(nameof(LinesB));
            }
        }

        private string linesO = "";
        public string LinesO
        {
            get { return linesO; }
            set
            {
                linesO = value;
                NotifyOfPropertyChange(nameof(LinesO));
            }
        }

        private double _Area;
        public double Area
        {
            get { return _Area; }
            set
            {
                _Area = value;
                NotifyOfPropertyChange(nameof(Area));
            }
        }

        private double maxArea = 0;
        public double MaxArea
        {
            get { return maxArea; }
            set
            {
                maxArea = value;
                NotifyOfPropertyChange(nameof(MaxArea));
            }
        }

        private double minArea;
        public double MinArea
        {
            get { return minArea; }
            set
            {
                minArea = value;
                NotifyOfPropertyChange(nameof(MinArea));
            }
        }

        private bool _Visb = false;
        public bool Visb
        {
            get { return _Visb; }
            set
            {
                _Visb = value;
                NotifyOfPropertyChange(nameof(Visb));
            }
        }


        public event AreaEvent AreaEvent;
        Dispatcher dis = null;
        public MathSemViewModel()
        {
            dis = Dispatcher.CurrentDispatcher;
        }


        public void btnTest()
        {
            //LinesA = "500,400,200,300";
            //LinesO = "100,400,200,300";
            //LinesB = "100,100,200,300";
            double th = 30;
            double maxDeg = 0;
            double minDeg = 0;
            double a = 200;
            double b = 300;
            double r = 100;
            double x = 0.0;
            double y = 0.0;
            double ar = 0.0;
            MaxArea = 0;
            MinArea = 100;
            Visb = false;
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(50);
                    double deg = th / 180 * Math.PI;
                    x = a + r * Math.Cos(deg);
                    y = b + r * Math.Sin(deg);

                    ar  = getVal(x, y, 500, 400);
                    ar += getVal(x, y, 100, 400);
                    ar += getVal(x, y, 100, 100);
                    AreaEvent?.Invoke(th, ar);
                    dis.Invoke(new Action(() =>
                    {
                        Area = Math.Round(ar, 4);
                        if (MaxArea < Area)
                        {
                            MaxArea = Area;
                            maxDeg = deg;
                        }
                        if (MinArea > Area)
                        {
                            MinArea = Area;
                            minDeg = deg;
                        }
                        LinesA = $"500,400,{(int)x},{(int)y}";
                        LinesO = $"100,400,{(int)x},{(int)y}";
                        LinesB = $"100,100,{(int)x},{(int)y}";
                    }));
                    th++;
                    if (th > 390)
                        break;
                }

                dis.Invoke(new Action(() =>
                {
                    x = a + r * Math.Cos(maxDeg);
                    y = b + r * Math.Sin(maxDeg);
                    double x2 = a + r * Math.Cos(minDeg);
                    double y2 = b + r * Math.Sin(minDeg);
                    LinesA = $"{(int)x2},{(int)y2},500,400,{(int)x},{(int)y}";
                    LinesO = $"100,400,100,400";
                    LinesB = $"100,100,100,100";
                    Visb = true;
                }));
            });
        }

        private double getVal(double x, double y, double orix, double oriy)
        {
            double dx = Math.Abs(x - orix)/100*2;
            double dy = Math.Abs(y - oriy)/100*2;
            return dx * dx + dy * dy;
        }

        private void Add(string str, double x, double y)
        {
            str += "," + x;
            str += "," + y;
        }

        private void GetPoint()
        {
            double x = 0.0;
            double y = 0.0;
            double r = 100;
            var d = (x - 200) * (x - 200) + (y - 300) * (y - 300);

        }
    }
}
