namespace HandicapModel.Admin.IO.TXT
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Messages;
    using CommonHandicapLib.Types;
    using HandicapModel.Interfaces.Admin.IO.TXT;
    using HandicapModel.Interfaces.SeasonModel.EventModel;
    using HandicapModel.SeasonModel.EventModel;
    using CommonMessenger = NynaeveLib.Messenger.Messenger;

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
        /// The root directory.
        /// </summary>
        private string rootDirectory;

        /// <summary>
        /// The path to all the season data.
        /// </summary>
        private string dataPath;

        /// <summary>
        /// Initialises a new instance of the <see cref="RawEventIO"/> class.
        /// </summary>
        /// <param name="logger">application logger</param>
        public RawEventIO(
            IJHcLogger logger)
        {
            this.logger = logger;

            this.rootDirectory = RootIO.LoadRootFile();
            this.dataPath = $"{this.rootDirectory}{Path.DirectorySeparatorChar}{IOPaths.dataPath}{Path.DirectorySeparatorChar}";

            CommonMessenger.Default.Register<ReinitialiseRoot>(this, this.ReinitialiseRoot);
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
                string filePath = this.dataPath + seasonName + Path.DirectorySeparatorChar + eventName + Path.DirectorySeparatorChar + IOPaths.rawEventDataFile;
                using (StreamWriter writer = 
                    new StreamWriter(
                        filePath, 
                        false))
                {
                    foreach (Raw raw in rawData)
                    {
                        writer.WriteLine(
                            raw.RaceNumber.ToString() +
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
                string filePath = this.dataPath + seasonName + Path.DirectorySeparatorChar + eventName + Path.DirectorySeparatorChar + IOPaths.rawEventDataFile;
                if (File.Exists(filePath))
                {
                    using (StreamReader reader = 
                        new StreamReader(
                            this.dataPath))
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
            int order = 0;

            if (splitLine.Count() == 3)
            {
                RaceTimeType time = new RaceTimeType(splitLine[1]);

                if (time.Minutes == 0 && time.Seconds == 0)
                {
                    return null;
                }

                string number = splitLine[0];

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

        /// <summary>
        /// Reinitialise the data path value from the file.
        /// </summary>
        /// <param name="message">reinitialise message</param>
        private void ReinitialiseRoot(ReinitialiseRoot message)
        {
            this.rootDirectory = RootIO.LoadRootFile();
            this.dataPath = $"{this.rootDirectory}{Path.DirectorySeparatorChar}{IOPaths.dataPath}{Path.DirectorySeparatorChar}";
        }
    }
}