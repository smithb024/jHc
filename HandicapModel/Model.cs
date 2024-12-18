namespace HandicapModel
{
    using System;
    using System.Collections.Generic;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using CommonLib.Enumerations;
    using CommonLib.Types;
    using HandicapModel.Admin.Manage;
    using HandicapModel.AthletesModel;
    using HandicapModel.ClubsModel;
    using HandicapModel.Interfaces;
    using HandicapModel.Interfaces.Admin.IO;
    using HandicapModel.Interfaces.Admin.IO.TXT;
    using HandicapModel.Interfaces.Admin.IO.XML;
    using HandicapModel.Interfaces.Common;
    using HandicapModel.Interfaces.SeasonModel;
    using HandicapModel.Interfaces.SeasonModel.EventModel;
    using HandicapModel.SeasonModel;
    using HandicapModel.SeasonModel.EventModel;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

    /// <summary>
    /// Junior Handicap model.
    /// </summary>
    public class Model : IModel
    {
        /// <summary>
        /// Instance of the normalisation configuration manager.
        /// </summary>
        private readonly INormalisationConfigMngr normalisationConfigurationManager;

        /// <summary>
        /// Instance of the results configuration manager.
        /// </summary>
        private readonly IResultsConfigMngr resultsConfigurationManager;

        /// <summary>
        /// The athlete data wrapper
        /// </summary>
        private readonly IAthleteData athleteData;

        /// <summary>
        /// The club data wrapper.
        /// </summary>
        private readonly IClubData clubData;

        /// <summary>
        /// The summary data wrapper
        /// </summary>
        private readonly ISummaryData summaryData;

        /// <summary>
        /// The summary data reader.
        /// </summary>
        private readonly ISummaryDataReader summaryDataReader;

        /// <summary>
        /// The event IO manager.
        /// </summary>
        private readonly IEventIo eventIo;

        /// <summary>
        /// The season IO manager.
        /// </summary>
        private readonly ISeasonIO seasonIo;

        /// <summary>
        /// The program logger;
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// Prevents a new instance of the HandicapModel class from being created.
        /// </summary>
        /// <param name="normalisationConfigMngr">Normalisation configuration manager</param>
        /// <param name="resultsConfigurationManager">Results configuration manager</param>
        /// <param name="athleteData">athlete data</param>
        /// <param name="clubData">club data</param>
        /// <param name="eventData">event data</param>
        /// <param name="summaryData">summary data</param>
        /// <param name="resultsTableReader">results table reader</param>
        /// <param name="seasonIo">season IO Manager</param>
        /// <param name="eventIo">event IO manager</param>
        /// <param name="rawEventIo">raw event IO manager</param>
        /// <param name="logger">application logger</param>
        public Model(
            INormalisationConfigMngr normalisationConfigMngr,
            IResultsConfigMngr resultsConfigurationManager,
            IAthleteData athleteData,
            IClubData clubData,
            IEventData eventData,
            ISummaryData summaryData,
            IResultsTableReader resultsTableReader,
            ISeasonIO seasonIo,
            IEventIo eventIo,
            IRawEventIo rawEventIo,
            IJHcLogger logger)
        {
            this.normalisationConfigurationManager = normalisationConfigMngr;
            this.resultsConfigurationManager = resultsConfigurationManager;
            this.athleteData = athleteData;
            this.clubData = clubData;
            this.summaryData = summaryData;
            this.eventIo = eventIo;
            this.seasonIo = seasonIo;
            this.logger = logger;

            // Setup local models.
            this.CurrentSeason =
                new Season(
                    resultsConfigurationManager,
                    this.athleteData,
                    this.clubData,
                    this.summaryData,
                    this.eventIo,
                    this.logger);
            this.CurrentEvent =
                new EventHC(
                    eventData,
                    this.summaryData,
                    resultsTableReader,
                    rawEventIo,
                    this.logger);
            try
            {
                this.Seasons = seasonIo.GetSeasons();
                this.Athletes = this.athleteData.ReadAthleteData();
                this.Clubs = this.clubData.LoadClubData();
                this.GlobalSummary = this.summaryData.LoadSummaryData();
            }
            catch (Exception ex)
            {
                this.logger.WriteLog(
                    $"Error setting up the model: {ex}");
            }

            CommonMessenger.Default.Register<LoadNewSeriesMessage>(this, this.LoadNewSeries);
        }

        /// <summary>
        /// Event which is used to inform interested parties that there has been a change to the
        /// list of season names.
        /// </summary>
        public event EventHandler SeasonsChangedEvent;

        /// <summary>
        /// Gets the current season.
        /// </summary>
        public ISeason CurrentSeason { get; private set; }

        /// <summary>
        /// Gets the current event.
        /// </summary>
        public IHandicapEvent CurrentEvent { get; private set; }

        /// <summary>
        /// Gets the current season.
        /// </summary>
        public List<string> Seasons { get; private set; }

        /// <summary>
        /// Gets the athletes model.
        /// </summary>
        public Athletes Athletes { get; private set;  }

        /// <summary>
        /// Gets the clubs model.
        /// </summary>
        public Clubs Clubs { get; private set; }

        /// <summary>
        /// Gets the system wide summary
        /// </summary>
        public ISummary GlobalSummary { get; private set; }

        /// <summary>
        /// Add a new season to the model.
        /// </summary>
        public void AddNewSeason(string newSeason)
        {
            this.Seasons.Add(newSeason);
            this.SeasonsChangedEvent?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Gets the events present in this season.
        /// </summary>
        public void UpdateEvents()
        {
            this.CurrentSeason.NewEventCreated();
        }

        /// <summary>
        /// Returns the club list data.
        /// </summary>
        /// <returns>club list</returns>
        public List<string> GetClubList()
        {
            return this.Clubs.ClubDetails;
        }

        /// <summary>
        /// Saves the club list.
        /// </summary>
        public void SaveClubList()
        {
            this.clubData.SaveClubData(this.Clubs);
        }

        /// <summary>
        /// Create a new athlete and add it to the athlete list.
        /// </summary>
        /// <param name="clubName">new name</param>
        public void CreateNewClub(string clubName)
        {
            this.Clubs.AddNewClub(clubName);
        }

        /// <summary>
        ///   Deletes an athlete from the list.
        /// </summary>
        /// <param name="clubName">club name</param>
        public void DeleteClub(string clubName)
        {
            this.Clubs.RemoveClub(clubName);
        }

        /// <summary>
        /// Saves the current athlete model.
        /// </summary>
        public void SaveAthleteList()
        {
            this.athleteData.SaveAthleteData(this.Athletes);
        }

        /// <summary>
        /// Create a new athlete and add it to the athlete list.
        /// </summary>
        /// <param name="athleteName">name of the athlete</param>
        /// <param name="club">athlete's club</param>
        /// <param name="initialHandicap">initial handicap</param>
        /// <param name="sex">athlete's sex</param>
        /// <param name="runningNumbers">collection of registed running numbers</param>
        /// <param name="birthYear">birth year, not used</param>
        /// <param name="birthMonth">birth month, not used</param>
        /// <param name="birthDay">birth day, not used</param>
        /// <param name="signedConsent">
        /// Indicates whether the parental consent form has been signed.
        /// </param>
        /// <param name="active">Indicates whether the athlete is currently active</param>
        public void CreateNewAthlete(
          string athleteName,
          string club,
          int initialHandicap,
          SexType sex,
          List<string> runningNumbers,
          string birthYear,
          string birthMonth,
          string birthDay,
          bool signedConsent,
          bool active)
        {
            Athletes.SetNewAthlete(
              new AthleteDetails(
                Athletes.NextKey,
                athleteName,
                club,
                new TimeType(initialHandicap, 0),
                sex,
                birthYear,
                birthMonth,
                birthDay,
                signedConsent,
                active,
                this.normalisationConfigurationManager)
              {
                  RunningNumbers = runningNumbers
              });
        }

        /// <summary>
        /// Create a new athlete and add it to the athlete list.
        /// </summary>
        /// <param name="athleteName">name of the athlete</param>
        /// <param name="club">athlete's club</param>
        /// <param name="initialHandicapMinutes">initial handicap (minutes)</param>
        /// <param name="initialHandicapSeconds">initial handicap (seconds)</param>
        /// <param name="sex">athlete's sex</param>
        /// <param name="runningNumbers">collection of registed running numbers</param>
        /// <param name="birthYear">birth year, not used</param>
        /// <param name="birthMonth">birth month, not used</param>
        /// <param name="birthDay">birth day, not used</param>
        /// <param name="signedConsent">
        /// Indicates whether the parental consent form has been signed.
        /// </param>
        /// <param name="active">Indicates whether the athlete is currently active</param>
        public void CreateNewAthlete(
          string athleteName,
          string club,
          int initialHandicapMinutes,
          int initialHandicapSeconds,
          SexType sex,
          List<string> runningNumbers,
          string birthYear,
          string birthMonth,
          string birthDay,
          bool signedConsent,
          bool active)
        {
            Athletes.SetNewAthlete(
              new AthleteDetails(
                Athletes.NextKey,
                athleteName,
                club,
                new TimeType(initialHandicapMinutes, initialHandicapSeconds),
                sex,
                birthYear,
                birthMonth,
                birthDay,
                signedConsent,
                active,
                this.normalisationConfigurationManager)
              {
                  RunningNumbers = runningNumbers
              });
        }

        /// <summary>
        ///   Deletes an athlete from the list.
        /// </summary>
        /// <param name="athleteKey">athlete's unique identifier</param>
        public void DeleteAthlete(int athleteKey)
        {
            this.Athletes.DeleteAthlete(athleteKey);
        }

        /// <summary>
        /// Changes an athlete's club.
        /// </summary>
        /// <param name="athleteKey">athlete's unique identifier</param>
        /// <param name="newClub">new club</param>
        /// <param name="runningNumbers">running number collection</param>
        /// <param name="signedConsent">
        /// indicates whether a signed consent form has been received.
        /// </param>
        /// <param name="active">indicates if the athlete is active or not</param>
        /// <param name="predeclaredHandicap">the predeclared handicap</param>
        public void UpdateAthlete(
          int athleteKey,
          string newClub,
          List<string> runningNumbers,
          bool signedConsent,
          bool active,
          TimeType predeclaredHandicap)
        {
            this.Athletes.ChangeAthlete(
              athleteKey,
              newClub,
              runningNumbers,
              signedConsent,
              active,
              predeclaredHandicap);
        }

        /// <summary>
        /// Reinitialise the season.
        /// </summary>
        public void ReinitialiseSeason()
        {
            this.CurrentSeason =
                new Season(
                    resultsConfigurationManager,
                    this.athleteData,
                    this.clubData,
                    this.summaryData,
                    this.eventIo,
                    this.logger);
        }

        /// <summary>
        /// Save the global summary data.
        /// </summary>
        public void SaveGlobalSummary()
        {
            this.summaryData.SaveSummaryData(GlobalSummary);
        }

        /// <summary>
        /// Increate the number of <paramref name="type"/> by one across all the summaries.
        /// </summary>
        /// <param name="type">
        /// The type of property to change.
        /// </param>
        public void IncrementSummaries(SummaryPropertiesType type)
        {
            this.CurrentEvent.IncrementSummary(type);
            this.CurrentSeason.IncrementSummary(type);
            this.GlobalSummary.Increment(type);
        }

        /// <summary>
        /// Save all tables.
        /// </summary>
        public void SaveAll()
        {
            this.CurrentEvent.SaveResultsTable();

            this.CurrentEvent.SaveEventSummary();
            this.CurrentSeason.SaveCurrentSeason();
            this.SaveGlobalSummary();
            this.SaveClubList();
            this.SaveAthleteList();
        }

        /// <summary>
        /// A load new series command has been initiated. Reload the models from the files.
        /// </summary>
        /// <param name="message"></param>
        private void LoadNewSeries(LoadNewSeriesMessage message)
        {
            this.Seasons = this.seasonIo.GetSeasons();
            this.Athletes = this.athleteData.ReadAthleteData();
            this.Clubs = this.clubData.LoadClubData();
            this.GlobalSummary = this.summaryData.LoadSummaryData();
       }
    }
}