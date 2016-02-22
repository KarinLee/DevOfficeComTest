using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;

namespace TestFramework
{
    public class Navigation : BasePage
    {
        [FindsBy(How = How.CssSelector, Using = "#navbar-collapse-1 > ul > li:nth-child(1) > a")]
        private IWebElement exploreLinkElement;

        [FindsBy(How = How.CssSelector, Using = "#navbar-collapse-1 > ul > li:nth-child(4) > a")]
        private IWebElement resourceLinkElement;

        [FindsBy(How = How.CssSelector, Using = "#navbar-collapse-1 > ul > li:nth-child(2) > a")]
        private IWebElement gettingstartedLinkElement;

        [FindsBy(How = How.CssSelector, Using = "#navbar-collapse-1 > ul > li:nth-child(3) > a")]
        private IWebElement codesamplesLinkElement;

        [FindsBy(How = How.CssSelector, Using = "#navbar-collapse-1 > ul > li:nth-child(5) > a")]
        private IWebElement documentationLinkElement;

        public void Select(string menuName, string itemName = "")
        {
            switch (menuName)
            {
                case ("Explore"):
                    Browser.Click(exploreLinkElement);
                    break;
                case ("Resources"):
                    Browser.Click(resourceLinkElement);
                    break;
                case ("Getting Started"):
                    Browser.Click(gettingstartedLinkElement);
                    break;
                case ("Code Samples"):
                    Browser.Click(codesamplesLinkElement);
                    break;
                case ("Documentation"):
                    Browser.Click(documentationLinkElement);
                    break;
                default:
                    break;
            }

            if (!itemName.Equals(""))
            {
                MenuItemOfExplore exploreItem;
                MenuItemOfResource resourceItem;
                MenuItemOfDocumentation documentationItem;
                if (Enum.TryParse(itemName, out exploreItem))
                {
                    IWebElement item;
                    switch (exploreItem)
                    {
                        case (MenuItemOfExplore.WhyOffice):
                        case (MenuItemOfExplore.OfficeUIFabric):
                        case (MenuItemOfExplore.MicrosoftGraph):
                            item = Browser.Driver.FindElement(By.CssSelector("#navbar-collapse-1 > ul > li.subnav__item.dropdown-toggle.dropdown.open > div > div > ul > li:nth-child(" + ((int)exploreItem + 1) + ") > a"));
                            break;
                        case (MenuItemOfExplore.Android):
                        case (MenuItemOfExplore.DotNET):
                        case (MenuItemOfExplore.iOS):
                        case (MenuItemOfExplore.JavaScript):
                        case (MenuItemOfExplore.Node):
                        case (MenuItemOfExplore.PHP):
                        case (MenuItemOfExplore.Python):
                        case (MenuItemOfExplore.Ruby):
                            item = Browser.Driver.FindElement(By.CssSelector("#navbar-collapse-1 > ul > li.subnav__item.dropdown-toggle.dropdown.open > div > div > div.tier-3.col-md-6.col-sm-5 > ul > li:nth-child(" + ((int)exploreItem - 13) + ") > a"));
                            break;
                        default:
                            item = Browser.Driver.FindElement(By.CssSelector("#navbar-collapse-1 > ul > li.subnav__item.dropdown-toggle.dropdown.open > div > div > div.tier-2.col-md-3.col-sm-4 > ul > li:nth-child(" + ((int)exploreItem - 2) + ") > a"));
                            break;
                    }

                    if (item != null)
                    {
                        Browser.Click(item);
                    }
                }

                if (Enum.TryParse(itemName, out resourceItem))
                {
                    var item = Browser.Driver.FindElement(By.CssSelector("#navbar-collapse-1 > ul > li.subnav__item.dropdown-toggle.dropdown.open > div > div > ul > li:nth-child(" + ((int)resourceItem + 1) + ") > a"));
                    Browser.Click(item);
                }

                if (Enum.TryParse(itemName, out documentationItem))
                {
                    var item = Browser.Driver.FindElement(By.CssSelector("#navbar-collapse-1 > ul > li.subnav__item.dropdown-toggle.dropdown.open > div > div > ul > li:nth-child(" + ((int)documentationItem + 1) + ") > a"));
                    Browser.Click(item);
                }
            }
        }

        public bool IsAtProductPage(string productName)
        {
            switch (productName)
            {
                case ("Outlook"):
                    bool canSwitchWindow = Browser.SwitchToNewWindow();
                    bool isAtOutlookPage = false;
                    if (canSwitchWindow)
                    {
                        var outlookPage = new NewWindowPage();
                        isAtOutlookPage = outlookPage.IsAt(productName);
                        Browser.SwitchBack();
                    }

                    Browser.GoBack();
                    return isAtOutlookPage;
                case ("DotNET"):
                case ("Node"):
                    MenuItemOfExplore menuItemResult;
                    Enum.TryParse(productName, out menuItemResult);
                    var page = new ProductPage();
                    string pageName = EnumExtension.GetDescription(menuItemResult).Replace(" ", "");
                    return page.ProductName.Contains(pageName);
                case ("PHP"):
                case ("Python"):
                case ("Ruby"):
                    // These three have no pages and currently redirect to Office365 API getting-started page
                    Platform platformResult;
                    Enum.TryParse(productName, out platformResult);
                    var platform = new Office365Page.CardSetupPlatform();
                    bool isShownPlatformSetup = platform.IsShowingPlatformSetup(platformResult);
                    return isShownPlatformSetup;
                default:
                    var productPage = new ProductPage();
                    return productPage.ProductName == productName;
            }
        }

