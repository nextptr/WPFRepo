using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GDIDrawing
{
    public class FontItem
    {
        //文本、字体、颜色
        private string text;
        private string fontName;
        private int fontSize;
        private Color fontColor;

        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }
        public string FontName
        {
            get
            {
                return fontName;
            }
            set
            {
                fontName = value;
            }
        }
        public int FontSize
        {
            get
            {
                return fontSize;
            }
            set
            {
                fontSize = value;
            }
        }
        public Color FontColor
        {
            get
            {
                return fontColor;
            }
            set
            {
                fontColor = value;
            }
        }


        //字体描边、深度、渐变
        private Color gradientColor1;
        private Color gradientColor2;
        private BitmapImage overlayImage;
        private Color strokeColor;
        private int strokeColorLength;

        public Color GradientColor1
        {
            get
            {
                return gradientColor1;
            }
            set
            {
                gradientColor1 = value;
            }
        }
        public Color GradientColor2
        {
            get
            {
                return gradientColor2;
            }
            set
            {
                gradientColor2 = value;
            }
        }
        public BitmapImage OverlayImage
        {
            get
            {
                return overlayImage;
            }
            set
            {
                overlayImage = value;
            }
        }
        public Color StrokeColor
        {
            get
            {
                return strokeColor;
            }
            set
            {
                strokeColor = value;
            }
        }
        public int StrokeColorLength
        {
            get
            {
                return strokeColorLength;
            }
            set
            {
                strokeColorLength = value;
            }
        }
    }
}
