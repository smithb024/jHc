namespace CommonHandicapLib.Interfaces
{
    /// <summary>
    /// Interface which describes the local logger 
    /// </summary>
    public interface IJHcLogger
    {
        /// <summary>
        /// Write a line to the log file.
        /// </summary>
        /// <param name="logEntry">
        /// Line to add to the log file.
        /// </param>
        void WriteLog(string logEntry);
    }
}