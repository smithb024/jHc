namespace HandicapModel.Admin.IO.RawResults
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using CommonHandicapLib.Types;
    using TXT;

    /// <summary>
    /// Factory class used to import the times from a file created by the 610P Stopwatch.
    /// </summary>
    public static class Import610PFactory
    {
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
                string rawTime = Import610PFactory.TranslateRawTime(line);

                List<int> timeDetails = Import610PFactory.TranslateTimeString(rawTime);

                if (timeDetails.Count != 4)
                {
                    continue;
                }

                rawImportedTimes.Add(
                  Import610PFactory.CalculateTime(
                    timeDetails[1],
                    timeDetails[2],
                    timeDetails[3]));
            }

            return rawImportedTimes;
        }

        /// <summary>
        /// Take a line which has been read from the time file. Split using the tab character. The raw
        /// file contains a large number of lines which are not interesting to the import, return an 
        /// empty string for these, otherwise return the section of line which contains the time.
        /// </summary>
        /// <param name="line"></param>
        /// <returns>translated time</returns>
        private static string TranslateRawTime(string line)
        {
            char splitChar = '	';

            string[] resultLine = line.Split(splitChar);

            return resultLine.Length == 4 ?
              resultLine[3] :
              string.Empty;
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
            List<int> timeCollection = new List<int>();

            string[] digits = Regex.Split(rawTime, @"\D+");

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
          int seconds,
          int hundreths)
        {
            RaceTimeType time =
              new RaceTimeType(
                minutes,
                seconds);

            if (hundreths >= 50)
            {
                RaceTimeType second =
                  new RaceTimeType(
                    0,
                    1);

                time = time + second;
            }

            return time;
        }
    }
}