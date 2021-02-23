using Common.EventAggregator;
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

namespace WpfTips.WpfLambda
{
    /// <summary>
    /// LambdaPanel.xaml 的交互逻辑
    /// </summary>
    public partial class LambdaPanel : UserControl
    {
        public LambdaPanel()
        {
            InitializeComponent();
            this.Loaded += LambdaPanel_Loaded;
        }

        private void LambdaPanel_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.IsVisible == true)
            {
                test1();
                test2();
                test3();
                test4();
            }
        }

        private void addMsg(object obj)
        {
            CommonEventAggregator.Instance.Publish(new MessageEventArgs(obj));
        }

        private List<Person> PersonList()
        {
            List<Person> persons = new List<Person>();
            for (int i = 0; i < 7; i++)
            {
                Person p = new Person() { Name = i + "儿子", Age = 8 - i, };
                persons.Add(p);
            }
            return persons;
        }

        /// <summary>
        /// 感受lambda
        /// </summary>
        private void test1()
        {
            List<Person> persons = PersonList();
            persons = persons.Where(p => p.Age > 6).ToList();
            foreach (var item in persons)
            {
                addMsg("age:" + item.Age + " name:" + item.Name);
            }

            Person per = persons.SingleOrDefault(p => p.Age == 1);

            persons = persons.Where(p => p.Name.Contains("儿子")).ToList();
            foreach (var item in persons)
            {
                addMsg("age:" + item.Age + " name:" + item.Name);
            }
            addMsg(" ");
        }

        /// <summary>
        /// gwl 传统委托函数
        /// gwc lambda委托方式
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        delegate int GuangChaoShi(int a);
        private void test2()
        {
            GuangChaoShi gwl = jiezhang;
            addMsg(gwl(10) + "");

            GuangChaoShi gwc = p => p + 20;
            addMsg(gwc(10) + "");
            addMsg(" ");
        }
        private int jiezhang(int a)
        {
            return a + 10;
        }

        /// <summary>
        /// 多参数的lambda委托
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        delegate int GuangTaoBao(int a, int b);
        private void test3()
        {
            GuangTaoBao gwl = (p, z) => z - (p + 10);
            addMsg(gwl(10, 100) + " ");

            GuangTaoBao gwc = (p, z) =>
            {
                int zuidixiaofei = 10;
                if (p < zuidixiaofei)
                {
                    return 100;
                }
                else
                {
                    return 100 + (p - 100) / z;
                }
            };
            addMsg(gwc(120, 3) + " ");
            addMsg(" ");
        }

        private void test4()
        {
            Func<int, string> gwl = p => p + 10 + "--返回类型为string";
            addMsg(gwl(123));

            Func<int, int, bool> gwc = (p, j) =>
            {
                if (p > j)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            };
            addMsg("1>2:" + gwc(1, 2));
            addMsg("2>1:" + gwc(2, 1));
            addMsg(" ");

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            addMsg("12");
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
