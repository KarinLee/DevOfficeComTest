using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.OfficeAddInPage
{
    public class CardPowerPoint : BasePage
    {
        public PowerPointExplore Explore
        {
            get
            {
                return new PowerPointExplore();
            }
        }
        public CardBuild Build
        {
            get
            {
                return new CardBuild(Product.PowerPoint);
            }
        }
        public PowerPointMoreResources MoreResouces
        {
            get
            {
                return new PowerPointMoreResources();
            }
        }
    }
    public class PowerPointExplore
    {
        public void play()
        {
            Browser.Driver.FindElement(By.CssSelector("button.ytp-large-play-button.ytp-button")).Click();
        }
    }
    public class PowerPointBuild
    {
        public void DownloadTheSampleFromGitHub()
        {
            Browser.Driver.FindElement(By.Id("build-downloadFromGithub")).Click();
        }

    }
    public class PowerPointMoreResources
    {
        private bool isAtCorrectResourcePage;
        private CommonMoreResources resource = new CommonMoreResources();

        public void OfficeAddInTypes()
        {
            resource.OfficeAddInTypes();
        }
        public void MoreCodeSamples()
        {
            resource.MoreCodeSamples(Product.PowerPoint);
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
            return resource.IsShowingCorrectResourcePage(Product.PowerPoint);
        }
    }
}