        public bool IsAtOpportunityPage()
        {
            var opportunityPage = new OpportunityPage();
            bool canLoadImage = opportunityPage.CanLoadImage();
            return canLoadImage;
        }

        public bool IsAtFabricPage(string fabricTitle)
        {
            var fabricPage = new FabricPage();
            string title = fabricPage.FabricPageTitle;
            Browser.GoBack();
            return title == fabricTitle;
        }

        public bool IsAtFabricGettingStartedPage(string fabricTitle)
        {
            var fabricPage = new FabricGettingStartedPage();
            string title = fabricPage.FabricPageTitle;
            Browser.GoBack();
            return title == fabricTitle;
        }

        public bool IsAtChoosingAPIEndpointPage(string Title)
        {
            var endpointPage = new ChoosingAPIEndpointPage();
            string title = endpointPage.EndpointPageTitle;
            return title == Title;
        }

        public bool IsAtGraphPage(string graphTitle)
        {
            var graphPage = new GraphPage();
            string title = graphPage.GraphTitle.Replace(" ", "");

            Browser.GoBack();
            return title.Contains(graphTitle.Replace(" ", ""));
        }

        public bool IsAtOfficeGettingStartedPage(string Title)
        {
            string pageTitle = Browser.Title;
            return pageTitle.Contains(Title);
        }

        public bool IsAtCodeSamplesPage(string Title)
        {
            var codeSamplesPage = new CodeSamplesPage();
            string pageTitle = codeSamplesPage.CodeSamplesPageTitle;
            return pageTitle.Contains(Title);
        }

        public bool IsAtResourcePage(MenuItemOfResource item)
        {
            var resourcePage = new ResourcePage();
            bool isAtResourcePage = false;
            switch (item)
            {
                case (MenuItemOfResource.MiniLabs):
                    string miniLabsName = EnumExtension.GetDescription(item).Replace("-", " ").ToLower();
                    isAtResourcePage = resourcePage.ResourceName.ToLower().Contains(miniLabsName);
                    break;
                case (MenuItemOfResource.SnackDemoVideos):
                    string snackVideosName = EnumExtension.GetDescription(item).Replace("Demo ", "").ToLower();
                    isAtResourcePage = resourcePage.ResourceName.ToLower().Contains(snackVideosName);
                    break;
                case (MenuItemOfResource.APISandbox):
                    bool canSwitchWindow = Browser.SwitchToNewWindow();
                    if (canSwitchWindow)
                    {
                        var sandboxPage = new NewWindowPage();
                        isAtResourcePage = sandboxPage.IsAt(EnumExtension.GetDescription(item));
                        Browser.SwitchBack();
                    }

                    Browser.GoBack();
                    break;
                default:
                    isAtResourcePage = resourcePage.ResourceName.ToLower().Contains(EnumExtension.GetDescription(item).ToLower());
                    break;
            }

            return isAtResourcePage;
        }

        public bool IsAtDocumentationPage(MenuItemOfDocumentation item)
        {
            switch (item)
            {
                case (MenuItemOfDocumentation.OfficeUIFabricGettingStarted):
                    return IsAtFabricGettingStartedPage(EnumExtension.GetDescription(item));
                case (MenuItemOfDocumentation.Office365RESTAPIs):
                    return IsAtChoosingAPIEndpointPage("Choosing your API endpoint");
                case (MenuItemOfDocumentation.MicrosoftGraphAPI):
                    return IsAtDocumentationPage("Microsoft Graph");
                case (MenuItemOfDocumentation.PreviousVersions):
                    return IsAtDocumentationPage("Office developer documentation");
                default:
                    return IsAtDocumentationPage(EnumExtension.GetDescription(item));
            }
        }

        public bool IsAtExplorePage(MenuItemOfExplore item)
        {
            Platform platformResult;
            Product productResult;
            OtherProduct otherProduct;
            if (Enum.TryParse(item.ToString(), out platformResult) || Enum.TryParse(item.ToString(), out productResult) ||
                item == MenuItemOfExplore.JavaScript)
            {
                return IsAtProductPage(item.ToString());
            }

            // These products have their own home page and will be navigated out of Dev.office.com
            if (Enum.TryParse(item.ToString(), out otherProduct))
            {
                bool canSwitchWindow = Browser.SwitchToNewWindow();
                bool isAtOtherProductPage = false;
                if (canSwitchWindow)
                {
                    var otherProductPage = new NewWindowPage();
                    isAtOtherProductPage = otherProductPage.IsAt(item.ToString());
                    Browser.SwitchBack();
                }

                Browser.GoBack();
                return isAtOtherProductPage;
            }

            switch (item)
            {
                case (MenuItemOfExplore.WhyOffice):
                    return IsAtOpportunityPage();
                case (MenuItemOfExplore.OfficeUIFabric):
                    return IsAtFabricPage(EnumExtension.GetDescription(item));
                case (MenuItemOfExplore.MicrosoftGraph):
                    return IsAtGraphPage(item.ToString());
            }

            return false;
        }

        private bool IsAtDocumentationPage(string pageTitle)
        {
            var documentationPage = new DocumentationPage();
            bool isAtDocumentationPage = false;
            bool canSwitchWindow = false;
            canSwitchWindow = Browser.SwitchToNewWindow();
            if (canSwitchWindow)
            {
                isAtDocumentationPage = documentationPage.DocumentationTitle.ToLower().Contains(pageTitle.ToLower());
                Browser.SwitchBack();
            }

            Browser.GoBack();
            return isAtDocumentationPage;
        }
    }
}