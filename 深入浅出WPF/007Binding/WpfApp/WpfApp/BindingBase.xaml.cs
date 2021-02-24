using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Xml.Linq;


namespace WpfApp
{
    /// <summary>
    /// BindingBase.xaml 的交互逻辑
    /// </summary>
    public partial class BindingBase : UserControl
    {
        private Student stu = null;
        List<Student> stuList = null;
        public BindingBase()
        {
            InitializeComponent();

            dataContest_binding();

            binding_code();
            stuList = new List<Student>()
            {
                new Student(){ ID=0,Name="Tim",Age=29},
                new Student(){ ID=1,Name="Tom",Age=28},
                new Student(){ ID=2,Name="Kyle",Age=27},
                new Student(){ ID=3,Name="Tony",Age=26},
                new Student(){ ID=4,Name="Vine",Age=25},
                new Student(){ ID=5,Name="Mike",Age=24},
            };
            binding_listBox();

            //list_view binding
            ls_view_full.ItemsSource = stuList;

            //button Linq
            btn_list_linq.Click += Btn_list_linq_Click;
            btn_xml_linq.Click += Btn_xml_linq_Click;

            //xmlbinding
            InitXmlBinding();
        }

        //DataContext自动赋值
        private void dataContest_binding()
        {
            int txt_int = 10;
            txt_auto.DataContext = txt_int;

            List<int> ls_int = new List<int>();
            ls_int.Add(1);
            ls_int.Add(2);
            ls_int.Add(3);
            ls_auto.ItemsSource = ls_int;
        }

        //代码绑定
        private void binding_code()
        {
            //数据源
            stu = new Student();

            //Binding
            Binding binding = new Binding();
            binding.Source = stu;
            binding.Path = new PropertyPath("Name");

            //连接Binding数据源和目标
            BindingOperations.SetBinding(this.txt_box, TextBox.TextProperty, binding);
            btn_test.Click += Btn_test_Click;
        }

        //ListBox绑定
        private void binding_listBox()
        {
            //listBox设置binding
            ls_box_list.ItemsSource = stuList;
            ls_box_list.DisplayMemberPath = "Name";
            ls_box_full.ItemsSource = stuList;

            //textBox设置binding
            Binding bind = new Binding("SelectedItem.ID") { Source = ls_box_list };
            txt_box_id.SetBinding(TextBox.TextProperty, bind);
        }
        private void Btn_test_Click(object sender, RoutedEventArgs e)
        {
            //stu.Name += "Name ";
            stu.Name += getObjDirectory();
        }

        //DataContext绑定
        private void Btn_context_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(btn_context.DataContext.ToString());
        }


        //xml
        private void InitXmlBinding()
        {
            //XmlDocument doc = new XmlDocument();
            //doc.Load(@"D:\RawData.xml");

            //XmlDataProvider xdp = new XmlDataProvider();
            //xdp.Document = doc;


            ////使用XPath选择需要暴露的数据
            ////现在时需要暴露一组Student
            //xdp.XPath = @"/StudentList/Student";
            //ls_view_xml.DataContext = xdp;
            //ls_view_xml.SetBinding(ListView.ItemsSourceProperty, new Binding());

            XmlDataProvider xdp = new XmlDataProvider();
            xdp.Source = new Uri(getObjDirectory() + @"\RawData.xml");

            //使用XPath选择需要暴露的数据
            //现在时需要暴露一组Student
            xdp.XPath = @"/StudentList/Student";
            ls_view_xml.DataContext = xdp;
            ls_view_xml.SetBinding(ListView.ItemsSourceProperty, new Binding());
        }

        private string getObjDirectory()
        {
            string ret = "";
            ret = Directory.GetCurrentDirectory() + @"\..\..\..\..";
            ret = Directory.GetParent(ret).FullName;
            return ret;
        }


        //linq
        private void Btn_list_linq_Click(object sender, RoutedEventArgs e)
        {
            ls_linq.ItemsSource = from stud in stuList where stud.ID.CompareTo(-1) > 0 ? true : false select stud;
        }
        private void Btn_xml_linq_Click(object sender, RoutedEventArgs e)
        {
            XDocument xdoc = XDocument.Load(getObjDirectory() + @"\Students.xml");
            ls_linq.ItemsSource = from element in xdoc.Descendants("Student")
                                  where element.Attribute("Name").Value.StartsWith("T")
                                  select new Student()
                                  {
                                      ID = int.Parse(element.Attribute("ID").Value),
                                      Age = int.Parse(element.Attribute("Age").Value),
                                      Name = element.Attribute("Name").Value
                                  };
        }
    }
}
