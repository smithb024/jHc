namespace jHCVMUI.Views.Configuration
{
    using System.Windows;
 
    /// <summary>
    /// Interaction logic for NormalisationConfigDialog.xaml
    /// </summary>
    public partial class NormalisationConfigDialog : Window
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="NormalisationConfigDialog"/> class.
        /// </summary>
        public NormalisationConfigDialog()
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
