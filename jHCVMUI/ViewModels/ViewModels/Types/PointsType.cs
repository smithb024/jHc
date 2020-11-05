namespace jHCVMUI.ViewModels.ViewModels.Types
{
  using System;
  using System.Collections.Generic;
  using System.Text;
  using CommonLib.Types;

  public class PointsType : ViewModelBase
  {
    private int      totalPoints     = 0;
    private int      finishingPoints = 0;
    private int      positionPoints  = 0;
    private int      sbPoints        = 0;
    private DateType pointsDate      = new DateType();

    /// <summary>
    /// Initialises a new instance of <see cref="PointsType"/> class.
    /// </summary>
    /// <param name="finishingPoints">finishing points</param>
    /// <param name="positionPoints">position points</param>
    /// <param name="sbPoints">season best points</param>
    /// <param name="date">date scored</param>
    public PointsType(int      finishingPoints,
                      int      positionPoints,
                      int      sbPoints,
                      DateType date)
    {
      FinishingPoints = finishingPoints;
      PositionPoints  = positionPoints;
      SBPoints        = sbPoints;
      TotalPoints     = FinishingPoints + PositionPoints + SBPoints;
      PointsDate      = date;
    }

    /// <summary>
    /// Total points scored.
    /// </summary>
    public int TotalPoints
    {
      get
      {
        return totalPoints;
      }
      set
      {
        totalPoints = value;
        RaisePropertyChangedEvent("TotalPoints");
      }
    }

    /// <summary>
    /// Finishing points scored.
    /// </summary>
    public int FinishingPoints
    {
      get
      {
        return finishingPoints;
      }
      set
      {
        finishingPoints = value;
        RaisePropertyChangedEvent("FinishingPoints");
      }
    }

    /// <summary>
    /// Position points scored.
    /// </summary>
    public int PositionPoints
    {
      get
      {
        return positionPoints;
      }
      set
      {
        positionPoints = value;
        RaisePropertyChangedEvent("PositionPoints");
      }
    }

    /// <summary>
    /// Season best points scored.
    /// </summary>
    public int SBPoints
    {
      get
      {
        return sbPoints;
      }
      set
      {
        sbPoints = value;
        RaisePropertyChangedEvent("SBPoints");
      }
    }

    /// <summary>
    /// Date for the points.
    /// </summary>
    public DateType PointsDate
    {
      get
      {
        return pointsDate;
      }
      set
      {
        pointsDate = value;
        RaisePropertyChangedEvent("PointsDate");
      }
    }
  }
}
