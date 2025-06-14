namespace HandicapModel.Interfaces
{
    using System;
    using System.Collections.Generic;

    using CommonLib.Enumerations;
    using CommonLib.Types;

    using HandicapModel.AthletesModel;
    using HandicapModel.ClubsModel;
    using HandicapModel.Interfaces.Common;
    using HandicapModel.Interfaces.SeasonModel;
    using HandicapModel.Interfaces.SeasonModel.EventModel;

    /// <summary>
    /// Interface describing the global model.
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// Event which is used to inform interested parties that there has been a change to the
        /// list of season names.
        /// </summary>
        event EventHandler SeasonsChangedEvent;

        /// <summary>
        /// Gets the current season.
        /// </summary>
        ISeason CurrentSeason { get; }

        /// <summary>
        /// Gets the current event.
        /// </summary>
        IHandicapEvent CurrentEvent { get; }

        /// <summary>
        /// Gets the current season.
        /// </summary>
        List<string> Seasons { get; }

        /// <summary>
        /// Gets the athletes
        /// </summary>
        Athletes Athletes { get; }

        /// <summary>
        /// Gets the clubs.
        /// </summary>
        Clubs Clubs { get; }

        /// <summary>
        /// Gets the system wide summary
        /// </summary>
        ISummary GlobalSummary { get; }

        /// <summary>
        /// Adds a new season name to the model.
        /// </summary>
        void AddNewSeason(string newSeasons);

        /// <summary>
        /// Gets the events present in this season.
        /// </summary>
        void UpdateEvents();

        /// <summary>
        /// Returns the club list data.
        /// </summary>
        /// <returns>club list</returns>
        List<string> GetClubList();

        /// <summary>
        /// Saves the club list.
        /// </summary>
        void SaveClubList();

        /// <summary>
        /// Create a new athlete and add it to the athlete list.
        /// </summary>
        /// <param name="clubName">new name</param>
        void CreateNewClub(string clubName);

        /// <summary>
        ///   Deletes an athlete from the list.
        /// </summary>
        /// <param name="clubName">club name</param>
        void DeleteClub(string clubName);

        /// <summary>
        /// Saves the athlete list.
        /// </summary>
        void SaveAthleteList();

        /// <summary>
        /// Create a new athlete and add it to the athlete list.
        /// </summary>
        /// <param name="forename">forename of the athlete</param>
        /// <param name="familyName">family name of the athlete</param>
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
        void CreateNewAthlete(
          string forename,
          string familyName,
          string club,
          int initialHandicap,
          SexType sex,
          List<string> runningNumbers,
          string birthYear,
          string birthMonth,
          string birthDay,
          bool signedConsent,
          bool active);

        /// <summary>
        /// Create a new athlete and add it to the athlete list.
        /// </summary>
        /// <param name="forename">forename of the athlete</param>
        /// <param name="familyName">family name of the athlete</param>
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
        void CreateNewAthlete(
          string forename,
          string familyName,
          string club,
          int initialHandicapMinutes,
          int initialHandicapSeconds,
          SexType sex,
          List<string> runningNumbers,
          string birthYear,
          string birthMonth,
          string birthDay,
          bool signedConsent,
          bool active);

        /// <summary>
        ///   Deletes an athlete from the list.
        /// </summary>
        /// <param name="athleteKey">athlete's unique identifier</param>
        void DeleteAthlete(int athleteKey);

        /// <summary>
        /// Changes an athlete's details.
        /// </summary>
        /// <param name="athleteKey">athlete's unique identifier</param>
        /// <param name="newClub">new club</param>
        /// <param name="runningNumbers">running number collection</param>
        /// <param name="signedConsent">
        /// indicates whether a signed consent form has been received.
        /// </param>
        /// <param name="active">indicates if the athlete is active or not</param>
        /// <param name="predeclaredHandicap">the predeclared handicap</param>
        void UpdateAthlete(
          int athleteKey,
          string newClub,
          List<string> runningNumbers,
          bool signedConsent,
          bool active,
          TimeType predeclaredHandicap);

        /// <summary>
        /// Changes an athlete's predeclared handicap.
        /// </summary>
        /// <param name="athleteKey">athlete's unique identifier</param>
        /// <param name="predeclaredHandicap">the predeclared handicap</param>
        void UpdateAthlete(
          int athleteKey,
          TimeType predeclaredHandicap);

        /// <summary>
        /// Reinitialise the season.
        /// </summary>
        void ReinitialiseSeason();

        /// <summary>
        /// Save the global summary data.
        /// </summary>
        void SaveGlobalSummary();

        /// <summary>
        /// Increate the number of <paramref name="type"/> by one across all the summaries.
        /// </summary>
        /// <param name="type">
        /// The type of property to change.
        /// </param>
        void IncrementSummaries(SummaryPropertiesType type);

        /// <summary>
        /// Attempt to set the fastest times across all summaries.
        /// </summary>
        /// <param name="sex">athlete sex</param>
        /// <param name="key">athlete key</param>
        /// <param name="name">athlete name</param>
        /// <param name="time">athlete time</param>
        /// <param name="date">date the time was set</param>
        void SetFastest(
            SexType sex,
            int key,
            string name,
            TimeType time,
            DateType date);

        /// <summary>
        /// Save all the tables.
        /// </summary>
        void SaveAll();
    }
}