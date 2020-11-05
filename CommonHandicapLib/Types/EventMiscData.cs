namespace CommonHandicapLib.Types
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using CommonLib.Types;

  public class EventMiscData
  {
    private DateType m_eventDate = new DateType();

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>EventData</name>
    /// <date>03/04/15</date>
    /// <summary>
    ///   Creates a new instance of the EventData class
    /// </summary>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public EventMiscData()
    {
    }

    /// ---------- ---------- ---------- ---------- ---------- ----------
    /// <summary>
    /// Gets and sets the date
    /// </summary>
    /// ---------- ---------- ---------- ---------- ---------- ----------
    public DateType EventDate
    {
      get { return m_eventDate; }
      set { m_eventDate = value; }
    }
  }
}
