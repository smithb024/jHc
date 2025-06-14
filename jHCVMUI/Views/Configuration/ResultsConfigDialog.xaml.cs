namespace jHCVMUI.Views.Configuration
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for ResultsConfigDialog.xaml
    /// </summary>
    public partial class ResultsConfigDialog : Window
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ResultsConfigDialog"/> class.
        /// </summary>
        public ResultsConfigDialog()
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
