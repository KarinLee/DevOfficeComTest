using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.OfficeAddInPage
{
    public class CardExplore : BasePage
    {
        public void ExploreProduct(Product product)
        {
            if (!Browser.Url.Contains("/getting-started/office365apis"))
            {
                Browser.Goto(Browser.BaseAddress + "/getting-started/office365apis#explore");
            }

            switch (product)
            {
                case Product.Excel:
                    {
                        break;
                    }
                case Product.Outlook:
                case Product.PowerPoint:
                case Product.Word:
                    {
                        break;
                    }
            }
        }
    }
}
