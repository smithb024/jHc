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
        /// The results configuration manager.
        /// </summary>
        private readonly IResultsConfigMngr resultsConfigurationManager;

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
        /// Initialises a new instance of the <see cref="BLMngr"/> class.
        /// </summary>
        /// <param name="model">junior handicap model</param>
        /// <param name="resultsConfigurationManager"></param>
        /// <param name="athleteData">athlete data</param>
        /// <param name="clubData">club data</param>
        /// <param name="eventData">event data</param>
        /// <param name="summaryData">summary data</param>
        /// <param name="seasonIO">season IO manager</param>
        /// <param name="eventIo">event IO manager</param>
        /// <param name="logger">application logger</param>
        public BLMngr(
            IModel model,
            IResultsConfigMngr resultsConfigurationManager,
            IAthleteData athleteData,
            IClubData clubData,
            IEventData eventData,
            ISummaryData summaryData,
            ISeasonIO seasonIO,
            IEventIo eventIo,
            IJHcLogger logger)
        {
            this.logger = logger;
            this.model = model;
            this.resultsConfigurationManager = resultsConfigurationManager;
            this.athleteData = athleteData;
            this.clubData = clubData;
            this.eventData = eventData;
            this.summaryData = summaryData;
            this.seasonIO = seasonIO;
            this.eventIo = eventIo;
            this.ModelRootDirectory = RootIO.LoadRootFile();

            this.resultsCalculator =
                new CalculateResultsMngr(
                    this.model,
                    this.resultsConfigurationManager);
        }

        /// <summary>
        /// Root directory for the model.
        /// </summary>
        public string ModelRootDirectory { get; }

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

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>LoadNewSeason</name>
        /// <date>21/03/15</date>
        /// <summary>
        /// Loads a new season into memory.
        /// </summary>
        /// <param name="seasonName">season to load</param>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public void LoadNewSeason(string seasonName)
        {
            this.model.LoadNewSeason(seasonName);
            this.SaveCurrentSeason(seasonName);
            Messenger.Default.Send(
                new HandicapProgressMessage(
                    $"New Season Loaded - {seasonName}"));
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

        /// <summary>
        /// Load a new event
        /// </summary>
        /// <param name="eventName">event name</param>
        public void LoadNewEvent(string eventName)
        {
            this.model.LoadNewEvent(eventName);

            Messenger.Default.Send(
                new HandicapProgressMessage(
                    $"New Event Loaded - {eventName}:{model.CurrentSeason.Name}"));
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
        public string LoadCurrentEvent()
        {
            return this.model.LoadCurrentEvent();
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
                    this.model);
            mngr.DeleteResults();
        }

        /// <summary>
        /// Save the root directory for the root of the model directory.
        /// </summary>
        /// <param name="rootDirectory">root directory to save</param>
        public void SaveRootDirectory(string rootDirectory)
        {
            RootIO.SaveRootFile(rootDirectory);
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
                if (!ResultsWriter.WriteResultsTable(this.model, folder))
                {
                    return;
                }

                if (!PointsTableWriter.SavePointsTable(this.model, folder))
                {
                    return;
                }

                if (!ClubPointsTableWriter.WriteClubPointsTable(this.model, folder))
                {
                    return;
                }

                if (!ClubPointsHarmonyTableWriter.Write(this.model, folder))
                {
                    return;
                }

                if (!HandicapWriter.WriteHandicapTable(this.model, folder))
                {
                    return;
                }

                if (!EventSummaryWriter.WriteEventSummaryTable(this.model, folder))
                {
                    return;
                }

                if (!NextRunnerWriter.WriteNextRunnerTable(this.model, folder))
                {
                    return;
                }

                SetProgressInformation("Print completed successfully");
            }
        }

        /// <summary>
        /// Saves the current season.
        /// </summary>
        /// <param name="season">current season</param>
        /// <returns>success flag</returns>
        private bool SaveCurrentSeason(string season)
        {
            return this.seasonIO.SaveCurrentSeason(season);
        }
    }
}