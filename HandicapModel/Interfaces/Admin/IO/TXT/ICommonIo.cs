namespace HandicapModel.Interfaces.Admin.IO.TXT
{
    using System.Collections.Generic;

    /// <summary>
    /// Generic IO functions
    /// </summary>
    public interface ICommonIo
    {
        /// <summary>
        /// Reads supplied file.
        /// </summary>
        /// <param name="fileName">file name</param>
        /// <returns>file contents</returns>
        List<string> ReadFile(string fileName);

        /// <summary>
        /// Reads a file in pairs of lines. The pairs are stored as a list of strings and the method
        /// returns a list of these items.
        /// </summary>
        /// <param name="fileName">raw position data file name</param>
        /// <returns>list of position data</returns>
        List<List<string>> ReadPairedStringListFomFile(string fileName);
    }
}
