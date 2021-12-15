namespace HandicapModel.Admin.IO.TXT
{
  using System;
  using System.Collections.Generic;
  using System.IO;
  using CommonHandicapLib;

  public static class CommonIO
  {
    /// <summary>
    /// Reads supplied file.
    /// </summary>
    /// <param name="fileName">file name</param>
    /// <returns>file contents</returns>
    public static List<string> ReadFile(string fileName)
    {
      try
      {
        List<string> fileContents = new List<string>();

        if (File.Exists(fileName))
        {
          using (StreamReader reader = new StreamReader(fileName))
          {
            string currentLine = reader.ReadLine();

            while (currentLine != null)
            {
              fileContents.Add(currentLine);

              currentLine = reader.ReadLine();
            }
          }
        }

        return fileContents;
      }
      catch (Exception ex)
      {
        JHcLogger.GetInstance().WriteLog(string.Format("Error, failed to read {0}: {1}", fileName, ex.ToString()));

        return new List<string>();
      }
    }

    /// <summary>
    /// Reads a file in pairs of lines. The pairs are stored as a list of strings and the method
    /// returns a list of these items.
    /// </summary>
    /// <param name="fileName">raw position data file name</param>
    /// <returns>list of position data</returns>
    public static List<List<string>> ReadPairedStringListFomFile(string fileName)
    {
      try
      {
        List<List<string>> rawPostions = new List<List<string>>();

        if (File.Exists(fileName))
        {
          using (StreamReader reader = new StreamReader(fileName))
          {
            string line1 = reader.ReadLine();
            string line2 = reader.ReadLine();

            while (line1 != null && line2 != null)
            {
              List<string> positionData = new List<string>();
              positionData.Add(line1);
              positionData.Add(line2);

              rawPostions.Add(positionData);

              // Read next lines
              line1 = reader.ReadLine();
              line2 = reader.ReadLine();
            }
          }
        }

        return rawPostions;
      }
      catch (Exception ex)
      {
        JHcLogger.GetInstance().WriteLog(string.Format("Error, failed to read {0}: {1}", fileName, ex.ToString()));

        return new List<List<string>>();
      }
    }
  }
}