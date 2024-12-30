﻿namespace HandicapModel.SeasonModel.EventModel
{
    using CommonLib.Enumerations;
    using CommonLib.Types;
    using CommonHandicapLib.Types;
    using HandicapModel.Common;
    using HandicapModel.Interfaces.Common;
    using HandicapModel.Interfaces.SeasonModel.EventModel;

    /// <summary>
    /// Single completed result, describes the results foe a single athlete.
    /// </summary>
    public class ResultsTableEntry : IResultsTableEntry
    {
        /// <summary>
        /// String to use for any first timer.
        /// </summary>
        private const string firstTimerNote = "First Timer";

        /// <summary>
        /// Creates a new instance of the ResultsTableEntry class
        /// </summary>
        /// <param name="name">athlete's name</param>
        /// <param name="time">total time</param>
        /// <param name="handicap">athlete's handicap</param>
        /// <param name="club">athlete's club</param>
        /// <param name="raceNumber">athlete's race number</param>
        /// <param name="points">athlete's points</param>
        /// <param name="teamTrophyPoints">points associated with the Team Trophy</param>
        /// <param name="pb">athlete's pb</param>
        /// <param name="yb">athlete's yb</param>
        /// <param name="notes">athlete's notes</param>
        /// <param name="position">the position of the current entry</param>
        public ResultsTableEntry(
          int key,
          string name,
          RaceTimeType time,
          int order,
          int runningOrder,
          RaceTimeType handicap,
          string club,
          SexType sex,
          string raceNumber,
          CommonPoints points,
          int teamTrophyPoints,
          bool pb,
          bool yb,
          string notes,
          string extraInfo,
          int position)
        {
            this.Key = key;
            this.Club = club;
            this.ExtraInfo = extraInfo;
            this.Handicap = handicap;
            this.Name = name;
            this.Order = order;
            this.PB = pb;
            this.Points = points;
            this.TeamTrophyPoints = teamTrophyPoints;
            this.RaceNumber = raceNumber;
            this.RunningOrder = runningOrder;
            this.Time = time;
            this.SB = yb;
            this.Sex = sex;
            this.Position = position;

            if (notes != null)
            {
                this.FirstTimer = notes.Contains(firstTimerNote);
            }
        }

        /// <summary>
        /// Create a new entry in the results table.
        /// </summary>
        /// <param name="name">Athlete's name</param>
        /// <param name="time">Total time</param>
        /// <param name="handicap">handicap for event</param>
        /// <param name="club">Athlete's club</param>
        /// <param name="raceNumber">Number used in event</param>
        /// <param name="date">event date</param>
        /// <param name="position">the position of the current entry</param>
        /// <param name="teamTrophyPoints">points associated with the Team Trophy</param>
        public ResultsTableEntry(
          int key,
          string name,
          RaceTimeType time,
          int order,
          RaceTimeType handicap,
          string club,
          SexType sex,
          string raceNumber,
          DateType date,
          int position,
          int teamTrophyPoints)
        {
            this.Key = key;
            this.Club = club;
            this.Handicap = handicap;
            this.Name = name;
            this.Points = new CommonPoints(date);
            this.TeamTrophyPoints = teamTrophyPoints;
            this.RaceNumber = raceNumber;
            this.Sex = sex;
            this.Time = time;
            this.ExtraInfo = string.Empty;

            if (time.Description == RaceTimeDescription.Relay)
            {
                this.Order = 1;
                this.Position = 1;
            }
            else
            {
                this.Order = order;
                this.Position = position;
            }
        }

        /// <summary>
        /// Gets the club.
        /// </summary>
        public string Club { get; private set; }

        /// <summary>
        /// Gets the athlete key.
        /// </summary>
        public int Key { get; private set; }

        /// <summary>
        /// Gets the handicap.
        /// </summary>
        public RaceTimeType Handicap { get; private set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets and sets the extra information.
        /// </summary>
        public string ExtraInfo { get; set; }

        /// <summary>
        /// Gets the notes.
        /// </summary>
        public string Notes
        {
            get
            {
                string note = ExtraInfo;

                if (FirstTimer)
                {
                    if (note != string.Empty)
                    {
                        note += " ";
                    }

                    note = note + firstTimerNote;
                }

                return note;
            }
        }

        /// <summary>
        /// Gets the first timer flag.
        /// </summary>
        public bool FirstTimer { get; set; }

        /// <summary>
        /// Gets the order.
        /// </summary>
        /// <remarks>
        /// Used to order athletes with the same finishing time.
        /// </remarks>
        public int Order { get; private set; }

        /// <summary>
        /// Gets and sets the running order.
        /// </summary>
        /// <remarks>
        /// Records the speed position of the athlete.
        /// </remarks>
        public int RunningOrder { get; set; }

        /// <summary>
        /// Gets the pb.
        /// </summary>
        public bool PB { get; set; }

        /// <summary>
        /// Gets the points.
        /// </summary>
        public ICommonPoints Points { get; set; }

        /// <summary>
        /// Gets or sets the points associated with the Team Trophy.
        /// </summary>
        public int TeamTrophyPoints { get; set; }

        /// <summary>
        /// Gets the race number.
        /// </summary>
        public string RaceNumber { get; private set; }

        /// <summary>
        /// Gets the time.
        /// </summary>
        public RaceTimeType Time { get; private set; }

        /// <summary>
        /// Gets the time taken to complete the run.
        /// </summary>
        /// <remarks>
        /// Complete time minus handicap.
        /// </remarks>
        public RaceTimeType RunningTime
        {
            get 
            {
                if (this.Time.Description != RaceTimeDescription.Finished)
                {
                    return this.Time;
                }

                return this.Time - this.Handicap; 
            }

        }

        /// <summary>
        /// Gets the athlete's sex.
        /// </summary>
        public SexType Sex { get; private set; }

        /// <summary>
        /// Gets the seasons best time.
        /// </summary>
        public bool SB { get; set; }

        /// <summary>
        /// Gets the position of the current entry.
        /// </summary>
        public int Position { get; }
    }
}