using System.Windows;
using System.Windows.Controls;

namespace WpfControlsDemo
{
    public enum PasteTableStatus
    {
        Idle,
        Error,
        LoadIng,
        AdjustIng,
        PasteIng,
        PasteOk
    }
    public class PasteTableControl : Control
    {
        public string CassetteTitle
        {
            get { return (string)GetValue(CassetteTitleProperty); }
            set { SetValue(CassetteTitleProperty, value); }
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
        public bool HasWafer
        {
            get { return (bool)GetValue(HasWaferProperty); }
            set { SetValue(HasWaferProperty, value); }
        }
        public bool HasRing
        {
            get { return (bool)GetValue(HasRingProperty); }
            set { SetValue(HasRingProperty, value); }
        }
        public bool HasPaste
        {
            get { return (bool)GetValue(HasPasteProperty); }
            set { SetValue(HasPasteProperty, value); }
        }
        public PasteTableStatus Status
        {
            get { return (PasteTableStatus)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        public static readonly DependencyProperty CassetteTitleProperty = DependencyProperty.Register("CassetteTitle", typeof(string), typeof(PasteTableControl),
          new FrameworkPropertyMetadata("载台", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));
        public static readonly DependencyProperty CassetteIdProperty = DependencyProperty.Register("CassetteId", typeof(string), typeof(PasteTableControl),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));
        public static readonly DependencyProperty WaferIdProperty = DependencyProperty.Register("WaferId", typeof(string), typeof(PasteTableControl),
            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));
        public static readonly DependencyProperty HasWaferProperty = DependencyProperty.Register("HasWafer", typeof(bool), typeof(PasteTableControl),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));
        public static readonly DependencyProperty HasRingProperty = DependencyProperty.Register("HasRing", typeof(bool), typeof(PasteTableControl),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));
        public static readonly DependencyProperty HasPasteProperty = DependencyProperty.Register("HasPaste", typeof(bool), typeof(PasteTableControl),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));
        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(PasteTableStatus), typeof(PasteTableControl),
            new FrameworkPropertyMetadata(PasteTableStatus.Idle, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null));
    }
}
