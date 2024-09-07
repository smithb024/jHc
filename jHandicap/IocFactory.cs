namespace jHandicap
{
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib;
    using CommunityToolkit.Mvvm.DependencyInjection;
    using jHandicap.ViewModel;
    using Microsoft.Extensions.DependencyInjection;
    using HandicapModel.Admin.IO.TXT;
    using HandicapModel.Interfaces.Admin.IO.TXT;
    using HandicapModel.Admin.IO;
    using HandicapModel.Interfaces.Admin.IO;
    using CommonHandicapLib.Interfaces.XML;
    using CommonHandicapLib.XML;
    using HandicapModel.Admin.IO.XML;
    using HandicapModel.Interfaces.Admin.IO.XML;
    using jHCVMUI.ViewModels.Primary;
    using jHCVMUI.ViewModels.ViewModels;
    using HandicapModel.Admin.Manage;
    using HandicapModel.Interfaces;
    using HandicapModel;

    /// <summary>
    /// Factory class, used to set up dependency injection
    /// </summary>
    public class IocFactory
    {
        /// <summary>
        /// Setup IOC.
        /// </summary>
        public static void Setup()
        {
            Ioc.Default.ConfigureServices(
                new ServiceCollection()
                .AddSingleton<IJHcLogger, JHcLogger>()
                .AddSingleton<ICommonIo, CommonIO>()
                .AddSingleton<IEventIo, EventIO>()
                .AddSingleton<IRawEventIo, RawEventIO>()
                .AddSingleton<ISeasonIO, SeasonIO>()
                .AddSingleton<IGeneralIo, GeneralIO>()
                .AddSingleton<INormalisationConfigReader, NormalisationConfigReader>()
                .AddSingleton<IResultsConfigReader, ResultsConfigReader>()
                .AddSingleton<ISeriesConfigReader, SeriesConfigReader>()
                .AddSingleton<IAthleteData, AthleteData>()
                .AddSingleton<IClubData, ClubData>()
                .AddSingleton<IEventData, EventData>()
                .AddSingleton<ISummaryData, SummaryData>()
                .AddSingleton<IResultsTableReader, ResultsTableReader>()
                .AddSingleton<INormalisationConfigMngr, NormalisationConfigMngr>()
                .AddSingleton<IResultsConfigMngr, ResultsConfigMngr>()
                .AddSingleton<ISeriesConfigMngr, SeriesConfigMngr>()
                .AddSingleton<IModel, Model>()
                .AddSingleton<IBLMngr, BLMngr>()
                .AddSingleton<PrimaryDisplayViewModel>()
                .AddSingleton<EventPaneViewModel>()
                .AddSingleton<SeasonPaneViewModel>()
                .AddSingleton<DataPaneViewModel>()
                .AddSingleton<IMainViewModel, MainViewModel>()
                .BuildServiceProvider());
        }
    }
}
