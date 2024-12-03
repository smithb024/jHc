namespace CommonHandicapLib.Types
{
    using System;
    using System.Runtime.CompilerServices;
    using CommonLib.Types;

    /// <summary>
    /// Class describing the time taken to complete an event.
    /// </summary>
    /// <remarks>
    /// Includes the flag did not finshed.
    /// </remarks>
    public class RaceTimeType : TimeType
    {
        /// <summary>
        /// The athlete started but did not finish.
        /// </summary>
        private const string dnfDescription = "DNF";

        /// <summary>
        /// The time is not known.
        /// </summary>
        private const string unknownDescription = "UNK";

        /// <summary>
        /// The time is from a relay event.
        /// </summary>
        private const string relayDescription = "Relay";

        /// <summary>
        /// Initialises a new instance of <see cref="RaceTimeType"/> type.
        /// </summary>
        /// <param name="time">new time</param>
        public RaceTimeType(string time)
        {
            if (time.CompareTo(dnfDescription) == 0)
            {
                this.Description = RaceTimeDescription.Dnf;
            }
            else if (time.CompareTo(relayDescription) == 0)
            {
                this.Description = RaceTimeDescription.Relay;
            }
            else if (time.CompareTo(unknownDescription) == 0)
            {
                this.Description = RaceTimeDescription.Unknown;
            }
            else
            {
                this.Description = RaceTimeDescription.Finished;
                this.Update(time);
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="TaceTimeType"/> class.
        /// </summary>
        /// <param name="minutes">new minutes</param>
        /// <param name="seconds">new seconds</param>
        public RaceTimeType(int minutes, int seconds)
          : base(minutes, seconds)
        {
            this.Description = RaceTimeDescription.Finished;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="RaceTimeType"/> type.
        /// </summary>
        /// <param name="description">the description decorator</param>
        public RaceTimeType(
          RaceTimeDescription description)
        {
            this.Description = description;
        }

        /// <summary>
        /// Gets or sets the description of the <see cref="RaceTimeType"/>. 
        /// </summary>
        /// <remarks>
        /// This decorator provides a description of the type of time being recorded. Only the
        /// <see cref="RaceTimeDescription.Finished"/> value would mean that the time is valid. The
        /// other values all refer to non numeric times.
        /// </remarks>
        public RaceTimeDescription Description { get; set; }

        /// <summary>
        /// Add time, if seconds role over to the next  minute increase minute by one and reset seconds.
        /// </summary>
        /// <param name="lhs">time to add (left)</param>
        /// <param name="rhs">time to add (right)</param>
        /// <returns>new time</returns>
        public static RaceTimeType operator +(RaceTimeType lhs,
                                              RaceTimeType rhs)
        {
            if (lhs.Description != RaceTimeDescription.Finished ||
                rhs.Description != RaceTimeDescription.Finished)
            {
                return new RaceTimeType(RaceTimeDescription.Unknown);
            }

            int productMinutes = lhs.Minutes + rhs.Minutes;
            int productSeconds = lhs.Seconds + rhs.Seconds;

            if (productSeconds >= 60)
            {
                ++productMinutes;
                productSeconds = productSeconds - 60;
            }

            return new RaceTimeType(productMinutes, productSeconds);
        }

        /// <summary>
        /// Subtract time, if seconds role over to the next minute decrease minute by one and reset seconds.
        /// </summary>
        /// <param name="lhs">distance to add (left)</param>
        /// <param name="rhs">distance to add (right)</param>
        /// <returns>new distance</returns>
        public static RaceTimeType operator -(RaceTimeType lhs,
                                              RaceTimeType rhs)
        {
            if (lhs.Description != RaceTimeDescription.Finished ||
                rhs.Description != RaceTimeDescription.Finished)
            {
                return new RaceTimeType(RaceTimeDescription.Unknown);
            }

            if (rhs.Minutes > lhs.Minutes ||
              (rhs.Minutes == lhs.Minutes && rhs.Seconds > lhs.Seconds))
            {
                return new RaceTimeType(0, 0);
            }
            else
            {
                int productMinutes = lhs.Minutes - rhs.Minutes;
                int productSeconds = lhs.Seconds - rhs.Seconds;

                if (productSeconds < 0)
                {
                    --productMinutes;
                    productSeconds = productSeconds + 60;
                }

                return new RaceTimeType(productMinutes, productSeconds);
            }
        }

        /// <summary>
        /// Overrides the equals operator to check the minutes and seconds match
        /// </summary>
        /// <param name="obj">object to compare</param>
        /// <returns>true or false</returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // Ensure that the obj can be cast to race time type.
            RaceTimeType time = obj as RaceTimeType;
            if ((System.Object)time == null)
            {
                return false;
            }

            return
              (time.Description == this.Description && this.Description != RaceTimeDescription.Finished) ||
              (time.Minutes == this.Minutes && time.Seconds == this.Seconds && this.Description == RaceTimeDescription.Finished);
        }

        /// <summary>
        /// Overrides the get hash code method
        /// </summary>
        /// <returns>hash code</returns>
        public override int GetHashCode()
        {
            return this.Minutes ^ this.Seconds;
        }

        /// <summary>
        /// equality operator
        /// </summary>
        /// <param name="lhs">left hand side</param>
        /// <param name="rhs">right hand side</param>
        /// <returns>equality flag</returns>
        public static bool operator ==(RaceTimeType lhs,
                                       RaceTimeType rhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                return object.ReferenceEquals(rhs, null);
            }

            if (object.ReferenceEquals(rhs, null))
            {
                return false;
            }

            return
              (rhs.Description == lhs.Description && rhs.Description != RaceTimeDescription.Finished) ||
              (lhs.Minutes == rhs.Minutes && lhs.Seconds == rhs.Seconds && rhs.Description == RaceTimeDescription.Finished);
        }

        /// <summary>
        /// inequality operator
        /// </summary>
        /// <param name="lhs">left hand side</param>
        /// <param name="rhs">right hand side</param>
        /// <returns>inequality flag</returns>
        public static bool operator !=(RaceTimeType lhs,
                                       RaceTimeType rhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                return !object.ReferenceEquals(rhs, null);
            }

            if (object.ReferenceEquals(rhs, null))
            {
                return true;
            }

            return (lhs.Minutes != rhs.Minutes ||
                lhs.Seconds != rhs.Seconds ||
                rhs.Description != lhs.Description);
        }

        /// <summary>
        /// Checks to see if one is larger than the other. It returns false if either is DNF.
        /// </summary>
        /// <param name="lhs">left hand side</param>
        /// <param name="rhs">right hand side</param>
        /// <returns>size indicator flag</returns>
        public static bool operator >(RaceTimeType lhs,
                                      RaceTimeType rhs)
        {
            if (lhs.Description != RaceTimeDescription.Finished ||
                rhs.Description != RaceTimeDescription.Finished)
            {
                return false;
            }

            if (lhs.Minutes > rhs.Minutes)
            {
                return true;
            }
            else if (lhs.Minutes == rhs.Minutes && lhs.Seconds > rhs.Seconds)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks to see if one is smaller than the other. It returns false if either is DNF.
        /// </summary>
        /// <param name="lhs">left hand side</param>
        /// <param name="rhs">right hand side</param>
        /// <returns>size indicator flag</returns>
        public static bool operator <(RaceTimeType lhs,
                                      RaceTimeType rhs)
        {
            if (lhs.Description != RaceTimeDescription.Finished ||
                rhs.Description != RaceTimeDescription.Finished)
            {
                return false;
            }

            if (lhs.Minutes < rhs.Minutes)
            {
                return true;
            }
            else if (lhs.Minutes == rhs.Minutes && lhs.Seconds < rhs.Seconds)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Divide a time by and integer, return DNF if time is DNF.
        /// </summary>
        /// <param name="lhs">distance to add (left)</param>
        /// <param name="rhs">distance to add (right)</param>
        /// <returns>new distance</returns>
        public static RaceTimeType operator /(RaceTimeType lhs,
                                              int rhs)
        {
            if (lhs.Description != RaceTimeDescription.Finished)
            {
                return new RaceTimeType(lhs.Description);
            }

            int minutesInt = 0;
            int secondsInt = 0;

            double step = lhs.Minutes + (lhs.Seconds / 60.0);
            double dividedTime = step / rhs;

            try
            {
                minutesInt = (int)Math.Truncate(dividedTime);
                double seconds = (dividedTime - Math.Truncate(dividedTime)) * 60;
                secondsInt = (int)Math.Round(seconds, MidpointRounding.AwayFromZero);
            }
            catch
            {
                return new RaceTimeType(0, 0);
            }

            return new RaceTimeType(minutesInt, secondsInt);
        }

        /// <summary>
        /// Return string of format "mm:ss"
        /// </summary>
        /// <returns>new string</returns>
        public override string ToString()
        {
            if (this.Description == RaceTimeDescription.Dnf)
            {
                return dnfDescription;
            }
            else if (this.Description == RaceTimeDescription.Relay)
            {
                return relayDescription;
            }
            else if (this.Description == RaceTimeDescription.Unknown)
            {
                return unknownDescription;
            }
            else
            {
                return this.Minutes.ToString() + this.separator + this.Seconds.ToString("00");
            }
        }
    }
}