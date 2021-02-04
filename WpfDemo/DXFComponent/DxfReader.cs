using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace DXFComponent
{
    public class DxfReader
    {
        protected static DxfReader _instance;
        public static DxfReader Instance
        {
            get
            {
                if (null == _instance)
                    _instance = new DxfReader();
                return _instance;
            }
        }
        private FileStream fs;
        private StreamReader sr;
        private List<PATH> _pathList = null;
        public ArrayList LayerList = new ArrayList();
        public ArrayList LineList = new ArrayList();
        public ArrayList ArcList = new ArrayList();
        public ArrayList EllipseList = new ArrayList();
        public ArrayList LwopolylineList = new ArrayList();
        public ArrayList SplineList = new ArrayList();
        public ArrayList PathArrayList = new ArrayList();
        public List<PATH> PathList
        {
            get
            {
                if (_pathList == null)
                {
                    _pathList = new List<PATH>();
                }
                else
                {
                    _pathList.Clear();
                }
                foreach (var th in PathArrayList)
                {
                    _pathList.Add(((PATH)th).Clon());
                }
                return _pathList;
            }
        }

        private string[] str = new string[2];
        private int count;
        private double leftx;
        private double lefty;
        private double rightx;
        private double righty;

        private string[] ReadPair()
        {
            string code = sr.ReadLine().Trim();
            string codedata = sr.ReadLine().Trim();
            count += 2;
            string[] result = new string[2] { code, codedata };
            return result;
        }
        public void Read(string path)
        {
            LayerList.Clear();
            LineList.Clear();
            ArcList.Clear();
            EllipseList.Clear();
            LwopolylineList.Clear();
            SplineList.Clear();
            PathArrayList.Clear();
            fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            sr = new StreamReader(fs);
            while (sr.Peek() != -1)
            {
                str = ReadPair();
                if (str[1] == "SECTION")
                {
                    str = ReadPair();
                    switch (str[1])
                    {
                        case "HEADER":
                            ReadHeader();
                            break;
                        case "TABLES":
                            ReadTable();
                            break;
                        case "ENTITIES":
                            ReadEntities();
                            break;
                    }
                }
            }
            sr.Close();
            fs.Close();
        }
        public void ReadHeader()
        {
            while (str[1] != "ENDSEC")
            {
                str = ReadPair();
                switch (str[1])
                {
                    case "$EXTMIN":
                        str = ReadPair();
                        leftx = Double.Parse(str[1]);
                        str = ReadPair();
                        lefty = Double.Parse(str[1]);
                        break;
                    case "$EXTMAX":
                        str = ReadPair();
                        rightx = Double.Parse(str[1]);
                        str = ReadPair();
                        righty = Double.Parse(str[1]);
                        break;
                }
            }
        }
        private void ReadLAYER()
        {
            LAYER newlayer = new LAYER();
            while (str[1] != "ENDTAB")
            {
                str = ReadPair();
                switch (str[0])
                {
                    case "2":
                        newlayer.name = str[1];
                        break;
                    case "62":
                        newlayer.colornum = str[1];
                        break;
                    case "6":
                        newlayer.lstyle = str[1];
                        break;
                    case "370":
                        newlayer.lwidth = str[1];
                        break;
                }
                if (str[0] == "0" && str[1] == "LAYER")
                {
                    LayerList.Add(newlayer);
                    return;
                }
            }
            LayerList.Add(newlayer);
        }
        private void ReadTable()
        {
            while (str[1] != "ENDSEC")
            {
                while (str[0] != "2" || str[1] != "LAYER")
                {
                    str = ReadPair();
                }
                while (str[0] != "0" || str[1] != "LAYER")
                {
                    str = ReadPair();
                }
                while (str[0] == "0" && str[1] == "LAYER")
                {
                    ReadLAYER();
                }
                while (str[1] != "ENDSEC")
                {
                    str = ReadPair();
                }
            }
        }
        private void ReadEntities()
        {
            while (str[1] != "ENDSEC")
            {
                switch (str[1])
                {
                    case "LINE":
                        ReadLine();
                        break;
                    case "ARC":
                        ReadArc();
                        break;
                    case "CIRCLE":
                        ReadArc();
                        break;
                    case "ELLIPSE":
                        ReadEllipse();
                        break;
                    case "LWPOLYLINE":
                        ReadLwpolyline();
                        break;
                    case "SPLINE":
                        ReadSpline();
                        break;
                    default:
                        str = ReadPair();
                        break;
                }
            }
            ///读取dxf完成后转换成统一的弧线和直线的集合，
            if (SplineList.Count > 0)
            {
                foreach (PATH pth in SplineList)
                {
                    if (pth.PointCount == 4)
                    {
                        PATH th = new PATH();
                        th.ePathType = EPathType.Line;
                        th.StartX = Math.Round(pth.throughPoints[0].X, 7);
                        th.StartY = Math.Round(pth.throughPoints[0].Y, 7);
                        th.EndX = Math.Round(pth.throughPoints[3].X, 7);
                        th.EndY = Math.Round(pth.throughPoints[3].Y, 7);
                        PathArrayList.Add(th);
                    }
                }
            }
            if (LineList.Count > 0)
            {
                foreach (var path in LineList)
                {
                    PATH pth = path as PATH;
                    PATH newPath = new PATH();
                    newPath.ePathType = EPathType.Line;
                    newPath.StartX = Math.Round(pth.StartX, 7);
                    newPath.StartY = Math.Round(pth.StartY, 7);
                    newPath.EndX = Math.Round(pth.EndX, 7);
                    newPath.EndY = Math.Round(pth.EndY, 7);
                    PathArrayList.Add(newPath);
                }
            }
            if (ArcList.Count > 0)
            {
                foreach (var path in ArcList)
                {
                    PATH pth = path as PATH;
                    PATH newPath = new PATH();
                    if (pth.StartAngle == 0.0 && pth.EndAngle == 0.0)
                    { //圆,暂时不做处理
                        newPath.ePathType = EPathType.Circle;
                        newPath.StartAngle = 0;
                        newPath.EndAngle = 0;
                        newPath.CenterX = pth.CenterX;
                        newPath.CenterY = pth.CenterY;
                        newPath.StartX = 0; //(x-a)*(x-a)+(y-b)*(y-b)=r*r  x==a
                        newPath.StartY = 0; //y=r+b
                        newPath.EndX = 0;
                        newPath.EndY = 0;
                        newPath.Radio = pth.Radio;
                        PathArrayList.Add(newPath);
                    }
                    else
                    {//圆弧
                        newPath.ePathType = EPathType.Arc;
                        newPath.StartAngle = pth.StartAngle;
                        newPath.EndAngle = pth.EndAngle;
                        newPath.CenterX = pth.CenterX;
                        newPath.CenterY = pth.CenterY;
                        newPath.StartX = pth.Radio * Math.Cos(pth.StartAngle * (Math.PI / 180)) + pth.CenterX; //x=r*cos(a)+centx
                        newPath.StartY = pth.Radio * Math.Sin(pth.StartAngle * (Math.PI / 180)) + pth.CenterY; //y=r*sin(a)+centy
                        newPath.EndX = pth.Radio * Math.Cos(pth.EndAngle * (Math.PI / 180)) + pth.CenterX;
                        newPath.EndY = pth.Radio * Math.Sin(pth.EndAngle * (Math.PI / 180)) + pth.CenterY;
                        newPath.Radio = pth.Radio;
                        PathArrayList.Add(newPath);
                    }
                }
            }
            if (LwopolylineList.Count > 0)
            {
                ConvertLwpolylineToPathList();
            }
        }

        private void ReadArc()
        {
            PATH newarc = new PATH();
            newarc.ePathType = EPathType.Arc;
            while (str[1] != "ENDSEC")
            {
                str = ReadPair();
                switch (str[0])
                {
                    case "8":
                        newarc.LName = str[1];
                        break;
                    case "10":
                        newarc.CenterX = Double.Parse(str[1]);
                        break;
                    case "20":
                        newarc.CenterY = Double.Parse(str[1]);
                        break;
                    case "40":
                        newarc.Radio = Double.Parse(str[1]);
                        break;
                    case "50":
                        newarc.StartAngle = Double.Parse(str[1]);
                        break;
                    case "51":
                        newarc.EndAngle = Double.Parse(str[1]);
                        break;
                    case "370":
                        newarc.lwidth = str[1];
                        break;
                    case "0":
                        ArcList.Add(newarc);
                        return;
                }
            }
        }
        private void ReadLine()
        {
            PATH newline = new PATH();
            newline.ePathType = EPathType.Line;
            while (str[1] != "ENDSEC")
            {
                str = ReadPair();
                switch (str[0])
                {
                    case "8":
                        newline.LName = str[1];
                        break;
                    case "10":
                        newline.StartX = Double.Parse(str[1]);
                        break;
                    case "20":
                        newline.StartY = Double.Parse(str[1]);
                        break;
                    case "11":
                        newline.EndX = Double.Parse(str[1]);
                        break;
                    case "21":
                        newline.EndY = Double.Parse(str[1]);
                        break;
                    case "62":
                        newline.colornum = str[1];
                        break;
                    case "370":
                        newline.lwidth = str[1];
                        break;
                    case "0":
                        LineList.Add(newline);
                        return;
                }
            }
        }
        private void ReadEllipse()
        {
            PATH newellipse = new PATH();
            newellipse.ePathType = EPathType.Ellipse;
            while (str[1] != "ENDSEC")
            {
                str = ReadPair();
                switch (str[0])
                {
                    case "8":
                        newellipse.LName = str[1];
                        break;
                    case "10":
                        newellipse.CenterX = Double.Parse(str[1]);
                        break;
                    case "20":
                        newellipse.CenterY = Double.Parse(str[1]);
                        break;
                    case "11":
                        newellipse.DeltaX = Double.Parse(str[1]);
                        break;
                    case "21":
                        newellipse.DeltaY = Double.Parse(str[1]);
                        break;
                    case "40":
                        newellipse.Radio = Double.Parse(str[1]);
                        break;
                    case "41":
                        newellipse.PStartAngle = Double.Parse(str[1]);
                        break;
                    case "42":
                        newellipse.PEndAngle = Double.Parse(str[1]);
                        break;
                    case "370":
                        newellipse.lwidth = str[1];
                        break;
                    case "0":
                        EllipseList.Add(newellipse);
                        return;
                }
            }
        }
        private void ReadLwpolyline()
        {
            PATH newlw = new PATH();
            newlw.ePathType = EPathType.Lwpolyline;
            while (str[1] != "ENDSEC")
            {
                str = ReadPair();
                switch (str[0])
                {
                    case "8":
                        newlw.LName = str[1];
                        break;
                    case "370":
                        newlw.lwidth = str[1];
                        break;
                    case "62":
                        newlw.colornum = str[1];
                        break;
                    case "90":
                        newlw.PointCount = Int32.Parse(str[1]);
                        break;
                    case "70":
                        newlw.Flag = Int32.Parse(str[1]);
                        break;
                    case "10":
                        newlw.pointx = new double[newlw.PointCount];
                        newlw.pointy = new double[newlw.PointCount];
                        newlw.converxity = new double[newlw.PointCount];

                        newlw.pointx[0] = Double.Parse(str[1]);
                        str = ReadPair();
                        newlw.pointy[0] = Double.Parse(str[1]);
                        for (int i = 1; i < newlw.PointCount; i++)
                        {
                            string temp = sr.ReadLine().Trim();
                            if (temp == "42")
                            {
                                newlw.converxity[i - 1] = Double.Parse(sr.ReadLine().Trim());
                                i--;
                            }
                            else if (temp == "20")
                            {
                                string r = sr.ReadLine().Trim();
                                newlw.pointy[i] = Double.Parse(r);
                            }
                            else
                            {
                                string r = sr.ReadLine().Trim();
                                newlw.pointx[i] = Double.Parse(r);
                                i--;
                            }
                        }
                        break;
                    case "42":
                        newlw.converxity[newlw.converxity.Length - 1] = Double.Parse(str[1].Trim());
                        break;
                    case "0":
                        {
                            LwopolylineList.Add(newlw);
                        }
                        return;
                }
            }
        }
        private void ReadSpline()
        {
            PATH newspline = new PATH();
            newspline.ePathType = EPathType.Spline;
            while (str[1] != "ENDSEC")
            {
                str = ReadPair();
                switch (str[0])
                {
                    case "8":
                        newspline.LName = str[1];
                        break;
                    case "73":
                        newspline.PointCount = Int32.Parse(str[1]);
                        newspline.throughPoints = new List<Point>();
                        break;
                    case "10":
                        Point pos = new Point();
                        pos.X = Double.Parse(str[1]);   //10
                        str = ReadPair();
                        pos.Y = Double.Parse(str[1]);   //20
                        str = ReadPair();               //30
                        newspline.throughPoints.Add(pos);
                        break;
                    case "0":
                        SplineList.Add(newspline);
                        return;
                }
            }
        }

        public void PathRotateAng(List<PATH> inList, List<PATH> outList, double angle)//PATH旋转
        {
            if (inList == null)
            {
                return;
            }
            if (outList == null)
            {
                outList = new List<PATH>();
            }
            else
            {
                outList.Clear();
            }
            foreach (PATH th in inList)
            {
                outList.Add(th.Rotate(angle));
            }
        }
        public void PathMirrorY(List<PATH> inList, List<PATH> outList)//Y轴镜像翻转
        {
            if (inList == null)
            {
                return;
            }
            if (outList == null)
            {
                outList = new List<PATH>();
            }
            else
            {
                outList.Clear();
            }
            foreach (PATH pth in inList)
            {
                PATH path = pth.Clon();
                if (pth.ePathType == EPathType.Arc)
                {
                    path.StartX = pth.EndX;
                    path.EndX = pth.StartX;

                    path.StartY = -1 * pth.EndY;
                    path.EndY = -1 * pth.StartY;

                    path.CenterY = -1 * pth.CenterY;
                }
                if (pth.ePathType == EPathType.Circle)
                {
                    path.CenterY = -1 * pth.CenterY;

                    path.StartY = -1 * pth.StartY;
                    path.EndY = -1 * pth.EndY;
                }
                if (pth.ePathType == EPathType.Lwpolyline)
                {
                    path.throughPoints.Clear();
                    path.centerPoints.Clear();
                    for (int i = 0; i < pth.throughPoints.Count; i++)
                    {
                        Point pos = new Point();
                        pos.X = pth.throughPoints[i].X;
                        pos.Y = -1 * pth.throughPoints[i].Y;
                        path.throughPoints.Add(pos);
                        Point pos2 = new Point();
                        pos2.X = pth.centerPoints[i].X;
                        pos2.Y = -1 * pth.centerPoints[i].Y;
                        path.centerPoints.Add(pos2);
                    }
                    path.StartY = -1 * pth.StartY;
                    path.EndY = -1 * pth.EndY;
                }
                if (pth.ePathType == EPathType.Line)
                {
                    path.StartY = -1 * pth.StartY;
                    path.EndY = -1 * pth.EndY;
                }
                outList.Add(path);
            }
        }
        public void TransformToOneQuadrant(List<PATH> inList, List<PATH> outList, ref double max_x, ref double max_y)//转换到第一象限
        {
            if (inList == null)
            {
                return;
            }
            if (outList == null)
            {
                outList = new List<PATH>();
            }
            else
            {
                outList.Clear();
            }
            //找到最小值，最大值
            double min_x = 0.0;
            double min_y = 0.0;
            for (int i = 0; i < inList.Count; i++)
            {
                if (inList[i].ePathType == EPathType.Line)
                {
                    min_x = inList[i].StartX;
                    min_y = inList[i].StartY;
                    max_x = inList[i].StartX;
                    max_y = inList[i].StartY;
                    break;
                }
                else if (inList[i].ePathType == EPathType.Lwpolyline)
                {
                    min_x = inList[i].throughPoints[0].X;
                    min_y = inList[i].throughPoints[0].Y;
                    max_x = inList[i].throughPoints[0].X;
                    max_y = inList[i].throughPoints[0].Y;
                    break;
                }
                else if (inList[i].ePathType == EPathType.Circle)
                {
                    min_x = inList[i].CenterX;
                    min_y = inList[i].CenterY;
                    max_x = inList[i].CenterX;
                    max_y = inList[i].CenterY;
                    break;
                }
            }
            foreach (PATH tmpPath in inList)
            {
                if (tmpPath.ePathType == EPathType.Line)
                {
                    //最大值
                    if (tmpPath.StartX < min_x)
                    {
                        min_x = tmpPath.StartX;
                    }
                    if (tmpPath.StartY < min_y)
                    {
                        min_y = tmpPath.StartY;
                    }
                    if (tmpPath.EndX < min_x)
                    {
                        min_x = tmpPath.EndX;
                    }
                    if (tmpPath.EndY < min_y)
                    {
                        min_y = tmpPath.EndY;
                    }
                    //最小值
                    if (tmpPath.StartX > max_x)
                    {
                        max_x = tmpPath.StartX;
                    }
                    if (tmpPath.StartY > max_y)
                    {
                        max_y = tmpPath.StartY;
                    }
                    if (tmpPath.EndX > max_x)
                    {
                        max_x = tmpPath.EndX;
                    }
                    if (tmpPath.EndY > max_y)
                    {
                        max_y = tmpPath.EndY;
                    }
                }
                else if (tmpPath.ePathType == EPathType.Lwpolyline)
                {
                    foreach (Point tmp in tmpPath.throughPoints)
                    {
                        //最小值
                        if (tmp.X < min_x)
                        {
                            min_x = tmp.X;
                        }
                        if (tmp.Y < min_y)
                        {
                            min_y = tmp.Y;
                        }
                        //最大值
                        if (tmp.X > max_x)
                        {
                            max_x = tmp.X;
                        }
                        if (tmp.Y > max_y)
                        {
                            max_y = tmp.Y;
                        }
                    }
                }
            }

            //将图像平移到第一象限           
            foreach (var tmp in inList)
            {
                PATH pth = new PATH();
                pth = (PATH)tmp;
                if (pth.ePathType == EPathType.Arc)
                {
                    pth.CenterX = pth.CenterX - min_x;
                    pth.CenterY = pth.CenterY - min_y;
                }
                else if (pth.ePathType == EPathType.Circle)
                {
                    pth.CenterX = Math.Round(pth.CenterX - min_x, 7);
                    pth.CenterY = Math.Round(pth.CenterY - min_y, 7);
                }
                else if (pth.ePathType == EPathType.Lwpolyline)
                {
                    for (int i = 0; i < pth.throughPoints.Count; i++)
                    {
                        Point pos = new Point();
                        pos.X = pth.throughPoints[i].X - min_x;
                        pos.Y = pth.throughPoints[i].Y - min_y;
                        pth.throughPoints[i] = pos;
                        Point pos2 = new Point();
                        pos2.X = pth.centerPoints[i].X - min_x;
                        pos2.Y = pth.centerPoints[i].Y - min_y;
                        pth.centerPoints[i] = pos2;
                    }
                }
                pth.StartX = Math.Round(pth.StartX - min_x, 7);
                pth.StartY = Math.Round(pth.StartY - min_y, 7);
                pth.EndX = Math.Round(pth.EndX - min_x, 7);
                pth.EndY = Math.Round(pth.EndY - min_y, 7);
                outList.Add(pth);
            }
            max_x = max_x - min_x;
            max_y = max_y - min_y;
        }
        public void FindMarkPoint(List<PATH> lst, out Point mak1, out Point mak2)//寻找mark点
        {
            //十字mark点
            mak1 = new Point(0.0, 0.0);
            mak2 = new Point(0.0, 0.0);
            List<Point> Points = new List<Point>();
            List<PATH> srcList = new List<PATH>();
            //找出十字线
            foreach (PATH th in lst)
            {
                if (th.ePathType == EPathType.Line) //长度小于2毫米的线为mark线
                {
                    if (Math.Abs(th.PathLength - 2.0) < 0.000001)
                    {
                        Point pos = new Point();
                        pos.X = (th.StartX + th.EndX) / 2;
                        pos.Y = (th.StartY + th.EndY) / 2;
                        Points.Add(pos);

                        srcList.Add(th);
                    }
                    else
                    {
                        srcList.Add(th);
                    }
                }
                else
                {
                    srcList.Add(th);
                }
            }
            lst.Clear();
            foreach (PATH th in srcList)
            {
                lst.Add(th);
            }
            //判断mark点
            if (Points.Count != 4)
            {
                return;
            }
            double v1 = ComMath.PointDistance(Points[0], Points[1]);
            double v2 = ComMath.PointDistance(Points[0], Points[2]);
            double v3 = ComMath.PointDistance(Points[0], Points[3]);

            Point tmp1 = new Point();
            Point tmp2 = new Point();

            if (v1 < 1)
            {
                tmp1 = Points[0];
                tmp2 = Points[2];
            }
            else if (v2 < 1)
            {
                tmp1 = Points[0];
                tmp2 = Points[1];
            }


            if (Math.Abs(tmp1.X - tmp1.X) < Math.Abs(tmp2.Y - tmp2.Y)) //竖直排列mark点
            {
                if (tmp1.Y > tmp2.Y)
                {
                    mak1 = tmp2;
                    mak2 = tmp1;
                }
                else
                {
                    mak1 = tmp1;
                    mak2 = tmp2;
                }
            }
            else  //水平排列mark点
            {
                if (tmp1.X > tmp2.X)
                {
                    mak1 = tmp2;
                    mak2 = tmp1;
                }
                else
                {
                    mak1 = tmp1;
                    mak2 = tmp2;
                }
            }


            //圆mark点
            //mak1 = new Point(0.0, 0.0);
            //mak2 = new Point(0.0, 0.0);
            //Dictionary<double,double> points = new Dictionary<double, double>();
            //foreach (PATH th in lst)
            //{
            //    if (th.ePathType == EPathType.Circle)
            //    {
            //        if (th.Radio <= 5)
            //        {
            //            points[th.CenterX] = th.CenterY;
            //        }
            //    }
            //}

            //List<double> tmplist = points.Keys.ToList(); //x参数排序
            //tmplist.Sort();
            //foreach (double xtmp in tmplist)
            //{
            //    mak1.X = xtmp;
            //    mak1.Y = points[xtmp];
            //}
        }

        public void FindAbsoMaxVal(List<PATH> inList, out double Max_x, out double Max_y)
        {
            double min_x = 0.0;
            double min_y = 0.0;
            Max_x = 0.0;
            Max_y = 0.0;
            for (int i = 0; i < inList.Count; i++)
            {
                if (inList[i].ePathType == EPathType.Line)
                {
                    min_x = inList[i].StartX;
                    min_y = inList[i].StartY;
                    Max_x = inList[i].StartX;
                    Max_y = inList[i].StartY;
                    break;
                }
                else if (inList[i].ePathType == EPathType.Lwpolyline)
                {
                    min_x = inList[i].throughPoints[0].X;
                    min_y = inList[i].throughPoints[0].Y;
                    Max_x = inList[i].throughPoints[0].X;
                    Max_y = inList[i].throughPoints[0].Y;
                    break;
                }
                else if (inList[i].ePathType == EPathType.Circle)
                {
                    min_x = inList[i].CenterX;
                    min_y = inList[i].CenterY;
                    Max_x = inList[i].CenterX;
                    Max_y = inList[i].CenterY;
                    break;
                }
            }
            foreach (PATH tmpPath in inList)
            {
                if (tmpPath.ePathType == EPathType.Line)
                {
                    //最大值
                    if (tmpPath.StartX < min_x)
                    {
                        min_x = tmpPath.StartX;
                    }
                    if (tmpPath.StartY < min_y)
                    {
                        min_y = tmpPath.StartY;
                    }
                    if (tmpPath.EndX < min_x)
                    {
                        min_x = tmpPath.EndX;
                    }
                    if (tmpPath.EndY < min_y)
                    {
                        min_y = tmpPath.EndY;
                    }
                    //最小值
                    if (tmpPath.StartX > Max_x)
                    {
                        Max_x = tmpPath.StartX;
                    }
                    if (tmpPath.StartY > Max_y)
                    {
                        Max_y = tmpPath.StartY;
                    }
                    if (tmpPath.EndX > Max_x)
                    {
                        Max_x = tmpPath.EndX;
                    }
                    if (tmpPath.EndY > Max_y)
                    {
                        Max_y = tmpPath.EndY;
                    }
                }
                else if (tmpPath.ePathType == EPathType.Lwpolyline)
                {
                    foreach (Point tmp in tmpPath.throughPoints)
                    {
                        //最小值
                        if (tmp.X < min_x)
                        {
                            min_x = tmp.X;
                        }
                        if (tmp.Y < min_y)
                        {
                            min_y = tmp.Y;
                        }
                        //最大值
                        if (tmp.X > Max_x)
                        {
                            Max_x = tmp.X;
                        }
                        if (tmp.Y > Max_y)
                        {
                            Max_y = tmp.Y;
                        }
                    }
                }
            }
        }
        private void SwqpStartAndEndPoint(ref PATH pth)
        {
            double tmp = 0.0;
            tmp = pth.StartX;
            pth.StartX = pth.EndX;
            pth.EndX = tmp;

            tmp = pth.StartY;
            pth.StartY = pth.EndY;
            pth.EndY = tmp;
        }

        private void ConvertLwpolylineToPathList()
        {
            for (int i = 0; i < LwopolylineList.Count; i++)
            {
                if (i > -1)//if ((i >= 47)&&(i <= 50))//
                {
                    PATH oriPath = (PATH)LwopolylineList[i];
                    PATH newPath = new PATH();
                    newPath.ePathType = EPathType.Lwpolyline;
                    newPath.throughPoints = new List<Point>();
                    newPath.centerPoints = new List<Point>();
                    newPath.types = new List<int>();
                    newPath.Flag = oriPath.Flag;
                    for (int j = 0; j < oriPath.pointx.Length; j++)
                    {
                        if ((oriPath.converxity[j] != 0) && (j != oriPath.pointx.Length - 1))
                        {
                            Point startPoint = new Point();
                            Point endPoint = new Point();
                            startPoint.X = Math.Round((float)oriPath.pointx[j], 7);
                            startPoint.Y = Math.Round((float)oriPath.pointy[j], 7);
                            endPoint.X = Math.Round((float)oriPath.pointx[j + 1], 7);
                            endPoint.Y = Math.Round((float)oriPath.pointy[j + 1], 7);
                            Assist_LwP2DxfY_DXF(startPoint, endPoint, oriPath.converxity[j], out Point cent, out Point angle, out double radius, out bool clockwise);//凸度转弧度，圆心

                            newPath.throughPoints.Add(startPoint);
                            newPath.centerPoints.Add(cent);
                            if (clockwise)
                            {
                                newPath.types.Add(1);
                            }
                            else
                            {
                                newPath.types.Add(2);
                            }
                        }
                        else if ((oriPath.converxity[j] != 0) && (j == oriPath.pointx.Length - 1))
                        {
                            Point startPoint = new Point();
                            Point endPoint = new Point();
                            startPoint.X = Math.Round((float)oriPath.pointx[j], 7);
                            startPoint.Y = Math.Round((float)oriPath.pointy[j], 7);
                            endPoint.X = Math.Round((float)oriPath.pointx[0], 7);
                            endPoint.Y = Math.Round((float)oriPath.pointy[0], 7);
                            Assist_LwP2DxfY_DXF(startPoint, endPoint, oriPath.converxity[j], out Point cent, out Point angle, out double radius, out bool clockwise);//凸度转弧度，圆心

                            newPath.throughPoints.Add(startPoint);
                            newPath.centerPoints.Add(cent);
                            if (clockwise)
                            {
                                newPath.types.Add(1);
                            }
                            else
                            {
                                newPath.types.Add(2);
                            }
                        }
                        else
                        {
                            newPath.throughPoints.Add(new Point(Math.Round(oriPath.pointx[j], 7), Math.Round(oriPath.pointy[j], 7)));
                            newPath.centerPoints.Add(new Point(0.0, 0.0));
                            newPath.types.Add(0);
                        }
                    }
                    if (oriPath.Flag == 1)
                    {
                        newPath.throughPoints.Add(new Point(newPath.throughPoints[0].X, newPath.throughPoints[0].Y));
                        newPath.centerPoints.Add(new Point(0.0, 0.0));
                        newPath.types.Add(0);
                    }
                    PathArrayList.Add(newPath);
                }
            }
        }
        private void Assist_LwP2DxfY_DXF(Point pointStart, Point pointEnd, double dCrown, out Point cent, out Point angle, out double dfR, out bool clockwise)
        {
            cent = new Point();
            angle = new Point();
            dfR = new double();
            clockwise = false; //顺时针方向

            //当凸度dCrown不等于0时，表示为圆弧
            if (0.0 != dCrown)
            {
                ////角度,包角
                double theta_degree = 4 * Math.Atan(Math.Abs(dCrown));

                double dStarX = 0.0, dStarY = 0.0;//圆弧起始点
                double dEndX = 0.0, dEndY = 0.0;//圆弧终止点 
                //起始点，终止点
                dStarX = pointStart.X;
                dStarY = pointStart.Y;
                dEndX = pointEnd.X;
                dEndY = pointEnd.Y;

                //弦长
                double dLength = Math.Sqrt(Math.Pow(dStarX - dEndX, 2) + Math.Pow(dStarY - dEndY, 2));
                //圆弧半径
                dfR = Math.Abs(0.5 * dLength / Math.Sin(0.5 * theta_degree));

                //弦的斜率
                double dCenterX = 0, dCenterY = 0;//圆心坐标
                double dCenterX1 = 0, dCenterY1 = 0;//圆心坐标1
                double dCenterX2 = 0, dCenterY2 = 0;//圆心坐标2
                double k = (dEndY - dStarY) / (dEndX - dStarX);
                if (k == 0)
                {
                    dCenterX1 = (dStarX + dEndX) / 2.0;
                    dCenterX2 = (dStarX + dEndX) / 2.0;
                    dCenterY1 = dStarY + Math.Sqrt(dfR * dfR - (dStarX - dEndX) * (dStarX - dEndX) / 4.0);
                    dCenterY2 = dEndY - Math.Sqrt(dfR * dfR - (dStarX - dEndX) * (dStarX - dEndX) / 4.0);
                }
                else
                {
                    double mid_x = 0.0, mid_y = 0.0;//弦的中点坐标
                    double a = 1.0;
                    double b = 1.0;
                    double c = 1.0;
                    double k_verticle = 0.0;//弦的中垂线的斜率

                    k_verticle = -1.0 / k;
                    mid_x = (dStarX + dEndX) / 2.0;
                    mid_y = (dStarY + dEndY) / 2.0;
                    double b1 = mid_y - k_verticle * mid_x;

                    a = 1.0 + k_verticle * k_verticle;
                    //b = -2 * (dStarX + k_verticle * dStarY - k_verticle * b1);
                    b = -2 * mid_x - k_verticle * k_verticle * (dStarX + dEndX);
                    //c = dStarX * dStarX + dStarY * dStarY + b1 * b1 - 2 * b1 * dStarY - dfR * dfR;
                    c = mid_x * mid_x + k_verticle * k_verticle * (dStarX + dEndX) * (dStarX + dEndX) / 4.0 - (dfR * dfR - ((mid_x - dStarX) * (mid_x - dStarX) + (mid_y - dStarY) * (mid_y - dStarY)));
                    double dt = b * b - 4 * a * c;
                    if (dt < 0.00000001)
                    {
                        dt = 0;
                    }
                    dCenterX1 = (-1.0 * b + Math.Sqrt(dt)) / (2 * a);
                    dCenterX2 = (-1.0 * b - Math.Sqrt(dt)) / (2 * a);
                    dCenterY1 = k_verticle * dCenterX1 - k_verticle * mid_x + mid_y;
                    dCenterY2 = k_verticle * dCenterX2 - k_verticle * mid_x + mid_y;
                }

                //凸度绝对值小于1表示圆弧包角小于180°，凸度绝对值大于1表示圆弧包角大于180°
                bool isMinorArc = true;//圆弧半径是否为较小的
                if (Math.Abs(dCrown) <= 1)
                {
                    isMinorArc = true;
                }
                else
                {
                    isMinorArc = false;
                }
                //确定圆弧的顺逆
                int direction = 0;//判断是G02顺还是G03逆
                if (0.0 > dCrown)
                {
                    direction = 2;
                }
                else
                {
                    direction = 3;
                }

                //确定圆心
                //弦向量X正方向的角度
                double angleChordX = Math.Acos((1 * (dEndX - dStarX) + 0 * (dEndY - dStarY)) / dLength) * 180 / Math.PI;
                if ((dEndY - dStarY) < 0)
                {
                    angleChordX *= -1;
                }
                if ((angleChordX > 0 && angleChordX < 180) || angleChordX == 180)
                {
                    if (direction == 2)//顺圆
                    {
                        if (isMinorArc)
                        {
                            dCenterX = dCenterX1;
                            dCenterY = dCenterY1;
                        }
                        else
                        {
                            dCenterX = dCenterX2;
                            dCenterY = dCenterY2;
                        }
                    }
                    else if (direction == 3)//逆圆
                    {
                        if (isMinorArc)
                        {
                            dCenterX = dCenterX2;
                            dCenterY = dCenterY2;
                        }
                        else
                        {
                            dCenterX = dCenterX1;
                            dCenterY = dCenterY1;
                        }
                    }
                }
                else
                {
                    if (direction == 2)//顺圆
                    {
                        if (isMinorArc)
                        {
                            dCenterX = dCenterX2;
                            dCenterY = dCenterY2;
                        }
                        else
                        {
                            dCenterX = dCenterX1;
                            dCenterY = dCenterY1;
                        }
                        clockwise = true;
                    }
                    else if (direction == 3)//逆圆
                    {
                        if (isMinorArc)
                        {
                            dCenterX = dCenterX1;
                            dCenterY = dCenterY1;
                        }
                        else
                        {
                            dCenterX = dCenterX2;
                            dCenterY = dCenterY2;
                        }
                        clockwise = false;
                    }
                }

                //起始角度、终止角度
                double dStartVale = (dStarX - dCenterX) / dfR;
                //浮点型中的结果1可能是1.00000000000000001，避免这种情况出现。
                if (dStartVale > 1)
                    dStartVale = 1;
                if (dStartVale < -1)
                    dStartVale = -1;
                double dStarC = Math.Acos(dStartVale) * 180 / Math.PI;
                if (dStarY < dCenterY)
                {
                    dStarC = 360 - dStarC;
                }

                //终止角度、终止角度
                double dEndVale = (dEndX - dCenterX) / dfR;
                //浮点型中的结果1可能是1.00000000000000001，避免这种情况出现。
                if (dEndVale > 1)
                    dEndVale = 1;
                if (dEndVale < -1)
                    dEndVale = -1;
                double dEndC = Math.Acos(dEndVale) * 180 / Math.PI;
                if (dStarY < dCenterY)
                {
                    dEndC = 360 - dEndC;
                }
                cent.X = (float)dCenterX;
                cent.Y = (float)dCenterY;

                angle.X = (float)dStarC;
                angle.Y = (float)dEndC;
            }
        }
    }

    public class LAYER
    {
        public string name;
        public string colornum;
        public string lstyle;
        public string lwidth;
    }

    public class PATH
    {
        public string ePathType;
        public string LName;
        public string lwidth;
        public string colornum;
        public int PointCount;
        //直线，样条
        public double StartX;
        public double StartY;
        public double EndX;
        public double EndY;
        //圆弧，椭圆
        public double CenterX;
        public double CenterY;
        public double DeltaX;
        public double DeltaY;
        public double PStartAngle;
        public double PEndAngle;
        public double Radio;
        public double StartAngle;
        public double EndAngle;
        //折线
        public int Flag;
        public double[] pointx;
        public double[] pointy;
        public double[] converxity;
        public List<Point> throughPoints = new List<Point>();
        public List<Point> centerPoints = new List<Point>();
        public List<int> types = new List<int>();  //0直线 ，1正圆弧，2负圆弧

        public void CopyFrom(PATH oth)
        {
            ePathType = oth.ePathType;
            LName = oth.LName;
            lwidth = oth.lwidth;
            colornum = oth.colornum;
            PointCount = oth.PointCount;

            StartX = oth.StartX;
            StartY = oth.StartY;
            EndX = oth.EndX;
            EndY = oth.EndY;
            CenterX = oth.CenterX;
            CenterY = oth.CenterY;
            DeltaX = oth.DeltaX;
            DeltaY = oth.DeltaY;
            Radio = oth.Radio;
            PStartAngle = oth.PStartAngle;
            PEndAngle = oth.PEndAngle;
            Radio = oth.Radio;
            StartAngle = oth.StartAngle;
            EndAngle = oth.EndAngle;

            Flag = oth.Flag;
            pointx = oth.pointx;
            pointy = oth.pointy;
            converxity = oth.converxity;
            throughPoints.Clear();
            foreach (Point tmp in oth.throughPoints)
            {
                throughPoints.Add(tmp);
            }
            centerPoints.Clear();
            foreach (Point tmp in oth.centerPoints)
            {
                centerPoints.Add(tmp);
            }
            types.Clear();
            foreach (var tmp in oth.types)
            {
                types.Add(tmp);
            }
        }
        public PATH AddOffset(Point pos)
        {
            PATH ret = new PATH();
            ret.CopyFrom(this);
            if (this.ePathType == EPathType.Lwpolyline)
            {
                ret.throughPoints.Clear();
                ret.centerPoints.Clear();
                foreach (Point tmp in this.throughPoints)
                {
                    ret.throughPoints.Add(new Point(tmp.X + pos.X, tmp.Y + pos.Y));
                }
                foreach (Point tmp in this.centerPoints)
                {
                    ret.centerPoints.Add(new Point(tmp.X + pos.X, tmp.Y + pos.Y));
                }
            }
            if ((this.ePathType == EPathType.Line) || (this.ePathType == EPathType.Spline))
            {
                ret.StartX = this.StartX + pos.X;
                ret.StartY = this.StartY + pos.Y;
                ret.EndX = this.EndX + pos.X;
                ret.EndY = this.EndY + pos.Y;
            }
            if (this.ePathType == EPathType.Arc)
            {
                ret.CenterX = this.CenterX + pos.X;
                ret.CenterY = this.CenterY + pos.Y;
                ret.StartX = this.StartX + pos.X;
                ret.StartY = this.StartY + pos.Y;
                ret.EndX = this.EndX + pos.X;
                ret.EndY = this.EndY + pos.Y;
            }
            return ret;
        }
        public PATH Clon()
        {
            PATH oth = new PATH();
            oth.CopyFrom(this);
            return oth;
        }
        public PATH Rotate(double ang) //路径旋转
        {
            //x'=x*cos(a)-y*sin(a)
            //y'=x*sin(a)+y*cos(a)

            PATH oth = new PATH();
            oth.CopyFrom(this);

            oth.StartX = ComMath.PointRotate(this.StartX, this.StartY, ang).X;
            oth.StartY = ComMath.PointRotate(this.StartX, this.StartY, ang).Y;
            oth.EndX = ComMath.PointRotate(this.EndX, this.EndY, ang).X;
            oth.EndY = ComMath.PointRotate(this.EndX, this.EndY, ang).Y;
            oth.CenterX = ComMath.PointRotate(this.CenterX, this.CenterY, ang).X;
            oth.CenterY = ComMath.PointRotate(this.CenterX, this.CenterY, ang).Y;
            oth.DeltaX = ComMath.PointRotate(this.DeltaX, this.DeltaY, ang).X;
            oth.DeltaY = ComMath.PointRotate(this.DeltaX, this.DeltaY, ang).Y;

            oth.PStartAngle += ang;
            oth.PEndAngle += ang;
            oth.StartAngle += ang;
            oth.EndAngle += ang;
            List<Point> ls = new List<Point>();

            for (int i = 0; i < this.throughPoints.Count; i++)
            {
                ls.Add(ComMath.PointRotate(this.throughPoints[i], ang));
            }
            oth.throughPoints.Clear();
            foreach (Point pos in ls)
            {
                oth.throughPoints.Add(pos);
            }

            ls.Clear();
            for (int i = 0; i < this.centerPoints.Count; i++)
            {
                ls.Add(ComMath.PointRotate(this.centerPoints[i], ang));
            }
            oth.centerPoints.Clear();
            foreach (Point pos in ls)
            {
                oth.centerPoints.Add(pos);
            }

            return oth;
        }
        public PATH Scale(Point cenPos, double rate)//路径缩放
        {
            PATH oth = new PATH();
            oth.CopyFrom(this);

            oth.StartX = ComMath.PointScale(cenPos, this.StartX, this.StartY, rate).X;
            oth.StartY = ComMath.PointScale(cenPos, this.StartX, this.StartY, rate).Y;
            oth.EndX = ComMath.PointScale(cenPos, this.EndX, this.EndY, rate).X;
            oth.EndY = ComMath.PointScale(cenPos, this.EndX, this.EndY, rate).Y;
            oth.CenterX = ComMath.PointScale(cenPos, this.CenterX, this.CenterY, rate).X;
            oth.CenterY = ComMath.PointScale(cenPos, this.CenterX, this.CenterY, rate).Y;
            oth.DeltaX = ComMath.PointScale(cenPos, this.DeltaX, this.DeltaY, rate).X;
            oth.DeltaY = ComMath.PointScale(cenPos, this.DeltaX, this.DeltaY, rate).Y;

            List<Point> ls = new List<Point>();

            for (int i = 0; i < this.throughPoints.Count; i++)
            {
                ls.Add(ComMath.PointScale(cenPos, this.throughPoints[i], rate));
            }
            oth.throughPoints.Clear();
            foreach (Point pos in ls)
            {
                oth.throughPoints.Add(pos);
            }

            ls.Clear();
            for (int i = 0; i < this.centerPoints.Count; i++)
            {
                ls.Add(ComMath.PointScale(cenPos, this.centerPoints[i], rate));
            }
            oth.centerPoints.Clear();
            foreach (Point pos in ls)
            {
                oth.centerPoints.Add(pos);
            }

            return oth;
        }

        public double PathLength
        {
            get
            {
                if (this.ePathType == EPathType.Line)
                {
                    if (this.StartX == this.EndX) //竖直线
                    {
                        return Math.Abs(this.StartY - this.EndY);
                    }
                    if (this.StartY == this.EndY) //水平线
                    {
                        return Math.Abs(this.StartX - this.EndX);
                    }
                    return 0.0;
                }
                else
                {
                    return 0.0;
                }
            }
        }
    }

    public class EPathType
    {
        public static string Line = "Line";
        public static string Arc = "Arc";
        public static string Ellipse = "Ellipse";
        public static string Lwpolyline = "Lwpolyline";
        public static string Spline = "Spline";
        public static string Circle = "Circle";
    }
}
