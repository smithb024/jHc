namespace HandicapModel.Admin.IO
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;
  using CommonHandicapLib.Types;
  using HandicapModel.Admin.IO.XML;

  public static class EventData
  {
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>SaveEventData</name>
    /// <date>04/04/15</date>
    /// <summary>
    /// Saves the event misc details
    /// </summary>
    /// <param name="seasonName">season name</param>
    /// <param name="eventName">event name</param>
    /// <param name="summaryDetails">details to save</param>
    /// <returns>success flag</returns>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static bool SaveEventData(string        seasonName,
                                     string        eventName,
                                     EventMiscData eventData)
    {
      return EventDataReader.SaveEventData(
        RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + eventName + Path.DirectorySeparatorChar + IOPaths.eventMiscFile,
        eventData);
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>LoadEventData</name>
    /// <date>04/04/15</date>
    /// <summary>
    /// Reads the event details.
    /// </summary>
    /// <param name="seasonName">season name</param>
    /// <returns>decoded event data</returns>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static EventMiscData LoadEventData(string seasonName,
                                              string eventName)
    {
      return EventDataReader.LoadEventData(
        RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + eventName + Path.DirectorySeparatorChar + IOPaths.eventMiscFile);
    }
  }
}
