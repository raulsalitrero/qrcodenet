using Gma.QrCodeNet.Encoding.Windows.Render;
using System.ComponentModel;


#if NETFX_CORE
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
#else
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
#endif

#if NETFX_CORE
namespace Gma.QrCodeNet.Encoding.Windows.WinRT
{
#else
namespace Gma.QrCodeNet.Encoding.Windows.Silverlight
{
#endif
    public class QrCodeGeoControl : Control
    {

        private QrCode m_QrCode = new QrCode();
        private int m_width = 21;

        #region QuietZoneModules
        public static readonly DependencyProperty QuietZoneModuleProperty =
            DependencyProperty.Register("QuietZoneModule", typeof(QuietZoneModules), typeof(QrCodeGeoControl), new PropertyMetadata(QuietZoneModules.Two));
        [EditorBrowsable(EditorBrowsableState.Always)]
        public QuietZoneModules QuietZoneModule
        {
            get { return (QuietZoneModules)GetValue(QuietZoneModuleProperty); }
            set
            {
                if (QuietZoneModule != value)
                {
                    this.SetQuietZoneModules(value);
                    this.UpdatePadding();
                }
            }
        }

        public void SetQuietZoneModules(QuietZoneModules quietZoneModules)
        {
            SetValue(QuietZoneModuleProperty, quietZoneModules);
        }
        #endregion

        #region LightBrush
        public static readonly DependencyProperty LightBrushProperty =
            DependencyProperty.Register("LightBrush", typeof(Brush), typeof(QrCodeGeoControl), new PropertyMetadata(new SolidColorBrush(Colors.White)));
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Brush LightBrush
        {
            get { return (Brush)GetValue(LightBrushProperty); }
            set
            { SetValue(LightBrushProperty, value); }
        }

        public void SetLightColor(Brush lightColor)
        { SetValue(LightBrushProperty, lightColor); }
        #endregion

        #region DarkBrush
        public static readonly DependencyProperty DarkBrushProperty =
            DependencyProperty.Register("DarkBrush", typeof(Brush), typeof(QrCodeGeoControl), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
        [EditorBrowsable(EditorBrowsableState.Always)]
        public Brush DarkBrush
        {
            get { return (Brush)GetValue(DarkBrushProperty); }
            set
            { SetValue(DarkBrushProperty, value); }
        }

        public void SetDarkColor(Brush darkColor)
        { SetValue(DarkBrushProperty, darkColor); }
        #endregion

        #region ErrorCorrectionLevel
        public static readonly DependencyProperty ErrorCorrectLevelProperty =
            DependencyProperty.Register("ErrorCorrectLevel", typeof(ErrorCorrectionLevel), typeof(QrCodeGeoControl), new PropertyMetadata(ErrorCorrectionLevel.M));
        [EditorBrowsable(EditorBrowsableState.Always)]
        public ErrorCorrectionLevel ErrorCorrectLevel
        {
            get { return (ErrorCorrectionLevel)GetValue(ErrorCorrectLevelProperty); }
            set
            {
                if (ErrorCorrectLevel != value)
                {
                    this.SetErrorCorrectLevel(value);
                    this.UpdatePath();
                    this.UpdatePadding();
                }
            }
        }

        public void SetErrorCorrectLevel(ErrorCorrectionLevel errorLevel)
        { SetValue(ErrorCorrectLevelProperty, errorLevel); }
        #endregion

        #region Text
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(QrCodeGeoControl), new PropertyMetadata(""));
        [EditorBrowsable(EditorBrowsableState.Always)]
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set
            {
                if (Text != value)
                {
                    this.SetText(value);
                    this.UpdatePath();
                    this.UpdatePadding();
                }
            }
        }

        public void SetText(string text)
        { SetValue(TextProperty, text); }
        #endregion

        public static DependencyProperty PathGeometryProperty =
            DependencyProperty.Register("PathGeometry", typeof(Geometry), typeof(QrCodeGeoControl), null);

        public Geometry PathGeometry
        {
            get { return (Geometry)GetValue(PathGeometryProperty); }
            private set { SetValue(PathGeometryProperty, value); }
        }

        public void UpdatePath()
        {
            new QrEncoder(ErrorCorrectLevel).TryEncode(Text, out m_QrCode);
            m_width = m_QrCode.Matrix == null ? 21 : m_QrCode.Matrix.Width;
            PathGeometry = GeometryRenderer.DarkModuleGeometry(m_QrCode.Matrix);
        }

        public void UpdatePadding()
        {
            double length = ActualWidth < ActualHeight ? ActualWidth : ActualHeight;
            double moduleSize = length / (m_width + 2 * (int)QuietZoneModule);
            this.Padding = new Thickness(moduleSize * (int)QuietZoneModule);
        }

        public QrCodeGeoControl()
            : base()
        {
            DefaultStyleKey = typeof(QrCodeGeoControl);
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;

            this.UpdatePath();
            this.UpdatePadding();
        }

#if NETFX_CORE
        protected override global::Windows.Foundation.Size ArrangeOverride(global::Windows.Foundation.Size finalSize)
        {
#else
        protected override Size ArrangeOverride(Size finalSize)
        {
#endif
            double width = finalSize.Width < finalSize.Height ? finalSize.Width : finalSize.Height;
            double moduleSize = width / (m_width + 4);
            this.Padding = new Thickness(2 * moduleSize);
            return base.ArrangeOverride(finalSize);
        }

    }
}
