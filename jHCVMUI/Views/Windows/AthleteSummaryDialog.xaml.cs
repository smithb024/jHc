namespace jHCVMUI.Views.Windows
{
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for AthleteSummary.xaml
    /// </summary>
    public partial class AthleteSummaryDialog : Window
    {
        public AthleteSummaryDialog()
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