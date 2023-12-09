namespace CommonHandicapLib
{
    /// <summary>
    /// Factory class which is used to guess a handicap based on a given age.
    /// </summary>
    public static class HandicapConverter
    {
        /// <summary>
        /// Convert an age to a handicap.
        /// </summary>
        /// <param name="athleteAge">The age to convert</param>
        /// <returns>The estimated handicap</returns>
        public static string ConvertAgeToHandicap(string athleteAge)
        {
            int age;
            int handicap = 0;

            if (int.TryParse(athleteAge, out age))
            {
                if (age < 6)
                {
                    handicap = 0;
                }
                else if (age < 9)
                {
                    handicap = 0;
                }
                else if (age < 12)
                {
                    handicap = 3;
                }
                else if (age < 15)
                {
                    handicap = 5;
                }
                else
                {
                    handicap = 7;
                }
            }

            return handicap.ToString();
        }
    }
}