namespace CommonLib.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TimeType
    {
        private int minutes = 59;
        private int seconds = 59;
        protected char separator = ':';
        protected char alternativeSeparator = '.';

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>TimeType</name>
        /// <date>17/01/15</date>
        /// <summary>
        ///   Creates a new instance of the TimeType class
        /// </summary>
        /// <param name="minutes">minutes</param>
        /// <param name="seconds">seconds</param>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public TimeType(int minutes,
                        int seconds)
        {
            Minutes = minutes;
            Seconds = seconds;
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>TimeType</name>
        /// <date>17/01/15</date>
        /// <summary>
        ///   Creates a new instance of the TimeType class
        /// </summary>
        /// <param name="time">time</param>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public TimeType(string time)
        {
            Update(time);
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>TimeType</name>
        /// <date>17/01/15</date>
        /// <summary>
        ///   Creates a new instance of the TimeType class
        /// </summary>
        /// <param name="time">time</param>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public TimeType() : this(59, 59)
        {
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <summary>
        /// Gets the minutes.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public int Minutes
        {
            get { return minutes; }
            private set { minutes = value; }
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <summary>
        /// Gets the seconds.
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public int Seconds
        {
            get { return seconds; }
            private set { seconds = value; }
        }

        /// <summary>
        /// Gets the time as a number of seconds.
        /// </summary>
        public int TotalSeconds => 60 * this.Minutes + this.Seconds;

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>Add</name>
        /// <date>17/01/15</date>
        /// <summary>
        /// Add time, if seconds role over to the next  minute increase 
        /// minute by one and reset seconds.
        /// </summary>
        /// <param name="lhs">time to add (left)</param>
        /// <param name="rhs">time to add (right)</param>
        /// <returns>new time</returns>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public static TimeType operator +(TimeType lhs,
                                          TimeType rhs)
        {
            int productMinutes = lhs.Minutes + rhs.Minutes;
            int productSeconds = lhs.Seconds + rhs.Seconds;

            if (productSeconds >= 60)
            {
                ++productMinutes;
                productSeconds = productSeconds - 60;
            }

            return new TimeType(productMinutes, productSeconds);
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>Subtract</name>
        /// <date>17/01/15</date>
        /// <summary>
        /// Subtract time, if seconds role over to the next minute decrease
        /// minute by one and reset seconds.
        /// </summary>
        /// <param name="lhs">distance to add (left)</param>
        /// <param name="rhs">distance to add (right)</param>
        /// <returns>new distance</returns>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public static TimeType operator -(TimeType lhs,
                                          TimeType rhs)
        {
            if (rhs.Minutes > lhs.Minutes ||
              (rhs.Minutes == lhs.Minutes && rhs.Seconds > lhs.Seconds))
            {
                // TODO Need to raise an exception.
                //Logger logger = Logger.GetInstance();
                //logger.WriteLog("TRACE: MilesChains Error: tried to subtract " +
                //                 rhs.ToString() +
                //                 "from" +
                //                 lhs.ToString());

                return new TimeType(0, 0);
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

                return new TimeType(productMinutes, productSeconds);
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

            // Ensure that the obj can be cast to time type.
            TimeType time = obj as TimeType;
            if ((System.Object)time == null)
            {
                return false;
            }

            return time.Minutes == Minutes && time.Seconds == Seconds;
        }

        /// <summary>
        /// Overrides the get hash code method
        /// </summary>
        /// <returns>hash code</returns>
        public override int GetHashCode()
        {
            return Minutes ^ Seconds;
        }

        /// <summary>
        /// equality operator
        /// </summary>
        /// <param name="lhs">left hand side</param>
        /// <param name="rhs">right hand side</param>
        /// <returns>equality flag</returns>
        public static bool operator ==(TimeType lhs,
                                       TimeType rhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                return object.ReferenceEquals(rhs, null);
            }

            if (object.ReferenceEquals(rhs, null))
            {
                return false;
            }

            return (lhs.Minutes == rhs.Minutes && lhs.Seconds == rhs.Seconds);
        }

        /// <summary>
        /// inequality operator
        /// </summary>
        /// <param name="lhs">left hand side</param>
        /// <param name="rhs">right hand side</param>
        /// <returns>inequality flag</returns>
        public static bool operator !=(TimeType lhs,
                                       TimeType rhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                return !object.ReferenceEquals(rhs, null);
            }

            if (object.ReferenceEquals(rhs, null))
            {
                return true;
            }

            return (lhs.Minutes != rhs.Minutes || lhs.Seconds != rhs.Seconds);
        }

        public static bool operator >(TimeType lhs,
                                      TimeType rhs)
        {
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

        public static bool operator <(TimeType lhs,
                                      TimeType rhs)
        {
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

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>Divide</name>
        /// <date>17/01/15</date>
        /// <summary>
        /// Subtract time, if seconds role over to the next minute decrease
        /// minute by one and reset seconds.
        /// </summary>
        /// <param name="lhs">distance to add (left)</param>
        /// <param name="rhs">distance to add (right)</param>
        /// <returns>new distance</returns>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public static TimeType operator /(TimeType lhs,
                                          int rhs)
        {
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
            catch (Exception ex)
            {
                // TODO Need to raise an exception.
                //Logger logger = Logger.GetInstance();
                //logger.WriteLog("Error Dividing " + lhs + " Error: " + ex.ToString());
                string exception = ex.ToString();
                return new TimeType(0, 0);
            }

            return new TimeType(minutesInt, secondsInt);
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>Round</name>
        /// <date>17/01/15</date>
        /// <summary>
        /// Round the time to the nearest 30 seconds
        /// </summary>
        /// <returns>rounded time</returns>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public TimeType Round()
        {
            if (Seconds < 15)
            {
                return new TimeType(Minutes, 0);
            }
            else if (Seconds < 45)
            {
                return new TimeType(Minutes, 30);
            }
            else
            {
                return new TimeType(Minutes + 1, 0);
            }
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>ToString</name>
        /// <date>17/01/15</date>
        /// <summary>
        /// Return string of format "mm:ss"
        /// </summary>
        /// <returns>new string</returns>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public override string ToString()
        {
            return Minutes.ToString() +
                   separator +
                   Seconds.ToString("00");
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>Update</name>
        /// <date>17/01/15</date>
        /// <summary>
        /// Return string of format "mm-cc"
        /// </summary>
        /// <returns>new string</returns>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        protected void Update(string time)
        {
            string[] cells = null;

            if (time.Contains(separator))
            {
                cells = time.Split(separator);
            }
            else
            {
                cells = time.Split(alternativeSeparator);
            }

            int minutesInt = 0;
            int secondsInt = 0;
            int minutesIndex;
            int secondsIndex;

            switch (cells.Length)
            {
                case 2:
                    minutesIndex = 0;
                    secondsIndex = 1;
                    break;
                case 3:
                    minutesIndex = 1;
                    secondsIndex = 2;
                    break;
                default:
                    return;
            }

            if (!int.TryParse(cells[minutesIndex], out minutesInt))
            {
                // TODO Need to raise an exception.
                //Logger logger = Logger.GetInstance();
                //logger.WriteLog("Failed to convert minues " + cells[0] + "to int");
            }
            else
            {
                if (!int.TryParse(cells[secondsIndex], out secondsInt))
                {
                    // TODO Need to raise an exception.
                    //Logger logger = Logger.GetInstance();
                    //logger.WriteLog("Failed to convert seconds " + cells[0] + "to int");
                }
                else
                {
                    Minutes = minutesInt;
                    Seconds = secondsInt;
                }
            }
        }
    }
}