namespace HandicapModel.Admin.Manage.Event
{
    using CommonHandicapLib;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Types;
    using CommonLib.Enumerations;
    using CommonLib.Types;
    using HandicapModel.AthletesModel;
    using HandicapModel.Interfaces;
    using HandicapModel.SeasonModel;
    using HandicapModel.SeasonModel.EventModel;

    /// <summary>
    /// Manager class for deleting results
    /// </summary>
    public class DeleteResultsMngr : EventResultsMngr
    {
        /// <summary>
        /// Application logger
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// The normalisation config manager.
        /// </summary>
        private readonly INormalisationConfigMngr normalisationConfigMngr;

        /// <summary>
        /// Initialises a new instance of the <see cref="DeleteResultsMngr"/> class.
        /// </summary>
        /// <param name="model">junior handicap model</param>
        /// <param name="normalisationConfigManager">normalisation config manager</param>
        /// <param name="logger">application logger</param>
        public DeleteResultsMngr(
            IModel model,
            INormalisationConfigMngr normalisationConfigManager,
            IJHcLogger logger)
            : base(model)
        {
            this.logger = logger;
            this.normalisationConfigMngr = normalisationConfigManager;
        }

        /// <summary>
        /// 
        /// </summary>
        public void DeleteResults()
        {
            this.logger.WriteLog("Delete results");
            DateType currentDate = this.Model.CurrentEvent.Date;
            NormalisationConfigType hcConfiguration = 
                this.normalisationConfigMngr.ReadNormalisationConfiguration();

            // Remove points from all clubs for the known date.
            this.RemoveMobTrophyPoints(currentDate);

            // In the athlete's season remove all points and times for the known date.
            // Remove the time from the athlete's data
            this.RemoveAthletePoints(currentDate);

            foreach (ResultsTableEntry athleteEntry in this.Model.CurrentEvent.ResultsTable.Entries)
            {
                this.ReduceNumberStatistics(athleteEntry.Sex, athleteEntry.FirstTimer);
                this.RemoveFastestTime(athleteEntry.Sex, athleteEntry.Key, athleteEntry.Name, athleteEntry.RunningTime);

                // If season best, then delete one from the global summary and season summary.
                if (athleteEntry.SB)
                {
                    this.RemoveSBRecord();
                }

                // If personal best, then delete one from the global summary and season summary.
                if (athleteEntry.PB)
                {
                    this.RemovePBRecord();
                }
            }

            // Re initialise the event summary.
            this.Model.CurrentEvent.SetResultsTable(new EventResults());
            this.Model.CurrentEvent.Summary.Reset();

            this.Model.SaveAll();

            this.logger.WriteLog("Delete results completed");
        }

        /// <summary>
        /// Remove a single SB record from all summary data.
        /// </summary>
        private void RemoveSBRecord()
        {
            this.Model.CurrentEvent.Summary.UpdateSummary(
                this.Model.CurrentEvent.Summary.MaleRunners,
                this.Model.CurrentEvent.Summary.FemaleRunners,
                this.Model.CurrentEvent.Summary.SBs - 1,
                this.Model.CurrentEvent.Summary.PBs,
                this.Model.CurrentEvent.Summary.FirstTimers);

            this.Model.CurrentSeason.Summary.UpdateSummary(
                this.Model.CurrentSeason.Summary.MaleRunners,
                this.Model.CurrentSeason.Summary.FemaleRunners,
                this.Model.CurrentSeason.Summary.SBs - 1,
                this.Model.CurrentSeason.Summary.PBs,
                this.Model.CurrentSeason.Summary.FirstTimers);

            this.Model.GlobalSummary.UpdateSummary(
                this.Model.GlobalSummary.MaleRunners,
                this.Model.GlobalSummary.FemaleRunners,
                this.Model.GlobalSummary.SBs - 1,
                this.Model.GlobalSummary.PBs,
                this.Model.GlobalSummary.FirstTimers);
        }

        /// <summary>
        /// Remove a single PB record from all summary data.
        /// </summary>
        private void RemovePBRecord()
        {
            this.Model.CurrentEvent.Summary.UpdateSummary(
                this.Model.CurrentEvent.Summary.MaleRunners,
                this.Model.CurrentEvent.Summary.FemaleRunners,
                this.Model.CurrentEvent.Summary.SBs,
                this.Model.CurrentEvent.Summary.PBs - 1,
                this.Model.CurrentEvent.Summary.FirstTimers);

            this.Model.CurrentSeason.Summary.UpdateSummary(
                this.Model.CurrentSeason.Summary.MaleRunners,
                this.Model.CurrentSeason.Summary.FemaleRunners,
                this.Model.CurrentSeason.Summary.SBs,
                this.Model.CurrentSeason.Summary.PBs - 1,
                this.Model.CurrentSeason.Summary.FirstTimers);

            this.Model.GlobalSummary.UpdateSummary(
                this.Model.GlobalSummary.MaleRunners,
                this.Model.GlobalSummary.FemaleRunners,
                this.Model.GlobalSummary.SBs,
                this.Model.GlobalSummary.PBs - 1,
                this.Model.GlobalSummary.FirstTimers);
        }

