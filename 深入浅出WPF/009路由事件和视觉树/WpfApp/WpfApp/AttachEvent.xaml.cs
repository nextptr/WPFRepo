using System;
using System.Collections.Generic;
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
    /// AttachEvent.xaml 的交互逻辑
    /// </summary>
    public partial class AttachEvent : UserControl
    {
        public AttachEvent()
        {
            InitializeComponent();
            btn_attach.Click += Btn_attach_Click;
            //unif_main.AddHandler(Student.NameChangedEvent, new RoutedEventHandler(studentNameChangeEventHandle));
            Student.AddNameChangedHandle(unif_main, new RoutedEventHandler(studentNameChangeEventHandle));
        }

        private void Btn_attach_Click(object sender, RoutedEventArgs e)
        {
            Student stu = new Student() { Id = 101, Name = "Tim" };
            stu.Name = "Tom";
            RoutedEventArgs arg = new RoutedEventArgs(Student.NameChangedEvent, stu);
            btn_attach.RaiseEvent(arg); //Student非UI元素、没有RaiseEvent接口，必须借用
        }

        private void studentNameChangeEventHandle( object sender, RoutedEventArgs e )
        {
            ls_box.Items.Add($"附加事件被激发:{(e.OriginalSource as Student).Id.ToString()}");
        }

    }

    public class Student
    {
        public static readonly RoutedEvent NameChangedEvent = EventManager.RegisterRoutedEvent("NameChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Student));

        public int Id { get; set; }

        public string Name { get; set; }

        public static void AddNameChangedHandle(DependencyObject d, RoutedEventHandler h)
        {
            UIElement e = d as UIElement;
            if (e != null)
            {
                e.AddHandler(Student.NameChangedEvent, h);
            }
        }

        public static void RemoveNameChangedHandle(DependencyObject d, RoutedEventHandler h)
        {
            UIElement e = d as UIElement;
            if (e != null)
            {
                e.RemoveHandler(Student.NameChangedEvent, h);
            }
        }
    }
}
