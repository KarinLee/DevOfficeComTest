using OpenQA.Selenium;
using System;

namespace TestFramework.OfficeAddInPage
{
    public class CardBuild : BasePage
    {
        private Product product;
        
        public bool IsShowingBuildPage()
        {
            bool canSwitchWindow = Browser.SwitchToNewWindow();
            bool isCorrectBuildPage = false;
            int retryCount = Int32.Parse(Utility.GetConfigurationValue("RetryCount"));
            int waitTime = Int32.Parse(Utility.GetConfigurationValue("WaitTime"));

            if (canSwitchWindow)
            {
                int i = 0;
                switch (product)
                {
                    case Product.Excel:
                        {
                            do
                            {
                                Browser.Wait(TimeSpan.FromSeconds(waitTime));
                                i++;
                                isCorrectBuildPage = Browser.Title.Contains("Task Pane Add-in Sample - Excel - Napa");
                            } while (i < retryCount && !isCorrectBuildPage);
                            break;
                        }
                    case Product.Outlook:
                        {
                            do
                            {
                                Browser.Wait(TimeSpan.FromSeconds(waitTime));
                                i++;
                                isCorrectBuildPage = Browser.Title.Contains("Mail Read Sample - Napa");
                            } while (i < retryCount && !isCorrectBuildPage);
                            break;
                        }
                    case Product.PowerPoint:
                    case Product.Word:
                        {
                            do
                            {
                                Browser.Wait(TimeSpan.FromSeconds(waitTime));
                                i++;
                                isCorrectBuildPage = Browser.Title.Contains("OfficeDev/Add-in-TaskPane-Sample");
                            } while (i < retryCount && !isCorrectBuildPage);
                            break;
                        }
                }
                Browser.SwitchBack();
            }
            return isCorrectBuildPage;
        }

        public CardBuild(Product product)
        {
            this.product = product;
        }
    }
}
