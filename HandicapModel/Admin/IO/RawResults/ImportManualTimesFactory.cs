namespace HandicapModel.Admin.IO.RawResults
{
    using System.Collections.Generic;

    using CommonHandicapLib.Types;
    using HandicapModel.Interfaces.Admin.IO.TXT;

    /// <summary>
    /// Factory class used to read a manually created list of times. 
    /// </summary>
    public static class ImportManualTimesFactory
    {
        /// <summary>
        /// Import the times from <paramref name="fileName"/>
        /// </summary>
        /// <param name="fileName">file containing times</param>
        /// <param name="commonIo">common IO manager</param>
        /// <returns>collection of race times.</returns>
        public static List<RaceTimeType> Import(
            string fileName,
            ICommonIo commonIo)
        {
            List<RaceTimeType> rawImportedTimes = new List<RaceTimeType>();
            List<string> rawTimes = commonIo.ReadFile(fileName);

            if (rawTimes == null ||
              rawTimes.Count == 0)
            {
                return rawImportedTimes;
            }

            foreach (string rawTime in rawTimes)
            {
                RaceTimeType time = new RaceTimeType(rawTime);
                rawImportedTimes.Add(time);
            }

            return rawImportedTimes;
        }
    }
}