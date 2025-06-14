namespace HandicapModel.SeasonModel
{
    using System;
    using System.Collections.Generic;
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
          string name)
        {
            this.Key = key;
            this.Name = name;
            this.Points = new AthleteSeasonPoints();
            this.Times = new List<Appearances>();
            this.TeamTrophyPoints = new AthleteSeasonTeamTrophyPoints();
        }

        /// <summary>
        /// Event which is used to inform interested parties that there has been a change to this model.
        /// </summary>
        public event EventHandler ModelUpdateEvent;

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
        public IAthleteSeasonPoints Points { get; set; }

        /// <summary>
        /// Gets and sets the number of points assoicated with the Team Trophy.
        /// </summary>
        public IAthleteSeasonTeamTrophyPoints TeamTrophyPoints { get; set; }

        /// <summary>
        /// Gets all the times run this season.
        /// </summary>
        public List<Appearances> Times { get; }

        /// <summary>
        /// Gets the number of events taken part in, in the current season.
        /// </summary>
        public int NumberOfAppearances
        {
            get => Times.Count;
        }

        /// <summary>
        /// Adds a new event time to the list.
        /// </summary>
        /// <param name="runningNumber">running number</param>
        public void AddNewTime(Appearances time)
        {
            this.Times.Add(time);
            this.ModelUpdateEvent?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Update the points earnt for position for the indicated athlete on the indicated date.
        /// </summary>
        /// <param name="key">unique key</param>
        /// <param name="date">date of the event</param>
        /// <param name="points">earned points</param>
        public void UpdatePositionPoints(DateType date, int points)
        {
            this.Points.UpdatePositionPoints(date, points);
        }

        /// <summary>
        /// Add some new points
        /// </summary>
        /// <param name="points">points to add</param>
        public void AddNewPoints(CommonPoints points)
        {
            this.Points.AddNewEvent(points);
        }

        /// <summary>
        /// Calculates a new handicap from the list of times.
        /// </summary>
        /// <remarks>
        /// There are a number of ways of returning null. Null is returned when a rounded handicap
        /// is not determined.
        /// </remarks>
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

            for (int index = this.NumberOfAppearances - 1; index >= 0; --index)
            {
                if (eventsIncluded < 3)
                {
                    if (this.Times[index].Time.Description == RaceTimeDescription.Finished)
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

            if (eventsIncluded == 0)
            {
                return null;
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
            this.Points.RemovePoints(date);
        }
    }
}