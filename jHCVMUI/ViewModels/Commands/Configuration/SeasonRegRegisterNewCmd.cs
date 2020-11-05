namespace jHCVMUI.ViewModels.Commands.Configuration
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Windows.Input;
  using jHCVMUI.ViewModels.ViewModels;

  public class SeasonRegRegisterNewCmd : ICommand
  {
    private AthleteRegisterToSeasonViewModel m_viewModel = null;

    /// <summary>
    /// Creates a new instance of the SeasonRegRegisterNewCmd
    /// </summary>
    /// <param name="viewModel">view model</param>
    public SeasonRegRegisterNewCmd(AthleteRegisterToSeasonViewModel viewModel)
    {
      m_viewModel = viewModel;
    }

    public bool CanExecute(object parameter)
    {
      return m_viewModel.CanRegisterAthleteForSeason();
    }

    public event EventHandler CanExecuteChanged
    {
      add { CommandManager.RequerySuggested += value; }
      remove { CommandManager.RequerySuggested -= value; }
    }

    public void Execute(object parameter)
    {
      m_viewModel.RegisterAthleteForSeason();
    }
  }
}
