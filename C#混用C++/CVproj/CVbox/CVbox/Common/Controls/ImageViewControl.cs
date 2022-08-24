using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CVbox.Common.Controls
{
    public class ImageViewControl : Control
    {
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(ImageViewControl),
            new FrameworkPropertyMetadata("123", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));


        public CSImage SourceImg
        {
            get { return (CSImage)GetValue(SourceImgProperty); }
            set 
            { 
                SetValue(SourceImgProperty, value);
            }
        }
        // Using a DependencyProperty as the backing store for SourceImg.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SourceImgProperty = DependencyProperty.Register("SourceImg", typeof(CSImage), typeof(ImageViewControl), 
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSourceImgChanged));

        private static void OnSourceImgChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ImageViewControl control = d as ImageViewControl;
            if (e.NewValue != null)
            {
                CSImage src = e.NewValue as CSImage;
                if (control._image != null)
                {
                    control._image.Source = src.BitMap;
                }
            }
        }

  
        Image _image;
        static ImageViewControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageViewControl), new FrameworkPropertyMetadata(typeof(ImageViewControl)));
        }
        public override void OnApplyTemplate()
        {
            _image = GetTemplateChild("img") as Image;
            if (_image != null&& SourceImg!=null)  //OnApplyTemplate 滞后与属性改变事件 但是总有办法可以给相关对象赋值
            {
                _image.Source = SourceImg.BitMap;
            }
            base.OnApplyTemplate();
        }
    }
}
