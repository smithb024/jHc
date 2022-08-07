namespace HandicapModel.SeasonModel
{
    using System;
    using System.Collections.Generic;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.Types;
    using CommonLib.Types;
    using GalaSoft.MvvmLight.Messaging;
    using HandicapModel.Admin.Manage;
    using HandicapModel.Common;
    using HandicapModel.Interfaces.Admin.IO;
    using HandicapModel.Interfaces.Admin.IO.TXT;
    using HandicapModel.Interfaces.Common;
    using HandicapModel.Interfaces.SeasonModel;

    /// <summary>
    /// Model which holds all information pertaining to the current season.
    /// </summary>
    public class Season : ISeason
    {
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
        /// The program logger;
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// Event specific IO manager.
        /// </summary>
        private readonly IEventIo eventIo;

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
        private List<IAthleteSeasonDetails> athletes;

        /// <summary>
        /// A list of club specific details for all clubs present in the current season.
        /// </summary>
        private List<IClubSeasonDetails> clubs = new List<IClubSeasonDetails>();

        /// <summary>
        /// Results configuration manager.
        /// </summary>
        private IResultsConfigMngr resultsConfigurationManager;

        /// <summary>
        /// Initialises a new instance of the <see cref="Season"/> class
        /// </summary>
        /// <param name="resultsConfigurationManager">
        ///  The results configuration manager
        /// </param>
        /// <param name="athleteData">athlete data</param>
        /// <param name="clubData">club data</param>
        /// <param name="summaryData">summary data</param>
        /// <param name="eventIo">event IO manager</param>
        /// <param name="logger">application logger</param>
        public Season(
            IResultsConfigMngr resultsConfigurationManager,
            IAthleteData athleteData,
            IClubData clubData,
            ISummaryData summaryData,
            IEventIo eventIo,
            IJHcLogger logger)
        {
            this.resultsConfigurationManager = resultsConfigurationManager;
            this.athleteData = athleteData;
            this.clubData = clubData;
            this.summaryData = summaryData;
            this.eventIo = eventIo;
            this.logger = logger;

            this.athletes = new List<IAthleteSeasonDetails>();
            this.clubs = new List<IClubSeasonDetails>();
            this.summary = new Summary();
            this.events = new List<string>();

            Messenger.Default.Register<LoadNewSeasonMessage>(this, this.LoadNewSeason);
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
        public List<IAthleteSeasonDetails> Athletes
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
        /// <param name="message">load a new season message</param>
        private void LoadNewSeason(LoadNewSeasonMessage message)
        {
            bool success = true;

            this.Name = message.Name;

            try
            {
                if (string.IsNullOrEmpty(this.Name))
                {
                    this.Summary = new Summary();
                    this.Athletes = new List<IAthleteSeasonDetails>();
                    this.Clubs = new List<IClubSeasonDetails>();
                    this.Events = new List<string>();
                }
                else
                {
                    this.Summary =
                        this.summaryData.LoadSummaryData(
                            this.Name);
                    this.Athletes =
                      this.athleteData.LoadAthleteSeasonData(
                        this.Name,
                        this.resultsConfigurationManager);
                    this.Clubs = this.clubData.LoadClubSeasonData(this.Name);
                    this.Events =
                        this.eventIo.GetEvents(
                            this.Name);
                }
            }
            catch (Exception ex)
            {
                this.logger.WriteLog($"Season, Failed to load Season {ex}");
                success = false;
            }

            if (success)
            {
                Messenger.Default.Send(
                new HandicapProgressMessage(
                    $"New Season Loaded - {this.Name}"));

                this.logger.WriteLog($"Season, loaded {this.Name}");
            }
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
                this.summaryData.SaveSummaryData(Name, Summary);
                this.athleteData.SaveAthleteSeasonData(Name, Athletes);
                this.clubData.SaveClubSeasonData(Name, Clubs);
            }
            catch (Exception ex)
            {
                this.logger.WriteLog($"Season, Failed to save Season {ex}");
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
            return this.eventIo.CreateNewEvent(Name, eventName);
        }

        /// <summary>
        /// A new event has been added, refresh the events list.
        /// </summary>
        public void NewEventCreated()
        {
            this.Events = this.eventIo.GetEvents(Name);
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

            this.athleteData.SaveAthleteSeasonData(Name, Athletes);

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
            IAthleteSeasonDetails athlete = Athletes.Find(a => a.Key == key);

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
            IAthleteSeasonDetails athlete = Athletes.Find(a => a.Key == key);

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
            IAthleteSeasonDetails athlete = Athletes.Find(a => a.Key == key);

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
        public void AddNewMobTrophyPoints(
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
        /// <param name="points">Team Trophy points to add</param>
        public void AddNewClubPoints(
            string clubName,
            ITeamTrophyEvent points)
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