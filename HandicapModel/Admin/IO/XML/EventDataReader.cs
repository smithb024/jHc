namespace HandicapModel.Admin.IO.XML
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using System.Xml.Linq;
  using CommonHandicapLib;
  using CommonHandicapLib.Types;
  using CommonLib.Types;

  internal static class EventDataReader
  {
    private const string c_rootElement = "Evt";
    private const string c_dateElement = "Date";

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>SaveEventData</name>
    /// <date>04/03/15</date>
    /// <summary>
    /// Save the event data.
    /// </summary>
    /// <param name="fileName">file name</param>
    /// <param name="date">event date</param>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static bool SaveEventData(string        fileName,
                                     EventMiscData eventData)
    {
      bool success = true;

      try
      {
        XDocument writer = new XDocument(
         new XDeclaration("1.0", "uft-8", "yes"),
         new XComment("Athlete's season XML"));
        XElement rootElement = new XElement(c_rootElement);
        XElement dateElement = new XElement(c_dateElement, eventData.EventDate.ToString());

        rootElement.Add(dateElement);
        writer.Add(rootElement);
        writer.Save(fileName);
      }
      catch (Exception ex)
      {
        Logger logger = Logger.GetInstance();
        logger.WriteLog("Error saving event file: " + ex.ToString());
        success = false;
      }

      return success;
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>LoadResultsTable</name>
    /// <date>04/03/15</date>
    /// <summary>
    /// Reads the event details xml from file and decodes it.
    /// </summary>
    /// <param name="fileName">name of xml file</param>
    /// <returns>decoded event data</returns>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static EventMiscData LoadEventData(string fileName)
    {
      EventMiscData eventData = new EventMiscData();

      try
      {
        XDocument reader      = XDocument.Load(fileName);
        XElement  rootElement = reader.Root;
        XElement  dateElement = rootElement.Element(c_dateElement);

        eventData.EventDate = new DateType(dateElement.Value);
      }
      catch (Exception ex)
      {
        Logger logger = Logger.GetInstance();
        logger.WriteLog("Error reading event data: " + ex.ToString());

        eventData = new EventMiscData();
      }

      return eventData;
    }
  }
}
