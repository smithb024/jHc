/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:jHandicap"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace jHandicap.ViewModel
{
    using CommonHandicapLib;
    using CommonHandicapLib.Interfaces;
    using HandicapModel;
    using HandicapModel.Admin.IO;
    using HandicapModel.Admin.IO.TXT;
    using HandicapModel.Admin.Manage;
    using HandicapModel.Interfaces;
    using HandicapModel.Interfaces.Admin.IO;
    using HandicapModel.Interfaces.Admin.IO.TXT;
    using jHCVMUI.ViewModels.Primary;
    using jHCVMUI.ViewModels.ViewModels;

    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<IJHcLogger, JHcLogger>();

            SimpleIoc.Default.Register<ICommonIo, CommonIO>();
            SimpleIoc.Default.Register<IEventIo, EventIO>();
            SimpleIoc.Default.Register<IRawEventIo, RawEventIO>();
            SimpleIoc.Default.Register<ISeasonIO, SeasonIO>();
            SimpleIoc.Default.Register<IGeneralIo, GeneralIO>();

            SimpleIoc.Default.Register<IResultsConfigMngr, ResultsConfigMngr>();
            SimpleIoc.Default.Register<IModel, Model>();

            SimpleIoc.Default.Register<IBLMngr, BLMngr>();


            SimpleIoc.Default.Register<PrimaryDisplayViewModel>();
            SimpleIoc.Default.Register<SeasonPaneViewModel>();
            SimpleIoc.Default.Register<EventPaneViewModel>();
            SimpleIoc.Default.Register<DataPaneViewModel>();


            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// Gets the primary display view model.
        /// </summary>
        public PrimaryDisplayViewModel PrimaryDisplayViewModel => ServiceLocator.Current.GetInstance<PrimaryDisplayViewModel>();

        /// <summary>
        /// Gets the season display view model.
        /// </summary>
        public SeasonPaneViewModel SeasonPaneViewModel => ServiceLocator.Current.GetInstance<SeasonPaneViewModel>();

        /// <summary>
        /// Gets the event display view model.
        /// </summary>
        public EventPaneViewModel EventPaneViewModel => ServiceLocator.Current.GetInstance<EventPaneViewModel>();

        /// <summary>
        /// Gets the data display view model.
        /// </summary>
        public DataPaneViewModel DataPaneViewModel => ServiceLocator.Current.GetInstance<DataPaneViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}