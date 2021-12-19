using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandicapModel.Interfaces.Admin.IO
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using CommonHandicapLib.Types;
    using HandicapModel.Admin.IO.XML;

    /// <summary>
    /// Event data interface
    /// </summary>
    public interface IEventData
    {
        /// <summary>
        /// Saves the event misc details
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <param name="eventName">event name</param>
        /// <param name="summaryDetails">details to save</param>
        /// <returns>success flag</returns>
        bool SaveEventData(
            string seasonName,
            string eventName,
            EventMiscData eventData);

        /// <summary>
        /// Reads the event details.
        /// </summary>
        /// <param name="seasonName">season name</param>
        /// <returns>decoded event data</returns>
        EventMiscData LoadEventData(
            string seasonName,
            string eventName);
    }
}