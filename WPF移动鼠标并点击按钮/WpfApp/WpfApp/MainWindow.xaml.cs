using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace WpfApp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        MouseHelper mouseHelper = new MouseHelper();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_Mouse(object sender, RoutedEventArgs e)
        {
            int posX = int.Parse(txt_x.Text.ToString());
            int posY = int.Parse(txt_y.Text.ToString());
            MouseHelper.SemiMouseEventAbsPix(MouseEventFlag.LeftDown | MouseEventFlag.LeftUp, posX, posY);
        }

        private void Button_Click_Keyboard(object sender, RoutedEventArgs e)
        {
            MouseHelper.SemiMouseEventAbsPix(MouseEventFlag.LeftDown | MouseEventFlag.LeftUp, 800, 600);
            MouseHelper.SemiKeyBoardEvent(Keys.G);
            MouseHelper.SemiKeyBoardEvent(Keys.S);
            MouseHelper.SemiKeyBoardEvent(Keys.D);
        }
        private void Button_Test(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show($"click event");
        }
    }
}
