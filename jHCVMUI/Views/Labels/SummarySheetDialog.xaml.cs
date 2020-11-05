namespace jHCVMUI.Views.Labels
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using System.Windows;
  using System.Windows.Controls;
  using System.Windows.Data;
  using System.Windows.Documents;
  using System.Windows.Input;
  using System.Windows.Media;
  using System.Windows.Media.Imaging;
  using System.Windows.Navigation;
  using System.Windows.Shapes;
  using CommonLib.Images;

  /// <summary>
  /// Interaction logic for SummarySheetDialog.xaml
  /// </summary>
  public partial class SummarySheetDialog : Window
  {
    public SummarySheetDialog()
    {
      InitializeComponent();
    }

    public void SaveMyWindow(int dpi, string filename)
    {
      GUIToImage.SaveGrid(this, this.SummarySheetGrid, dpi, filename);
    }
  }
}