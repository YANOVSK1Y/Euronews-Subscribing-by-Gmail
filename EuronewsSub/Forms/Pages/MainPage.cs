using OpenQA.Selenium;
using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;


namespace EuronewsSub.Forms.Pages
{
    public class MainPage: Form
    {
        protected IButton Newsletters => ElementFactory.GetButton(By.XPath("//span[@class='c-internal-links__text' and contains(text(), 'Newsletters')]"), "News letters");
        protected IButton PrivacyAccept => ElementFactory.GetButton(By.Id("didomi-notice-agree-button"), "Privacy"); 
        protected IButton NewslettersAlternative => ElementFactory.GetButton(By.XPath("//a[contains(@class, 'c-footer-sitemap__list-item--follow') and contains(text() , 'Newsletters')]"), "News letters alternative");
        public MainPage() : base(By.XPath("//section[@data-event='barre-now-tags']"), "Main page")
        {
        }
        public void AcceptPrivacy() => PrivacyAccept.Click();
        
        public void OpenNewslettersPage()
        {
            if (Newsletters.State.IsDisplayed)
            {
                Newsletters.Click();
            }
            else
            {
                NewslettersAlternative.Click();
            }
        }
    }

}
