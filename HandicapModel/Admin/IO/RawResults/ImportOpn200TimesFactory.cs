namespace HandicapModel.Admin.IO.RawResults
{
    using System.Collections.Generic;

    using CommonHandicapLib.Types;
    using HandicapModel.Interfaces.Admin.IO.TXT;

    /// <summary>
    /// Factory class used to import the times from a file created by the OPN200 barcode reader.
    /// </summary>
    public static class ImportOpn200TimesFactory
    {
        /// <summary>
        /// Import the times from <paramref name="fileName"/>
        /// </summary>
        /// <param name="fileName">file containing times</param>
        /// <param name="commonIo">common Io manager</param>
        /// <returns>collection of race times.</returns>
        public static List<RaceTimeType> Import(
            string fileName,
            ICommonIo commonIo)
        {
            List<RaceTimeType> rawImportedTimes = new List<RaceTimeType>();
            List<string> rawTimes = commonIo.ReadFile(fileName);

            if (rawTimes == null ||
              rawTimes.Count < 3)
            {
                return rawImportedTimes;
            }

            char splitChar = ',';

            for (int index = 2; index < rawTimes.Count; ++index)
            {
                string[] resultLine = rawTimes[index].Split(splitChar);
                if (resultLine.Length == 3)
                {
                    RaceTimeType time = new RaceTimeType(resultLine[2]);
                    rawImportedTimes.Add(time);
                }
            }

            return rawImportedTimes;
        }
    }
}