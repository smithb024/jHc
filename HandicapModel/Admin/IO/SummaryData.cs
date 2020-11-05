namespace HandicapModel.Admin.IO
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using HandicapModel.Admin.IO.XML;
  using HandicapModel.Common;
    using HandicapModel.Interfaces.Common;

  public static class SummaryData
  {
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>SaveSummaryData</name>
    /// <date>02/04/15</date>
    /// <summary>
    /// Saves the summary details
    /// </summary>
    /// <param name="summaryDetails">details to save</param>
    /// <returns>success flag</returns>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static bool SaveSummaryData(ISummary summaryDetails)
    {
      return SummaryDataReader.SaveSummaryData(
        RootPath.DataPath + IOPaths.globalSummaryFile,
        summaryDetails);
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>SaveSummaryData</name>
    /// <date>02/04/15</date>
    /// <summary>
    /// Saves the summary details
    /// </summary>
    /// <param name="seasonName">season name</param>
    /// <param name="summaryDetails">details to save</param>
    /// <returns>success flag</returns>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static bool SaveSummaryData(string  seasonName,
                                       ISummary summaryDetails)
    {
      return SummaryDataReader.SaveSummaryData(
        RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + seasonName + IOPaths.xmlExtension,
        summaryDetails);
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>SaveSummaryData</name>
    /// <date>02/04/15</date>
    /// <summary>
    /// Saves the summary details
    /// </summary>
    /// <param name="seasonName">season name</param>
    /// <param name="eventName">event name</param>
    /// <param name="summaryDetails">details to save</param>
    /// <returns>success flag</returns>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static bool SaveSummaryData(string  seasonName,
                                       string  eventName,
                                       ISummary summaryDetails)
    {
      return SummaryDataReader.SaveSummaryData(
        RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + eventName + Path.DirectorySeparatorChar + eventName + IOPaths.xmlExtension,
        summaryDetails);
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>LoadSummaryData</name>
    /// <date>02/04/15</date>
    /// <summary>
    /// Reads the summary details.
    /// </summary>
    /// <param name="seasonName">season name</param>
    /// <returns>decoded summary data</returns>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static ISummary LoadSummaryData()
    {
      return SummaryDataReader.ReadCompleteSummaryData(RootPath.DataPath + IOPaths.globalSummaryFile);
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>LoadSummaryData</name>
    /// <date>02/04/15</date>
    /// <summary>
    /// Reads the summary details.
    /// </summary>
    /// <param name="seasonName">season name</param>
    /// <returns>decoded summary data</returns>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static ISummary LoadSummaryData(string seasonName)
    {
      return SummaryDataReader.ReadCompleteSummaryData(
        RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + seasonName + IOPaths.xmlExtension);
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>LoadSummaryData</name>
    /// <date>02/04/15</date>
    /// <summary>
    /// Reads the summary details.
    /// </summary>
    /// <param name="seasonName">season name</param>
    /// <returns>decoded summary data</returns>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static ISummary LoadSummaryData(string seasonName,
                                          string eventName)
    {
      return SummaryDataReader.ReadCompleteSummaryData(
        RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + eventName + Path.DirectorySeparatorChar + eventName + IOPaths.xmlExtension);
    }
  }
}
