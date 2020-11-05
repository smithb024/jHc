namespace CommonHandicapLib.XML
{
  using System;
  using System.Collections.Generic;
  using System.Xml.Linq;
  using CommonHandicapLib.Types;

  /// <summary>
  /// Reads and writes the results configuration data to a file.
  /// </summary>
  public class ResultsConfigReader : IResultsConfigReader
  {
    private const string rootElement                  = "ResultsConfiguration";
    private const string racePointsElement            = "RacePoints";
    private const string clubPointsElement            = "ClubPoints";

    private const string finishingPointsAttribute     = "Finishing";
    private const string seasonBestAttribute          = "SB";
    private const string scoringPositionAttribute    = "Positions";
    private const string teamFinishingPointsAttribute = "TFinishing";
    private const string useTeamsAttribute = "Use";
    private const string scoresToCountAttribute = "ToCount";
    private const string allResultsAttribute = "All";
    private const string scoresDescendingAttribute = "Descending";
    private const string excludeFirstTimersAttribute = "ExclFT";

    private const string teamSizeAttribute            = "NumberCount";
    private const string teamSeasonBestAttribute      = "SB";

    /// <summary>
    /// Initialise a new instance of the <see cref="ResultsConfigReader"/> class.
    /// </summary>
    public ResultsConfigReader()
    {
    }

    /// <summary>
    /// Save the configuration data.
    /// </summary>
    /// <param name="fileName">file name</param>
    /// <param name="configData">configuration data</param>
    /// <returns>success flag</returns>
    public bool SaveResultsConfigData(
      string fileName,
      ResultsConfigType configData)
    {
      bool success = true;

      try
      {
        XDocument writer = new XDocument(
         new XDeclaration("1.0", "uft-8", "yes"),
         new XComment("Results Configuration XML"));
        XElement root = new XElement(rootElement);

        XElement racePoints =
          new XElement(
            racePointsElement,
            new XAttribute(finishingPointsAttribute, configData.FinishingPoints),
            new XAttribute(seasonBestAttribute, configData.SeasonBestPoints),
            new XAttribute(scoringPositionAttribute, configData.NumberOfScoringPositions),
            new XAttribute(excludeFirstTimersAttribute, configData.ExcludeFirstTimers),
            new XAttribute(allResultsAttribute, configData.AllResults),
            new XAttribute(scoresToCountAttribute, configData.ScoresToCount),
            new XAttribute(scoresDescendingAttribute, configData.ScoresAreDescending));
        XElement clubPoints =
          new XElement(
            clubPointsElement,
            new XAttribute(useTeamsAttribute, configData.UseTeams),
            new XAttribute(teamFinishingPointsAttribute, configData.TeamFinishingPoints),
            new XAttribute(teamSizeAttribute, configData.NumberInTeam),
            new XAttribute(teamSeasonBestAttribute, configData.TeamSeasonBestPoints));

        root.Add(racePoints);
        root.Add(clubPoints);

        writer.Add(root);
        writer.Save(fileName);
      }
      catch (Exception ex)
      {
        Logger logger = Logger.GetInstance();
        logger.WriteLog("Error writing Results Configuration data " + ex.ToString());
      }

      return success;
    }

    /// <summary>
    /// Gets the results configuration data
    /// </summary>
    /// <param name="fileName">file name</param>
    /// <returns>configuration data</returns>
    public ResultsConfigType LoadResultsConfigData(string fileName)
    {
      try
      {
        XDocument reader = XDocument.Load(fileName);
        XElement  root   = reader.Root;

        XElement RacePoints = root.Element(racePointsElement);
        XElement ClubPoints = root.Element(clubPointsElement);

        int finishingPoints      = (int)RacePoints.Attribute(finishingPointsAttribute);
        int seasonBestPoints     = (int)RacePoints.Attribute(seasonBestAttribute);
        int scoringPositions     = (int)RacePoints.Attribute(scoringPositionAttribute);
        bool excludeFirstTimers =
          this.ReadBoolAttribute(
            RacePoints,
            excludeFirstTimersAttribute,
            true);
        bool allResults =
          this.ReadBoolAttribute(
            RacePoints,
            allResultsAttribute,
            true);
        int scoresToCount =
          this.ReadIntAttribute(
            RacePoints,
            scoresToCountAttribute,
            0);
        bool scoresAreDescending =
          this.ReadBoolAttribute(
            RacePoints,
            scoresDescendingAttribute,
            true);

        bool useTeams =
          this.ReadBoolAttribute(
            ClubPoints,
            useTeamsAttribute,
            true);
        int teamFinishingPoints  = (int)ClubPoints.Attribute(teamFinishingPointsAttribute);
        int teamSize             = (int)ClubPoints.Attribute(teamSizeAttribute);
        int teamSeasonBestPoints = (int)ClubPoints.Attribute(teamSeasonBestAttribute);

        return new ResultsConfigType(
          finishingPoints,
          seasonBestPoints,
          scoringPositions,
          teamFinishingPoints,
          teamSize,
          teamSeasonBestPoints,
          scoresToCount,
          allResults,
          useTeams,
          scoresAreDescending,
          excludeFirstTimers);
      }
      catch (Exception ex)
      {
        Logger logger = Logger.GetInstance();
        logger.WriteLog("Error reading Results Configuration data " + ex.ToString());
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
    private bool ReadBoolAttribute(
      XElement element,
      string attributeName,
      bool defaultValue)
    {
      // TODO, could be generic. There are others
      try
      {
        return (bool)element.Attribute(attributeName);
      }
      catch (Exception ex)
      {
        Logger.GetInstance().WriteLog(
          $"Error reading Series Configuration data attribute: {ex}");
        return defaultValue;
      }
    }

    /// <summary>
    /// Read an integer attribute. Log and return a default value if there is an issue.
    /// </summary>
    /// <param name="element">element to read</param>
    /// <param name="attributeName">attribute name</param>
    /// <param name="defaultValue">default value</param>
    /// <returns>attribute value</returns>
    private int ReadIntAttribute(
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
        Logger.GetInstance().WriteLog(
          $"Error reading Normalisation Configuration data attribute: {ex}");
        return defaultValue;
      }
    }
  }
}