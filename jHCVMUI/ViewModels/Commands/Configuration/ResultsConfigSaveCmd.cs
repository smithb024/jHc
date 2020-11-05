namespace jHCVMUI.ViewModels.Commands.Configuration
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using System.Windows.Input;
  using jHCVMUI.ViewModels.Config;

  public class NormalisationConfigSaveCmd : ICommand
  {
    private NormalisationConfigViewModel viewModel = null;

    public NormalisationConfigSaveCmd(NormalisationConfigViewModel newViewModel)
    {
      viewModel = newViewModel;
    }

    public event System.EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter)
    {
      return viewModel.ValidEntries();
    }

    public void Execute(object parameter)
    {
      viewModel.SaveConfig();
    }
  }
}