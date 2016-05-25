using System;
using System.ComponentModel;
using System.Reflection;

namespace TestFramework
{
    public enum Product
    {
        Excel,
        Outlook,
        PowerPoint,
        Word
    }

    public enum ItemOfOfficeAddinDoc
    {
        Word,
        Excel,
        PowerPoint,
        Outlook,
        SharePoint,
        Project
    }

    public enum ItemOfMSGraphDoc
    {
        User,
        Group,
        Calendar,
        Message,
        File,
        Contact
    }

    public enum KeyWord
    {
        [Description("Microsoft Graph")]
        MicrosoftGraph,
        [Description("Microsoft Graph API")]
        MicrosoftGraphAPI,
        [Description("Graph API")]
        GraphAPI,
        [Description("Graph Microsoft")]
        GraphMicrosoft
    }

    public enum OtherProduct
    {
        Access,
        Project,
        OneDrive,
        OneNote,
        SharePoint,
        Skype,
        Yammer
    }

    public enum Platform
    {
        Android,
        [Description("ASP.NET MVC")]
        DotNET,
        iOS,
        [Description("Node.js")]
        Node,
        PHP,
        Python,
        Ruby,
        Angular,
        [Description("Universal Windows Platform")]
        WindowsUniversal
    }

    public enum ServiceToTry
    {
        [Description("messages")]
        GetMessages,
        [Description("events")]
        GetEvents,
        [Description("contacts")]
        GetContacts,
        GetFiles,
        GetUsers,
        GetGroups
    }

    public enum MenuItemOfExplore
    {
        [Description("Why Office?")]
        WhyOffice,
        [Description("Office UI Fabric")]
        OfficeUIFabric,
        [Description("Microsoft Graph")]
        MicrosoftGraph,
        Word,
        Excel,
        [Description("Powerpoint")]
        PowerPoint,
        Access,
        Project,
        OneDrive,
        OneNote,
        Outlook,
        SharePoint,
        Skype,
        Yammer,
        Android,
        [Description("ASP .NET")]
        DotNET,
        iOS,
        JavaScript,
        [Description("Node.js")]
        Node,
        [Description("PHP (coming soon)")]
        PHP,
        [Description("Python (coming soon)")]
        Python,
        [Description("Ruby (coming soon)")]
        Ruby
    }

    public enum OfficeAppItem
    {
        Word,
        Excel,
        PowerPoint,
        Access,
        Project,
        OneDrive,
        OneNote,
        Outlook,
        SharePoint,
        Skype,
        Yammer
    }

    public enum MenuItemOfResource
    {
        [Description("Patterns and Practices")]
        PatternsAndPractices,
        [Description("App Registration Tool")]
        AppRegistrationTool,
        Events,
        Podcasts,
        Training,
        Videos,
        [Description("Snack Demo Videos")]
        SnackDemoVideos,
        Showcase,
        [Description("API Sandbox")]
        APISandbox
    }

    public enum MenuItemOfDocumentation
    {
        [Description("Office UI Fabric")]
        OfficeUIFabricGettingStarted,
        [Description("Office Add-ins")]
        OfficeAddin,
        [Description("Office Add-in Availability")]
        OfficeAddinAvailability,
        [Description("SharePoint Add-ins")]
        SharePointAddin,
        [Description("Microsoft Graph API")]
        MicrosoftGraphAPI,
        [Description("Office 365 REST APIs")]
        Office365RESTAPIs,
        [Description("All Documentation")]
        AllDocumentation
    }

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

    public enum GetMessagesValue
    {
        Inbox,
        SentItems,
        Drafts,
        DeletedItems
    }

    public enum GetFilesValue
    {
        drive_root_children,
        me_drive
    }
    public enum GetUsersValue
    {
        me,
        me_select_skills,
        me_manager,
        myOrganization_users
    }
    public enum GetGroupValue
    {
        me_memberOf,
        members,
        drive_root_children,
        conversations
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

    /// <summary>
    /// The top nav bar items on Fabric page
    /// </summary>
    public enum FabricNavItem
    {
        [Description("Overview")]
        Overview,
        [Description("Get Started")]
        GetStarted,
        [Description("Styles")]
        Styles,
        [Description("Components")]
        Components,
        [Description("Resources")]
        Resources,
        [Description("Blog")]
        Blog
    }

    /// <summary>
    /// The top nav bar items on Skype page
    /// </summary>
    public enum SkypeNavItem
    {
        [Description("Overview")]
        Overview,
        [Description("Explore")]
        Explore,
        [Description("Getting Started")]
        GettingStarted,
        [Description("Skype APIs")]
        SkypeAPIs,
        [Description("Get Involved")]
        GetInvolved,
        [Description("Marketplace")]
        Marketplace
    }

    public static class EnumExtension
    {
        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                  (DescriptionAttribute[])fi.GetCustomAttributes(
                  typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }
    }

}
