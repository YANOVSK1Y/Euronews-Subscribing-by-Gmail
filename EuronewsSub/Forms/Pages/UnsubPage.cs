using Aquality.Selenium.Forms;
using OpenQA.Selenium;
using Aquality.Selenium.Elements.Interfaces; 

namespace EuronewsSub.Forms.Pages
{
    public class UnsubPage: Form
    {
        protected ITextBox Email => ElementFactory.GetTextBox(By.XPath("//input[@type='email']"), "Email");
        protected IButton SubmitButton => ElementFactory.GetButton(By.XPath("//button"), "Submit");
        public UnsubPage(): base(By.XPath("//form"), "Unsub page")
        {
        }
        public void EnterEmail(string UserEmailForSubscribeOnNewsletter) => Email.ClearAndType(UserEmailForSubscribeOnNewsletter);
        public void SubmitEmail() => SubmitButton.Click();
    }
}
