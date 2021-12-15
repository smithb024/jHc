namespace HandicapModel.Admin.IO.TXT
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Types;
    using HandicapModel.Interfaces.Admin.IO.TXT;
    using HandicapModel.Interfaces.SeasonModel.EventModel;
    using HandicapModel.SeasonModel.EventModel;

    /// <summary>
    /// Raw event specific IO.
    /// </summary>
    public class RawEventIO : IRawEventIo
    {
        /// <summary>
        /// data splitter character
        /// </summary>
        private static char dataSplitter = '|';

        /// <summary>
        /// Application logger.
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="RawEventIO"/> class.
        /// </summary>
        /// <param name="logger">application logger</param>
        public RawEventIO(
            IJHcLogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Saves the raw results data for the indicated event
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <param name="eventName">event name</param>
        /// <param name="rawData">raw data to save</param>
        /// <returns></returns>
        public bool SaveRawEventData(
            string seasonName,
            string eventName,
            List<IRaw> rawData)
        {
            bool success = true;

            try
            {
                using (StreamWriter writer = new StreamWriter(RootPath.DataPath +
                                                              seasonName +
                                                              Path.DirectorySeparatorChar +
                                                              eventName +
                                                              Path.DirectorySeparatorChar +
                                                              IOPaths.rawEventDataFile, false))
                {
                    foreach (Raw raw in rawData)
                    {
                        writer.WriteLine(raw.RaceNumber.ToString() +
                                         dataSplitter +
                                         raw.TotalTime.ToString() +
                                         dataSplitter +
                                         raw.Order.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error, failed to save current event raw results data: " + ex.ToString());
                success = false;
            }

            return success;
        }

        /// <summary>
        /// Loads the raw event data for the indicated event.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <param name="eventName">event name</param>
        /// <returns>raw event results data</returns>
        public List<IRaw> LoadRawEventData(
            string seasonName,
            string eventName)
        {
            List<IRaw> rawData = new List<IRaw>();
            string line;

            try
            {
                if (File.Exists(RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + eventName + Path.DirectorySeparatorChar + IOPaths.rawEventDataFile))
                {
                    using (StreamReader reader = new StreamReader(RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + eventName + Path.DirectorySeparatorChar + IOPaths.rawEventDataFile))
                    {
                        while ((line = reader.ReadLine()) != null)
                        {
                            IRaw converted = this.ConvertLine(line);

                            if (converted != null)
                            {
                                rawData.Add(converted);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error, failed to read current event raw results data: " + ex.ToString());
            }

            return rawData;
        }

        /// <summary>
        /// Converts a line from the file to a raw piece of data. If there are any problems
        /// then a null value is returned.
        /// </summary>
        /// <param name="line">line read from file</param>
        /// <returns>raw piece of data</returns>
        private IRaw ConvertLine(string line)
        {
            string[] splitLine = line.Split(dataSplitter);
            RaceTimeType time = null;
            int order = 0;
            string number = string.Empty;

            if (splitLine.Count() == 3)
            {
                time = new RaceTimeType(splitLine[1]);

                if (time.Minutes == 0 && time.Seconds == 0)
                {
                    return null;
                }

                number = splitLine[0];

                if (!int.TryParse(splitLine[2], out order))
                {
                    return null;
                }

                return new Raw(number, time, order);
            }
            else
            {
                return null;
            }
        }
    }
}