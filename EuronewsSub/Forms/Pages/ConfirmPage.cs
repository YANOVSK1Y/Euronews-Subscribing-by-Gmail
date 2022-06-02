using OpenQA.Selenium;
using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;

namespace EuronewsSub.Forms.Pages
{
    public class ConfirmPage: Form 
    {
        protected IButton BackToTHeSite => ElementFactory.GetButton(By.XPath("//*[@href='//www.euronews.com']"), "Back to the site");
        public ConfirmPage(): base(By.XPath("//*[contains(text(), 'successfully')]"), "Confirm page")
        {
        }
        public void BackToTheCite() => BackToTHeSite.Click();
    }
}
