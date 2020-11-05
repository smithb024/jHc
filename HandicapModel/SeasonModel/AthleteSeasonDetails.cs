namespace HandicapModel.SeasonModel
{
    using System.Collections.Generic;
    using Admin.Manage;
    using CommonHandicapLib.Helpers;
    using CommonHandicapLib.Types;
    using HandicapModel.Common;
    using CommonLib.Types;
    using HandicapModel.Interfaces.SeasonModel;

    /// <summary>
    /// Class which describes an athletes details for a specific season.
    /// </summary>
    public class AthleteSeasonDetails : IAthleteSeasonDetails
    {
        /// <summary>
        /// Minutes value used in the default season best time.
        /// </summary>
        private const int DefaultSBMins = 59;

        /// <summary>
        /// Seconds value used in the default season best time.
        /// </summary>
        private const int DefaultSBSeconds = 59;

        /// <summary>
        /// Creates a new instance of the HandicapSeason class
        /// </summary>
        /// <param name="key">new key</param>
        public AthleteSeasonDetails(
          int key,
          string name,
          IResultsConfigMngr resultsConfigurationManager)
        {
            Key = key;
            this.Name = name;
            Points =
              new AthleteSeasonPoints(
                resultsConfigurationManager);
            this.Times = new List<Appearances>();
            this.HarmonyPoints = new AthleteSeasonHarmonyPoints();
        }

        /// <summary>
        /// Gets and sets the unique key.
        /// </summary>
        public int Key { get; private set; }

        /// <summary>
        /// Gets the athlete name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets and sets the best time this season.
        /// </summary>
        public TimeType SB
        {
            get
            {
                TimeType sb = new TimeType(DefaultSBMins, DefaultSBSeconds);

                foreach (Appearances appearance in this.Times)
                {
                    if (appearance.Time < sb)
                    {
                        sb = appearance.Time;
                    }
                }

                return sb;
            }
        }

        /// <summary>
        /// Gets and sets the number of points
        /// </summary>
        public AthleteSeasonPoints Points { get; set; }

        /// <summary>
        /// Gets and sets the number of points assoicated with the harmnony competiion.
        /// </summary>
        public IAthleteSeasonHarmonyPoints HarmonyPoints { get; set; }

        /// <summary>
        /// Gets all the times run this season.
        /// </summary>
        public List<Appearances> Times { get; }

        /// <summary>
        /// Gets the number of events taken part in, in the current season.
        /// </summary>
        public int NumberOfAppearances
        {
            get { return Times.Count; }
        }

        /// <summary>
        /// Adds a new event time to the list.
        /// </summary>
        /// <param name="runningNumber">running number</param>
        public void AddNewTime(Appearances time)
        {
            this.Times.Add(time);
        }

        /// <summary>
        /// Update the points earnt for position for the indicated athlete on the indicated date.
        /// </summary>
        /// <param name="key">unique key</param>
        /// <param name="date">date of the event</param>
        /// <param name="points">earned points</param>
        public void UpdatePositionPoints(DateType date, int points)
        {
            Points.UpdatePositionPoints(date, points);
        }

        /// <summary>
        /// Add some new points
        /// </summary>
        /// <param name="points">points to add</param>
        public void AddNewPoints(CommonPoints points)
        {
            Points.AddNewEvent(points);
        }

        /// <summary>
        /// Calculates a new handicap from the list of times.
        /// </summary>
        public RaceTimeType GetRoundedHandicap(NormalisationConfigType hcConfiguration)
        {
            RaceTimeType handicap;
            RaceTimeType handicapWorking = new RaceTimeType(0, 0);

            if (!hcConfiguration.UseCalculatedHandicap)
            {
                return null;
            }

            if (this.NumberOfAppearances == 0)
            {
                return null;
            }

            int eventsIncluded = 0;

            for (int index = NumberOfAppearances - 1; index >= 0; --index)
            {
                if (eventsIncluded < 3)
                {
                    if (!this.Times[index].Time.DNF && !this.Times[index].Time.Unknown)
                    {
                        handicapWorking = handicapWorking + this.Times[index].Time;
                        ++eventsIncluded;
                    }
                }
                else
                {
                    break;
                }
            }

            handicap = new RaceTimeType(hcConfiguration.HandicapTime, 0) - (handicapWorking / eventsIncluded);

            return HandicapHelper.RoundHandicap(
              hcConfiguration,
              handicap);
        }

        /// <summary>
        /// Remove all appearances corresponding to the argument date. Do nothing if there are no appearances for the date.
        /// </summary>
        /// <param name="date">date to remove.</param>
        public void RemoveAppearances(DateType date)
        {
            if (this.Times.Exists(dateCheck => dateCheck.Date == date))
            {
                this.Times.Remove(this.Times.Find(dateCheck => dateCheck.Date == date));
            }
        }

        /// <summary>
        /// Remove all points corresponding to the argument date. Do nothing if there are no appearances for the date.
        /// </summary>
        /// <param name="date">date to remove</param>
        public void RemovePoints(DateType date)
        {
            Points.RemovePoints(date);
        }
    }
}