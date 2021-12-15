namespace HandicapModel
{
    using System;
    using System.Collections.Generic;
    using CommonLib.Enumerations;
    using CommonLib.Types;
    using HandicapModel.Admin.IO;
    using HandicapModel.Admin.IO.TXT;
    using HandicapModel.Admin.Manage;
    using HandicapModel.AthletesModel;
    using HandicapModel.ClubsModel;
    using HandicapModel.Interfaces;
    using HandicapModel.Interfaces.Common;
    using HandicapModel.Interfaces.SeasonModel;
    using HandicapModel.Interfaces.SeasonModel.EventModel;
    using HandicapModel.SeasonModel;
    using HandicapModel.SeasonModel.EventModel;

    /// <summary>
    /// Junior Handicap model.
    /// </summary>
    public class Model : IModel
    {
        /// <summary>
        /// Instance of the results configuration manager.
        /// </summary>
        private IResultsConfigMngr resultsConfigurationManager;

        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>HandicapModel</name>
        /// <date>25/03/15</date>
        /// <summary>
        /// Prevents a new instance of the HandicapModel class from being created.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
        public Model(
            IResultsConfigMngr resultsConfigurationManager,
            ISeasonIO seasonIO)
        {
            this.resultsConfigurationManager = resultsConfigurationManager;

            // Check for global files and create fresh if don't exist.
            GeneralIO.CreateDataFolder();
            GeneralIO.CreateConfigurationFolder();
            this.resultsConfigurationManager.SaveDefaultResultsConfiguration();
            NormalisationConfigMngr.SaveDefaultNormalisationConfiguration();

            // Setup local models.
            this.CurrentSeason =
                new Season(
                    resultsConfigurationManager);
            this.CurrentEvent = new EventHC();
            this.Seasons = seasonIO.GetSeasons();
            this.Athletes = AthleteData.ReadAthleteData();
            this.Clubs = ClubData.LoadClubData();
            this.GlobalSummary = SummaryData.LoadSummaryData();
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
        public List<string> Seasons { get; }

        /// <summary>
        /// Gets the athletes model.
        /// </summary>
        public Athletes Athletes { get; }

        /// <summary>
        /// Gets the clubs model.
        /// </summary>
        public Clubs Clubs { get; }

        /// <summary>
        /// Gets the system wide summary
        /// </summary>
        public ISummary GlobalSummary { get; }

        /// <summary>
        /// Load the season in to memory.
        /// </summary>
        /// <param name="seasonName">season to load</param>
        /// <returns>success flag</returns>
        public bool LoadNewSeason(string seasonName)
        {
            return this.CurrentSeason.LoadNewSeason(seasonName);
        }

        /// <summary>
        /// Load the event in to memory.
        /// </summary>
        /// <param name="eventName">event to load</param>
        /// <returns>success flag</returns>
        public bool LoadNewEvent(string eventName)
        {
            bool success;

            success =
                this.CurrentEvent.LoadNewEvent(
                    this.CurrentSeason.Name,
                    eventName);

            EventIO.SaveCurrentEvent(
                this.CurrentSeason.Name,
                eventName);

            return success;
            //return this.CurrentSeason.LoadNewEvent(eventName);
        }

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
            ClubData.SaveClubData(this.Clubs);
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
            AthleteData.SaveAthleteData(this.Athletes);
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
                active)
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
                active)
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
        /// Loads the current event.
        /// </summary>
        /// <returns>returns the name of the current event</returns>
        public string LoadCurrentEvent()
        {
            return EventIO.LoadCurrentEvent(CurrentSeason.Name);
        }

        /// <summary>
        /// Reinitialise the season.
        /// </summary>
        public void ReinitialiseSeason()
        {
            this.CurrentSeason =
                new Season(
                    resultsConfigurationManager);
        }

        /// <summary>
        /// Save the global summary data.
        /// </summary>
        public void SaveGlobalSummary()
        {
            SummaryData.SaveSummaryData(GlobalSummary);
        }
    }
}