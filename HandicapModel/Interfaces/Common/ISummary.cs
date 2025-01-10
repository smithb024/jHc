namespace HandicapModel.Interfaces.Common
{
    using System;
    using System.Collections.Generic;
    using CommonLib.Enumerations;
    using CommonLib.Types;

    /// <summary>
    /// Interface which describes the summary of a set of results.
    /// </summary>
    public interface ISummary
    {
        /// <summary>
        /// Event which is used to inform interested parties that there has been a change to this
        /// summary. This event focuses on the summary data.
        /// </summary>
        event EventHandler SummaryDataChangedEvent;

        /// <summary>
        /// Event which is used to inform interested parties that there has been a change to this
        /// summary. This event focuses on the fastest athletes data.
        /// </summary>
        event EventHandler FastestDataChangedEvent;

        /// <summary>
        /// Gets the number of runners
        /// </summary>
        int Runners { get; }

        /// <summary>
        /// Gets the number of male runners in the event.
        /// </summary>
        int MaleRunners { get; }

        /// <summary>
        /// Gets the number of female runners in the event.
        /// </summary>
        int FemaleRunners { get; }

        /// <summary>
        /// Gets the number of season bests in the event.
        /// </summary>
        int SBs { get; }

        /// <summary>
        /// Gets the number of personal best in the event.
        /// </summary>
        int PBs { get; }

        /// <summary>
        /// Gets the number of first timers.
        /// </summary>
        int FirstTimers { get; }

        /// <summary>
        /// Gets the Fastest boys list.
        /// </summary>
        List<IAthleteTime> FastestBoys { get; }

        /// <summary>
        /// Gets the Fastest girls list.
        /// </summary>
        List<IAthleteTime> FastestGirls { get; }

        /// <summary>
        /// Gets the name of the fastest boy. Returns empty string if there is no fastest time noted.
        /// </summary>
        string FastestBoy { get; }

        /// <summary>
        /// Gets the time of the fastest boy. Returns 59,59 if there is no fastest time noted.
        /// </summary>
        TimeType FastestBoyTime { get; }

        /// <summary>
        /// Gets the name of the fastest girl. Returns empty string if there is no fastest time noted.
        /// </summary>
        string FastestGirl { get; }

        /// <summary>
        /// Gets the time of the fastest girl. Returns 59,59 if there is no fastest time noted.
        /// </summary>
        TimeType FastestGirlTime { get; }

        /// <summary>
        /// Update the summary data.
        /// </summary>
        /// <param name="MaleRunners">number of male runners</param>
        /// <param name="FemaleRunners">number of female runners</param>
        /// <param name="SBs">number of season bests</param>
        /// <param name="PBs">number of personal bests</param>
        /// <param name="FirstTimers">number of first timers</param>
        void UpdateSummary(
            int MaleRunners,
            int FemaleRunners,
            int SBs,
            int PBs,
            int FirstTimers);

        /// <summary>
        /// Increate the number of <paramref name="type"/> by one.
        /// </summary>
        /// <param name="type">
        /// The type of property to change.
        /// </param>
        void Increment(SummaryPropertiesType type);

        /// <summary>
        /// Sets the fastest boy time
        /// </summary>
        /// <param name="key">athlete's key</param>
        /// <param name="name">athlete's name</param>
        /// <param name="time">athlete's time</param>
        /// <param name="date">date of the time</param>
        void SetFastestBoy(
            int key,
            string name,
            TimeType time,
            DateType date);

        /// <summary>
        /// Sets the fastest girl time
        /// </summary>
        /// <param name="key">athlete's key</param>
        /// <param name="name">athlete's name</param>
        /// <param name="time">athlete's time</param>
        /// <param name="date">date of the time</param>
        void SetFastestGirl(
            int key,
            string name,
            TimeType time,
            DateType date);

        /// <summary>
        /// Remove a record corresponding to the argument data if one exists.
        /// </summary>
        /// <param name="key">athlete key</param>
        /// <param name="name">athlete name</param>
        /// <param name="time">athlete time</param>
        void RemoveFastestBoy(
            int key,
            string name,
            TimeType time);

        /// <summary>
        /// Remove a record corresponding to the argument data if one exists.
        /// </summary>
        /// <param name="key">athlete key</param>
        /// <param name="name">athlete name</param>
        /// <param name="time">athlete time</param>
        void RemoveFastestGirl(
            int key,
            string name,
            TimeType time);

        /// <summary>
        /// Load a new set of fastest collections into the summary.
        /// </summary>
        /// <param name="fastestBoys">fastest boys</param>
        /// <param name="fastestGirls">fastest girls</param>
        void LoadFastest(
            List<IAthleteTime> fastestBoys,
            List<IAthleteTime> fastestGirls);

        /// <summary>
        /// Reset the model.
        /// </summary>
        void Reset();
    }
}