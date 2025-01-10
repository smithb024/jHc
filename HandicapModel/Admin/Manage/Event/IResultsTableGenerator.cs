namespace HandicapModel.Admin.Manage.Event
{
    using HandicapModel.Interfaces.SeasonModel.EventModel;
    using HandicapModel.SeasonModel.EventModel;
    using System.Collections.Generic;

    /// <summary>
    /// Factory Class which is used to create the <see cref="EventResults"/> table.
    /// </summary>
    public interface IResultsTableGenerator
    {
        /// <summary>
        /// Generate the results table from the raw results and return it.
        /// </summary>
        /// <param name="rawResults">raw results</param>
        /// <returns>event results table</returns>
        EventResults Generate(
            List<IRaw> rawResults);
   }
}
