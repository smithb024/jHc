namespace HandicapModel.Admin.Manage.Event
{
    using System.Collections.Generic;
    using CommonHandicapLib;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Types;
    using CommonLib.Enumerations;
    using CommonLib.Types;
    using HandicapModel.Admin.Types;
    using HandicapModel.Common;
    using HandicapModel.Interfaces;
    using HandicapModel.Interfaces.Common;
    using HandicapModel.Interfaces.SeasonModel;
    using HandicapModel.Interfaces.SeasonModel.EventModel;
    using HandicapModel.SeasonModel;
    using HandicapModel.SeasonModel.EventModel;

    /// <summary>
    /// Manager class for calculating results.
    /// </summary>
    public class CalculateResultsMngr : EventResultsMngr
    {
        /// <summary>
        /// Value used to indicate a no score in the harmony competition.
        /// </summary>
        private const int HarmonyNoScore = -1;

        /// <summary>
        /// Results Input/Output
        /// </summary>
        private readonly IResultsConfigMngr resultsConfiguration;

        /// <summary>
        /// Manager which contains all handicap configuration details.
        /// </summary>
        private readonly NormalisationConfigType hcConfiguration;

        /// <summary>
        /// Manager which contains all series configuration details.
        /// </summary>
        private readonly SeriesConfigType seriesConfiguration;

        /// <summary>
        /// Application logger
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="CalculateResultsMngr"/> class.
        /// </summary>
        /// <param name="model">junior handicap model</param>
        /// <param name="normalisationConfigurationManager">
        /// normalisation configuration manager
        /// </param>
        /// <param name="resultsConfigurationManager">
        /// results configuration manager
        /// </param>
        /// <param name="seriesConfigurationManager">
        /// series configuration manager
        /// </param>
        /// <param name="logger">application logger</param>
        public CalculateResultsMngr(
            IModel model,
            INormalisationConfigMngr normalisationConfigurationManager,
            IResultsConfigMngr resultsConfigurationManager,
            ISeriesConfigMngr seriesConfigurationManager,
            IJHcLogger logger)
            : base(model)
        {
            this.logger = logger;
            this.resultsConfiguration = resultsConfigurationManager;
            this.hcConfiguration = normalisationConfigurationManager.ReadNormalisationConfiguration();
            this.seriesConfiguration = seriesConfigurationManager.ReadSeriesConfiguration();
        }

        /// <summary>
        /// Calculate the results for the loaded event.
        /// </summary>
        public void CalculateResults()
        {
            this.logger.WriteLog("Calculate results");

            if (this.resultsConfiguration == null)
            {
                this.logger.WriteLog("Error reading the results config file. Results not generated");
                return;
            }

            List<IRaw> rawResults = this.Model.CurrentEvent.LoadRawResults();

            // Set up the array to work out the points table
            List<ClubPoints> clubPoints = this.SetupClubPoints();

            // ensure that each athlete is registered for season.
            this.RegisterAllAthletesForTheCurrentSeason(rawResults);

            // Analyse results
            EventResults resultsTable = 
                this.GenerateResultsTable(
                    rawResults);

            // Sort by running time to work out the speed order.
            resultsTable.ApplySpeedOrder();

            // Sort by time: Add position points, note first boy, first girl, second and third.
            this.AddPositionPoints(
              resultsTable,
              this.Model.CurrentEvent.Date,
              clubPoints);
            resultsTable.OrderByFinishingTime();
            this.AddPlacings(resultsTable);
            this.AssignClubPoints(
              resultsTable,
              this.Model.CurrentEvent.Date,
              clubPoints);
            this.CalculateTeamHarmonyPoints(
                resultsTable,
                this.Model.CurrentEvent.Date);

            this.Model.CurrentEvent.SetResultsTable(resultsTable);

            this.SaveAll();

            this.logger.WriteLog("Calculate results completed.");
        }

        /// <summary>
        /// Create a <see cref="ClubPoints"/> object for each registered club and then return them.
        /// </summary>
        /// <returns>Collection of <see cref="ClubPoints"/> objects, one per club</returns>
        private List<ClubPoints> SetupClubPoints()
        {
            List<ClubPoints> clubPoints = new List<ClubPoints>();
            foreach (string club in this.Model.Clubs.ClubDetails)
            {
                clubPoints.Add(
                  new ClubPoints(
                    club,
                    this.resultsConfiguration.ResultsConfigurationDetails.NumberInTeam,
                    this.resultsConfiguration.ResultsConfigurationDetails.TeamFinishingPoints,
                    this.resultsConfiguration.ResultsConfigurationDetails.TeamSeasonBestPoints));
            }

            return clubPoints;
        }

        /// <summary>
        /// Loop through all athletes in the raw results. Check to see if they have been registered
        /// for the current season, register them if they have not.
        /// </summary>
        /// <param name="rawResults">raw results.</param>
        private void RegisterAllAthletesForTheCurrentSeason(List<IRaw> rawResults)
        {
            foreach (IRaw raw in rawResults)
            {
                int? athleteKey = this.Model.Athletes.GetAthleteKey(raw.RaceNumber);

                if (athleteKey != null)
                {
                    int athleteKeyInt = (int)athleteKey;

                    if (!this.Model.CurrentSeason.Athletes.Exists(a => a.Key == athleteKeyInt))
                    {
                        // TODO, last two arguments don't seem to be used.
                        this.Model.CurrentSeason.AddNewAthlete(
                          athleteKeyInt,
                          this.Model.Athletes.GetAthleteName(athleteKeyInt),
                          string.Empty,
                          this.Model.Athletes.IsFirstTimer(athleteKeyInt));
                    }
                }
            }

        }

        /// <summary>
        /// Generate the results table from the raw results and return it.
        /// </summary>
        /// <param name="rawResults">raw results</param>
        /// <returns>event results table</returns>
        private EventResults GenerateResultsTable(
            List<IRaw> rawResults)
        {
            EventResults resultsTable = new EventResults();
            DateType eventDate = this.Model.CurrentEvent.Date;

            foreach (Raw raw in rawResults)
            {
                CommonPoints pointsEarned = new CommonPoints(eventDate);

                // Get athlete key.
                int key = this.Model.Athletes.GetAthleteKey(raw.RaceNumber) ?? 0;

                // Note the current handicap.
                RaceTimeType athleteHandicap =
                  this.GetAthleteHandicap(
                    key);

                // Loop through all the entries in the raw results.
                ResultsTableEntry singleResult =
                  new ResultsTableEntry(
                    key,
                    this.Model.Athletes.GetAthleteName(key),
                    raw.TotalTime,
                    raw.Order,
                    athleteHandicap,
                    this.Model.Athletes.GetAthleteClub(key),
                    this.Model.Athletes.GetAthleteSex(key),
                    raw.RaceNumber,
                    this.Model.CurrentEvent.Date,
                    this.Model.Athletes.GetAthleteAge(key),
                    resultsTable.Entries.Count + 1,
                    999999);

                if (!raw.TotalTime.DNF && !raw.TotalTime.Unknown)
                {
                    if (this.Model.Athletes.IsFirstTimer(key))
                    {
                        singleResult.FirstTimer = true;
                    }

                    pointsEarned.FinishingPoints = this.resultsConfiguration.ResultsConfigurationDetails.FinishingPoints;

                    // Work out the season best information
                    if (this.Model.CurrentSeason.GetSB(key) > singleResult.RunningTime)
                    {
                        // Can only count as season best if one time has been set.
                        if (this.Model.CurrentSeason.GetAthleteAppearancesCount(key) > 0)
                        {
                            pointsEarned.BestPoints = this.resultsConfiguration.ResultsConfigurationDetails.SeasonBestPoints;
                            singleResult.SB = true;
                            this.RecordNewSB();
                        }
                    }

                    singleResult.Points = pointsEarned;

                    // Work out the personal best information.
                    if (this.Model.Athletes.GetPB(key) > singleResult.RunningTime)
                    {
                        // Only not as PB if not the first run.
                        if (!singleResult.FirstTimer)
                        {
                            singleResult.PB = true;
                            this.RecordNewPB();
                        }
                    }

                    this.CheckForFastestTime(this.Model.Athletes.GetAthleteSex(key),
                                        key,
                                        this.Model.Athletes.GetAthleteName(key),
                                        raw.TotalTime - athleteHandicap,
                                        eventDate);
                    this.UpdateNumberStatistics(this.Model.Athletes.GetAthleteSex(key),
                                           singleResult.FirstTimer);

                }

                this.Model.Athletes.AddNewTime(key, new Appearances(singleResult.RunningTime, eventDate));
                this.Model.CurrentSeason.AddNewTime(key, new Appearances(singleResult.RunningTime, eventDate));
                this.Model.CurrentSeason.AddNewPoints(key, pointsEarned);

                // End loop through all the entries in the raw results.
                resultsTable.AddEntry(singleResult);
            }

            return resultsTable;
        }

        private void AddPositionPoints(
          EventResults resultsTable,
          DateType currentDate,
          List<ClubPoints> clubPointsWorking)
        {
            if (this.resultsConfiguration.ResultsConfigurationDetails.ScoresAreDescending)
            {
                this.AddPositionPointsDescending(
                  resultsTable,
                  currentDate,
                  clubPointsWorking);
            }
            else
            {
                this.AddPositionPointsAscending(
                  resultsTable,
                  currentDate,
                  clubPointsWorking);
            }
        }

        /// <summary>
        /// Add the points for finishing in the top <see cref="positionPoint"/>. Start with the 
        /// highest points and work down to 0.
        /// </summary>
        /// <param name="resultsTable">reference to current results table</param>
        /// <param name="currentDate">date of current event</param>
        /// <param name="scoringPositions">number of scoring positions</param>
        private void AddPositionPointsDescending(
          EventResults resultsTable,
          DateType currentDate,
          List<ClubPoints> clubPointsWorking)
        {
            int positionPoint = this.resultsConfiguration.ResultsConfigurationDetails.NumberOfScoringPositions;

            resultsTable.OrderByFinishingTime();

            foreach (ResultsTableEntry result in resultsTable.Entries)
            {
                if (!result.Time.DNF && !result.Time.Unknown)
                {
                    if (this.PositionScoreToBeCounted(result.FirstTimer) &&
                      !(positionPoint == 0))
                    {
                        result.Points.PositionPoints = positionPoint;
                        this.Model.CurrentSeason.UpdatePositionPoints(result.Key, currentDate, positionPoint);

                        if (positionPoint > 0)
                        {
                            --positionPoint;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Add the points for finishing in the top <see cref="positionPoint"/>. Find the first girl/boy, second and third
        /// and make a note.
        /// </summary>
        /// <param name="resultsTable">reference to current results table</param>
        /// <param name="currentDate">date of current event</param>
        /// <param name="scoringPositions">number of scoring positions</param>
        private void AddPositionPointsAscending(
          EventResults resultsTable,
          DateType currentDate,
          List<ClubPoints> clubPointsWorking)
        {
            int positionPoint = 1;

            resultsTable.OrderByFinishingTime();

            foreach (ResultsTableEntry result in resultsTable.Entries)
            {
                if (!result.Time.DNF && !result.Time.Unknown)
                {
                    if (this.PositionScoreToBeCounted(result.FirstTimer))
                    {
                        result.Points.PositionPoints = positionPoint;
                        this.Model.CurrentSeason.UpdatePositionPoints(result.Key, currentDate, positionPoint);

                        ++positionPoint;
                    }
                }
            }
        }

        /// <summary>
        /// Assign club points from the athlete results. Update the model.
        /// </summary>
        /// <param name="resultsTable">reference to current results table</param>
        /// <param name="currentDate">date of current event</param>
        /// <param name="scoringPositions">number of scoring positions</param>
        private void AssignClubPoints(
          EventResults resultsTable,
          DateType currentDate,
          List<ClubPoints> clubPointsWorking)
        {
            if (!this.resultsConfiguration.ResultsConfigurationDetails.UseTeams)
            {
                return;
            }

            resultsTable.OrderByFinishingTime();

            foreach (ResultsTableEntry result in resultsTable.Entries)
            {
                if (result.Club == string.Empty)
                {
                    continue;
                }

                ClubPoints club = clubPointsWorking.Find(clubName => clubName.ClubName.CompareTo(result.Club) == 0);

                if (club != null)
                {
                    club.AddNewResult(
                      result.Points.PositionPoints,
                      result.FirstTimer,
                      result.SB);
                }
            }

            this.SaveClubPointsToModel(
              clubPointsWorking,
              currentDate);
        }

        /// <summary>
        /// Loop through the results and work out all the points for the harmony competition.
        /// </summary>
        /// <param name="resultsTable">results table</param>
        /// <param name="currentDate">date of the event</param>
        private void CalculateTeamHarmonyPoints(
          EventResults resultsTable,
          DateType currentDate)
        {
            int position = 0;
            int nextScore = 1;
            resultsTable.OrderByFinishingTime();
            Dictionary<string, IHarmonyEvent> eventDictionary = new Dictionary<string, IHarmonyEvent>();

            foreach(IClubSeasonDetails club in this.Model.CurrentSeason.Clubs)
            {
                IHarmonyEvent newEvent =
                    new HarmonyEvent(
                        currentDate);
                eventDictionary.Add(
                    club.Name,
                    newEvent);
            }

            foreach (ResultsTableEntry result in resultsTable.Entries)
            {
                IAthleteHarmonyPoints athletePoints;
                AthleteSeasonDetails athlete =
                    this.Model.CurrentSeason.Athletes.Find(
                        a => a.Key == result.Key);

                if (athlete == null)
                {
                    this.logger.WriteLog(
                        $"Calculate Results Manager - Can'f find athlete {result.Key}");
                    continue;
                }

                ++position;
                if (result.Club == string.Empty ||
                    result.FirstTimer)
                {
                    result.HarmonyPoints = HarmonyNoScore;

                    athletePoints =
                        new AthleteHarmonyPoints(
                            HarmonyNoScore, 
                            currentDate);

                    athlete.HarmonyPoints.AddNewEvent(athletePoints);
                    continue;
                }

                IHarmonyEvent clubEvent = eventDictionary[result.Club];

                ICommonHarmonyPoints clubPoint =
                    new CommonHarmonyPoints(
                        position,
                        result.Name,
                        result.Key,
                        true,
                        currentDate);
                bool success = clubEvent.AddPoint(clubPoint);

                if (success)
                {
                    nextScore = position + 1;
                    result.HarmonyPoints = position;
                }
                else
                {
                    result.HarmonyPoints = HarmonyNoScore;
                }

                athletePoints =
                   new AthleteHarmonyPoints(
                       result.HarmonyPoints,
                       currentDate);
                athlete.HarmonyPoints.AddNewEvent(athletePoints);
            }

            foreach (KeyValuePair<string, IHarmonyEvent> entry in eventDictionary)
            {
                entry.Value.Complete(HarmonyEvent.TeamSize, nextScore);
                this.Model.CurrentSeason.AddNewClubPoints(entry.Key, entry.Value);
            }
        }

        /// <summary>
        /// Add the points for finishing in the top <see cref="positionPoint"/>. Find the first girl/boy, second and third
        /// and make a note.
        /// </summary>
        /// <param name="resultsTable">reference to current results table</param>
        private void AddPlacings(
          EventResults resultsTable)
        {
            bool firstBoyFound = false;
            bool firstGirlFound = false;
            bool secondFound = false;
            bool thirdFound = false;
            bool secondBoyFound = false;
            bool secondGirlFound = false;
            bool thirdBoyFound = false;
            bool thirdGirlFound = false;

            foreach (ResultsTableEntry result in resultsTable.Entries)
            {
                if (result.Time.DNF)
                {
                    continue;
                }

                if (result.Time.Unknown)
                {
                    continue;
                }

                if (result.FirstTimer && this.resultsConfiguration.ResultsConfigurationDetails.ScoresAreDescending)
                {
                    continue;
                }

                if (
                  !(firstBoyFound &&
                      firstGirlFound &&
                      secondFound &&
                      thirdFound))
                {
                    if (!firstBoyFound && result.Sex == SexType.Male)
                    {
                        result.ExtraInfo = "First Boy";
                        firstBoyFound = true;
                    }
                    else if (!firstGirlFound && result.Sex == SexType.Female)
                    {
                        result.ExtraInfo = "First Gal";
                        firstGirlFound = true;
                    }
                    else if (!this.seriesConfiguration.AllPositionsShown && !secondFound && result.Sex != SexType.NotSpecified)
                    {
                        result.ExtraInfo = "Second";
                        secondFound = true;
                    }
                    else if (!this.seriesConfiguration.AllPositionsShown && !thirdFound && result.Sex != SexType.NotSpecified)
                    {
                        result.ExtraInfo = "Third";
                        thirdFound = true;
                    }
                    else if (this.seriesConfiguration.AllPositionsShown && !secondBoyFound && result.Sex == SexType.Male)
                    {
                        result.ExtraInfo = "Second Boy";
                        secondBoyFound = true;
                    }
                    else if (this.seriesConfiguration.AllPositionsShown && !secondGirlFound && result.Sex == SexType.Female)
                    {
                        result.ExtraInfo = "Second Gal";
                        secondGirlFound = true;
                    }
                    else if (this.seriesConfiguration.AllPositionsShown && !thirdBoyFound && result.Sex == SexType.Male)
                    {
                        result.ExtraInfo = "Third Boy";
                        thirdBoyFound = true;
                    }
                    else if (this.seriesConfiguration.AllPositionsShown && !thirdGirlFound && result.Sex == SexType.Female)
                    {
                        result.ExtraInfo = "Third Gal";
                        thirdGirlFound = true;
                    }
                }
            }
        }

        /// <summary>
        /// Add one new SB to all summary data.
        /// </summary>
        private void RecordNewSB()
        {
            this.Model.CurrentEvent.Summary.UpdateSummary(
                this.Model.CurrentEvent.Summary.MaleRunners,
                this.Model.CurrentEvent.Summary.FemaleRunners,
                this.Model.CurrentEvent.Summary.SBs + 1,
                this.Model.CurrentEvent.Summary.PBs,
                this.Model.CurrentEvent.Summary.FirstTimers);

            this.Model.CurrentSeason.Summary.UpdateSummary(
                this.Model.CurrentSeason.Summary.MaleRunners,
                this.Model.CurrentSeason.Summary.FemaleRunners,
                this.Model.CurrentSeason.Summary.SBs + 1,
                this.Model.CurrentSeason.Summary.PBs,
                this.Model.CurrentSeason.Summary.FirstTimers);

            this.Model.GlobalSummary.UpdateSummary(
                this.Model.GlobalSummary.MaleRunners,
                this.Model.GlobalSummary.FemaleRunners,
                this.Model.GlobalSummary.SBs + 1,
                this.Model.GlobalSummary.PBs,
                this.Model.GlobalSummary.FirstTimers);
        }

        /// <summary>
        /// Add one new PB to all summary data.
        /// </summary>
        private void RecordNewPB()
        {
            this.Model.CurrentEvent.Summary.UpdateSummary(
                this.Model.CurrentEvent.Summary.MaleRunners,
                this.Model.CurrentEvent.Summary.FemaleRunners,
                this.Model.CurrentEvent.Summary.SBs,
                this.Model.CurrentEvent.Summary.PBs + 1,
                this.Model.CurrentEvent.Summary.FirstTimers);

            this.Model.CurrentSeason.Summary.UpdateSummary(
                this.Model.CurrentSeason.Summary.MaleRunners,
                this.Model.CurrentSeason.Summary.FemaleRunners,
                this.Model.CurrentSeason.Summary.SBs,
                this.Model.CurrentSeason.Summary.PBs + 1,
                this.Model.CurrentSeason.Summary.FirstTimers);

            this.Model.GlobalSummary.UpdateSummary(
                this.Model.GlobalSummary.MaleRunners,
                this.Model.GlobalSummary.FemaleRunners,
                this.Model.GlobalSummary.SBs,
                this.Model.GlobalSummary.PBs + 1,
                this.Model.GlobalSummary.FirstTimers);
        }

        /// <summary>
        /// Check to see if the time can be added to the fastest times lists.
        /// </summary>
        /// <param name="sex">athlete sex</param>
        /// <param name="key">athlete key</param>
        /// <param name="name">athlete name</param>
        /// <param name="time">athlete time</param>
        /// <param name="date">date the time was set</param>
        private void CheckForFastestTime(
            SexType sex,
            int key,
            string name,
            TimeType time,
            DateType date)
        {
            if (sex == SexType.Female)
            {
                this.Model.CurrentEvent.Summary.SetFastestGirl(key, name, time, date);
                this.Model.CurrentSeason.Summary.SetFastestGirl(key, name, time, date);
                this.Model.GlobalSummary.SetFastestGirl(key, name, time, date);
            }
            else if (sex == SexType.Male)
            {
                this.Model.CurrentEvent.Summary.SetFastestBoy(key, name, time, date);
                this.Model.CurrentSeason.Summary.SetFastestBoy(key, name, time, date);
                this.Model.GlobalSummary.SetFastestBoy(key, name, time, date);
            }
        }

        /// <summary>
        /// Update all the number statistics.
        /// </summary>
        /// <param name="sex">athlete sex</param>
        /// <param name="firstTimer">indicates if the athlete is a first timer</param>
        private void UpdateNumberStatistics(
            SexType sex,
            bool firstTimer)
        {
            this.Model.CurrentEvent.Summary.UpdateSummary(
                sex == SexType.Male ? this.Model.CurrentEvent.Summary.MaleRunners + 1 : this.Model.CurrentEvent.Summary.MaleRunners,
                sex == SexType.Female ? this.Model.CurrentEvent.Summary.FemaleRunners + 1 : this.Model.CurrentEvent.Summary.FemaleRunners,
                this.Model.CurrentEvent.Summary.SBs,
                this.Model.CurrentEvent.Summary.PBs,
                firstTimer ? this.Model.CurrentEvent.Summary.FirstTimers + 1 : this.Model.CurrentEvent.Summary.FirstTimers);

            this.Model.CurrentSeason.Summary.UpdateSummary(
                sex == SexType.Male ? this.Model.CurrentSeason.Summary.MaleRunners + 1 : this.Model.CurrentSeason.Summary.MaleRunners,
                sex == SexType.Female ? this.Model.CurrentSeason.Summary.FemaleRunners + 1 : this.Model.CurrentSeason.Summary.FemaleRunners,
                this.Model.CurrentSeason.Summary.SBs,
                this.Model.CurrentSeason.Summary.PBs,
                firstTimer ? this.Model.CurrentSeason.Summary.FirstTimers + 1 : this.Model.CurrentSeason.Summary.FirstTimers);

            this.Model.GlobalSummary.UpdateSummary(
                sex == SexType.Male ? this.Model.GlobalSummary.MaleRunners + 1 : this.Model.GlobalSummary.MaleRunners,
                sex == SexType.Female ? this.Model.GlobalSummary.FemaleRunners + 1 : this.Model.GlobalSummary.FemaleRunners,
                this.Model.GlobalSummary.SBs,
                this.Model.GlobalSummary.PBs,
                firstTimer ? this.Model.GlobalSummary.FirstTimers + 1 : this.Model.GlobalSummary.FirstTimers);
        }

        /// <summary>
        /// Loop through all clubs and set the points in the mode.
        /// </summary>
        /// <param name="clubPoints">points for all clubs</param>
        /// <param name="currentDate">current date</param>
        private void SaveClubPointsToModel(
          List<ClubPoints> clubPoints,
          DateType currentDate)
        {
            foreach (ClubPoints club in clubPoints)
            {
                this.Model.CurrentSeason.AddNewClubPoints(
                  club.ClubName,
                  new CommonPoints(
                    club.FinishingPoints,
                    club.PositionPoints,
                    club.BestPoints,
                    currentDate));
            }
        }

        /// <summary>
        /// Determine whether the position score can be valid based on the config and if the result
        /// is for a first timer.
        /// </summary>
        /// <param name="isFirstTimer">is a first timer</param>
        /// <returns>flag indicating whether the score is valid</returns>
        private bool PositionScoreToBeCounted(
          bool isFirstTimer)
        {
            return !(this.resultsConfiguration.ResultsConfigurationDetails.ExcludeFirstTimers && isFirstTimer);
        }

        /// <summary>
        /// Gets the athlete handicap from the current season. If none is available, it gets it from
        /// the athlete main body as the predefined handicap.
        /// </summary>
        /// <param name="athleteKey">athlete key</param>
        /// <returns>athlete handicap</returns>
        private RaceTimeType GetAthleteHandicap(
          int athleteKey)
        {
            // Note the current handicap.
            RaceTimeType athleteHandicap =
              this.Model.CurrentSeason.GetAthleteHandicap(
                athleteKey,
                this.hcConfiguration);

            if (athleteHandicap == null)
            {
                TimeType globalHandicap =
                  this.Model.Athletes.GetRoundedHandicap(athleteKey);
                athleteHandicap =
                  new RaceTimeType(
                    globalHandicap.Minutes,
                    globalHandicap.Seconds);
            }

            return athleteHandicap;
        }
    }
}