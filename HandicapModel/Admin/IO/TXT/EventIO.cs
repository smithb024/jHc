namespace HandicapModel.Admin.IO.TXT
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using CommonHandicapLib;

  public static class EventIO
  {
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>GetEvents</name>
    /// <date>21/03/15</date>
    /// <summary>
    /// Returns a list of all events.
    /// </summary>
    /// <param name="season">event's season</param>
    /// <returns>list of all events in the season</returns>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static List<string> GetEvents(string season)
    {
      List<string> events = new List<string>();
      string[] eventsArray = System.IO.Directory.GetDirectories(RootPath.DataPath + season);

      foreach (string occasion in eventsArray)
      {
        events.Add(occasion.Substring(occasion.LastIndexOf('\\') + 1));
      }

      return events;
    }


    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>CreateNewEvent</name>
    /// <date>21/03/15</date>
    /// <summary>
    /// Creates a directory for a new event
    /// </summary>
    /// <param name="eventName">new event</param>
    /// <param name="seasonName">event's season</param>
    /// <returns>success flag</returns>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static bool CreateNewEvent(string seasonName,
                                      string eventName)
    {
      try
      {
        Directory.CreateDirectory(RootPath.DataPath + seasonName + '\\' + eventName);
      }
      catch (Exception ex)
      {
        Logger logger = Logger.GetInstance();
        logger.WriteLog("Can't create new event: " + ex.ToString());
        return false;
      }

      return true;
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>LoadCurrentEvent</name>
    /// <date>25/03/15</date>
    /// <summary>
    /// Loads the current event name.
    /// </summary>
    /// <returns>current event</returns>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static string LoadCurrentEvent(string seasonName)
    {
      try
      {
        if (File.Exists(RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + IOPaths.currentEvent))
        {
          using (StreamReader reader = new StreamReader(RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + IOPaths.currentEvent))
          {
            return reader.ReadLine();
          }
        }
      }
      catch (Exception ex)
      {
        Logger logger = Logger.GetInstance();
        logger.WriteLog("Error, failed to read current event: " + ex.ToString());
      }

      return string.Empty;
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>SaveCurrentEvent</name>
    /// <date>25/03/15</date>
    /// <summary>
    /// Saves the current event name.
    /// </summary>
    /// <param name="season">current event</param>
    /// <returns>success flag</returns>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static bool SaveCurrentEvent(string seasonName,
                                        string eventName)
    {
      bool success = false;

      try
      {
        using (
          StreamWriter writer =
          new StreamWriter(
            RootPath.DataPath + seasonName + Path.DirectorySeparatorChar + IOPaths.currentEvent,
            false))
        {
          writer.Write(eventName);
          success = true;
        }
      }
      catch (Exception ex)
      {
        Logger logger = Logger.GetInstance();
        logger.WriteLog("Error, failed to save current season: " + ex.ToString());
      }

      return success;
    }
  }
}
