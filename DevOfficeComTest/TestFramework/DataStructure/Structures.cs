namespace TestFramework
{
    using System;

    /// <summary>
    /// This struct contains the searched result's information
    /// </summary>
    public struct SearchedResult
    {
        /// <summary>
        /// The result name
        /// </summary>
        public string Name;

        /// <summary>
        /// The result description
        /// </summary>
        public string Description;
        
        /// <summary>
        /// The count of views
        /// </summary>
        public long ViewCount;

        /// <summary>
        /// The updated date
        /// </summary>
        public DateTime? UpdatedDate;

        /// <summary>
        /// The detail link
        /// </summary>
        public string DetailLink;
    }
}