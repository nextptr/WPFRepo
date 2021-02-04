using DXFComponent;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DXFAnalyse
{
    /// <summary>
    /// DxfManager.xaml 的交互逻辑
    /// </summary>
    public partial class DxfManager : UserControl
    {
        double max_x = 0.0;
        double max_y = 0.0;
        public DxfManager()
        {
            InitializeComponent();
            this.Loaded += DxfWindow_Loaded;

            btn_AddRecipe.Click += Btn_AddRecipe_Click;
            btn_DelRecipe.Click += Btn_DelRecipe_Click;

            dg_recipe.SelectionChanged += Dg_recipe_SelectionChanged;
        }

        private int selectRecIndex = -1;
        private DxfItem selectRceItem = null;
        private void DxfWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.IsVisible)
            {
                this.Loaded -= DxfWindow_Loaded;
                dg_recipe.ItemsSource = ParameterInstance.Instance.dxfParameter.Datas;
            }
        }
        private void Dg_recipe_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ParameterInstance.Instance.dxfParameter.Datas.Count < 1)
            {
                return;
            }
            selectRecIndex = dg_recipe.SelectedIndex;
            if (selectRecIndex < 0)
            {
                return;
            }
            selectRceItem = ParameterInstance.Instance.dxfParameter.Datas[selectRecIndex];
            dxfWind.PaintingPath(selectRceItem.RecipePath);
        }

        private void Btn_AddRecipe_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog().Value)
            {
                //读取dxf，获取路径和文件名
                string path = openFileDialog1.FileName;
                string[] arr = openFileDialog1.SafeFileName.Split('.');
                string name = arr[0];
                DxfReader.Instance.Read(path);
                if (DxfReader.Instance.PathList.Count < 0)
                {
                    MessageBox.Show("dxf文件读取失败");
                    return;
                }
                List<PATH> rotateLs = new List<PATH>();  //旋转后路径
                List<PATH> OneQuadLs = new List<PATH>(); //第一象限路径

                DxfReader.Instance.TransformToOneQuadrant(DxfReader.Instance.PathList, OneQuadLs, ref max_x, ref max_y);
                DxfReader.Instance.PathRotateAng(OneQuadLs, rotateLs, 180);
                DxfReader.Instance.TransformToOneQuadrant(rotateLs, OneQuadLs, ref max_x, ref max_y);
                //获取时间
                string tim = DateTime.Now.ToString("yyMMdd HH:mm");
                //保存参数
                DxfItem item = new DxfItem();
                item.RecipeName = name;
                item.RecipeDate = tim;
                item.RecipePath = OneQuadLs;

                item.Max_x = max_x;
                item.Max_y = max_y;
                ParameterInstance.Instance.dxfParameter.Datas.Add(item);
                //绘图
                int index = selectRecIndex;
                if (index < dg_recipe.Items.Count - 1)
                {
                    index = dg_recipe.Items.Count - 1;
                }
                //PaintingPath(ls, max_x, max_y);
                dg_recipe.ItemsSource = null;
                dg_recipe.ItemsSource = ParameterInstance.Instance.dxfParameter.Datas;
                dg_recipe.SelectedIndex = index + 1;
            }
        }
        private void Btn_DelRecipe_Click(object sender, RoutedEventArgs e)
        {
            if ((selectRecIndex != -1) && (ParameterInstance.Instance.dxfParameter.Datas.Count >= 1))
            {
                ParameterInstance.Instance.dxfParameter.Datas.RemoveAt(selectRecIndex);
            }
            int index = selectRecIndex;
            dg_recipe.ItemsSource = null;
            dg_recipe.ItemsSource = ParameterInstance.Instance.dxfParameter.Datas;
            if (index <= ParameterInstance.Instance.dxfParameter.Datas.Count - 1)
            {
                dg_recipe.SelectedIndex = index;
            }
            else
            {
                if (index - 1 >= 0)
                {
                    dg_recipe.SelectedIndex = index - 1;
                }
            }
        }
    }
}
