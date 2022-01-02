namespace CommonHandicapLib.XML
{
    using System;
    using System.IO;
    using System.Xml.Linq;
    using CommonHandicapLib.Interfaces;
    using CommonHandicapLib.Interfaces.XML;
    using CommonHandicapLib.Types;

    /// <summary>
    /// Reads and writes the series configuration data to a file.
    /// </summary>
    public class SeriesConfigReader : ISeriesConfigReader
    {
        private const string rootElement = "SeriesConfiguration";
        private const string numberElement = "Number";

        private const string prefixAttribute = "Prefix";
        private const string allPositionsAttribute = "MF123";

        /// <summary>
        /// The application logger.
        /// </summary>
        private readonly IJHcLogger logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="SeriesConfigReader"/> class.
        /// </summary>
        /// <param name="logger"></param>
        public SeriesConfigReader(IJHcLogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Save the configuration data.
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <param name="configData">configuration data</param>
        /// <returns>success flag</returns>
        public bool SaveSeriesConfigData(
          string fileName,
          SeriesConfigType configData)
        {
            bool success = true;

            try
            {
                XDocument writer = new XDocument(
                 new XDeclaration("1.0", "uft-8", "yes"),
                 new XComment("Series Configuration XML"));
                XElement root = new XElement(rootElement);

                XElement racePoints =
                  new XElement(
                    numberElement,
                    new XAttribute(
                      prefixAttribute,
                      configData.NumberPrefix),
                    new XAttribute(
                      allPositionsAttribute,
                      configData.AllPositionsShown));

                root.Add(racePoints);

                writer.Add(root);
                writer.Save(fileName);
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error writing Series Configuration data " + ex.ToString());
            }

            return success;
        }

        /// <summary>
        /// Gets the series configuration data
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <returns>configuration data</returns>
        public SeriesConfigType LoadSeriesConfigData(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                {
                    string error = string.Format("Athlete Data file missing, one created - {0}", fileName);
                    this.logger.WriteLog(error);

                    SeriesConfigType newConfiguration =
                      new SeriesConfigType(
                        "TMP",
                        true);

                    this.SaveSeriesConfigData(
                      fileName,
                      newConfiguration);
                }

                XDocument reader = XDocument.Load(fileName);
                XElement root = reader.Root;

                XElement number = root.Element(numberElement);

                bool allPositions =
                  this.ReadBoolAttribute(
                    number,
                    allPositionsAttribute,
                    false,
                    fileName);

                return new SeriesConfigType(
                  (string)number.Attribute(prefixAttribute),
                  allPositions);
            }
            catch (Exception ex)
            {
                this.logger.WriteLog("Error reading Series Configuration data " + ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Read an integer attribute. Log and return a default value if there is an issue.
        /// </summary>
        /// <param name="element">element to read</param>
        /// <param name="attributeName">attribute name</param>
        /// <param name="defaultValue">default value</param>
        /// <param name="path">path to the file being read</param>
        /// <returns>attribute value</returns>
        private bool ReadBoolAttribute(
          XElement element,
          string attributeName,
          bool defaultValue,
          string path)
        {
            // TODO, could be generic. There are others
            try
            {
                return (bool)element.Attribute(attributeName);
            }
            catch (Exception ex)
            {
                this.logger.WriteLog(
                  $"Error reading Series Configuration data attribute: {attributeName} : {path} :{ex}");
                return defaultValue;
            }
        }
    }
}