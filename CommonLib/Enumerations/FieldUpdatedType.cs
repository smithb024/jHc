namespace CommonLib.Enumerations
{
    /// <summary>
    /// Used to monitor changes to a property.
    /// </summary>
    public enum FieldUpdatedType
    {
        /// <summary>
        /// The field is unchanged.
        /// </summary>
        Unchanged,

        /// <summary>
        /// The field has been changed.
        /// </summary>
        Changed,

        /// <summary>
        /// The field is invalid
        /// </summary>
        Invalid,

        /// <summary>
        /// The field has been disabled
        /// </summary>
        Disabled
    }
}