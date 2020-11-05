namespace jHandicap
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
  using System.Windows.Shapes;
  using HandicapModel.Admin.Manage;
  using jHCVMUI.ViewModels.ViewModels;

  /// <summary>
  /// Interaction logic for PrimaryDisplay.xaml
  /// </summary>
  public partial class PrimaryDisplay : Window
  {
    public PrimaryDisplay()
    {
      InitializeComponent();

      //IResultsConfigMngr resultsConfigurationManager =
      //  new ResultsConfigMngr();

      //IBLMngr businessLayerManager =
      //  new BLMngr(
      //    resultsConfigurationManager);

      //PrimaryDisplayViewModel displayContext =
      //  new PrimaryDisplayViewModel(
      //    businessLayerManager,
      //    resultsConfigurationManager);

      //this.DataContext =
      //  displayContext;
    }
  }
}
