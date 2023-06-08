using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
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
        [Import]
        public IBookService Service { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            this.Compose();
            if (this.Service != null)
            {
                lsbox.Items.Add(this.Service.GetBookName());
            }

            foreach (var item in UseMef.Instance.MefLs)
            {
                lsbox.Items.Add(item.GetResult());
            }

            foreach (var item in UseMef.Instance.MefLs)
            {
                if (item.MefType == "NewYear")
                {
                    lsbox.Items.Add("NewYear " + item.GetResult());
                }
            }
        }

        private void Compose()
        {
            var catalog = new AssemblyCatalog(Assembly.GetExecutingAssembly());
            CompositionContainer container = new CompositionContainer(catalog);
            container.ComposeParts(this);
        }
    }
}
