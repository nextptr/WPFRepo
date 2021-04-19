using PowerMeterDevice.Common;
using PowerMeterDevice.Interf;
using PowerMeterDevice.Parameter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerMeterDevice
{
    public class LinearFit : NotifyPropertyChanged, IDataFitting
    {
        private double _k = 1;
        public double FitK
        {
            get
            {
                return _k;
            }
            set
            {
                _k = value;
            }
        }

        private double _b = 1;
        public double FitB
        {
            get
            {
                return _b;
            }
            set
            {
                _b = value;
            }
        }

        private string fitType = "最小二乘法";
        public string FittingType
        {
            get
            {
                return fitType;
            }
            set
            {
                fitType = value;
                RaisePropertyChanged(nameof(FittingType));
            }
        }

        public double GetKeyFromValue(double inPutValue)
        {
            return Math.Round((inPutValue - FitB) / FitK, 3);
        }
        public double GetValueFromKey(double inPutKey)
        {
            return Math.Round(FitK * inPutKey + FitB, 3);
        }
        public void ImportKeyValueDatas(List<TestDataItem> ls)
        {
            double xsum = 0;
            double ysum = 0;
            double xysum = 0;
            double x2sum = 0;
            int m = ls.Count;
            for (int i = 0; i < m; i++)
            {
                xsum = xsum + ls[i].TestKey;
                ysum = ysum + ls[i].TestValue;
                xysum = xysum + ls[i].TestKey * ls[i].TestValue;
                x2sum = x2sum + ls[i].TestKey * ls[i].TestKey;
            }
            FitK = (m * xysum - xsum * ysum) / (m * x2sum - xsum * xsum + 1e-10);
            FitB = (ysum - FitK * xsum) / m;
        }

        public LinearFit()
        { }
    }

    public class CurveFuncs
    {
        public static bool CalculateCurve(List<TestDataItem> Points, int dim, bool izero, double[] x)
        {
            double[] transX = new double[x.Length];
            if (izero)
            {
                double[,] A = new double[Points.Count, dim];
                double[] B = new double[Points.Count];
                int intValid = 0;
                for (int i = 0; i < Points.Count; i++)
                {
                    for (int j = 0; j < dim; j++)
                    {
                        A[intValid, j] = Math.Pow(Points[i].TestKey, j + 1);
                    }
                    B[intValid] = Points[i].TestValue;
                    intValid++;
                }
                if (intValid < dim)
                {
                    return false;
                }
                MatrixEquation(Points.Count, dim, A, B, transX);
                for (int i = 0; i < x.Length; i++)
                {
                    x[i] = transX[i];
                }
            }
            else
            {
                double[,] A = new double[Points.Count, dim + 1];
                double[] B = new double[Points.Count];
                int intValid = 0;
                for (int i = 0; i < Points.Count; i++)
                {
                    A[intValid, 0] = 1;
                    for (int j = 0; j < dim; j++)
                        A[intValid, j + 1] = Math.Pow(Points[i].TestKey, j + 1);
                    B[intValid] = Points[i].TestValue;
                    intValid++;
                }
                if (intValid < dim + 1)
                {
                    return false;
                }
                MatrixEquation(Points.Count, dim + 1, A, B, transX);
                for (int i = 0; i < x.Length; i++)
                {
                    x[i] = transX[transX.Length - i - 1];
                }
            }

            return true;
        }

        public static void Trmul(int m, int n, int k, double[,] a, double[,] b, double[,] c) //矩阵相乘
        {
            // a[m,n] b[n,k] c[m,k]
            int i, j, L;
            for (i = 0; i <= m - 1; i++)
                for (j = 0; j <= k - 1; j++)
                {
                    c[i, j] = 0.0;
                    for (L = 0; L <= n - 1; L++)
                        c[i, j] = c[i, j] + a[i, L] * b[L, j];
                }
        }

        public static void Rinv(int n, double[,] a)//矩阵的逆
        {
            int[] si = new int[n];
            int[] sj = new int[n];
            int i, j, k;
            double d, p;

            for (k = 0; k < n; k++)
            {
                d = 0.0;
                for (i = k; i < n; i++)
                    for (j = k; j < n; j++)
                    {
                        p = System.Math.Abs(a[i, j]);
                        if (p > d)
                        {
                            d = p; si[k] = i; sj[k] = j;
                        }
                    }
                if ((d + 1.0) == 1.0)
                {
                    throw new Exception("Can not Make Curve");
                }
                if (si[k] != k)
                    for (j = 0; j < n; j++)
                    {
                        p = a[k, j];
                        a[k, j] = a[si[k], j];
                        a[si[k], j] = p;
                    }
                if (sj[k] != k)
                    for (i = 0; i < n; i++)
                    {
                        p = a[i, k];
                        a[i, k] = a[i, sj[k]];
                        a[i, sj[k]] = p;
                    }

                a[k, k] = 1.0 / a[k, k];
                for (j = 0; j < n; j++)
                    if (j != k)
                        a[k, j] = a[k, j] * a[k, k];
                for (i = 0; i < n; i++)
                    if (i != k)
                        for (j = 0; j < n; j++)
                            if (j != k)
                                a[i, j] = a[i, j] - a[i, k] * a[k, j];
                for (i = 0; i < n; i++)
                    if (i != k)
                        a[i, k] = -a[i, k] * a[k, k];
            }

            for (k = n - 1; k >= 0; k--)
            {
                if (sj[k] != k)
                    for (j = 0; j <= n - 1; j++)
                    {
                        p = a[k, j]; a[k, j] = a[sj[k], j]; a[sj[k], j] = p;
                    }
                if (si[k] != k)
                    for (i = 0; i <= n - 1; i++)
                    {
                        p = a[i, k]; a[i, k] = a[i, si[k]]; a[i, si[k]] = p;
                    }
            }
        }

        public static void MatrixEquation(int m, int n, double[,] a, double[] b, double[] x)//解矩阵方程(Ax=B)
        {
            double[,] at = new double[n, m]; //A转置矩阵
            double[,] a1 = new double[n, n]; //B逆矩阵
            double[,] c = new double[m, 2];
            double[,] b2 = new double[m, 2];
            for (int i = 0; i <= n - 1; i++)
                for (int j = 0; j <= m - 1; j++)
                {
                    at[i, j] = a[j, i];
                }
            Trmul(n, m, n, at, a, a1);
            Rinv(n, a1);
            for (int i = 0; i <= m - 1; i++)
                b2[i, 0] = b[i];
            Trmul(n, m, 1, at, b2, c);
            Trmul(n, n, 1, a1, c, at);
            for (int i = 0; i <= n - 1; i++)
                x[i] = at[i, 0];
        }

    }
}
