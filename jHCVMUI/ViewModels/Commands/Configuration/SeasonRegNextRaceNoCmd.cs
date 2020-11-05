namespace jHCVMUI.ViewModels.Commands.Configuration
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Windows.Input;
  using jHCVMUI.ViewModels.ViewModels;

  public class SeasonRegNextRaceNoCmd : ICommand
  {
    private AthleteRegisterToSeasonViewModel m_viewModel = null;

    /// <summary>
    /// Creates a new instance of the SeasonRegNextRaceNoCmd
    /// </summary>
    /// <param name="viewModel">view model</param>
    public SeasonRegNextRaceNoCmd(AthleteRegisterToSeasonViewModel viewModel)
    {
      m_viewModel = viewModel;
    }

    public bool CanExecute(object parameter)
    {
      return true;
    }

    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public void Execute(object parameter)
    {
      m_viewModel.NextRaceNumber();
    }
  }
}
