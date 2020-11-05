namespace jHCVMUI.Views.Configuration
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for AthleteConfigurationDialog.xaml
    /// </summary>
    public partial class AthleteConfigurationDialog : Window
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="AthleteConfigurationDialog"/> class.
        /// </summary>
        public AthleteConfigurationDialog()
        {
            this.InitializeComponent();
            this.AthleteCollectionList.AddHandler(
              MouseWheelEvent,
              new RoutedEventHandler(this.MouseWheelHandler),
              true);
        }

        /// <summary>
        /// Handle the mouse wheel, when focused on <paramref name="sender"/>/.
        /// </summary>
        /// <param name="sender">sender object</param>
        /// <param name="e">event argements</param>
        private void MouseWheelHandler(
          object sender,
          RoutedEventArgs e)
        {
            MouseWheelEventArgs wheelArgument = (MouseWheelEventArgs)e;

            double x = (double)wheelArgument.Delta;

            double y = this.AthleteScroll.VerticalOffset;

            this.AthleteScroll.ScrollToVerticalOffset(y - x);
        }
    }
}