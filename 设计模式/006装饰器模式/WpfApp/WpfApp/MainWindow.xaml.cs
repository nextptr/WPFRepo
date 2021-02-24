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

            ConcreateComponent personCai = new ConcreateComponent(ls_bx, "小菜");
            ConcreateComponent personNiao = new ConcreateComponent(ls_bx, "大鸟");

            ConcreteDecoratorWhiteHat   _WhiteHat = new ConcreteDecoratorWhiteHat(ls_bx);
            ConcreteDecoratorBlackHat   _BlackHat = new ConcreteDecoratorBlackHat(ls_bx);
            ConcreteDecoratorTShirt     _TShirt = new ConcreteDecoratorTShirt(ls_bx);
            ConcreteDecoratorSuit       _Suit = new ConcreteDecoratorSuit(ls_bx);
            ConcreteDecoratorTie        _Tie = new ConcreteDecoratorTie(ls_bx);
            ConcreteDecoratorBigTrouser _BigTrouser = new ConcreteDecoratorBigTrouser(ls_bx);

            _WhiteHat.SetComponent(personCai);
            _TShirt.SetComponent(_WhiteHat);
            _BigTrouser.SetComponent(_TShirt);
            _BigTrouser.operation();

            ls_bx.Items.Add("");

            _BlackHat.SetComponent(personNiao);
            _Suit.SetComponent(_BlackHat);
            _Tie.SetComponent(_Suit);
            _Tie.operation();
        }
    }
}
