namespace CommonHandicapLib.Types
{
    using System;

    /// <summary>
    /// Class which stores a date of birth
    /// </summary>
    public class DateOfBirth
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="DateOfBirth"/> class
        /// </summary>
        /// <param name="birthYear">birth year</param>
        /// <param name="birthMonth">birth month</param>
        /// <param name="birthDay">birth day</param>
        public DateOfBirth(
          string birthYear,
          string birthMonth,
          string birthDay)
        {
            int year;
            int month;
            int day;
            DateTime dateOfBirth;

            if (string.IsNullOrEmpty(birthYear) ||
              string.IsNullOrEmpty(birthMonth) ||
              string.IsNullOrEmpty(birthDay))
            {
                this.SetDefaultValues();
                return;
            }

            if (!int.TryParse(birthDay, out day) ||
              !int.TryParse(birthMonth, out month) ||
              !int.TryParse(birthYear, out year))
            {
                this.SetDefaultValues();
                return;
            }

            dateOfBirth = new DateTime(year, month, day);

            this.BirthYear = birthYear;
            this.BirthMonth = birthMonth;
            this.BirthDay = birthDay;
            this.BirthDate = dateOfBirth;
        }

        /// <summary>
        /// Gets the birth year.
        /// </summary>
        public string BirthYear { get; private set; }

        /// <summary>
        /// Gets the birth month.
        /// </summary>
        public string BirthMonth { get; private set; }

        /// <summary>
        /// Gets the birth day.
        /// </summary>
        public string BirthDay { get; private set; }

        /// <summary>
        /// Gets the birth date
        /// </summary>
        public DateTime? BirthDate { get; private set; }

        /// <summary>
        /// Gets the current age.
        /// </summary>
        public int? Age
        {
            get
            {
                if (this.BirthDate == null)
                {
                    return null;
                }

                TimeSpan age = DateTime.Now - (DateTime)this.BirthDate;
                double ageYears = age.Days / 365.25;

                return (int)ageYears;
            }
        }

        /// <summary>
        /// Reset the class.
        /// </summary>
        private void SetDefaultValues()
        {
            this.BirthYear = string.Empty;
            this.BirthMonth = string.Empty;
            this.BirthDay = string.Empty;

            this.BirthDate = null;
        }
    }
}