using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace TestFramework.Office365Page
{
    public class CardTryItOut : BasePage
    {
        private static bool isParameterValueContained = false;
        private static ServiceToTry currentSerivce = new ServiceToTry();

        public void ChooseService(ServiceToTry serviceToTry)
        {
            if (!Browser.Url.Contains("/getting-started/office365apis"))
            {
                Browser.Goto(Browser.BaseAddress + "/getting-started/office365apis#try-it-out");
            }

            int serviceIndex = (int)serviceToTry;
            var service = Browser.Driver.FindElement(By.Id("serviceOption"+serviceIndex));
            Browser.Click(service);
            currentSerivce = serviceToTry;
        }
		
        public bool ChooseServiceValue(object value)
        {
            string serviceValue = null;
            switch (currentSerivce)
            {
                case ServiceToTry.GetMessages:
                    {
                        switch ((GetMessagesValue)value)
                        {
                            case GetMessagesValue.Inbox:
                            case GetMessagesValue.Drafts:
                            case GetMessagesValue.DeletedItems:
                            case GetMessagesValue.SentItems:
                                serviceValue = value.ToString();
                                break;
                            default:
                                serviceValue = null;
                                break;
                        }
                        break;
                    }
                case ServiceToTry.GetFiles:
                    {
                        switch ((GetFilesValue)value)
                        {
                            case GetFilesValue.drive_root_children:
                                serviceValue = "drive/root/children";
                                break;
                            case GetFilesValue.me_drive:
                                serviceValue = "me/drive";
                                break;
                            default:
                                serviceValue = null;
                                break;
                        }
                        break;
                    }
                case ServiceToTry.GetUsers:
                    {
                        switch ((GetUsersValue)value)
                        {
                            case GetUsersValue.me:
                                serviceValue = "me";
                                break;
                            case GetUsersValue.me_manager:
                                serviceValue = "me/manager";
                                break;
                            case GetUsersValue.me_select_skills:
                                serviceValue = "me?$select=skills";
                                break;
                            case GetUsersValue.myOrganization_users:
                                serviceValue = "myOrganization/users";
                                break;
                            default:
                                serviceValue = null;
                                break;

                        }
                        break;
                    }
                case ServiceToTry.GetGroups:
                    {
                        switch ((GetGroupValue)value)
                        {
                            case GetGroupValue.me_memberOf:
                                serviceValue = "me/memberOf";
                                break;
                            case GetGroupValue.members:
                                serviceValue = "groups/41525360-8eca-49ce-bcee-b205cd0aa747/members";
                                break;
                            case GetGroupValue.drive_root_children:
                                serviceValue = "groups/41525360-8eca-49ce-bcee-b205cd0aa747/drive/root/children";
                                break;
                            case GetGroupValue.conversations:
                                serviceValue = "groups/41525360-8eca-49ce-bcee-b205cd0aa747/conversations";
                                break;
                            default:
                                serviceValue = null;
                                break;
                        }
                        break;
                    }
                case ServiceToTry.GetContacts:
                case ServiceToTry.GetEvents:
                default:
                    serviceValue = null;
                    break;
            }

            if (serviceValue != null)
            {
                Browser.SelectElement(Browser.Driver.FindElement(By.Id("valueSelection"))).SelectByText(serviceValue);
                isParameterValueContained = Browser.Driver.FindElement(By.Id("urlValue")).Text.Contains(serviceValue);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ClickTry()
        {
            var tryBtn = Browser.Driver.FindElement(By.Id("invokeurlBtn"));
            Browser.Click(tryBtn);

            //if (Browser.Driver.FindElement(By.Id("responseBody")) != null)
            //{
            //    Browser.Wait(TimeSpan.FromSeconds(3));
            //}
            // var wait = new WebDriverWait(Browser.Driver as IWebDriver, TimeSpan.FromSeconds(5));
            //wait.Until(d => d.FindElement(By.Id("response-container")));
            //WebDriverWait wait = new WebDriverWait((Browser.Driver as IWebDriver), TimeSpan.FromSeconds(10));
            //IWebElement responseContainer = wait.Until(d =>
            //{
            //    return d.FindElement(By.Id("response-container"));
            //});

            try
            {
                var action = new Actions(Browser.Driver as IWebDriver);
                var responseContainer = Browser.Driver.FindElement(By.Id("response-container"));
                action.MoveToElement(responseContainer);
                action.Perform();
            }
            catch (Exception)
            {
                { }
                throw;
            }
        }

        public bool CanGetResponse(object value)
        {
            var responseBody = Browser.Driver.FindElement(By.Id("responseBody"));
            string responseText = responseBody.Text.ToLower();
            string textToBePresent = string.Empty;
            int serviceIndex = (int)currentSerivce;
            switch (serviceIndex)
            {
                case (0):
                    switch ((GetMessagesValue)value)
                    {
                        case GetMessagesValue.Inbox:
                            textToBePresent = @"/mailFolders('Inbox')/messages";
                            break;
                        case GetMessagesValue.Drafts:
                            textToBePresent = @"/mailFolders('Drafts')/messages";
                            break;
                        case GetMessagesValue.DeletedItems:
                            textToBePresent = @"/mailFolders('DeletedItems')/messages";
                            break;
                        case GetMessagesValue.SentItems:
                            textToBePresent = @"/mailFolders('SentItems')/messages";
                            break;
                        default:
                            break;
                    }
                    break;
                case (1):
                    textToBePresent = @"/events";
                    break;
                case (2):
                    textToBePresent = @"/contacts";
                    break;
                case (3):
                    switch ((GetFilesValue)value)
                    {
                        case GetFilesValue.drive_root_children:
                            textToBePresent = @"https://graph.microsoft.com/v1.0/$metadata#drive/root/children";
                            break;
                        case GetFilesValue.me_drive:
                            textToBePresent = @"https://graph.microsoft.com/v1.0/$metadata#drives/$entity";
                            break;
                        default:
                            break;
                    }
                    break;
                case (4):
                    switch ((GetUsersValue)value)
                    {
                        case GetUsersValue.me:
                            textToBePresent = @"https://graph.microsoft.com/v1.0/$metadata#users/$entity";
                            break;
                        case GetUsersValue.me_manager:
                            textToBePresent = @"https://graph.microsoft.com/v1.0/$metadata#directoryObjects/$entity";
                            break;
                        case GetUsersValue.me_select_skills:
                            textToBePresent = @"https://graph.microsoft.com/v1.0/$metadata#users(skills)/$entity";
                            break;
                        case GetUsersValue.myOrganization_users:
                            textToBePresent = @"https://graph.microsoft.com/v1.0/$metadata#users";
                            break;
                        default:
                            break;
                    }
                    break;
                case (5):
                    switch ((GetGroupValue)value)
                    {
                        case GetGroupValue.me_memberOf:
                            textToBePresent = @"https://graph.microsoft.com/v1.0/$metadata#directoryObjects";
                            break;
                        case GetGroupValue.members:
                            textToBePresent = @"https://graph.microsoft.com/v1.0/$metadata#directoryObjects";
                            break;
                        case GetGroupValue.drive_root_children:
                            textToBePresent = @"https://graph.microsoft.com/v1.0/$metadata#groups('41525360-8eca-49ce-bcee-b205cd0aa747')/drive/root/children";
                            break;
                        case GetGroupValue.conversations:
                            textToBePresent = @"https://graph.microsoft.com/v1.0/$metadata#groups('41525360-8eca-49ce-bcee-b205cd0aa747')/conversations";
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            var wait = new WebDriverWait((IWebDriver)Browser.Driver, TimeSpan.FromSeconds(30));
            wait.Until(ExpectedConditions.TextToBePresentInElement(responseBody, textToBePresent));

            return true; 
        }

        public bool UrlContainsServiceName()
        {
            if (IsParameterTableDisplayed())
            {
                switch (currentSerivce)
                {
                    case ServiceToTry.GetMessages:
                    case ServiceToTry.GetEvents:
                    case ServiceToTry.GetContacts:
                        return Browser.Driver.FindElement(By.Id("urlValue")).Text.Contains(EnumExtension.GetDescription(currentSerivce));
                    case ServiceToTry.GetFiles:
                    case ServiceToTry.GetUsers:
                    case ServiceToTry.GetGroups:
                        return isParameterValueContained;
                    default:
                        return false;
                }
            }
            else
            {
                switch (currentSerivce)
                {
                    case ServiceToTry.GetMessages:
                    case ServiceToTry.GetEvents:
                    case ServiceToTry.GetContacts:
                        return Browser.Driver.FindElement(By.Id("urlValue")).Text.Contains(EnumExtension.GetDescription(currentSerivce));
                    case ServiceToTry.GetFiles:
                        return Browser.Driver.FindElement(By.Id("urlValue")).Text.Contains("drive/root/children");
                    case ServiceToTry.GetUsers:
                        return Browser.Driver.FindElement(By.Id("urlValue")).Text.Contains("me");
                    case ServiceToTry.GetGroups:
                        return Browser.Driver.FindElement(By.Id("urlValue")).Text.Contains("me/memberOf");
                    default:
                        return false;
                }
            }
        }

        public bool UrlContainsParameterValue()
        {
            if (IsParameterTableDisplayed())
            {
                return isParameterValueContained;
            }
            else
            {
                switch (currentSerivce)
                {
                    case ServiceToTry.GetMessages:
                        return Browser.Driver.FindElement(By.Id("urlValue")).Text.Contains(GetMessagesValue.Inbox.ToString());
                    case ServiceToTry.GetFiles:
                        return Browser.Driver.FindElement(By.Id("urlValue")).Text.Contains("drive/root/children");
                    case ServiceToTry.GetUsers:
                        return Browser.Driver.FindElement(By.Id("urlValue")).Text.Contains("me");
                    case ServiceToTry.GetGroups:
                        return Browser.Driver.FindElement(By.Id("urlValue")).Text.Contains("me/memberOf");
                    default:
                        return false;
                }
            }
        }

        public bool IsParameterTableDisplayed()
        {
            IWebElement parameterDetail = Browser.Driver.FindElement(By.Id("parameterDetails"));
            return parameterDetail.Text != null && parameterDetail.Text != string.Empty;
        }
    }
}