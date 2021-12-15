﻿namespace CommonHandicapLib
{
    using NynaeveLib.Logger;

    /// <summary>
    /// Wrapper for the <see cref="Logger"/> class.
    /// </summary>
    public class JHcLogger
    {
        /// <summary>
        /// The library logger.
        /// </summary>
        private readonly Logger logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="JHcLogger"/> class.
        /// </summary>
        public JHcLogger()
        {
            Logger.SetInitialInstance("jHandicapLog");
            this.logger = Logger.Instance;
        }

        /// <summary>
        /// Gets a new instance of the <see cref="JHcLogger"/> class.
        /// </summary>
        public static JHcLogger Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = new JHcLogger();
                }

                return m_instance;
            }
        }

        /// <summary>
        /// Writes the log entry string to the log.
        /// </summary>
        /// <param name="logEntry">entry to log.</param>
        public void WriteLog(string logEntry)
        {
            this.logger.WriteLog(logEntry);
        }
    }
}