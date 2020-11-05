namespace jHCVMUI.ViewModels.Commands.Configuration
{
  using System;
  using System.Windows.Input;

  using jHCVMUI.ViewModels.ViewModels;

  public class AthleteConfigAddNumberCmd : ICommand
  {
    private AthleteConfigurationViewModel viewModel = null;

    public AthleteConfigAddNumberCmd(AthleteConfigurationViewModel viewModel)
    {
      this.viewModel = viewModel;
    }

    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public bool CanExecute(object parameter)
    {
      return this.viewModel.CanChange() && this.viewModel.NewNumberValid();
    }

    public void Execute(object parameter)
    {
      this.viewModel.AddNewNumber();
    }
  }
}