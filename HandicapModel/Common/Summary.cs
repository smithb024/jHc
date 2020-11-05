namespace HandicapModel.Common
{
    using System;
    using System.Collections.Generic;
    using CommonLib.Types;
    using HandicapModel.Interfaces.Common;

    /// <summary>
    /// The summary of a set of results
    /// </summary>
    public class Summary : ISummary
    {
        /// <summary>
        /// The maximum number of athletes stored as fastest times.
        /// </summary>
        private const int maxSize = 50;

        /// <summary>
        ///   Creates a new instance of the <see cref="Summary"/> class
        /// </summary>
        public Summary()
            : this(0, 0, 0, 0, 0)
        {
        }

        /// <summary>
        ///   Creates a new instance of the SummaryType class
        /// </summary>
        public Summary(
            int numberOfMaleRunners,
            int numberOfFemaleRunners,
            int numberOfYB,
            int numberOfPB,
            int numberOfFirstTimers)
        {
            MaleRunners = numberOfMaleRunners;
            FemaleRunners = numberOfFemaleRunners;
            SBs = numberOfYB;
            PBs = numberOfPB;
            FirstTimers = numberOfFirstTimers;
            this.FastestBoys = new List<IAthleteTime>();
            this.FastestGirls = new List<IAthleteTime>();
        }

        /// <summary>
        /// Event which is used to inform interested parties that there has been a change to this
        /// summary. This event focuses on the summary data.
        /// </summary>
        public event EventHandler SummaryDataChangedEvent;

        /// <summary>
        /// Event which is used to inform interested parties that there has been a change to this
        /// summary. This event focuses on the fastest athletes data.
        /// </summary>
        public event EventHandler FastestDataChangedEvent;

        /// <summary>
        /// Gets the number of runners in the event.
        /// </summary>
        public int Runners => this.MaleRunners + this.FemaleRunners;

        /// <summary>
        /// Gets the number of male runners in the event.
        /// </summary>
        public int MaleRunners { get; private set; }

        /// <summary>
        /// Gets the number of female runners in the event.
        /// </summary>
        public int FemaleRunners { get; private set; }

        /// <summary>
        /// Gets the number of season bests in the event.
        /// </summary>
        public int SBs { get; private set; }

        /// <summary>
        /// Gets the number of personal best in the event.
        /// </summary>
        public int PBs { get; private set; }

        /// <summary>
        /// Gets the number of first timers.
        /// </summary>
        public int FirstTimers { get; private set; }

        /// <summary>
        /// Gets the Fastest boys list.
        /// </summary>
        public List<IAthleteTime> FastestBoys { get; private set; }

        /// <summary>
        /// Gets the Fastest girls list.
        /// </summary>
        public List<IAthleteTime> FastestGirls { get; private set; }

        /// <summary>
        /// Gets the name of the fastest boy. Returns empty string if there is no fastest time noted.
        /// </summary>
        public string FastestBoy
        {
            get
            {
                if (this.FastestBoys.Count > 0)
                {
                    return this.FastestBoys[0].Name;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets the time of the fastest boy. Returns 59,59 if there is no fastest time noted.
        /// </summary>
        public TimeType FastestBoyTime
        {
            get
            {
                if (this.FastestBoys.Count > 0)
                {
                    return this.FastestBoys[0].Time;
                }
                else
                {
                    return new TimeType(59, 59);
                }
            }
        }

        /// <summary>
        /// Gets the name of the fastest girl. Returns empty string if there is no fastest time noted.
        /// </summary>
        public string FastestGirl
        {
            get
            {
                if (this.FastestGirls.Count > 0)
                {
                    return this.FastestGirls[0].Name;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets the time of the fastest girl. Returns 59,59 if there is no fastest time noted.
        /// </summary>
        public TimeType FastestGirlTime
        {
            get
            {
                if (this.FastestGirls.Count > 0)
                {
                    return this.FastestGirls[0].Time;
                }
                else
                {
                    return new TimeType(59, 59);
                }
            }
        }

        /// <summary>
        /// Update the summary data.
        /// </summary>
        /// <param name="maleRunners">number of male runners</param>
        /// <param name="femaleRunners">number of female runners</param>
        /// <param name="sBs">number of season bests</param>
        /// <param name="pBs">number of personal bests</param>
        /// <param name="firstTimers">number of first timers</param>
        public void UpdateSummary(
            int maleRunners,
            int femaleRunners,
            int sBs,
            int pBs,
            int firstTimers)
        {
            this.MaleRunners = maleRunners;
            this.FemaleRunners = femaleRunners;
            this.SBs = sBs;
            this.PBs = pBs;
            this.FirstTimers = firstTimers;

            this.SummaryDataChangedEvent?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Sets the fastest boy time
        /// </summary>
        /// <param name="key">athlete's key</param>
        /// <param name="name">athlete's name</param>
        /// <param name="time">athlete's time</param>
        /// <param name="date">date of the time</param>
        public void SetFastestBoy(
            int key,
            string name,
            TimeType time,
            DateType date)
        {
            if (FastestBoys.Count > 0 && time < FastestBoys[FastestBoys.Count - 1].Time)
            {
                for (int index = 0; index < FastestBoys.Count; ++index)
                {
                    if (time < FastestBoys[index].Time)
                    {
                        FastestBoys.Insert(index, new AthleteTime(key, name, time, date));
                        break;
                    }
                }
            }
            else if (FastestBoys.Count < maxSize)
            {
                FastestBoys.Add(new AthleteTime(key, name, time, date));
            }

            // Remove the last element if the list now exceeds the maximum size.
            if (FastestBoys.Count > maxSize)
            {
                FastestBoys.RemoveAt(FastestBoys.Count - 1);
            }

            this.FastestDataChangedEvent?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Sets the fastest girl time
        /// </summary>
        /// <param name="key">athlete's key</param>
        /// <param name="name">athlete's name</param>
        /// <param name="time">athlete's time</param>
        /// <param name="date">date of the time</param>
        public void SetFastestGirl(
            int key,
            string name,
            TimeType time,
            DateType date)
        {
            if (FastestGirls.Count > 0 && time < FastestGirls[FastestGirls.Count - 1].Time)
            {
                for (int index = 0; index < FastestGirls.Count; ++index)
                {
                    if (time < FastestGirls[index].Time)
                    {
                        FastestGirls.Insert(index, new AthleteTime(key, name, time, date));
                        break;
                    }
                }
            }
            else if (FastestGirls.Count < maxSize)
            {
                FastestGirls.Add(new AthleteTime(key, name, time, date));
            }

            // Remove the last element if the list now exceeds the maximum size.
            if (FastestGirls.Count > maxSize)
            {
                FastestGirls.RemoveAt(FastestGirls.Count - 1);
            }

            this.FastestDataChangedEvent?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Remove a record corresponding to the argument data if one exists.
        /// </summary>
        /// <param name="key">athlete key</param>
        /// <param name="name">athlete name</param>
        /// <param name="time">athlete time</param>
        public void RemoveFastestBoy(
            int key,
            string name,
            TimeType time)
        {
            if (FastestBoys.Count > 0)
            {
                for (int index = 0; index < FastestBoys.Count; ++index)
                {
                    if (key == FastestBoys[index].Key &&
                        name == FastestBoys[index].Name &&
                        time == FastestBoys[index].Time)
                    {
                        FastestBoys.RemoveAt(index);
                        this.FastestDataChangedEvent?.Invoke(this, new EventArgs());
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Remove a record corresponding to the argument data if one exists.
        /// </summary>
        /// <param name="key">athlete key</param>
        /// <param name="name">athlete name</param>
        /// <param name="time">athlete time</param>
        public void RemoveFastestGirl(
            int key,
            string name,
            TimeType time)
        {
            if (FastestGirls.Count > 0)
            {
                for (int index = 0; index < FastestGirls.Count; ++index)
                {
                    if (key == FastestGirls[index].Key &&
                        name == FastestGirls[index].Name &&
                        time == FastestGirls[index].Time)
                    {
                        FastestGirls.RemoveAt(index);
                        this.FastestDataChangedEvent?.Invoke(this, new EventArgs());
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Load a new set of fastest collections into the summary.
        /// </summary>
        /// <param name="fastestBoys">fastest boys</param>
        /// <param name="fastestGirls">fastest girls</param>
        public void LoadFastest(
            List<IAthleteTime> fastestBoys,
            List<IAthleteTime> fastestGirls)
        {
            this.FastestBoys = fastestBoys;
            this.FastestGirls = fastestGirls;

            this.FastestDataChangedEvent?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Reset the model.
        /// </summary>
        public void Reset()
        {
            this.MaleRunners = 0;
            this.FemaleRunners = 0;
            this.SBs = 0;
            this.PBs = 0;
            this.FirstTimers = 0;
            this.FastestBoys = new List<IAthleteTime>();
            this.FastestGirls = new List<IAthleteTime>();

            this.SummaryDataChangedEvent?.Invoke(this, new EventArgs());
            this.FastestDataChangedEvent?.Invoke(this, new EventArgs());
        }
    }
}