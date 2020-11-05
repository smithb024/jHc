namespace HandicapModel.Interfaces.Common
{
    using CommonLib.Types;

    /// <summary>
    /// Interface which breaks down a point within the team harmony competition.
    /// </summary>
    public interface ICommonHarmonyPoints : IAthleteHarmonyPoints 
    {
        /// <summary>
        /// Gets a value indicated whether this is a real athlete, or just filler.
        /// </summary>
        bool IsReal { get; }

        /// <summary>
        /// Gets the name of the athlete
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the athletes key
        /// </summary>
        int AthleteKey { get; }
    }
}