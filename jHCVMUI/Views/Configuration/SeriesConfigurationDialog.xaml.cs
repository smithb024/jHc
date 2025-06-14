namespace jHCVMUI.Views.Configuration
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for SeriesConfigurationDialog.xaml
    /// </summary>
    public partial class SeriesConfigurationDialog : Window
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SeriesConfigurationDialog"/> class.
        /// </summary>
        public SeriesConfigurationDialog()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Ok is clicked on the dialog.
        /// </summary>
        /// <param name="sender">Sender object</param>
        /// <param name="e">Event arguments</param>
        private void OkClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
