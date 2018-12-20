using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Windows.Media;

namespace MarcArmbruster.Wpf.Controls.Input
{
    /// <summary>
    /// Interaction logic for WpfTextInput.xaml
    /// </summary>
    public partial class WpfTextInput : UserControl
    {
        #region Private Fields

        private static Brush defaultDisabledBackgroundBrush = Brushes.LightGray;
        private static Brush activeBackgroundBrush = Brushes.White;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// The Image Orientation Enumeration.
        /// </summary>
        public enum ImageOrientation
        {
            /// <summary>
            /// Image will be docked on left side.
            /// </summary>
            Left,

            /// <summary>
            /// Image will be docked on right side.
            /// </summary>
            Right,

            /// <summary>
            /// Image will be hidden.
            /// </summary>
            None
        };

        #endregion Public Properties

        #region Dependency Properties    

        /// <summary>
        /// The TextWrapping value.
        /// </summary>
        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextWrapping.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(WpfTextInput), new PropertyMetadata(TextWrapping.NoWrap));

        /// <summary>
        /// Gets or sets a value indicating whether text input control is read only.
        /// </summary>        
        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsReadOnly.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsReadOnlyProperty =
            DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(WpfTextInput), new PropertyMetadata(false, OnIsReadOnlyChanged));

        /// <summary>
        /// Gets or sets the watermark background color for ReadOnly/Disabled status.
        /// </summary>      
        public Brush BackgroundOnDisabled
        {
            get { return (Brush)GetValue(BackgroundOnDisabledProperty); }
            set { SetValue(BackgroundOnDisabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for BackgroundOnDisabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BackgroundOnDisabledProperty =
            DependencyProperty.Register("BackgroundOnDisabled", typeof(Brush), typeof(WpfTextInput), new PropertyMetadata(defaultDisabledBackgroundBrush));

        /// <summary>
        /// Gets or sets a value indicating whether text input control is enabled.
        /// </summary>        
        public new bool IsEnabled
        {
            get { return (bool)GetValue(IsEnabledProperty); }
            set { SetValue(IsEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsEnabled.  This enables animation, styling, binding, etc...
        public static new readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.Register("IsEnabled", typeof(bool), typeof(WpfTextInput), new PropertyMetadata(true, OnIsEnabledChanged));

        /// <summary>
        /// The wartermark foreground color
        /// </summary>
        public Brush WatermarkForeground
        {
            get { return (Brush)GetValue(WatermarkForegroundProperty); }
            set { SetValue(WatermarkForegroundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WatermarkForeground.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatermarkForegroundProperty =
            DependencyProperty.Register("WatermarkForeground", typeof(Brush), typeof(WpfTextInput), new PropertyMetadata(Brushes.Black));

        /// <summary>
        /// Gets or sets the watermark image orientation.
        /// </summary>       
        public ImageOrientation WatermarkImageOrientation
        {
            get { return (ImageOrientation)GetValue(WatermarkImageOrientationProperty); }
            set { SetValue(WatermarkImageOrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WatermarkImageOrientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatermarkImageOrientationProperty =
            DependencyProperty.Register("WatermarkImageOrientation", typeof(ImageOrientation), typeof(WpfTextInput), new PropertyMetadata(ImageOrientation.Right, OnOrientationChanged));

        /// <summary>
        /// Gets or sets the watermark image source.
        /// </summary>      
        public ImageSource WatermarkImageSource
        {
            get { return (ImageSource)GetValue(WatermarkImageSourceProperty); }
            set { SetValue(WatermarkImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WatermarkImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatermarkImageSourceProperty =
            DependencyProperty.Register("WatermarkImageSource", typeof(ImageSource), typeof(WpfTextInput), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the watermark text.
        /// </summary>       
        public string WatermarkText
        {
            get { return (string)GetValue(WatermarkTextProperty); }
            set { SetValue(WatermarkTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WatermarkText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WatermarkTextProperty =
            DependencyProperty.Register("WatermarkText", typeof(string), typeof(WpfTextInput), new PropertyMetadata(string.Empty));

        /// <summary>
        /// Gets or sets the input text.
        /// </summary>       
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Input.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(WpfTextInput), new PropertyMetadata(string.Empty, TextChanged));



        public char[] ForbiddenCharacters
        {
            get { return (char[])GetValue(ForbiddenCharactersProperty); }
            set { SetValue(ForbiddenCharactersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ForbiddenCharacters.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ForbiddenCharactersProperty =
            DependencyProperty.Register("ForbiddenCharacters", typeof(char[]), typeof(WpfTextInput), new PropertyMetadata(new char[] { }));

        #endregion Dependency Properties

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="WpfTextInput"/> class.
        /// </summary>
        public WpfTextInput()
        {            
            InitializeComponent();
            TextChanged(this, new DependencyPropertyChangedEventArgs());
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Internal Handler.
        /// </summary>
        /// <param name="d">Dependency Object (WpfTextInput).</param>
        /// <param name="e">Event Arguments.</param>
        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as WpfTextInput;
            if (ctrl == null)
            {
                return;
            }

            if (e.NewValue != null && e.NewValue.ToString() != string.Empty)
            {
                ctrl.txtInput.Opacity = 1;
                ctrl.ClearButton.Visibility = Visibility.Visible;
            }
            else
            {
                ctrl.txtInput.Opacity = 0.6;
                ctrl.ClearButton.Visibility = Visibility.Collapsed;
            }

            ctrl.HideValidationErrorText();
            if (Validation.GetHasError(d))
            {
                string error = Validation.GetErrors(d).FirstOrDefault().ErrorContent?.ToString();
                if (!string.IsNullOrWhiteSpace(error))
                {
                    ctrl.ShowValidationErrorText(error);
                }
            }
        }

        /// <summary>
        /// Called when orientation changed.
        /// </summary>
        /// <param name="d">The dependency object.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as WpfTextInput;
            if (ctrl != null && e.NewValue != null)
            {
                if (e.NewValue.Equals(ImageOrientation.Left))
                {
                    DockPanel.SetDock(ctrl.WatermarkImage, Dock.Left);
                }

                if (e.NewValue.Equals(ImageOrientation.Right))
                {
                    DockPanel.SetDock(ctrl.WatermarkImage, Dock.Right);
                }

                if (e.NewValue.Equals(ImageOrientation.None))
                {
                    ctrl.WatermarkImage.Visibility = Visibility.Collapsed;
                }
            }
        }

        /// <summary>
        /// Called when is enabled changed.
        /// </summary>
        /// <param name="d">The Dependency Object (WpfTextInput).</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as WpfTextInput;
            if (ctrl != null && e.NewValue != null)
            {
                if ((bool)e.NewValue == false)
                {
                    ctrl.WatermarkArea.Visibility = Visibility.Collapsed;
                    ctrl.Background = ctrl.BackgroundOnDisabled;
                }

                if ((bool)e.NewValue == true && !ctrl.IsReadOnly)
                {
                    ctrl.WatermarkArea.Visibility = Visibility.Visible;
                    ctrl.Background = activeBackgroundBrush;
                }
            }
        }

        /// <summary>
        /// Called when is read only changed.
        /// </summary>
        /// <param name="d">The Dependency Object (WpfTextInput).</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as WpfTextInput;
            if (ctrl != null && e.NewValue != null)
            {
                if ((bool)e.NewValue == true)
                {
                    ctrl.WatermarkArea.Visibility = Visibility.Collapsed;
                    ctrl.Background = ctrl.BackgroundOnDisabled;
                }

                if ((bool)e.NewValue == false && ctrl.IsEnabled)
                {
                    ctrl.WatermarkArea.Visibility = Visibility.Visible;
                    ctrl.Background = activeBackgroundBrush;
                }
            }
        }

        private void HideValidationErrorText()
        {
            this.txtError.Visibility = Visibility.Collapsed;
            this.txtError.Text = string.Empty;
        }

        private void ShowValidationErrorText(string error)
        {
            this.txtError.Visibility = Visibility.Visible;
            this.txtError.Text = error;
        }

        /// <summary>
        /// Clear action.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The event arguments.</param>
        private void OnClearButtonClick(object sender, RoutedEventArgs eventArgs)
        {
            this.Text = string.Empty;
        }

        #endregion Methods
    }
}
