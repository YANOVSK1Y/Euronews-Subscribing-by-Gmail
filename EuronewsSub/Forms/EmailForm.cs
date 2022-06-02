using OpenQA.Selenium;
using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;


namespace EuronewsSub.Forms
{
    public class EmailForm : Form
    {
        protected IButton Submit => ElementFactory.GetButton(By.XPath("//input[@value='Submit']"), "Submit");
        protected ITextBox Email => ElementFactory.GetTextBox(Locator, "Email");
        public EmailForm() : base(By.XPath("//input[@type='email']"), "Email form")
        {
        }
        public void EnterEmail(string UserEmailForSubscribeOnNewsletter)
        {
            Email.ClearAndType(UserEmailForSubscribeOnNewsletter);
        }
        public void SubmitEmail() => Submit.Click();

    }
}
