namespace HandicapModel.Admin.IO
{
    using System;
    using System.IO;

    using TXT;

    /// <summary>
    /// Static class to describe the root paths.
    /// </summary>
    public static class RootPath
    {
        /// <summary>
        /// Configuration path;
        /// </summary>
        /// <remarks>
        /// Remember to listen for the ReinitialiseRoot message.
        /// </remarks>
        [Obsolete]
        public static string ConfigurationPath =>
          RootIO.LoadRootFile() +
          Path.DirectorySeparatorChar +
          IOPaths.configurationPath;

        /// <summary>
        /// Data path;
        /// </summary>
        /// <remarks>
        /// Remember to listen for the ReinitialiseRoot message.
        /// </remarks>
        [Obsolete]
        public static string DataPath =>
          RootIO.LoadRootFile() +
          Path.DirectorySeparatorChar +
          IOPaths.dataPath +
          Path.DirectorySeparatorChar;
    }
}