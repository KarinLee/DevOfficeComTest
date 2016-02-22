using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.OfficeAddInPage
{
    public class CardOutlook : BasePage
    {
        public OutlookExplore Explore
        {
            get
            {
                return new OutlookExplore();
            }
        }
        public CardBuild Build
        {
            get
            {
                return new CardBuild(Product.Outlook);
            }
        }
        public OutlookMoreResources MoreResouces
        {
            get
            {
                return new OutlookMoreResources();
            }
        }
    }
    public class OutlookExplore
    {
        public void play()
        {
            Browser.Driver.FindElement(By.CssSelector("button.ytp-large-play-button.ytp-button")).Click();
        }
    }
    public class OutlookBuild
    {
        public void StartBuilding()
        {
            Browser.Driver.FindElement(By.Id("more-playground")).Click();
        }

    }
    public class OutlookMoreResources
    {
        private bool isAtCorrectResourcePage;
        private CommonMoreResources resource = new CommonMoreResources();

        public void OutlookDevCenter()
        {
            resource.OutlookDevCenter();
        }
        public void OfficeAddInTypes()
        {
            resource.OfficeAddInTypes();
        }
        public void DownLoadStarterSample()
        {
            resource.DownLoadStarterSample();
        }
        public void MoreCodeSamples()
        {
            resource.MoreCodeSamples(Product.Outlook);
        }
        public void ReadTheDocs()
        {
            resource.ReadTheDocs();
        }
        public void DesignYourAddIn()
        {
            resource.DesignYourAddIn();
        }
        public void PublishYourAddIn()
        {
            resource.PublishYourAddIn();
        }

        public bool IsShowingCorrectResourcePage()
        {
            return resource.IsShowingCorrectResourcePage(Product.Outlook);
        }
    }

}
