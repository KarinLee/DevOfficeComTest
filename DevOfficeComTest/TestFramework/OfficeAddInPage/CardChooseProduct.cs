using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.OfficeAddInPage
{
    public class CardChooseProduct : BasePage
    {
        public void ChooseProduct(Product productName)
        {
            if (!Browser.Url.Contains("/getting-started/addins"))
            {
                Browser.Goto(Browser.BaseAddress + "/getting-started/addins#selectapp");
            }

            var product = Browser.Driver.FindElement(By.Id(productName.ToString()));
            Browser.Click(product);

            // Need refactor: Sometimes case failed for the product explore text is not changed in time
            Browser.Wait(TimeSpan.FromSeconds(2));
        }

        public bool IsShowingProductExplore(Product productName)
        {
            string product = productName.ToString().ToLower();
            try
            {
                var exploreInfoOfProduct = Browser.Driver.FindElement(By.CssSelector("#embedContents>div>h1"));
                return exploreInfoOfProduct.Text.ToLower().Contains(product);
            }
            catch (NoSuchElementException)
            {
                var exploreInfoOfProduct = Browser.Driver.FindElement(By.CssSelector("#embedContents>h1"));
                return exploreInfoOfProduct.Text.ToLower().Contains(product);
            }
        }

        public bool IsShowingVideo(Product productName)
        {
            var videoIframe = Browser.FindElement(By.CssSelector("#embedContents>iframe"));
            string videoUrl = videoIframe.GetAttribute("src"); 
            switch (productName)
            {
                case Product.Excel:
                    return videoUrl == "https://www.youtube.com/embed/aNHPSGUfZq0";
                case Product.Outlook:
                    return videoUrl == "https://www.youtube.com/embed/Hov8f_VniCc";
                case Product.PowerPoint:
                    return videoUrl == "https://www.youtube.com/embed/tFq_dl1yUUc"; 
                case Product.Word:
                    return videoUrl == "https://www.youtube.com/embed/S23rcdX96Wc";
                default:
                    return false;
            }

        }
    }
}
