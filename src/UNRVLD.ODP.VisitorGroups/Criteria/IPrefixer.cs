namespace UNRVLD.ODP.VisitorGroups.Criteria
{
    /// <summary>
    /// Represents a prefixer that can be used to add or split prefixes from strings.
    /// </summary>
    public interface IPrefixer
    {
        /// <summary>
        /// Adds the specified prefix to the given value.
        /// </summary>
        /// <param name="value">The value to add the prefix to.</param>
        /// <param name="prefix">The prefix to add.</param>
        /// <returns>The value with the prefix added.</returns>
        public string Prefix(string value, string prefix);

        /// <summary>
        /// Splits the prefix from the given value.
        /// </summary>
        /// <param name="value">The value to split the prefix from.</param>
        /// <returns>A tuple containing the value without the prefix and the prefix itself.</returns>
        public (string? prefix, string value) SplitPrefix(string value);
    }
}