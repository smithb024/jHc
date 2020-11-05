namespace jHCVMUI.ViewModels.Commands.Configuration
{
  using System;
  using System.Windows.Input;
  using jHCVMUI.ViewModels.Config;

  public class SeriesConfigSaveCmd : ICommand
  {
    private SeriesConfigViewModel viewModel = null;

    public SeriesConfigSaveCmd(SeriesConfigViewModel newViewModel)
    {
      viewModel = newViewModel;
    }

    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter)
    {
      return viewModel.CanSaveConfig();
    }

    public void Execute(object parameter)
    {
      viewModel.SaveConfig();
    }
  }
}