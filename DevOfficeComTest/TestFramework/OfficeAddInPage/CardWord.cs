using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.OfficeAddInPage
{
    public class CardWord : BasePage
    {
        public WordExplore Explore
        {
            get
            {
                return new WordExplore();
            }
        }
        public CardBuild Build
        {
            get
            {
                return new CardBuild(Product.Word);
            }
        }
        public WordMoreResources MoreResouces
        {
            get
            {
                return new WordMoreResources();
            }
        }
    }
    public class WordExplore
    {
        public void play()
        {
            Browser.Driver.FindElement(By.CssSelector("button.ytp-play-button.ytp-button")).Click();
        }
    }
    public class WordMoreResources
    {
        private bool isAtCorrectResourcePage;
        private CommonMoreResources resource = new CommonMoreResources();

        public void OfficeAddInTypes()
        {
            resource.OfficeAddInTypes();
        }
        public void MoreCodeSamples()
        {
            resource.MoreCodeSamples(Product.Word);
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
            return resource.IsShowingCorrectResourcePage(Product.Word);
        }
    }
}
