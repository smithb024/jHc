namespace HandicapModel.Admin.IO.RawResults
{
    using System.Collections.Generic;
    using System.Linq;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Helpers.EventRawResults;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.Types;
    using HandicapModel.Interfaces.Admin.IO.TXT;
    using GalaSoft.MvvmLight.Messaging;

    
    /// <summary>
    /// Factory class used to import the positions from a file created by the OPN200 barcode reader.
    /// </summary>
    public static class ImportParkrunPositionFactory
    {
        /// <summary>
        /// String which marks the first line of the file. This is not a data line.
        /// </summary>
        const string StartString = "Start of File";

        /// <summary>
        /// Indicates the number of separate cells in a valid . It provides a check that the
        /// time string has been split up correctly.
        /// </summary>
        const int NumberOfSectionsInAValdEntry = 3;

        /// <summary>
        /// Import the times from <paramref name="fileName"/>
        /// </summary>
        /// <param name="fileName">file containing times</param>
        /// <param name="commonIo">common Io manager</param>
        /// <param name="logger">program logger</param>
        /// <returns>collection of race times.</returns>
        public static List<RawPositionResults> Import(
            string fileName,
            ICommonIo commonIo,
            IJHcLogger logger)
        {
            List<RawPositionResults> rawImportedPositions = new List<RawPositionResults>();
            List<string> rawPositions = commonIo.ReadFile(fileName);

            foreach (string positionAthleteData in rawPositions)
            {
                char splitChar = ',';

                string[] resultLine = positionAthleteData.Split(splitChar);

                // Ensure this line is not the first one and its valid.
                if (string.Equals(resultLine[0], StartString) ||
                    resultLine.Length != ImportParkrunPositionFactory.NumberOfSectionsInAValdEntry)
                {
                    continue;
                }

                // Ensure that the interesting data is not empty.
                if (string.IsNullOrEmpty(resultLine[0]) ||
                    string.IsNullOrEmpty(resultLine[1]) ||
                    !ResultsDecoder.IsPositionValue(resultLine[1]))
                {
                    continue;
                }

                int? position =
                    ResultsDecoder.ConvertPositionValue(
                        resultLine[1]);

                RawPositionResults result =
                    new RawPositionResults(
                        resultLine[0],
                        (int)position);

                rawImportedPositions.Add(result);
            }

            rawImportedPositions =
                rawImportedPositions
                .OrderBy(position => position.Position)
                .ToList();

            return rawImportedPositions;
        }

        /// <summary>
        /// Takes a result and populate the position or race number depending on what it is.
        /// </summary>
        /// <param name="result">position / athlete result</param>
        /// <param name="position">position of result</param>
        /// <param name="raceNumber">race number of result</param>
        private static void UpdatePositionAthleteData(
            string result,
            ref int? position,
            ref string raceNumber)
        {
            if (ResultsDecoder.IsPositionValue(result))
            {
                position = ResultsDecoder.ConvertPositionValue(result);
            }
            else
            {
                raceNumber = result;
            }
        }

    }
}