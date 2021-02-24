using System;
using System.Collections.Generic;
using System.IO;
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

namespace WpfApp
{
    /// <summary>
    /// DependencyPropertyBase.xaml 的交互逻辑
    /// </summary>
    public partial class DependencyPropertyBase : UserControl
    {
        public DependencyPropertyBase()
        {
            InitializeComponent();
            test_1();
            test_2();
            test_3();
        }


        //普通CLR属性
        private int _age;
        public int Age
        {
            get
            {
                return _age;
            }
            set
            {
                if (value >= 0 && value <= 100)
                {
                    _age = value;
                }
                else
                {
                    throw new OverflowException("Age overflow.");
                }
            }
        }

        //1最简依赖属性
        Student stu;
        private void test_1()
        {
            stu = new Student();
            //1.1 无包装器使用依赖属性
            {
                //Binding binding = new Binding("Text") { Source = txt_base_box1 };
                //btn_base_ok.Click += Btn_base_ok_Click;
                //BindingOperations.SetBinding(stu, Student.NameProperty, binding);
            }
            //1.2 直接关联两个TextBox
            {
                //Binding binding = new Binding("Text") { Source = txt_base_box1 };
                //txt_base_box2.SetBinding(TextBox.TextProperty, binding);
            }
            //1.3 使用CLR属性包装依赖项属性
            {
                //btn_base_ok.Click += Btn_base_ok_Click1;
            }
            //1.4 添加SetBinding功能
            {
                stu.SetBinding(Student.NameProperty, new Binding("Text") { Source = txt_base_box1 }); //依赖属性作为绑定的目标
                txt_base_box2.SetBinding(TextBox.TextProperty, new Binding("Name") { Source = stu }); //依赖属性作为绑定的源
            }
        }

        private void Btn_base_ok_Click(object sender, RoutedEventArgs e)
        {
            stu = new Student();//依赖属性
            stu.SetValue(Student.NameProperty, txt_base_box1.Text); //设置依赖属性
            txt_base_box2.Text = (string)stu.GetValue(Student.NameProperty); //获取依赖属性
            MessageBox.Show(stu.GetValue(Student.NameProperty).ToString());
        }
        private void Btn_base_ok_Click1(object sender, RoutedEventArgs e)
        {
            stu.Name = txt_base_box1.Text;  //使用包装属性来设置依赖属性值
            txt_base_box2.Text = stu.Name;  //使用包装属性来获取依赖属性值
        }

        //2附加属性测试
        private void Msg(object obj)
        {
            ls_box.Items.Add(obj);
        }
        private void test_2()
        {
            btn_test_att.Click += Btn_test_att_Click;
        }
        private void Btn_test_att_Click(object sender, RoutedEventArgs e)
        {
            Human human = new Human();
            School.SetGrade(human, 6);
            int grad = School.GetGrade(human);
            Msg($"School的GlobalIndex:{School.GradeProperty.GlobalIndex}");
            Msg($"human的LocalValueEnumerator的HashCode:{human.GetLocalValueEnumerator().GetHashCode()}");
            Msg($"human附加School的Grad属性值:{grad}");
            Msg($"human附加School的依赖属性个数:{human.GetLocalValueEnumerator().Count}");

            //遍历依赖属性
            LocalValueEnumerator loc = human.GetLocalValueEnumerator();
            while (loc.MoveNext())
            {
                Msg($"property: {loc.Current.Property.ToString()}");
                Msg($"Value: {loc.Current.Value.ToString()}");
                Msg($"GetHashCode: {loc.Current.GetHashCode().ToString()}");
                Msg($"GetType: {loc.Current.Property.GetType().ToString()}");
            }
        }

        //3典型附加属性使用
        private void test_3()
        {
            this.Loaded += DependencyPropertyBase_Loaded;
        }

        private void DependencyPropertyBase_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.IsVisible == true)
            {
                auto_grid();
            }
        }

        private void auto_grid()
        {
            Grid grd = new Grid();
            grd.ShowGridLines = true;
            grd.RowDefinitions.Add(new RowDefinition());
            grd.RowDefinitions.Add(new RowDefinition());
            grd.RowDefinitions.Add(new RowDefinition());

            grd.ColumnDefinitions.Add(new ColumnDefinition());
            grd.ColumnDefinitions.Add(new ColumnDefinition());
            grd.ColumnDefinitions.Add(new ColumnDefinition());

            Button btn = new Button();
            btn.Content = "使用依赖属性";
            Grid.SetColumn(btn, 1);
            Grid.SetRow(btn, 1);
            grd.Children.Add(btn);
            lab_auto_att.Content = grd;
        }
    }


    //最简依赖属性
    public class Student : DependencyObject
    {
        //依赖属性定义
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(Student));


        //依赖属性CLR包装
        public string Name
        {
            get
            {
                return (string)GetValue(Student.NameProperty);
            }
            set
            {
                SetValue(NameProperty, value);
            }
        }


        //SetBinding包装
        public BindingExpressionBase SetBinding(DependencyProperty dp, BindingBase binding)
        {
            return BindingOperations.SetBinding(this, dp, binding);
        }
        
    }

    //附加属性
    public class School : DependencyObject
    {
        public static readonly DependencyProperty GradeProperty =
            DependencyProperty.RegisterAttached("Grad", typeof(int), typeof(School), new UIPropertyMetadata(0));


        public static int GetGrade(DependencyObject obj)
        {
            return (int)obj.GetValue(GradeProperty);
        }
        public static void SetGrade(DependencyObject obj, int value)
        {
            obj.SetValue(GradeProperty, value);
        }
    }

    public class Human : DependencyObject
    {
    }


}
