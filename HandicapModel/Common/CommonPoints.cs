namespace HandicapModel.Common
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Text;
  using CommonHandicapLib;
  using CommonLib.Types;
    using Interfaces.Common;

  public class CommonPoints : ICommonPoints
  {
    private const char     separator       = 'p';
    private const int      elements        = 3;
    private       int      finishingPoints = 0;
    private       int      positionPoints  = 0;
    private       int      bestPoints      = 0;
    private       DateType date            = new DateType();

    /// <summary>
    /// Initialises a new instance of the <see cref="CommonPoints"/> class.
    /// </summary>
    /// <param name="finishingPoints">finishing points</param>
    /// <param name="positionPoints">position points</param>
    /// <param name="bestPoints">best points</param>
    /// <param name="date">date points earned</param>
    public CommonPoints(int      finishingPoints,
                        int      positionPoints,
                        int      bestPoints,
                        DateType date)
    {
      FinishingPoints = finishingPoints;
      PositionPoints  = positionPoints;
      BestPoints      = bestPoints;
      Date            = date;
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="CommonPoints"/> class.
    /// </summary>
    /// <param name="points"></param>
    /// <param name="date"></param>
    public CommonPoints(string   points,
                         DateType date)
    {
      string[] rawPoints = points.Split(separator);
      if (rawPoints.Count() == elements)
      {
        int tempPoints = 0;
        if (int.TryParse(rawPoints[0], out tempPoints))
        {
          FinishingPoints = tempPoints;
        }

        if (int.TryParse(rawPoints[1], out tempPoints))
        {
          PositionPoints = tempPoints;
        }

        if (int.TryParse(rawPoints[2], out tempPoints))
        {
          BestPoints = tempPoints;
        }
      }
      else
      {
        FinishingPoints = 0;
        PositionPoints  = 0;
        BestPoints      = 0;
      }

      Date = date;
    }

    /// <summary>
    /// Initialises a new instance of <see cref="CommonPoints"/> class.
    /// </summary>
    /// <param name="date"></param>
    public CommonPoints(DateType date) : this (0, 0, 0, date)
    {
    }

    /// <summary>
    /// Gets all points won tby the athlete.
    /// </summary>
    public int TotalPoints
    {
      get { return FinishingPoints + PositionPoints + BestPoints; }
    }

    /// <summary>
    /// Gets and sets the points allocated to the athlete for finishing.
    /// </summary>
    public int FinishingPoints
    {
      get { return finishingPoints; }
      set { finishingPoints = value; }
    }

    /// <summary>
    /// Gets and set the points allocated to the athlete for finishing in a points scoring position.
    /// </summary>
    public int PositionPoints
    {
      get { return positionPoints; }
      set { positionPoints = value; }
    }

    /// <summary>
    /// Gets and sets the points allocated to the althete for running a seasons best time.
    /// </summary>
    public int BestPoints
    {
      get { return bestPoints; }
      set { bestPoints = value; }
    }

    /// <summary>
    /// Gets and sets the date the points were allocated.
    /// </summary>
    public DateType Date
    {
      get { return date; }
      set { date = value; }
    }

    /// <summary>
    /// Overides the to string method.
    /// </summary>
    /// <returns>output string</returns>
    public override string ToString()
    {
      return string.Format("{0}{1}{2}{3}{4}",
                           FinishingPoints,
                           separator,
                           PositionPoints,
                           separator,
                           BestPoints);
    }
  }
}