        /// <summary>
        /// Check to see if the time can be removed from the fastest times lists.
        /// </summary>
        /// <param name="sex">athlete sex</param>
        /// <param name="key">athlete key</param>
        /// <param name="name">athlete name</param>
        /// <param name="time">athlete time</param>
        private void RemoveFastestTime(
            SexType sex,
            int key,
            string name,
            TimeType time)
        {
            if (sex == SexType.Female)
            {
                this.Model.CurrentEvent.Summary.RemoveFastestGirl(key, name, time);
                this.Model.CurrentSeason.Summary.RemoveFastestGirl(key, name, time);
                this.Model.GlobalSummary.RemoveFastestGirl(key, name, time);
            }
            else if (sex == SexType.Male)
            {
                this.Model.CurrentEvent.Summary.RemoveFastestBoy(key, name, time);
                this.Model.CurrentSeason.Summary.RemoveFastestBoy(key, name, time);
                this.Model.GlobalSummary.RemoveFastestBoy(key, name, time);
            }
        }

        /// <summary>
        /// Update all the number statistics.
        /// </summary>
        /// <param name="sex">athlete sex</param>
        /// <param name="firstTimer">indicates if the athlete is a first timer</param>
        private void ReduceNumberStatistics(
            SexType sex,
            bool firstTimer)
        {
            this.Model.CurrentEvent.Summary.UpdateSummary(
                sex == SexType.Male ? this.Model.CurrentEvent.Summary.MaleRunners - 1 : this.Model.CurrentEvent.Summary.MaleRunners,
                sex == SexType.Female ? this.Model.CurrentEvent.Summary.FemaleRunners - 1 : this.Model.CurrentEvent.Summary.FemaleRunners,
                this.Model.CurrentEvent.Summary.SBs,
                this.Model.CurrentEvent.Summary.PBs,
                firstTimer ? this.Model.CurrentEvent.Summary.FirstTimers - 1 : this.Model.CurrentEvent.Summary.FirstTimers);

            this.Model.CurrentSeason.Summary.UpdateSummary(
                sex == SexType.Male ? this.Model.CurrentSeason.Summary.MaleRunners - 1 : this.Model.CurrentSeason.Summary.MaleRunners,
                sex == SexType.Female ? this.Model.CurrentSeason.Summary.FemaleRunners - 1 : this.Model.CurrentSeason.Summary.FemaleRunners,
                this.Model.CurrentSeason.Summary.SBs,
                this.Model.CurrentSeason.Summary.PBs,
                firstTimer ? this.Model.CurrentSeason.Summary.FirstTimers - 1 : this.Model.CurrentSeason.Summary.FirstTimers);

            this.Model.GlobalSummary.UpdateSummary(
                sex == SexType.Male ? this.Model.GlobalSummary.MaleRunners - 1 : this.Model.GlobalSummary.MaleRunners,
                sex == SexType.Female ? this.Model.GlobalSummary.FemaleRunners - 1 : this.Model.GlobalSummary.FemaleRunners,
                this.Model.GlobalSummary.SBs,
                this.Model.GlobalSummary.PBs,
                firstTimer ? this.Model.GlobalSummary.FirstTimers - 1 : this.Model.GlobalSummary.FirstTimers);
        }

        /// <summary>
        /// Remove appearances and points from the global and season athlete data sets.
        /// </summary>
        /// <param name="currentDate"></param>
        private void RemoveAthletePoints(DateType currentDate)
        {
            // Remove from the global athletes data set.
            foreach (AthleteDetails athlete in this.Model.Athletes.AthleteDetails)
            {
                athlete.RemoveAppearances(currentDate);
            }

            // Remove from the season athletes data set.
            foreach (AthleteSeasonDetails athlete in this.Model.CurrentSeason.Athletes)
            {
                athlete.RemoveAppearances(currentDate);
                athlete.RemovePoints(currentDate);
            }
        }

        /// <summary>
        /// Remove points from the season club data set.
        /// </summary>
        /// <param name="currentDate"></param>
        private void RemoveMobTrophyPoints(DateType currentDate)
        {
            foreach (ClubSeasonDetails club in this.Model.CurrentSeason.Clubs)
            {
                club.MobTrophy.RemovePoints(currentDate);
            }
        }

    }
}