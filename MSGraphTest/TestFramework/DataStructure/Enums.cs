using System;
using System.ComponentModel;
using System.Reflection;

namespace TestFramework
{
    public enum SliderMenuItem
    {
        [Description("Get Started")]
        GetStarted,
        News,
        Opportunity,
        Transform,
        [Description("Featured App")]
        FeaturedApp
    }

    /// <summary>
    /// The sort types
    /// </summary>
    public enum SortType
    {
        /// <summary>
        /// Sort by view count
        /// </summary>
        ViewCount,
        
        /// <summary>
        /// Sort by date
        /// </summary>
        Date
    }
}
