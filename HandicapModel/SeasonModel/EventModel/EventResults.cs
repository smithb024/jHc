namespace HandicapModel.SeasonModel.EventModel
{
    using System.Collections.Generic;
    using System.Linq;
    using CommonLib.Types;
    using HandicapModel.Interfaces.SeasonModel.EventModel;

    /// <summary>
    /// Results for an event.
    /// </summary>
    public class EventResults : IEventResults
    {
        /// <summary>
        /// Creates a new instance of the ResultsTable class.
        /// </summary>
        public EventResults()
        {
            this.Entries = new List<IResultsTableEntry>();
        }

        /// <summary>
        /// Gets the a results entry.
        /// </summary>
        public List<IResultsTableEntry> Entries { get; private set; }

        /// <summary>
        /// Adds an entry to the list.
        /// </summary>
        /// <param name="entry">new entry</param>
        public void AddEntry(IResultsTableEntry entry)
        {
            Entries.Add(entry);
        }

        /// <summary>
        /// Order the results table by the total time taken.
        /// </summary>
        public void OrderByFinishingTime()
        {
            this.Entries = this.Entries.OrderBy(order => order.Order).ToList();
            this.Entries = this.Entries.OrderBy(order => order.Time.Seconds).ToList();
            this.Entries = this.Entries.OrderBy(order => order.Time.Minutes).ToList();
        }

        /// <summary>
        /// Order the results tabke by the time spent running.
        /// </summary>
        public void OrderByRunningTime()
        {
            this.Entries = this.Entries.OrderBy(order => order.RunningTime.Seconds).ToList();
            this.Entries = this.Entries.OrderBy(order => order.RunningTime.Minutes).ToList();
        }

        /// <summary>
        /// Sort by running time, then go through the list applying the speed order.
        /// </summary>
        /// <remarks>
        /// If two athletes have the same time, then they have the same speed order.
        /// </remarks>
        public void ApplySpeedOrder()
        {
            int position = 1;
            TimeType lastTime = new TimeType(59, 59);
            int lastPostion = position;

            OrderByRunningTime();

            foreach (ResultsTableEntry result in this.Entries = this.Entries)
            {
                if (result.RunningTime.Equals(lastTime))
                {
                    result.RunningOrder = lastPostion;
                }
                else
                {
                    result.RunningOrder = position;
                    lastPostion = position;
                    lastTime = result.RunningTime;
                }

                ++position;
            }
        }
    }
}