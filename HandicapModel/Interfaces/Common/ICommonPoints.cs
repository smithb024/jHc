namespace HandicapModel.Interfaces.Common
{
    using CommonLib.Types;

    /// <summary>
    /// Interface which describes the points break down for finishing in the traditional manner.
    /// </summary>
    public interface ICommonPoints
    {
        /// <summary>
        /// Gets all points won by an athlete.
        /// </summary>
        int TotalPoints { get; }

        /// <summary>
        /// Gets and sets the points allocated for an athlete finishing.
        /// </summary>
        int FinishingPoints { get; set; }

        /// <summary>
        /// Gets and set the points allocated for an athlete finishing in a points scoring position.
        /// </summary>
        int PositionPoints { get; set; }

        /// <summary>
        /// Gets and sets the points allocated for an athlete running a seasons best time.
        /// </summary>
        int BestPoints { get; set; }

        /// <summary>
        /// Gets and sets the date the points were allocated.
        /// </summary>
        DateType Date { get; set; }

        /// <summary>
        /// Overides the to string method.
        /// </summary>
        /// <returns>output string</returns>
        string ToString();
    }
}