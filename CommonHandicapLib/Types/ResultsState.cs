namespace CommonHandicapLib.Types
{
  /// <summary>
  /// Describes the state of a result. Used to help highlight it.
  /// </summary>
  public enum ResultsState
  {
    /// <summary>
    /// Default state.
    /// </summary>
    DefaultState,

    /// <summary>
    /// New Personal best run.
    /// </summary>
    NewPB,

    /// <summary>
    /// New season best run.
    /// </summary>
    NewSB,

    /// <summary>
    /// Indicates the first time the event is run.
    /// </summary>
    FirstTimer
  }
}
