namespace jHCVMUI.Views.Configuration
{
  using System.Windows;

  /// <summary>
  /// Interaction logic for SeriesConfigurationDialog.xaml
  /// </summary>
  public partial class SeriesConfigurationDialog : Window
  {
    public SeriesConfigurationDialog()
    {
      this.InitializeComponent();
    }

    private void OkClick(object sender, RoutedEventArgs e)
    {
      this.DialogResult = true;
    }
  }
}
