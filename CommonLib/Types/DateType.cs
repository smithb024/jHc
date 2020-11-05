namespace CommonLib.Types
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DateType
    {
        private int m_day = 1;
        private int m_month = 1;
        private int m_year = 1;
        private char m_separator = '-';

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>DateType</name>
        /// <date>03/04/15</date>
        /// <summary>
        ///   Creates a new instance of the DateType class
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public DateType()
        {
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>DateType</name>
        /// <date>03/04/15</date>
        /// <summary>
        ///   Creates a new instance of the DateType class
        /// </summary>
        /// <param name="day">day value</param>
        /// <param name="month">month value</param>
        /// <param name="year">year value</param>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public DateType(int day,
                        int month,
                        int year)
        {
            Day = day;
            Month = month;
            Year = year;
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <name>DateType</name>
        /// <date>03/04/15</date>
        /// <summary>
        ///   Creates a new instance of the DateType class
        /// </summary>
        /// <param name="day">day value</param>
        /// <param name="month">month value</param>
        /// <param name="year">year value</param>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public DateType(string date)
        {
            string[] dateArray = date.Split(m_separator);
            int day = 0;
            int month = 0;
            int year = 0;

            if (dateArray.Count() == 3)
            {
                if (int.TryParse(dateArray[0], out day))
                {
                    if (int.TryParse(dateArray[1], out month))
                    {
                        if (int.TryParse(dateArray[2], out year))
                        {
                            Day = day;
                            Month = month;
                            Year = year;
                        }
                    }
                }
            }
            Day = day;
            Month = month;
            Year = year;
        }


        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <summary>
        /// Gets and sets the day
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public int Day
        {
            get { return m_day; }
            set { m_day = value; }
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <summary>
        /// Gets and sets the year
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public int Month
        {
            get { return m_month; }
            set { m_month = value; }
        }

        /// ---------- ---------- ---------- ---------- ---------- ----------
        /// <summary>
        /// Gets and sets the year
        /// </summary>
        /// ---------- ---------- ---------- ---------- ---------- ----------
        public int Year
        {
            get { return m_year; }
            set { m_year = value; }
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
            return Day.ToString() +
                   m_separator +
                   Month.ToString() +
                   m_separator +
                   Year.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // Ensure that the obj can be cast to time type.
            DateType time = obj as DateType;
            if ((System.Object)time == null)
            {
                return false;
            }

            return (time.Year == Year && time.Month == Month && time.Day == Day);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator ==(DateType lhs, DateType rhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                return object.ReferenceEquals(rhs, null);
            }

            if (object.ReferenceEquals(rhs, null))
            {
                return false;
            }

            return (lhs.Year == rhs.Year && lhs.Month == rhs.Month && lhs.Day == rhs.Day);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator !=(DateType lhs, DateType rhs)
        {
            if (object.ReferenceEquals(lhs, null))
            {
                return !object.ReferenceEquals(rhs, null);
            }

            if (object.ReferenceEquals(rhs, null))
            {
                return true;
            }

            return (lhs.Year != rhs.Year && lhs.Month != rhs.Month && lhs.Day != rhs.Day);
        }

        /// <summary>
        /// Greater than opeator
        /// </summary>
        /// <param name="lhs">left hand side</param>
        /// <param name="rhs">right hand side</param>
        /// <returns>
        /// Indictes whether the <paramref name="lhs"/> is greater than the 
        /// <paramref name="rhs"/>.
        /// </returns>
        public static bool operator >(DateType lhs,
                                      DateType rhs)
        {
            if (lhs.Year > rhs.Year)
            {
                return true;
            }
            else if (lhs.Year == rhs.Year && lhs.Month > rhs.Month)
            {
                return true;
            }
            else if (lhs.Year == rhs.Year && lhs.Month == rhs.Month && lhs.Day > rhs.Day)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Less than operator
        /// </summary>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        /// <returns></returns>
        public static bool operator <(DateType lhs,
                                      DateType rhs)
        {
            if (lhs.Year < rhs.Year)
            {
                return true;
            }
            else if (lhs.Year == rhs.Year && lhs.Month < rhs.Month)
            {
                return true;
            }
            else if (lhs.Year == rhs.Year && lhs.Month == rhs.Month && lhs.Day < rhs.Day)
            {
                return true;
            }

            return false;
        }
    }
}
