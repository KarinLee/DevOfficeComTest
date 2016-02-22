namespace TestFramework
{
    public static class Pages
    {
        public static HomePage HomePage
        {
            get
            {
                return new HomePage();
            }
        }

        public static Navigation Navigation
        {
            get
            {
                return new Navigation();
            }
        }

        public static OfficeGettingStartedPage OfficeGettingStartedPage
        {
            get
            {
                return new OfficeGettingStartedPage();
            }
        }

        public static Office365Page.Office365Page Office365Page
        {
            get
            {
                return new Office365Page.Office365Page();
            }
        }

        public static OfficeAddInPage.OfficeAddInPage OfficeAddInPage
        {
            get
            {
                return new OfficeAddInPage.OfficeAddInPage();
            }
        }
    }
}
