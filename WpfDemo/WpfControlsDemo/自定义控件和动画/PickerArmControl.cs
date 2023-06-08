using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfControlsDemo
{
    public class PickerArmControl : Control
    {
        public double ArmAngle
        {
            get { return (double)GetValue(ArmAngleProperty); }
            set
            {
                SetValue(ArmAngleProperty, value);
            }
        }
        public static readonly DependencyProperty ArmAngleProperty = DependencyProperty.Register("ArmAngle", typeof(double), typeof(PickerArmControl),
            new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnArmAnglePropertyChanged));
        private static void OnArmAnglePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PickerArmControl control = d as PickerArmControl;
            if (e.NewValue != null)
            {
                double armAngle = (double)e.NewValue;
                if (control._rotateTransform_arm != null)
                {
                    control._rotateTransform_arm.Angle = armAngle;
                    control._rotateTransform_txt.Angle = -1 * armAngle;
                }
            }
        }


        public string CassetteId
        {
            get { return (string)GetValue(CassetteIdProperty); }
            set { SetValue(CassetteIdProperty, value); }
        }
        public string WaferId
        {
            get { return (string)GetValue(WaferIdProperty); }
            set { SetValue(WaferIdProperty, value); }
        }
        public bool IsHasWafer
        {
            get { return (bool)GetValue(IsHasWaferProperty); }
            set { SetValue(IsHasWaferProperty, value); }
        }

        public static readonly DependencyProperty CassetteIdProperty = DependencyProperty.Register("CassetteId", typeof(string), typeof(PickerArmControl),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));
        public static readonly DependencyProperty WaferIdProperty = DependencyProperty.Register("WaferId", typeof(string), typeof(PickerArmControl),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));
        public static readonly DependencyProperty IsHasWaferProperty = DependencyProperty.Register("IsHasWafer", typeof(bool), typeof(PickerArmControl),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));

        private const string ElementRotateTransform_Arm = "PART_ROTATETRANSFORM";
        private const string ElementRotateTransform_Txt = "TEXT_ROTATETRANSFORM";
        RotateTransform _rotateTransform_arm = null;
        RotateTransform _rotateTransform_txt = null;
        static PickerArmControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PickerArmControl), new FrameworkPropertyMetadata(typeof(PickerArmControl)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _rotateTransform_arm = GetTemplateChild(ElementRotateTransform_Arm) as RotateTransform;
            _rotateTransform_txt = GetTemplateChild(ElementRotateTransform_Txt) as RotateTransform;
        }
    }
}
