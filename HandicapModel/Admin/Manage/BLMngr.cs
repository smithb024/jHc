namespace HandicapModel.Admin.Manage
{
    using System.Collections.Generic;
    using System.IO;

    using CommonHandicapLib;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.Types;

    using CommonLib.Types;

    using Event;
    using GalaSoft.MvvmLight.Messaging;
    using HandicapModel.Admin.IO.ResultsCSV;
    using HandicapModel.Admin.IO.TXT;

    using HandicapModel.Common;
    using HandicapModel.Interfaces;
    using HandicapModel.Interfaces.Admin.IO;
    using HandicapModel.Interfaces.Admin.IO.TXT;
    using HandicapModel.Interfaces.Common;
    using HandicapModel.Interfaces.SeasonModel;
    using HandicapModel.SeasonModel;

    /// <summary>
    /// Class which manages the business layer.
    /// </summary>
    public class BLMngr : IBLMngr
    {
        /// <summary>
        /// The Junior Handicap model.
        /// </summary>
        private readonly IModel model;

        /// <summary>
        /// The instance of the logger.
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// The normalisation configuration manager.
        /// </summary>
        private readonly INormalisationConfigMngr normalisationConfigurationManager;

        /// <summary>
        /// The results configuration manager.
        /// </summary>
        private readonly IResultsConfigMngr resultsConfigurationManager;

        /// <summary>
        /// The series configuration manager.
        /// </summary>
        private readonly ISeriesConfigMngr seriesConfigurationManager;

        /// <summary>
        /// The results calculation manager
        /// </summary>
        private readonly CalculateResultsMngr resultsCalculator;

        /// <summary>
        /// The athlete data wrapper
        /// </summary>
        private readonly IAthleteData athleteData;

        /// <summary>
        /// The club data wrapper.
        /// </summary>
        private readonly IClubData clubData;

        /// <summary>
        /// The event data wrapper.
        /// </summary>
        private readonly IEventData eventData;

        /// <summary>
        /// The summary data wrapper
        /// </summary>
        private readonly ISummaryData summaryData;

        /// <summary>
        /// The season IO manager.
        /// </summary>
        private readonly ISeasonIO seasonIO;

        /// <summary>
        /// The event IO manager.
        /// </summary>
        private readonly IEventIo eventIo;

        /// <summary>
        /// The general IO manager.
        /// </summary>
        private readonly IGeneralIo generalIo;

        /// <summary>
        /// The name of the currently selected season. It is set from the view model.
        /// </summary>
        private string currentSeason;

        /// <summary>
        /// Initialises a new instance of the <see cref="BLMngr"/> class.
        /// </summary>
        /// <param name="model">junior handicap model</param>
        /// <param name="normalisationConfigurationManager">the normalisation config manager</param>
        /// <param name="resultsConfigurationManager">the results config manager</param>
        /// <param name="seriesConfigurationManager">the series config manager</param>
        /// <param name="athleteData">athlete data</param>
        /// <param name="clubData">club data</param>
        /// <param name="eventData">event data</param>
        /// <param name="summaryData">summary data</param>
        /// <param name="seasonIO">season IO manager</param>
        /// <param name="eventIo">event IO manager</param>
        /// <param name="generalIo">general IO manager</param>
        /// <param name="logger">application logger</param>
        public BLMngr(
            IModel model,
            INormalisationConfigMngr normalisationConfigurationManager,
            IResultsConfigMngr resultsConfigurationManager,
            ISeriesConfigMngr seriesConfigurationManager,
            IAthleteData athleteData,
            IClubData clubData,
            IEventData eventData,
            ISummaryData summaryData,
            ISeasonIO seasonIO,
            IEventIo eventIo,
            IGeneralIo generalIo,
            IJHcLogger logger)
        {
            this.logger = logger;
            this.model = model;
            this.normalisationConfigurationManager = normalisationConfigurationManager;
            this.resultsConfigurationManager = resultsConfigurationManager;
            this.seriesConfigurationManager = seriesConfigurationManager;
            this.athleteData = athleteData;
            this.clubData = clubData;
            this.eventData = eventData;
            this.summaryData = summaryData;
            this.seasonIO = seasonIO;
            this.eventIo = eventIo;
            this.generalIo = generalIo;
            this.ModelRootDirectory = RootIO.LoadRootFile();
            this.currentSeason = string.Empty;

            this.resultsCalculator =
                new CalculateResultsMngr(
                    this.model,
                    this.normalisationConfigurationManager,
                    this.resultsConfigurationManager,
                    this.seriesConfigurationManager,
                    this.logger);

            this.IsValid =
                this.generalIo.DataFolderExists && this.generalIo.ConfigurationFolderExists;

            Messenger.Default.Register<LoadNewSeasonMessage>(this, this.NewCurrentSeason);
            Messenger.Default.Register<LoadNewEventMessage>(this, this.NewCurrentEvent);
            Messenger.Default.Register<LoadNewSeriesMessage>(this, this.LoadNewSeries);
            Messenger.Default.Register<CreateNewSeriesMessage>(this, this.CreateNewSeries);
            Messenger.Default.Register<ReinitialiseRoot>(this, this.ReinitialiseRoot);
        }

        /// <summary>
        /// Root directory for the model.
        /// </summary>
        public string ModelRootDirectory { get; private set; }

        /// <summary>
        /// Gets a value inddicating whether the current root location has a valid series.
        /// </summary>
        public bool IsValid { get; private set; }

        /// <summary>
        /// Initialise the model.
        /// </summary>
        public void InitialiseModel()
        {
            this.model.ReinitialiseSeason();
        }

        /// <summary>
        /// Creates a directory for a new season
        /// </summary>
        /// <param name="seasonName">new season</param>
        /// <returns>success flag</returns>
        public bool CreateNewSeason(string seasonName)
        {
            bool success = true;
            this.logger.WriteLog(string.Format("Create new season {0}", seasonName));
            Messenger.Default.Send(
                new HandicapErrorMessage(
                    string.Empty));

            success = this.seasonIO.CreateNewSeason(seasonName);

            if (success)
            {
                this.model.AddNewSeason(seasonName);

                ISummary summary = new Summary();
                List<AthleteSeasonDetails> athletes = new List<AthleteSeasonDetails>();
                List<IClubSeasonDetails> clubs = new List<IClubSeasonDetails>();

                success = this.summaryData.SaveSummaryData(seasonName, summary);

                if (success)
                {
                    success = this.athleteData.SaveAthleteSeasonData(seasonName, athletes);
                }

                if (success)
                {
                    success = this.clubData.SaveClubSeasonData(seasonName, clubs);
                }
            }


            if (success)
            {
                this.logger.WriteLog(string.Format("Finishing creating new season {0}", seasonName));
            }
            else
            {
                this.logger.WriteLog("Failed to Create New Season");
                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        "Season creation failed"));
            }

            return success;
        }

        /// <summary>
        /// Create a new event.
        /// </summary>
        /// <param name="eventName">Event name</param>
        /// <param name="date">date of the event</param>
        /// <returns>success flag</returns>
        public bool CreateNewEvent(string eventName, DateType date)
        {
            bool success = 
                this.eventIo.CreateNewEvent(
                    this.model.CurrentSeason.Name, 
                    eventName);

            if (success)
            {
                this.model.UpdateEvents();


                success = 
                    this.eventData.SaveEventData(
                        this.model.CurrentSeason.Name,
                        eventName,
                        new EventMiscData() { EventDate = date });

                if (success)
                {
                    success = 
                        this.summaryData.SaveSummaryData(
                            this.model.CurrentSeason.Name,
                            eventName,
                            new Summary());
                }
            }

            if (success)
            {
                this.logger.WriteLog(string.Format("Finishing creating new event {0}", eventName));
            }
            else
            {
                this.logger.WriteLog("Failed to Create New Event");
                Messenger.Default.Send(
                    new HandicapErrorMessage(
                        "Event creation failed"));
            }

            return success;
        }

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>LoadCurrentSeason</name>
        /// <date>15/04/15</date>
        /// <summary>
        /// Loads the current season.
        /// </summary>
        /// <remarks>
        /// The current season is stored in a text file to make it persistent from one running instance of the 
        /// application to the next.
        /// </remarks>
        /// <returns>returns the name of the current season</returns>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public string LoadCurrentSeason()
        {
            string currentSeason = this.seasonIO.LoadCurrentSeason();

            return currentSeason;
        }

        /// <summary>
        /// Loads the current season.
        /// </summary>
        /// <remarks>
        /// The current season is stored in a text file to make it persistent from one running instance of the 
        /// application to the next.
        /// </remarks>
        /// <returns>returns the name of the current season</returns>
        public string LoadCurrentEvent()
        {
            string currentEvent =
                this.eventIo.LoadCurrentEvent(
                    this.currentSeason);

            return currentEvent;
        }

        /// <summary>
        /// Caclulate the results for the current event.
        /// </summary>
        public void CalculateResults()
        {
            this.resultsCalculator.CalculateResults();
        }

        /// <summary>
        /// Remove the results for the current event.
        /// </summary>
        public void DeleteResults()
        {
            DeleteResultsMngr mngr =
                new DeleteResultsMngr(
                    this.model,
                    this.normalisationConfigurationManager,
                    this.logger);
            mngr.DeleteResults();
        }

        /// <summary>
        /// Save the root directory for the root of the model directory.
        /// </summary>
        /// <param name="rootDirectory">root directory to save</param>
        public void SaveRootDirectory(string rootDirectory)
        {
            RootIO.SaveRootFile(rootDirectory);

            ReinitialiseRoot message = new ReinitialiseRoot();
            Messenger.Default.Send(message);
        }

        /// <summary>
        /// Set the progress information field in the model.
        /// </summary>
        /// <param name="progress">string to add</param>
        public void SetProgressInformation(string progress)
        {
            Messenger.Default.Send(
                new HandicapProgressMessage(
                    progress));
        }

        /// <summary>
        /// Print a full set of information if the folder exists.
        /// </summary>
        /// <param name="folder">folder to save to</param>
        public void PrintAll(string folder)
        {
            if (Directory.Exists(folder))
            {
                if (!ResultsWriter.WriteResultsTable(this.model, folder, this.logger))
                {
                    return;
                }

                if (!PointsTableWriter.SavePointsTable(this.model, folder, this.logger))
                {
                    return;
                }

                if (!MobTrophyTableWriter.WriteMobTrophyPointsTable(this.model, folder, this.eventData, this.logger))
                {
                    return;
                }

                if (!ClubPointsTeamTrophyTableWriter.Write(this.model, folder, this.eventData, this.logger))
                {
                    return;
                }

                if (!HandicapWriter.WriteHandicapTable(this.model, folder, this.normalisationConfigurationManager, this.logger))
                {
                    return;
                }

                if (!EventSummaryWriter.WriteEventSummaryTable(this.model, folder, this.logger))
                {
                    return;
                }

                if (!NextRunnerWriter.WriteNextRunnerTable(this.model, folder, this.seriesConfigurationManager, this.logger))
                {
                    return;
                }

                SetProgressInformation("Print completed successfully");
            }
        }

        /// <summary>
        /// Saves the selected season to file and make a note of the season name.
        /// </summary>
        /// <param name="season">load season message</param>
        private void NewCurrentSeason(LoadNewSeasonMessage message)
        {
            this.currentSeason = message.Name;
            this.seasonIO.SaveCurrentSeason(message.Name);
        }

        /// <summary>
        /// Save the selected event to file.
        /// </summary>
        /// <param name="message">load event message</param>
        private void NewCurrentEvent(LoadNewEventMessage message)
        {
            this.eventIo.SaveCurrentEvent(
                this.currentSeason,
                message.Name);
        }

        /// <summary>
        /// A new series has loaded, check the local validity.
        /// </summary>
        /// <param name="message">load series message</param>
        private void LoadNewSeries(LoadNewSeriesMessage message)
        {
            this.IsValid =
                this.generalIo.DataFolderExists && this.generalIo.ConfigurationFolderExists;

            ValidLocationMessage locationMessage =
                new ValidLocationMessage(this.IsValid);

            Messenger.Default.Send(locationMessage);

        }

        /// <summary>
        /// Request to create a new series.
        /// </summary>
        /// <param name="message"></param>
        private void CreateNewSeries(CreateNewSeriesMessage message)
        {
            this.generalIo.CreateConfigurationFolder();
            this.generalIo.CreateDataFolder();
            this.resultsConfigurationManager.SaveDefaultResultsConfiguration();
            this.normalisationConfigurationManager.SaveDefaultNormalisationConfiguration();

        }

        /// <summary>
        /// Reinitialise the data path value from the file.
        /// </summary>
        /// <param name="message">reinitialise message</param>
        private void ReinitialiseRoot(ReinitialiseRoot message)
        {
            this.ModelRootDirectory = RootIO.LoadRootFile();
        }
    }
}