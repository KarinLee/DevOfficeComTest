using System;
using OpenQA.Selenium;

namespace TestFramework.Office365Page
{
    public class CardRegisterApp : BasePage
    {
        public SigninCommand SigninAs(string userName)
        {
            if (!Browser.Url.Contains("/getting-started/office365apis"))
            {
                Browser.Goto(Browser.BaseAddress + "/Getting-Started/office365Apis#register-app");
            }

            return new SigninCommand(userName);
        }

        public RegisterCommand Register()
        {
            return new RegisterCommand();
        }

        /// <summary>
        /// Choose to sign in later in the register app card
        /// </summary>
        public void SigninLater()
        {
            var signedinLater = Browser.Driver.FindElement(By.Id("app-reg-signin-later"));
            Browser.Click(signedinLater);
        }

        public bool IsSignedin(string userName)
        {
            Browser.Wait(TimeSpan.FromSeconds(2));
            var registrationForm = Browser.Driver.FindElement(By.Id("registration-form"));
            return registrationForm.Displayed;
        }

        public bool IsRegistered()
        {
            Browser.Wait(TimeSpan.FromSeconds(2));
            var registrationResult = Browser.Driver.FindElement(By.Id("registration-result"));
            IWebElement resultText = registrationResult.FindElement(By.TagName("div"));
            return (registrationResult.Displayed && resultText.Text.Equals("Registration Successful!"));
        }
    }

    public class RegisterCommand
    {
        private string appName;

        public RegisterCommand Register()
        {
            return this;
        }

        public void WithAppName(string name)
        {
            this.appName = name;
            var appNameInput = Browser.Driver.FindElement(By.Id("appNameField"));
            appNameInput.SendKeys(appName);
            Browser.Wait(TimeSpan.FromSeconds(0.5));

            var registerBtn = Browser.Driver.FindElement(By.Id("register-button"));
            Browser.Click(registerBtn);
            Browser.Wait(TimeSpan.FromSeconds(2));
        }
    }

    public class SigninCommand
    {
        private readonly string userName;
        private string password;

        public SigninCommand(string userName)
        {
            this.userName = userName;
        }

        public SigninCommand WithPassword(string password)
        {
            this.password = password;
            return this;
        }

        public void Signin()
        {
            var signinGoBtn = Browser.Driver.FindElement(By.Id("app-reg-signin"));
            Browser.Click(signinGoBtn);
            Browser.Wait(TimeSpan.FromSeconds(1));

            var signinInput = Browser.Driver.FindElement(By.Name("login"));
            signinInput.SendKeys(userName);
            var passwordInput = Browser.Driver.FindElement(By.Name("passwd"));
            passwordInput.SendKeys(password);
            var signinBtn = Browser.Driver.FindElement(By.Id("cred_sign_in_button"));
            Browser.Wait(TimeSpan.FromSeconds(1));

            Browser.Click(signinBtn);
        }
    }
}
