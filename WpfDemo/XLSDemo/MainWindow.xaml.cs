using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using static XLSDemo.ExportToExcelHelper;

namespace XLSDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ExportToExcelHelper.Instance.msg = msg;
            this.btnTest.Click += BtnTest_Click;
            btnReadPlc.Click += BtnReadPlc_Click;
        }
        private void msg(string str)
        {
            lsBox.Items.Insert(0,str);
            if (lsBox.Items.Count > 30)
            {
                lsBox.Items.RemoveAt(30);
            }
        }

        private void BtnTest_Click(object sender, RoutedEventArgs e)
        {
            ExcelSheet sheet = new ExcelSheet("sheet");
            for (int i = 0; i < 5; i++)
            {
                ExcelCol col = new ExcelCol($"header{i}");
                for (int j = 0; j < 7; j++)
                {
                    col.Cols.Add(i + j);
                }
                sheet.ListColums.Add(col);
            }
            ExcelFile excel = new ExcelFile("NpoiExel");
            excel.ListSheet.Add(sheet);
            ExportToExcelHelper.Instance.CreateExcel(excel);
        }


        private void BtnReadPlc_Click(object sender, RoutedEventArgs e)
        {
            double d = Math.Sqrt(60 * 60 + 70 * 70);
            int count = (int)((2 * Math.PI * 150) / d);


            string fileName = @"F:\文件处理\欧姆龙PLC.xlsx";
            string txtName = @"F:\文件处理\PLCConfig.xlsx";
            ExportToExcelHelper.Instance.ReadPlc(fileName, txtName);
        }
    }
}
