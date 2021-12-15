namespace CommonHandicapLib.XML
{
  using System;
  using System.Xml.Linq;
  using Types;

  /// <summary>
  /// Reads and writes the results configuration data to a file.
  /// </summary>
  public static class NormalisationConfigReader
  {
    private const string rootElement           = "NormalisationConfiguration";
    private const string useElement            = "Use";
    private const string handicapsElement      = "Handicaps";

    private const string useHandicapAttribute  = "Handicap";
    private const string handicapTimeAttribute = "Time";
    private const string minimumTimeAttribute  = "MinimumTime";
    private const string handicapIntervalAttribute = "Interval";

    /// <summary>
    /// Save the configuration data.
    /// </summary>
    /// <param name="fileName">file name</param>
    /// <param name="configData">configuration data</param>
    /// <returns>success flag</returns>
    public static bool SaveNormalisationConfigData(
      string fileName,
      NormalisationConfigType configData)
    {
      bool success = true;

      try
      {
        XDocument writer = new XDocument(
         new XDeclaration("1.0", "uft-8", "yes"),
         new XComment("HC Normalisation Config XML"));
        XElement root = new XElement(rootElement);

        XElement racePoints =
          new XElement(
            useElement,
            new XAttribute(
              useHandicapAttribute,
              configData.UseCalculatedHandicap));

        XElement clubPoints =
          new XElement(
            handicapsElement,
            new XAttribute(
              handicapTimeAttribute,
              configData.HandicapTime),
            new XAttribute(
              minimumTimeAttribute,
              configData.MinimumHandicap),
            new XAttribute(
              handicapIntervalAttribute,
              configData.HandicapInterval));

        root.Add(racePoints);
        root.Add(clubPoints);

        writer.Add(root);
        writer.Save(fileName);
      }
      catch (Exception ex)
      {
        JHcLogger logger = JHcLogger.GetInstance();
        logger.WriteLog("Error writing Normalisation Configuration data " + ex.ToString());
      }

      return success;
    }

    /// <summary>
    /// Gets the normalisation configuration data
    /// </summary>
    /// <param name="fileName">file name</param>
    /// <returns>configuration data</returns>
    public static NormalisationConfigType LoadNormalisationConfigData(string fileName)
    {
      try
      {
        XDocument reader = XDocument.Load(fileName);
        XElement  root   = reader.Root;

        XElement useHandicap   = root.Element(useElement);
        XElement handicapTimes = root.Element(handicapsElement);

        bool useHandicaps = (bool)useHandicap.Attribute(useHandicapAttribute);

        int  handicapTime =
          NormalisationConfigReader.ReadIntAttribute(
            handicapTimes,
            handicapTimeAttribute,
            20);
        int  minimumTime =
          NormalisationConfigReader.ReadIntAttribute(
            handicapTimes,
            minimumTimeAttribute,
            5);
        int  handicapInterval =
          NormalisationConfigReader.ReadIntAttribute(
            handicapTimes,
            handicapIntervalAttribute,
            30);

        return new NormalisationConfigType(
          useHandicaps,
          handicapTime,
          minimumTime,
          handicapInterval);
      }
      catch (Exception ex)
      {
        JHcLogger logger = JHcLogger.GetInstance();
        logger.WriteLog("Error reading Normalisation Configuration data " + ex.ToString());
        return null;
      }
    }

    /// <summary>
    /// Read an integer attribute. Log and return a default value if there is an issue.
    /// </summary>
    /// <param name="element">element to read</param>
    /// <param name="attributeName">attribute name</param>
    /// <param name="defaultValue">default value</param>
    /// <returns>attribute value</returns>
    private static int ReadIntAttribute(
      XElement element,
      string attributeName,
      int defaultValue)
    {
      try
      {
        return (int)element.Attribute(attributeName);
      }
      catch (Exception ex)
      {
        JHcLogger.GetInstance().WriteLog(
          $"Error reading Normalisation Configuration data attribute: {ex}");
        return defaultValue;
      }
    }
  }
}