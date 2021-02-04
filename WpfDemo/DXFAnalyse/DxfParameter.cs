using Common;
using Common.Parameter;
using DXFComponent;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace DXFAnalyse
{
    public class DxfItem : NotifyPropertyChanged
    {
        private string _recipeName = "";
        private string _recipeDate = "";
        private List<PATH> _recipePath = new List<PATH>();

        public double Max_x = 0.0;
        public double Max_y = 0.0;

        public string RecipeName
        {
            get { return _recipeName; }
            set { if (_recipeName == value) { return; } _recipeName = value; OnPropertyChanged("RecipeName"); }
        }
        public string RecipeDate
        {
            get { return _recipeDate; }
            set { if (_recipeDate == value) { return; } _recipeDate = value; OnPropertyChanged("RecipeDate"); }
        }
        public List<PATH> RecipePath
        {
            get
            {
                return _recipePath;
            }
            set
            {
                _recipePath.Clear();
                for (int i = 0; i < value.Count; i++)
                {
                    _recipePath.Add(value[i].Clon());
                }
            }
        }

        public DxfItem()
        {
        }
        private PATH SortPolyLine(PATH pth)
        {
            int index = 0;
            int count = 0;
            double mid_x = 0.0;
            double mid_y = 0.0;
            PATH ret = new PATH();
            ret.CopyFrom(pth);
            List<Point> ls_src = new List<Point>();
            List<Point> ls_th = new List<Point>();
            List<Point> ls_cent = new List<Point>();
            List<int> ls_tp = new List<int>();
            foreach (Point tmp in ret.throughPoints)
            {
                ls_src.Add(new Point(tmp.X, tmp.Y));
                count++;
            }
            //闭合多线段去除重合终点
            if ((ls_src[count - 1].X == ls_src[0].X) && (ls_src[count - 1].Y == ls_src[0].Y))
            {
                ls_src.RemoveAt(count - 1);
                ret.centerPoints.RemoveAt(count - 1);
                ret.types.RemoveAt(count - 1);
                //找到直线的位置
                List<int> idx = new List<int>();
                List<int> idy = new List<int>();
                for (int i = 0; i < ls_src.Count - 2; i++)
                {
                    if (ret.types[i] == 0)
                    {
                        if (Math.Round(ls_src[i].Y, 4) == Math.Round(ls_src[i + 1].Y, 4))
                        {
                            idy.Add(i);
                        }
                        if (Math.Round(ls_src[i].X, 4) == Math.Round(ls_src[i + 1].X, 4))
                        {
                            idx.Add(i);
                        }
                    }
                }
                //找到最大直线的位置
                double maxLen = 0.0;
                foreach (int id in idx)
                {
                    double tmplen = Math.Abs(ls_src[id].X - ls_src[id + 1].X);
                    if (tmplen > maxLen)
                    {
                        maxLen = tmplen;
                        index = id;
                    }
                }
                foreach (int id in idy)
                {
                    double tmplen = Math.Abs(ls_src[id].Y - ls_src[id + 1].Y);
                    if (tmplen > maxLen)
                    {
                        maxLen = tmplen;
                        index = id;
                    }
                }
                //在直线中插入两个分割点
                mid_x = (ls_src[index].X + ls_src[index + 1].X) / 2;
                mid_y = (ls_src[index].Y + ls_src[index + 1].Y) / 2;
                ls_th.Add(new Point(mid_x, mid_y));
                ls_cent.Add(new Point(0.0, 0.0));
                ls_tp.Add(0);
                for (int i = index + 1; i < ls_src.Count; i++)
                {
                    ls_th.Add(new Point(ls_src[i].X, ls_src[i].Y));
                    ls_cent.Add(ret.centerPoints[i]);
                    ls_tp.Add(ret.types[i]);
                }
                for (int i = 0; i <= index; i++)
                {
                    ls_th.Add(new Point(ls_src[i].X, ls_src[i].Y));
                    ls_cent.Add(ret.centerPoints[i]);
                    ls_tp.Add(ret.types[i]);
                }
                ls_th.Add(new Point(mid_x, mid_y));
                ls_cent.Add(new Point(0.0, 0.0));
                ls_tp.Add(0);
            }
            else
            {
                for (int i = 0; i < ls_src.Count; i++)
                {
                    ls_th.Add(new Point(ls_src[i].X, ls_src[i].Y));
                    ls_cent.Add(ret.centerPoints[i]);
                    ls_tp.Add(ret.types[i]);
                }
            }
            //折线多点排序分割完成
            ret.throughPoints.Clear();
            ret.centerPoints.Clear();
            ret.types.Clear();
            foreach (Point tmp in ls_th)
            {
                ret.throughPoints.Add(tmp);
            }
            foreach (var tmp in ls_cent)
            {
                ret.centerPoints.Add(tmp);
            }
            foreach (var tmp in ls_tp)
            {
                ret.types.Add(tmp);
            }
            return ret;
        }
        public DxfItem clone()
        {
            DxfItem ret = new DxfItem();
            ret.RecipeName = this.RecipeName;
            ret.RecipeDate = this.RecipeDate;
            ret.RecipePath = new List<PATH>();
            foreach (var item in this.RecipePath)
            {
                ret.RecipePath.Add(item.Clon());
            }
            return ret;
        }
    }

    public class DxfParameter : ParameterBase
    {
        private ObservableCollection<DxfItem> datas;
        public ObservableCollection<DxfItem> Datas
        {
            get
            {
                return datas;
            }
            set
            {
                datas = value;
                OnPropertyChanged(nameof(Datas));
            }
        }

        public override void Copy(IParameter source)
        {
            DxfParameter sp = source as DxfParameter;
            if (sp != null)
            {
                this.Datas.Clear();
                foreach (var item in sp.Datas)
                {
                    this.Datas.Add(item.clone());
                }
            }
        }

        public DxfParameter()
        {
            Datas = new ObservableCollection<DxfItem>();
        }
    }

    public class ParameterInstance
    {
        private ParameterInstance()
        {
            dxfParameter = new DxfParameter();
        }
        public static ParameterInstance Instance
        {
            get
            {
                return Nested._instance;
            }
        }
        private class Nested
        {
            static Nested()
            { }
            internal static readonly ParameterInstance _instance = new ParameterInstance();
        }


        public DxfParameter dxfParameter;

    }

}
