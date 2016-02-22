namespace TestFramework
{
    public static class GraphPages
    {
        public static GraphHomePage HomePage
        {
            get
            {
                return new GraphHomePage();
            }
        }

        public static GraphNavigation Navigation
        {
            get
            {
                return new GraphNavigation();
            }
        }
    }
}
