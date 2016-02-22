using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.Office365Page
{
    public class CardDownloadCode : BasePage
    {
        public void DownloadCode()
        {
            var downloadBtn = Browser.Driver.FindElement(By.Id("downloadCodeSampleButton"));
            Browser.Click(downloadBtn);

            // When the card indicates all thing is done is displayed, the click event can be considered as finished.
            Browser.Wait(By.Id("AllSet"));
        }

        public bool IsCodeDownloaded()
        {
            var downloadResult = Browser.Driver.FindElement(By.Id("post-download-instructions"));
            return downloadResult.Displayed;
        }
    }
}
