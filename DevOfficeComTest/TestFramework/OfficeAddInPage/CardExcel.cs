using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TestFramework.OfficeAddInPage
{
    public class CardExcel : BasePage
    {
        public ExcelExplore Explore
        {
            get
            {
                return new ExcelExplore();
            }
        }
        public CardBuild Build
        {
            get
            {
                return new CardBuild(Product.Excel);
            }
        }
        public ExcelMoreResources MoreResouces
        {
            get
            {
                return new ExcelMoreResources();
            }
        }
    }
    public class ExcelExplore
    {

    }
    public class ExcelMoreResources
    {
        private bool isAtCorrectResourcePage;
        private CommonMoreResources resource = new CommonMoreResources();

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
            resource.MoreCodeSamples(Product.Excel);
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
            return resource.IsShowingCorrectResourcePage(Product.Excel);
        }
    }
}
