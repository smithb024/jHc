namespace CommonHandicapLib.Messages
{
    /// <summary>
    /// Message used to indicated the validity of a newly selected location.
    /// </summary>
    public class ValidLocationMessage
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ValidLocationMessage"/> class.
        /// </summary>
        /// <param name="isValid">indicates the base path's validity</param>
        public ValidLocationMessage(bool isValid)
        {
            this.IsValid = isValid;
        }

        /// <summary>
        /// Gets a value which indicates whether the current base location is valid.
        /// </summary>
        public bool IsValid { get; }
    }
}