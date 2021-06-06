namespace HandicapModel.Admin.IO.RawResults
{
    using System.Collections.Generic;

    using CommonHandicapLib.Types;
    using TXT;

    /// <summary>
    /// Factory class used to import the times from a file created by the parkrun timer smartphone
    /// application.
    /// </summary>
    public static class ImportParkrunTimerFactory
    {
        /// <summary>
        /// String which marks the first line of the file. This is not a data line.
        /// </summary>
        const string StartString = "STARTOFEVENT";

        /// <summary>
        /// String which marks the last line of the file. This is not a data line.
        /// </summary>
        const string TerminationString = "ENDOFEVENT";

        /// <summary>
        /// Indicates the number of separate numbers in a time string. It provides a check that the
        /// time string has been split up correctly.
        /// </summary>
        const int NumberOfSectionsInATime = 3;

        /// <summary>
        /// Indicates the number of separate cells in a valid . It provides a check that the
        /// time string has been split up correctly.
        /// </summary>
        const int NumberOfSectionsInAValdEntry = 3;

        /// <summary>
        /// Import the times from <paramref name="fileName"/>
        /// </summary>
        /// <param name="fileName">file containing times</param>
        /// <returns>collection of race times.</returns>
        public static List<RaceTimeType> Import(string fileName)
        {
            List<RaceTimeType> rawImportedTimes = new List<RaceTimeType>();
            List<string> rawTimes = CommonIO.ReadFile(fileName);

            if (rawTimes == null ||
              rawTimes.Count == 0)
            {
                return rawImportedTimes;
            }

            foreach (string line in rawTimes)
            {
                string rawTime = ImportParkrunTimerFactory.TranslateRawTime(line);

                List<int> timeDetails = ImportParkrunTimerFactory.TranslateTimeString(rawTime);

                if (timeDetails.Count != ImportParkrunTimerFactory.NumberOfSectionsInATime)
                {
                    continue;
                }

                rawImportedTimes.Add(
                  ImportParkrunTimerFactory.CalculateTime(
                    timeDetails[1],
                    timeDetails[2]));
            }

            return rawImportedTimes;
        }

        /// <summary>
        /// Take a line which has been read from the time file. This is a CSV files, so split 
        /// using the comma character. The raw file contains some lines which are not interesting 
        /// to the import, return an empty string for these, otherwise return the section of line 
        /// which contains the time.
        /// </summary>
        /// <param name="line">line from the file</param>
        /// <returns>translated time</returns>
        private static string TranslateRawTime(string line)
        {
            char splitChar = ',';

            string[] resultLine = line.Split(splitChar);

            if (string.Equals(resultLine[0], StartString) ||
                string.Equals(resultLine[0], TerminationString) ||
                resultLine.Length != ImportParkrunTimerFactory.NumberOfSectionsInAValdEntry)
            {
                return string.Empty;
            }

            return resultLine[2];
        }

        /// <summary>
        /// Split the raw time string up into the consistuent numbers.
        /// </summary>
        /// <remarks>
        /// A typical string is 0:06'28.15.
        /// </remarks>
        /// <param name="rawTime">raw time from the file</param>
        /// <returns>a collection of each of the integers from the time string</returns>
        private static List<int> TranslateTimeString(string rawTime)
        {
            if (string.IsNullOrEmpty(rawTime))
            {
                return new List<int>();
            }

            List<int> timeCollection = new List<int>();

            char splitChar = ':';

            string[] digits = rawTime.Split(splitChar);
            
            foreach (string value in digits)
            {
                int number;
                if (int.TryParse(value, out number))
                {
                    timeCollection.Add(number);
                }
            }

            return timeCollection;
        }

        /// <summary>
        /// Determine the race time. Manage rounding to the nearest second.
        /// </summary>
        /// <param name="minutes">time minutes</param>
        /// <param name="seconds">seconds minutes</param>
        /// <param name="hundreths">hundreths of a second</param>
        /// <returns>race time</returns>
        private static RaceTimeType CalculateTime(
          int minutes,
          int seconds)
        {
            RaceTimeType time =
              new RaceTimeType(
                minutes,
                seconds);

            return time;
        }
    }
}