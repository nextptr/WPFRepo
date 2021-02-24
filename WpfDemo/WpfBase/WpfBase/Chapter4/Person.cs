using System.Windows;

namespace WpfBase.Chapter4
{
    public class Person: DependencyObject
    {
        //1、依赖属性的所在类型继承自DependencyObject类。
        //2、使用public static 声明一个DependencyProperty的变量，该变量就是真正的依赖属性。
        //3、类型的静态构造函数中通过Register方法完成依赖属性的元数据注册。
        //4、提供依赖属性的包装属性，通过这个属性来完成对依赖属性的读写操作。
        public string Name
        {
            //CLR属性包装器，使得依赖属性NameProperty在外部能够像普通属性那样使用 
            get
            {
                return (string)GetValue(NameProperty);
            }
            set
            {
                SetValue(NameProperty, value);                
            }
        }
        //依赖属性必须为static readonly
        //DependencyProperty.Register 参数说明
        //第一个参数是string类型的，是属性名。
        //第二个参数是这个依赖项属性的类型。
        //第三个参数是这个拥有这个依赖项属性的类型。
        //第四个参数是具有附加属性设置的FramWorkPropertyMetadata对象
        public static readonly DependencyProperty NameProperty = DependencyProperty.Register("Name", typeof(string), typeof(Person), new PropertyMetadata("DefaultName"));
        // 从上面代码可以看出，依赖属性是通过调用DependencyObject的GetValue和SetValue来对依赖属性进行读写的。
        // 它使用哈希表来进行存储的，对应的Key就是属性的HashCode值，而值（Value）则是注册的DependencyPropery；
        // 而C#中的属性是类私有字段的封装，可以通过对该字段进行操作来对属性进行读写。属性是字段的包装，WPF中使用属性对依赖属性进行包装。
    }
}
