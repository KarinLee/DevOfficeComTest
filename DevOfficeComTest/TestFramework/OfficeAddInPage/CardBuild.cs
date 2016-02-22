using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.OfficeAddInPage
{
    public class CardBuild : BasePage
    {
        private Product product;
        public void StartBuilding()
        {
            if (!Browser.Url.Contains("/getting-started/addins"))
            {
                Browser.Goto(Browser.BaseAddress + "/getting-started/addins#build");
            }

            switch (product)
            {
                case Product.Excel:
                case Product.Outlook:
                    {
                        var buildBtn = Browser.Driver.FindElement(By.Id("more-playground"));
                        Browser.Click(buildBtn);
                        break;
                    }
                case Product.PowerPoint:
                case Product.Word:
                    {
                        var buildBtn = Browser.Driver.FindElement(By.Id("build-downloadFromGithub"));
                        Browser.Click(buildBtn);
                        break;
                    }
            }

            // Need refactor: The waiting time is not stable
            Browser.Wait(TimeSpan.FromSeconds(10));

        }
        public bool IsShowingBuildPage()
        {
            bool canSwitchWindow = Browser.SwitchToNewWindow();
            bool isCorrectBuildPage = false;
            if (canSwitchWindow)
            {
                switch (product)
                {
                    case Product.Excel:
                        {
                            isCorrectBuildPage = Browser.Title.Contains("Task Pane Add-in Sample - Excel - Napa");
                            break;
                        }
                    case Product.Outlook:
                        {
                            isCorrectBuildPage = Browser.Title.Contains("Mail Read Sample - Napa");
                            break;
                        }
                    case Product.PowerPoint:
                    case Product.Word:
                        {
                            isCorrectBuildPage = Browser.Title.Contains("OfficeDev/Add-in-TaskPane-Sample");
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
