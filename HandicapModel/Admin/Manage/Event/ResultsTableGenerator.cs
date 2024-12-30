namespace HandicapModel.Admin.Manage.Event
{
    using CommonHandicapLib.Types;
    using CommonLib.Enumerations;
    using CommonLib.Types;
    using HandicapModel.AthletesModel;
    using HandicapModel.Common;
    using HandicapModel.Interfaces;
    using HandicapModel.Interfaces.SeasonModel;
    using HandicapModel.Interfaces.SeasonModel.EventModel;
    using HandicapModel.SeasonModel.EventModel;
    using System.Collections.Generic;

    /// <summary>
    /// Factory Class which is used to create the <see cref="EventResults"/> table.
    /// </summary>
    public class ResultsTableGenerator : IResultsTableGenerator
    {
        /// <summary>
        /// Results Input/Output
        /// </summary>
        private readonly IResultsConfigMngr resultsConfiguration;

        /// <summary>
        /// Manager which contains all handicap configuration details.
        /// </summary>
        private readonly NormalisationConfigType hcConfiguration;

        /// <summary>
        /// The model object.
        /// </summary>
        private readonly IModel model;

        /// <summary>
        /// The athletes in the model.
        /// </summary>
        private Athletes athletes;

        /// <summary>
        /// Initialises a new instance of the <see cref="ResultsTableGenerator"/> class.
        /// </summary>
        /// <param name="resultsConfiguration">The results configuration manager.</param>
        /// <param name="normalisationConfigurationManager">
        /// normalisation configuration manager
        /// </param>
        /// <param name="model">The application model object</param>
        public ResultsTableGenerator(
            IResultsConfigMngr resultsConfiguration,
            INormalisationConfigMngr normalisationConfigurationManager,
            IModel model)
        {
            this.resultsConfiguration = resultsConfiguration;
            this.hcConfiguration = normalisationConfigurationManager.ReadNormalisationConfiguration();
            this.model = model;
            this.athletes = null;
        }

        /// <summary>
        /// Generate the results table from the raw results and return it.
        /// </summary>
        /// <param name="rawResults">raw results</param>
        /// <returns>event results table</returns>
        public EventResults Generate(
            List<IRaw> rawResults)
        {
            EventResults resultsTable = new EventResults();
            DateType eventDate = this.model.CurrentEvent.Date;
            ISeason currentSeason = this.model.CurrentSeason;
            this.athletes = this.model.Athletes;

            foreach (IRaw raw in rawResults)
            {
                CommonPoints pointsEarned = new CommonPoints(eventDate);

                // Get athlete key.
                int key = this.athletes.GetAthleteKey(raw.RaceNumber) ?? 0;
                string athleteName = this.athletes.GetAthleteName(key);
                string athleteClub = this.athletes.GetAthleteClub(key);
                SexType athleteSex = this.athletes.GetAthleteSex(key);
                TimeType pb = this.athletes.GetPB(key);

                // Note the current handicap.
                RaceTimeType athleteHandicap =
                  this.GetAthleteHandicap(
                    key);

                // Loop through all the entries in the raw results.
                ResultsTableEntry singleResult =
                  new ResultsTableEntry(
                    key,
                    athleteName,
                    raw.TotalTime,
                    raw.Order,
                    athleteHandicap,
                    athleteClub,
                    athleteSex,
                    raw.RaceNumber,
                    eventDate,
                    resultsTable.Entries.Count + 1,
                    999999);

                if (raw.TotalTime.Description == RaceTimeDescription.Finished)
                {
                    // The athlete finished successfully - this is a normal event.
                    // Go through and set all the points/times and bests.
                    if (this.athletes.IsFirstTimer(key))
                    {
                        singleResult.FirstTimer = true;
                    }

                    pointsEarned.FinishingPoints = this.resultsConfiguration.ResultsConfigurationDetails.FinishingPoints;

                    // Work out the season best information
                    if (currentSeason.GetSB(key) > singleResult.RunningTime)
                    {
                        // Can only count as season best if one time has been set.
                        if (currentSeason.GetAthleteAppearancesCount(key) > 0)
                        {
                            pointsEarned.BestPoints = this.resultsConfiguration.ResultsConfigurationDetails.SeasonBestPoints;
                            singleResult.SB = true;
                            this.model.IncrementSummaries(SummaryPropertiesType.SB);
                        }
                    }

                    singleResult.Points = pointsEarned;

                    // Work out the personal best information.
                    if (pb > singleResult.RunningTime)
                    {
                        // Only not as PB if not the first run.
                        if (!singleResult.FirstTimer)
                        {
                            singleResult.PB = true;
                            this.model.IncrementSummaries(SummaryPropertiesType.PB);
                        }
                    }

                    this.model.SetFastest(
                        athleteSex,
                        key,
                        athleteName,
                        raw.TotalTime - athleteHandicap,
                        eventDate);
                    this.UpdateNumberStatistics(
                        athleteSex,
                        singleResult.FirstTimer);

                }
                else if (raw.TotalTime.Description == RaceTimeDescription.Dnf)
                {
                    // The athlete started, but couldn't finish. Apply the default points earned.
                    singleResult.Points = pointsEarned;
                }
                else if (raw.TotalTime.Description == RaceTimeDescription.Relay)
                {
                    // The athlete competed in a relay event. 
                    // Add finishing points and update the statistics. 
                    pointsEarned.FinishingPoints = this.resultsConfiguration.ResultsConfigurationDetails.FinishingPoints;
                    singleResult.Points = pointsEarned;

                    this.UpdateNumberStatistics(
                        athleteSex,
                        false);
                }

                this.athletes.AddNewTime(key, new Appearances(singleResult.RunningTime, eventDate));
                currentSeason.AddNewTime(key, new Appearances(singleResult.RunningTime, eventDate));
                currentSeason.AddNewPoints(key, pointsEarned);

                // End loop through all the entries in the raw results.
                resultsTable.AddEntry(singleResult);
            }

            return resultsTable;
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
              this.model.CurrentSeason.GetAthleteHandicap(
                athleteKey,
                this.hcConfiguration);

            if (athleteHandicap == null)
            {
                TimeType globalHandicap =
                  this.athletes.GetRoundedHandicap(athleteKey);
                athleteHandicap =
                  new RaceTimeType(
                    globalHandicap.Minutes,
                    globalHandicap.Seconds);
            }

            return athleteHandicap;
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
            if (sex == SexType.Male)
            {
                this.model.IncrementSummaries(SummaryPropertiesType.Male);
            }
            else if (sex == SexType.Female)
            {
                this.model.IncrementSummaries(SummaryPropertiesType.Female);
            }

            if (firstTimer)
            {
                this.model.IncrementSummaries(SummaryPropertiesType.FT);
            }
        }
    }
}
