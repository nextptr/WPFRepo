using System;
using System.Drawing.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GDIDrawing
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private FontItem fontItem;
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.IsVisible == true)
            {
                initFontName();
                initGradientColor();
                this.Loaded -= MainWindow_Loaded;
            }
        }

        //字体绘制、颜色
        private void initFontName()
        {
            try
            {
                txt_input.TextChanged += Txt_input_TextChanged;
                com_font_type.SelectionChanged += Com_font_type_SelectionChanged;
                num_font_size.ValueChanged += Num_font_size_ValueChanged;
                rect_font_color.MouseLeftButtonDown += Rect_font_color_MouseLeftButtonDown;


                InstalledFontCollection insfont = new InstalledFontCollection();
                System.Drawing.FontFamily[] families = insfont.Families;
                foreach (System.Drawing.FontFamily family in families)
                {
                    this.com_font_type.Items.Add(family.Name);
                }
                this.com_font_type.SelectedItem = "宋体";
            }
            catch (Exception ex)
            {
            }
        }
        private void Com_font_type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fontItem != null)
            {
                fontItem.FontName = com_font_type.SelectedValue.ToString();
                DrawText();
            }
        }
        private void Num_font_size_ValueChanged(object sender, EventArgs e)
        {
            if (fontItem != null)
            {
                fontItem.FontSize = (int)num_font_size.Value;
                DrawText();
            }
        }
        private void Rect_font_color_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ColorSelectorDialog csw = new ColorSelectorDialog();
            csw.ShowDialog();
            if (fontItem != null)
            {
                fontItem.FontColor = csw.returnSelectColor;
                rect_font_color.Fill = new SolidColorBrush(csw.returnSelectColor);
                DrawText();
            }
        }

        private void Txt_input_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            GetFontItem(txt_input.Text.Trim());
            DrawText();
        }
        private void GetFontItem(string text)
        {
            if (text.Length < 0)
            {
                fontItem = null;
                return;
            }

            fontItem = new FontItem();
            fontItem.FontColor = ((SolidColorBrush)rect_font_color.Fill).Color;
            fontItem.FontName = com_font_type.SelectedValue.ToString();
            fontItem.FontSize = (int)num_font_size.Value;
            fontItem.Text = text;
        }
        private void DrawText1()
        {
            try
            {
                if (fontItem == null)
                {
                    this.imgFont.Source = null;
                    return;
                }

                System.Drawing.Font fontText = new System.Drawing.Font(fontItem.FontName, fontItem.FontSize);
                System.Drawing.Size sizeText = System.Windows.Forms.TextRenderer.MeasureText(fontItem.Text, fontText, new System.Drawing.Size(0, 0), System.Windows.Forms.TextFormatFlags.NoPadding);
                Rect viewport = new Rect(0, 0, sizeText.Width, sizeText.Height);

                if ((int)viewport.Width == 0 || (int)viewport.Height == 0)
                    return;

                System.Drawing.Bitmap tempMap = new System.Drawing.Bitmap((int)viewport.Width, (int)viewport.Height);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(tempMap);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                System.Drawing.RectangleF rect = new System.Drawing.RectangleF(0, 0, sizeText.Width, sizeText.Height);
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddString(fontItem.Text, fontText.FontFamily, (int)fontText.Style, fontText.Size, rect, System.Drawing.StringFormat.GenericDefault);


                //描边
                g.DrawPath(new System.Drawing.Pen(new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(fontItem.StrokeColor.A, fontItem.StrokeColor.R, fontItem.StrokeColor.G, fontItem.StrokeColor.B)), fontItem.StrokeColorLength), path);
                //颜色
                g.FillPath(new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(fontItem.FontColor.A, fontItem.FontColor.R, fontItem.FontColor.G, fontItem.FontColor.B)), path);
                //渐变
                g.FillPath(new System.Drawing.Drawing2D.LinearGradientBrush(rect, System.Drawing.Color.FromArgb(fontItem.GradientColor1.A, fontItem.GradientColor1.R, fontItem.GradientColor1.G, fontItem.GradientColor1.B), System.Drawing.Color.FromArgb(fontItem.GradientColor2.A, fontItem.GradientColor2.R, fontItem.GradientColor2.G, fontItem.GradientColor2.B), System.Drawing.Drawing2D.LinearGradientMode.Vertical), path);


                //if (fontItem.OverlayImage != null)
                //{
                //    System.Drawing.TextureBrush brush = new System.Drawing.TextureBrush(ImageHelper.BitmapImageToIamge(fontItem.OverlayImage), System.Drawing.Drawing2D.WrapMode.TileFlipXY);//可改变渐变方式
                //    g.FillPath(brush, path);
                //}
                //else
                {
                    g.FillPath(new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(fontItem.FontColor.A, fontItem.FontColor.R, fontItem.FontColor.G, fontItem.FontColor.B)), path);
                }


                path.Dispose();

                BitmapImage tempImage = ImageHelper.BitmapToBitmapImage(tempMap, System.Drawing.Imaging.ImageFormat.Png);
                g.Dispose();
                tempMap.Dispose();

                if (tempImage != null)
                {
                    this.imgFont.Source = tempImage;
                    this.imgFont.Width = tempImage.Width;
                    this.imgFont.Height = tempImage.Height;
                    Canvas.SetLeft(this.imgFont, (this.mainCanvas.ActualWidth - tempImage.Width) / 2);
                    Canvas.SetTop(this.imgFont, (this.mainCanvas.ActualHeight - tempImage.Height) / 2);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void DrawText()
        {
            try
            {
                if (fontItem == null)
                {
                    this.imgFont.Source = null;
                    return;
                }

                System.Drawing.Font fontText = new System.Drawing.Font(fontItem.FontName, fontItem.FontSize);
                System.Drawing.Size sizeText = System.Windows.Forms.TextRenderer.MeasureText(fontItem.Text, fontText, new System.Drawing.Size(0, 0), System.Windows.Forms.TextFormatFlags.NoPadding);
                Rect viewport = new Rect(0, 0, sizeText.Width, sizeText.Height);

                if ((int)viewport.Width == 0 || (int)viewport.Height == 0)
                    return;

                System.Drawing.Bitmap tempMap = new System.Drawing.Bitmap((int)viewport.Width, (int)viewport.Height);
                System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(tempMap);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                System.Drawing.RectangleF rect = new System.Drawing.RectangleF(0, 0, sizeText.Width, sizeText.Height);
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddString(fontItem.Text, fontText.FontFamily, (int)fontText.Style, fontText.Size, rect, System.Drawing.StringFormat.GenericDefault);

                //描边
                g.DrawPath(new System.Drawing.Pen(new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(fontItem.StrokeColor.A, fontItem.StrokeColor.R, fontItem.StrokeColor.G, fontItem.StrokeColor.B)), fontItem.StrokeColorLength), path);
                //颜色
                g.FillPath(new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(fontItem.FontColor.A, fontItem.FontColor.R, fontItem.FontColor.G, fontItem.FontColor.B)), path);
                //渐变
                g.FillPath(new System.Drawing.Drawing2D.LinearGradientBrush(rect, System.Drawing.Color.FromArgb(fontItem.GradientColor1.A, fontItem.GradientColor1.R, fontItem.GradientColor1.G, fontItem.GradientColor1.B), System.Drawing.Color.FromArgb(fontItem.GradientColor2.A, fontItem.GradientColor2.R, fontItem.GradientColor2.G, fontItem.GradientColor2.B), System.Drawing.Drawing2D.LinearGradientMode.Vertical), path);
                //图片叠加
                if (fontItem.OverlayImage != null)
                {
                    System.Drawing.TextureBrush brush = new System.Drawing.TextureBrush(ImageHelper.BitmapImageToIamge(fontItem.OverlayImage), System.Drawing.Drawing2D.WrapMode.TileFlipXY);//可改变渐变方式
                    g.FillPath(brush, path);
                }

                path.Dispose();

                BitmapImage tempImage = ImageHelper.BitmapToBitmapImage(tempMap, System.Drawing.Imaging.ImageFormat.Png);
                g.Dispose();
                tempMap.Dispose();

                if (tempImage != null)
                {
                    this.imgFont.Source = tempImage;
                    this.imgFont.Width = tempImage.Width;
                    this.imgFont.Height = tempImage.Height;
                    Canvas.SetLeft(this.imgFont, (this.mainCanvas.ActualWidth - tempImage.Width) / 2);
                    Canvas.SetTop(this.imgFont, (this.mainCanvas.ActualHeight - tempImage.Height) / 2);
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        private void initGradientColor()
        {
            rect_stroke_color.MouseLeftButtonDown += Rect_stroke_color_MouseLeftButtonDown;
            num_stroke_length.ValueChanged += Num_stroke_length_ValueChanged;
            rGradientColor1.MouseLeftButtonDown += RGradientColor1_MouseLeftButtonDown;
            rGradientColor2.MouseLeftButtonDown += RGradientColor2_MouseLeftButtonDown;
            btn_addimg.Click += Btn_addimg_Click;
        }

        /// <summary>
        /// 描边颜色
        /// </summary>
        private void Rect_stroke_color_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ColorSelectorDialog csw = new ColorSelectorDialog();
            csw.ShowDialog();
            if (fontItem != null)
            {
                fontItem.StrokeColor = csw.returnSelectColor;
                this.rect_stroke_color.Fill = new SolidColorBrush(csw.returnSelectColor);
                DrawText();
            }
        }

        /// <summary>
        /// 描边深度
        /// </summary>
        private void Num_stroke_length_ValueChanged(object sender, EventArgs e)
        {
            if (fontItem != null)
            {
                fontItem.StrokeColorLength = (int)this.num_stroke_length.Value;
                DrawText();
            }
        }


        /// <summary>
        /// 渐变起始颜色
        /// </summary>
        private void RGradientColor1_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ColorSelectorDialog csw = new ColorSelectorDialog();
            csw.ShowDialog();
            if (fontItem != null)
            {
                fontItem.GradientColor1 = csw.returnSelectColor;
                this.rGradientColor1.Fill = new SolidColorBrush(csw.returnSelectColor);
                DrawText();
            }
        }
        /// <summary>
        /// 渐变结束颜色
        /// </summary>
        private void RGradientColor2_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ColorSelectorDialog csw = new ColorSelectorDialog();
            csw.ShowDialog();
            if (fontItem != null)
            {
                fontItem.GradientColor2 = csw.returnSelectColor;
                this.rGradientColor2.Fill = new SolidColorBrush(csw.returnSelectColor);
                DrawText();
            }
        }

        /// <summary>
        /// 图片叠加
        /// </summary>
        private void Btn_addimg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择图片";
            openFileDialog.Filter = "图片(*.jpg,*.png,*.bmp)|*.jpg;*.png;*.bmp";
            openFileDialog.FileName = string.Empty;
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                BitmapImage bi = ImageHelper.LoadBitmapImageByPath(openFileDialog.FileName);
                if (bi != null)
                {
                    if (fontItem != null)
                    {
                        fontItem.OverlayImage = bi;
                        DrawText();
                    }
                }
            }
        }
    }
}
