﻿namespace HandicapModel.Interfaces.SeasonModel
{ 
    using CommonHandicapLib.Types;
    using CommonLib.Types;
    using HandicapModel.Common;
    using System;
    using System.Collections.Generic;
    using HandicapModel.Interfaces.Common;
    using CommonLib.Enumerations;

    /// <summary>
    /// Interface which describes a single season and manages the changes to it.
    /// </summary>
    public interface ISeason
    {
        /// <summary>
        /// Event which is used to inform interested parties that there has been a change to the
        /// list of handicap event names.
        /// </summary>
        event EventHandler HandicapEventsChanged;

        /// <summary>
        /// Event which is used to inform interested parties that there has been a change to the
        /// list of clubs within this season.
        /// </summary>
        event EventHandler ClubsChangedEvent;

        /// <summary>
        /// Event which is used to inform interested parties that there has been a change to the
        /// list of athletes within this season.
        /// </summary>
        event EventHandler AthletesChangedEvent;

        /// <summary>
        /// Event which is used to inform interested parties that there has been a change to the
        /// number of athletes registered this season.
        /// </summary>
        event EventHandler AthleteCollectionChangedEvent;

        /// <summary>
        /// Event which is used to inform interested parties that there has been a change to this
        /// season's summary.
        /// </summary>
        event EventHandler SummaryChangedEvent;

        /// <summary>
        /// Gets the season specific details for each athlete present in the current season.
        /// </summary>
        List<IAthleteSeasonDetails> Athletes { get; }

        /// <summary>
        /// Gets the club specific details for each club present in the current season.
        /// </summary>
        List<IClubSeasonDetails> Clubs { get; }

        /// <summary>
        /// Gets the list of events in the current season.
        /// </summary>
        List<string> Events { get; }

        /// <summary>
        /// Gets the name of the current season.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the summary of the current season.
        /// </summary>
        ISummary Summary { get; }

        /// <summary>
        /// Add an athlete to this season.
        /// </summary>
        /// <param name="key">athlete id</param>
        /// <param name="name">athlete name</param>
        /// <param name="handicap">athlete handicap</param>
        /// <param name="firstTimer">indicates whether the athlete is a first timer</param>
        void AddNewAthlete(int key, string name, string handicap, bool firstTimer);

        /// <summary>
        /// Add points to the named club.
        /// </summary>
        /// <param name="clubName">name of the club</param>
        /// <param name="newEvent">full details of the event</param>
        void AddNewMobTrophyPoints(
            string clubName,
            ICommonPoints newEvent);

        /// <summary>
        /// Add Team Trophy points to the named club.
        /// </summary>
        /// <param name="clubName">name of the club</param>
        /// <param name="newEvent">full details of the event</param>
        void AddNewClubPoints(
            string clubName,
            ITeamTrophyEvent newEvent);

        /// <summary>
        /// Add points to the indicated athlete
        /// </summary>
        /// <param name="key">athlete id</param>
        /// <param name="points">athlete points</param>
        void AddNewPoints(int key, CommonPoints points);

        /// <summary>
        /// Add a time to the named athlete.
        /// </summary>
        /// <param name="key">athlete id</param>
        /// <param name="newTime">time and date</param>
        void AddNewTime(int key, Appearances newTime);

        /// <summary>
        /// Create a new event
        /// </summary>
        /// <param name="eventName">event name</param>
        /// <returns>success flag</returns>
        bool CreateNewEvent(string eventName);

        /// <summary>
        /// Gets the number of appearances for the indicated athlete
        /// </summary>
        /// <param name="key">athlete id</param>
        /// <returns>number of appearances</returns>
        int GetAthleteAppearancesCount(int key);

        /// <summary>
        /// Gets the handicap for the indicated athlete.
        /// </summary>
        /// <param name="key">athlete id</param>
        /// <param name="hcConfiguration">type of handicap to retrieve</param>
        /// <returns>athlete's handicap</returns>
        RaceTimeType GetAthleteHandicap(int key, NormalisationConfigType hcConfiguration);

        /// <summary>
        /// Gets the season best for the indicated athlete.
        /// </summary>
        /// <param name="key">athlete id</param>
        /// <returns>season best time</returns>
        TimeType GetSB(int key);

        /// <summary>
        /// Save the current model to a file.
        /// </summary>
        /// <returns>success flag</returns>
        bool SaveCurrentSeason();

        /// <summary>
        /// A new event has been added, refresh the events list.
        /// </summary>
        void NewEventCreated();

        /// <summary>
        /// Update position points for the indicated athlete.
        /// </summary>
        /// <param name="key">athlete key</param>
        /// <param name="date">date of the update</param>
        /// <param name="points">new points</param>
        void UpdatePositionPoints(int key, DateType date, int points);

        /// <summary>
        /// Within the summary, increate the number of <paramref name="type"/> by one.
        /// </summary>
        /// <param name="type">
        /// The type of property to change.
        /// </param>
        void IncrementSummary(SummaryPropertiesType type);

        /// <summary>
        /// Set the fastest times in the summary.
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
    }
}