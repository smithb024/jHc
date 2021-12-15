namespace HandicapModel.SeasonModel
{
    using System;
    using System.Collections.Generic;
    using CommonHandicapLib;
    using CommonHandicapLib.Types;
    using CommonLib.Types;
    using HandicapModel.Admin.IO;
    using HandicapModel.Admin.IO.TXT;
    using HandicapModel.Admin.Manage;
    using HandicapModel.Common;
    using HandicapModel.Interfaces.Common;
    using HandicapModel.Interfaces.SeasonModel;

    /// <summary>
    /// Model which holds all information pertaining to the current season.
    /// </summary>
    public class Season : ISeason
    {
        /// <summary>
        /// List of all in the current season.
        /// </summary>
        private List<string> events;

        /// <summary>
        /// Summary of the current season.
        /// </summary>
        private ISummary summary;

        /// <summary>
        /// A list of season specific details for all athletes present in the current season.
        /// </summary>
        private List<AthleteSeasonDetails> athletes;

        /// <summary>
        /// A list of club specific details for all clubs present in the current season.
        /// </summary>
        private List<IClubSeasonDetails> clubs = new List<IClubSeasonDetails>();

        /// <summary>
        /// Results configuration manager.
        /// </summary>
        private IResultsConfigMngr resultsConfigurationManager;

        /// <summary>
        /// Creates a new instance of the HandicapSeason class
        /// </summary>
        public Season(IResultsConfigMngr resultsConfigurationManager)
        {
            this.resultsConfigurationManager = resultsConfigurationManager;

            this.athletes = new List<AthleteSeasonDetails>();
            this.clubs = new List<IClubSeasonDetails>();
            this.summary = new Summary();
            this.events = new List<string>();
        }

        /// <summary>
        /// Event which is used to inform interested parties that there has been a change to the
        /// list of handicap event names.
        /// </summary>
        public event EventHandler HandicapEventsChanged;

        /// <summary>
        /// Event which is used to inform interested parties that there has been a change to the
        /// list of clubs within this season.
        /// </summary>
        public event EventHandler ClubsChangedEvent;

        /// <summary>
        /// Event which is used to inform interested parties that there has been a change to the
        /// list of athletes within this season.
        /// </summary>
        public event EventHandler AthletesChangedEvent;

        /// <summary>
        /// Event which is used to inform interested parties that there has been a change to this
        /// season's summary.
        /// </summary>
        public event EventHandler SummaryChangedEvent;

        /// <summary>
        /// Gets the name of this season.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the athletes lists.
        /// </summary>
        public List<AthleteSeasonDetails> Athletes
        {
            get
            {
                return this.athletes;
            }

            private set
            {
                if (this.athletes != value)
                {
                    this.athletes = value;
                    this.AthletesChangedEvent?.Invoke(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Gets the club details.
        /// </summary>
        public List<IClubSeasonDetails> Clubs
        {
            get
            {
                return this.clubs;
            }

            private set
            {
                if (this.clubs != value)
                {
                    this.clubs = value;
                    this.ClubsChangedEvent?.Invoke(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Summary for the current season.
        /// </summary>
        public ISummary Summary
        {
            get
            {
                return this.summary;
            }

            private set
            {
                if (this.summary != value)
                {
                    this.summary = value;
                    this.SummaryChangedEvent?.Invoke(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Gets the current season.
        /// </summary>
        public List<string> Events
        {
            get
            {
                return this.events;
            }
            private set
            {
                if (this.events != value)
                {
                    this.events = value;
                    this.HandicapEventsChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Load a new season into the model.
        /// </summary>
        /// <param name="seasonName">season to load</param>
        /// <returns>success flag</returns>
        public bool LoadNewSeason(string seasonName)
        {
            bool success = true;

            this.Name = seasonName;

            try
            {
                this.Summary =
                    SummaryData.LoadSummaryData(
                        seasonName);
                this.Athletes =
                  AthleteData.LoadAthleteSeasonData(
                    seasonName,
                    this.resultsConfigurationManager);
                this.Clubs = ClubData.LoadClubSeasonData(seasonName);
                this.Events =
                    EventIO.GetEvents(
                        seasonName);
            }
            catch (Exception ex)
            {
                JHcLogger.GetInstance().WriteLog($"Season, Failed to load Season {ex}");
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Save the current model to a file.
        /// </summary>
        /// <returns>success flag</returns>
        public bool SaveCurrentSeason()
        {
            bool success = true;

            try
            {
                SummaryData.SaveSummaryData(Name, Summary);
                AthleteData.SaveAthleteSeasonData(Name, Athletes);
                ClubData.SaveClubSeasonData(Name, Clubs);
            }
            catch (Exception ex)
            {
                JHcLogger.GetInstance().WriteLog($"Season, Failed to save Season {ex}");
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Create a new event
        /// </summary>
        /// <param name="eventName">event name</param>
        /// <returns>success flag</returns>
        public bool CreateNewEvent(string eventName)
        {
            return EventIO.CreateNewEvent(Name, eventName);
        }

        /// <summary>
        /// A new event has been added, refresh the events list.
        /// </summary>
        public void NewEventCreated()
        {
            this.Events = EventIO.GetEvents(Name);
        }

        /// <summary>
        /// Create and add a new athlete to the athletes list. Save the list.
        /// </summary>
        /// <param name="key">athlete key</param>
        /// <param name="name">athlete name</param>
        /// <param name="runningNumbers">athlete running numbers</param>
        /// <param name="handicap">athlete handicap</param>
        /// <param name="firstTimer">first timer flag</param>
        public void AddNewAthlete(
          int key,
          string name,
          string handicap,
          bool firstTimer)
        {
            AthleteSeasonDetails newAthlete =
              new AthleteSeasonDetails(
                key,
                name,
                this.resultsConfigurationManager);

            Athletes.Add(newAthlete);

            AthleteData.SaveAthleteSeasonData(Name, Athletes);

            this.AthletesChangedEvent?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Get the current handicap for the athlete. The handicap is the rounded one.
        /// </summary>
        /// <param name="key">unique key</param>
        /// <returns>rounded handicap</returns>
        public RaceTimeType GetAthleteHandicap(int key, NormalisationConfigType hcConfiguration)
        {
            if (Athletes != null && Athletes.Count > 0)
            {
                if (Athletes.Exists(athlete => athlete.Key == key))
                {
                    return Athletes.Find(athlete => athlete.Key == key).GetRoundedHandicap(hcConfiguration);
                }
            }

            return null;
        }

        /// <summary>
        /// Get the number of appearances by the athlete in the current season
        /// </summary>
        /// <param name="key">unique key</param>
        /// <returns>number of appearances</returns>
        public int GetAthleteAppearancesCount(int key)
        {
            return Athletes.Find(athlete => athlete.Key == key)?.NumberOfAppearances ?? 0;
        }

        /// <summary>
        /// Get the current handicap season best
        /// </summary>
        /// <param name="key">unique key</param>
        /// <returns>season best time</returns>
        public TimeType GetSB(int key)
        {
            return Athletes.Find(athlete => athlete.Key == key)?.SB ?? new TimeType(0, 0);
        }

        /// <summary>
        /// Add a new time to the indicated (key) athlete.
        /// </summary>
        /// <param name="key">unique key</param>
        /// <param name="newTime">new time to add</param>
        public void AddNewTime(int key, Appearances newTime)
        {
            AthleteSeasonDetails athlete = Athletes.Find(a => a.Key == key);

            if (athlete == null)
            {
                return;
            }

            athlete.AddNewTime(newTime);
        }

        /// <summary>
        /// Update the points earnt for position for the indicated athlete on the indicated date.
        /// </summary>
        /// <param name="key">unique key</param>
        /// <param name="date">date of the event</param>
        /// <param name="points">earned points</param>
        public void UpdatePositionPoints(int key, DateType date, int points)
        {
            AthleteSeasonDetails athlete = Athletes.Find(a => a.Key == key);

            if (athlete == null)
            {
                return;
            }

            athlete.UpdatePositionPoints(date, points);
        }

        /// <summary>
        /// Add a new time to the indicated (key) athlete.
        /// </summary>
        /// <param name="key">unique key</param>
        /// <param name="points">new points to add</param>
        public void AddNewPoints(
            int key,
            CommonPoints points)
        {
            AthleteSeasonDetails athlete = Athletes.Find(a => a.Key == key);

            if (athlete == null)
            {
                return;
            }

            athlete.AddNewPoints(points);
            this.AthletesChangedEvent?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Add points to the named club
        /// </summary>
        /// <param name="clubName">name of club</param>
        /// <param name="points">points to add</param>
        public void AddNewClubPoints(
            string clubName,
            ICommonPoints points)
        {
            IClubSeasonDetails clubDetails = Clubs.Find(club => club.Name.CompareTo(clubName) == 0);

            if (clubDetails != null)
            {
                clubDetails.AddNewEvent(points);
            }
            else
            {
                ClubSeasonDetails newClubDetails = new ClubSeasonDetails(clubName);
                newClubDetails.AddNewEvent(points);

                Clubs.Add(newClubDetails);
            }

            this.ClubsChangedEvent?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Add points to the named club
        /// </summary>
        /// <param name="clubName">name of club</param>
        /// <param name="points">harmony points to add</param>
        public void AddNewClubPoints(
            string clubName,
            IHarmonyEvent points)
        {
            IClubSeasonDetails clubDetails = Clubs.Find(club => club.Name.CompareTo(clubName) == 0);

            if (clubDetails != null)
            {
                clubDetails.AddNewEvent(points);
            }
            else
            {
                ClubSeasonDetails newClubDetails = new ClubSeasonDetails(clubName);
                newClubDetails.AddNewEvent(points);

                Clubs.Add(newClubDetails);
            }

            this.ClubsChangedEvent?.Invoke(this, new EventArgs());
        }
    }
}