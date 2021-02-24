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
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SomePeople people = new SomePeople("李华");
            people.SetUserClient(lsbox);
            people.SearchGoogle("1+1=?");
            people.WatchYoutube("joker is who");
            people.FreshTwitter("blabla");
        }
    }

    public interface internet  //公共接口
    {
        void SetUserClient(ListBox box);
        void WatchYoutube(string msg);
        void SearchGoogle(string msg);
        void FreshTwitter(string msg);
    }

    public class internetProxy : internet
    {
        ListBox box;
        public void SetUserClient(ListBox lsbox)
        {
            box = lsbox;
        }
        public void FreshTwitter(string msg)
        {
            box.Items.Add(msg);
        }
        public void SearchGoogle(string msg)
        {
            box.Items.Add(msg);
        }
        public void WatchYoutube(string msg)
        {
            box.Items.Add(msg);
        }
    }

    public class SomePeople : internet
    {
        internetProxy proxy = new internetProxy();
        string name = "";
        public SomePeople(string nam)
        {
            name = nam;
        }
        public void SetUserClient(ListBox box)
        {
            proxy.SetUserClient(box);
        }

        public void FreshTwitter(string msg)
        {
            proxy.FreshTwitter(name + ": " + msg);
        }

        public void SearchGoogle(string msg)
        {
            proxy.SearchGoogle(name + ": " + msg);
        }

        public void WatchYoutube(string msg)
        {
            proxy.WatchYoutube(name + ": " + msg);
        }
    }

}
