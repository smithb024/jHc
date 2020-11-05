namespace CommonHandicapLib
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;

  public class Logger
  {
    private static Logger m_instance = null;
    private static readonly object s_locker = new object();

    private string       m_logName = "jHandicapLog";
    private const string c_logPath = ".\\log";
    private const string c_dirSeparator = "\\";
    private string       m_fileName = string.Empty;

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>Logger</name>
    /// <date>22/02/15</date>
    /// <summary>
    /// Prevents a default instance of the logger class from being created.
    /// </summary>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    private Logger()
    {
      if (!Directory.Exists(c_logPath))
      {
        Directory.CreateDirectory(c_logPath);
      }

      m_fileName = c_logPath + c_dirSeparator + DateTime.Now.ToString("yyyyMMdd") + " " + DateTime.Now.ToLongTimeString() + m_logName + ".txt";
      m_fileName = m_fileName.Replace(" ", "_");
      m_fileName = m_fileName.Replace(":", "_");

      FileInfo file = new FileInfo(m_fileName);
      FileStream stream = file.Create();
      stream.Close();

      WriteLog("--------------------------------------");
      WriteLog("Junior Handicap Log File");
      WriteLog("--------------------------------------");
    }

    /// <summary>
    /// Gets a new instance of the <see cref="Logger"/> class.
    /// </summary>
    public static Logger Instance
    {
      get
      {
        if (m_instance == null)
        {
          m_instance = new Logger();
        }

        return m_instance;
      }
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>GetInstance</name>
    /// <date>22/02/15</date>
    /// <summary>
    /// Returns an instance of the Logger class
    /// </summary>
    /// <returns>instance of the logger.</returns>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public static Logger GetInstance()
    {
      if (m_instance == null)
      {
        m_instance = new Logger();
      }

      return m_instance;
    }

    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    /// <name>WriteLog</name>
    /// <date>22/02/15</date>
    /// <summary>
    /// Writes the log entry string to the log.
    /// </summary>
    /// <param name="logEntry">entry to log.</param>
    /// ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ---------- ----------
    public void WriteLog(string logEntry)
    {
      StreamWriter writer = null;

      lock (s_locker)
      {
        try
        {
          using (writer = File.AppendText(m_fileName))
          {
            writer.Write("{0} {1}|"
              , DateTime.Now.ToLongTimeString()
              , DateTime.Now.ToLongDateString());
            writer.WriteLine(logEntry);
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine(ex.ToString());
        }
      }
    }
  }
}
