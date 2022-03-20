namespace HandicapModel.Admin.Manage.Event
{
    using System.Collections.Generic;
    using System.Linq;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
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
    using GalaSoft.MvvmLight.Messaging;

    /// <summary>
    /// Manager class for calculating results.
    /// </summary>
    public class CalculateResultsMngr : EventResultsMngr
    {
        /// <summary>
        /// Value used to indicate a no score in the Team Trophy.
        /// </summary>
        private const int TeamTrophyNoScore = -1;

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
            HandicapProgressMessage startMessage = new HandicapProgressMessage("Calculate Results");
            Messenger.Default.Send(startMessage);

            if (this.resultsConfiguration == null)
            {
                this.logger.WriteLog("Error reading the results config file. Results not generated");

                HandicapErrorMessage faultMessage =
                    new HandicapErrorMessage(
                        "Can't calculate results - invalid config");
                Messenger.Default.Send(faultMessage);
                HandicapProgressMessage terminateMessage = new HandicapProgressMessage("Calculate Results - Terminated");
                Messenger.Default.Send(terminateMessage);

                return;
            }

            if (this.resultsConfiguration.ResultsConfigurationDetails.TeamTrophyPoints == null)
            {
                this.logger.WriteLog("Can't calculate results, Team Trophy points are invalid");

                HandicapErrorMessage faultMessage =
                    new HandicapErrorMessage(
                        "Can't calculate results - check config");
                Messenger.Default.Send(faultMessage);
                HandicapProgressMessage terminateMessage = new HandicapProgressMessage("Calculate Results - Terminated");
                Messenger.Default.Send(terminateMessage);

                return;
            }

            List<IRaw> rawResults = this.Model.CurrentEvent.LoadRawResults();

            // Set up the array to work out the points table
            List<MobTrophyPoints> mobTrophyPoints = this.SetupMobTrophyPoints();

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
              this.Model.CurrentEvent.Date);
            resultsTable.OrderByFinishingTime();
            this.AddPlacings(resultsTable);
            this.AssignMobTrophyPoints(
              resultsTable,
              this.Model.CurrentEvent.Date,
              mobTrophyPoints);
            this.CalculateTeamTrophyPoints(
                resultsTable,
                this.Model.CurrentEvent.Date);

            this.Model.CurrentEvent.SetResultsTable(resultsTable);

            this.SaveAll();

            this.logger.WriteLog("Calculate results completed.");
            HandicapProgressMessage finishedMessage = new HandicapProgressMessage("Calculate Results - Completed");
            Messenger.Default.Send(finishedMessage);

        }

        /// <summary>
        /// Create a <see cref="MobTrophyPoints"/> object for each registered club and then return them.
        /// </summary>
        /// <returns>Collection of <see cref="MobTrophyPoints"/> objects, one per club</returns>
        private List<MobTrophyPoints> SetupMobTrophyPoints()
        {
            List<MobTrophyPoints> mobTrophyPoints = new List<MobTrophyPoints>();
            foreach (string club in this.Model.Clubs.ClubDetails)
            {
                mobTrophyPoints.Add(
                  new MobTrophyPoints(
                    club,
                    this.resultsConfiguration.ResultsConfigurationDetails.NumberInTeam,
                    this.resultsConfiguration.ResultsConfigurationDetails.TeamFinishingPoints,
                    this.resultsConfiguration.ResultsConfigurationDetails.TeamSeasonBestPoints));
            }

            return mobTrophyPoints;
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
          DateType currentDate)
        {
            if (this.resultsConfiguration.ResultsConfigurationDetails.ScoresAreDescending)
            {
                this.AddPositionPointsDescending(
                  resultsTable,
                  currentDate);
            }
            else
            {
                this.AddPositionPointsAscending(
                  resultsTable,
                  currentDate);
            }
        }

        /// <summary>
        /// Add the points for finishing in the top <see cref="positionPoint"/>. Start with the 
        /// highest points and work down to 0.
        /// </summary>
        /// <param name="resultsTable">reference to current results table</param>
        /// <param name="currentDate">date of current event</param>
        private void AddPositionPointsDescending(
          EventResults resultsTable,
          DateType currentDate)
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
        private void AddPositionPointsAscending(
          EventResults resultsTable,
          DateType currentDate)
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
        private void AssignMobTrophyPoints(
          EventResults resultsTable,
          DateType currentDate,
          List<MobTrophyPoints> mobTrophyPointsWorking)
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

                MobTrophyPoints club = mobTrophyPointsWorking.Find(clubName => clubName.ClubName.CompareTo(result.Club) == 0);

                if (club != null)
                {
                    club.AddNewResult(
                      result.Points.PositionPoints,
                      result.FirstTimer,
                      result.SB);
                }
            }

            this.SaveMobTrophyPointsToModel(
              mobTrophyPointsWorking,
              currentDate);
        }

        /// <summary>
        /// Loop through the results and work out all the points for the Team Trophy.
        /// </summary>
        /// <param name="resultsTable">results table</param>
        /// <param name="currentDate">date of the event</param>
        private void CalculateTeamTrophyPoints(
          EventResults resultsTable,
          DateType currentDate)
        {
            // Next score is used to complete the Team Trophy by filling in any blank spots.
            // The position is used to assign points to an athlete in the Team Trophy.
            int teamTrophyCompetitionPosition = 0;
            int nextScore = 1;
 
            resultsTable.OrderByFinishingTime();
            Dictionary<string, ITeamTrophyEvent> eventDictionary = new Dictionary<string, ITeamTrophyEvent>();

            foreach(IClubSeasonDetails club in this.Model.CurrentSeason.Clubs)
            {
                ITeamTrophyEvent newEvent =
                    new TeamTrophyEvent(
                        currentDate,
                        this.resultsConfiguration.ResultsConfigurationDetails.NumberInTeamTrophyTeam);
                eventDictionary.Add(
                    club.Name,
                    newEvent);
            }

            foreach (ResultsTableEntry result in resultsTable.Entries)
            {
                IAthleteTeamTrophyPoints athletePoints;
                AthleteSeasonDetails athlete =
                    this.Model.CurrentSeason.Athletes.Find(
                        a => a.Key == result.Key);

                if (athlete == null)
                {
                    this.logger.WriteLog(
                        $"Calculate Results Manager - Can'f find athlete {result.Key}");
                    continue;
                }

                if (result.Club == string.Empty ||
                    result.FirstTimer)
                {
                    result.TeamTrophyPoints = TeamTrophyNoScore;

                    athletePoints =
                        new AthleteTeamTrophyPoints(
                            TeamTrophyNoScore, 
                            currentDate);

                    athlete.TeamTrophyPoints.AddNewEvent(athletePoints);

                    // Not part of the Team Trophy, move onto the next loop.
                    continue;
                }

                ++teamTrophyCompetitionPosition;
                ITeamTrophyEvent clubEvent = eventDictionary[result.Club];

                ICommonTeamTrophyPoints clubPoint =
                    new CommonTeamTrophyPoints(
                        teamTrophyCompetitionPosition,
                        result.Name,
                        result.Key,
                        true,
                        currentDate);

                // Attempt to add point to the club. It will fail if the team is already full.
                bool success = clubEvent.AddPoint(clubPoint);

                if (success)
                {
                    nextScore = teamTrophyCompetitionPosition + 1;
                    result.TeamTrophyPoints = teamTrophyCompetitionPosition;
                }
                else
                {
                    // Add points failed, revert the Team Trophy position.
                    --teamTrophyCompetitionPosition;
                    result.TeamTrophyPoints = TeamTrophyNoScore;
                }

                athletePoints =
                   new AthleteTeamTrophyPoints(
                       result.TeamTrophyPoints,
                       currentDate);
                athlete.TeamTrophyPoints.AddNewEvent(athletePoints);
            }

            List<ITeamTrophyEvent> orderedEvent = new List<ITeamTrophyEvent>();
            foreach (KeyValuePair<string, ITeamTrophyEvent> entry in eventDictionary)
            {
                entry.Value.Complete(
                    this.resultsConfiguration.ResultsConfigurationDetails.NumberInTeamTrophyTeam,
                    nextScore);
                orderedEvent.Add(entry.Value);
            }

            // Apply the score for each team as defined by the configuration file.
            // To order the teams, they've needed to be pulled out from the dictionary into a list.
            orderedEvent = orderedEvent.OrderBy(e => e.TotalAthletePoints).ToList();

            int lastPoints = -1;
            int lastScoringIndex = 0;

            for (int index = 0; index < orderedEvent.Count; ++index)
            {
                if (orderedEvent[index].NumberOfAthletes == 0)
                {
                    break;
                }

                if (orderedEvent[index].TotalAthletePoints == lastPoints)
                {
                    orderedEvent[index].Score =
                        this.resultsConfiguration.ResultsConfigurationDetails.TeamTrophyPoints[lastScoringIndex];
                }
                else if (index < this.resultsConfiguration.ResultsConfigurationDetails.TeamTrophyPoints.Count)
                {
                    orderedEvent[index].Score = this.resultsConfiguration.ResultsConfigurationDetails.TeamTrophyPoints[index];
                    lastScoringIndex = index;
                }

                lastPoints = orderedEvent[index].TotalAthletePoints;
            }

            foreach (KeyValuePair<string, ITeamTrophyEvent> entry in eventDictionary)
            {
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
        /// <param name="mobTrophyPoints">points for all clubs</param>
        /// <param name="currentDate">current date</param>
        private void SaveMobTrophyPointsToModel(
          List<MobTrophyPoints> mobTrophyPoints,
          DateType currentDate)
        {
            foreach (MobTrophyPoints club in mobTrophyPoints)
            {
                this.Model.CurrentSeason.AddNewMobTrophyPoints(
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