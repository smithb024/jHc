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
    public static class ImportOpn200PositionFactory
    {
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
            List<List<string>> rawPositions = commonIo.ReadPairedStringListFomFile(fileName);

            foreach (List<string> positionAthleteData in rawPositions)
            {
                // Ensure 2 results present
                if (positionAthleteData.Count == 2)
                {
                    int? position = null;
                    string raceNumber = null;

                    string result1 = ResultsDecoder.OpnScannerResultsBarcode(positionAthleteData[0]);
                    string result2 = ResultsDecoder.OpnScannerResultsBarcode(positionAthleteData[1]);

                    UpdatePositionAthleteData(
                        result1,
                        ref position,
                        ref raceNumber);
                    UpdatePositionAthleteData(
                        result2,
                        ref position,
                        ref raceNumber);

                    if (position != null && raceNumber != null)
                    {
                        RawPositionResults results =
                            new RawPositionResults(
                                raceNumber,
                                (int)position);
                        rawImportedPositions.Add(results);
                    }
                    else
                    {
                        string errorString = $"Can't decode {positionAthleteData[0]}/{positionAthleteData[1]}";
                        HandicapProgressMessage message =
                            new HandicapProgressMessage(
                                errorString);

                        Messenger.Default.Send(
                            new HandicapProgressMessage(
                                errorString));

                        logger.WriteLog(errorString);
                    }
                }
                else
                {
                    string errorString = "Please check results, result/barcode pair invalid";
                    Messenger.Default.Send(
                        new HandicapProgressMessage(
                            errorString));
                    logger.WriteLog(errorString);
                }
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