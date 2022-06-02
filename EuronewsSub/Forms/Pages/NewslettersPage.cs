using OpenQA.Selenium;
using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using System.Collections.Generic;
using EuronewsSub.Utils;
using System;


namespace EuronewsSub.Forms.Pages
{
    public class NewslettersPage: Form 
    {   
        enum NewsLetters{

        }
        protected IList<ILabel> AllNewsletters => ElementFactory.FindElements<ILabel>(By.XPath("//div[contains(@class, 'bg-white') and contains(@class, 'shadow-lg')]//label[contains(@class, 'unchecked-label')]"));
        protected IList<ILabel> NewslettersPreviews => ElementFactory.FindElements<ILabel>(By.XPath("//div[contains(@class, 'bg-white') and contains(@class, 'shadow-lg')]//a"));
        protected ILabel UnsubscribeNewsletter => ElementFactory.GetLabel(By.XPath("//a[contains(text(), 'unsubscribe')]"), "Unsubscribe");
        protected string Frame(EuroNewsPreviews NewsletterName) => $"//*[@id='newsletters-form']//*[contains(@id, '{NewsletterName}')]//iframe";
        public enum EuroNewsPreviews
        {
            Today = 0,
            Briefing = 1, 
            Watch = 2, 
            Culture = 3, 
            Next = 4,
            Green = 5,
            Travel = 6, 
            Special 
        }; 
        public NewslettersPage(): base(By.XPath("//span[contains(text(), 'Our newsletters')]"), "Newsletters page")
        {
        }
        public EuroNewsPreviews SubmitOnNewsletter() 
        {
            EuroNewsPreviews Preview = new EuroNewsPreviews(); 
            var Newsletter = RandomListSelector.RandomEnumValue<EuroNewsPreviews>();
            AllNewsletters[((int)Newsletter)].Click();
            return Newsletter; 
        }
        
        public string GetSubmitionUrl(string HostEmailAddress, string Sender, string ClientSecretFilename, int MessageWaitTime = 1, int MessagePolimngInterval = 2, DateTime? MessageTime = null, bool OnlyUnreadMessage = false)
        {
            return WaitUtils.WaitForEmail(HostEmailAddress, Sender, ClientSecretFilename, MessageWaitTime, MessagePolimngInterval, MessageTime, OnlyUnreadMessage);
        }
        public void  SeePreview(EuroNewsPreviews preview) => NewslettersPreviews[((int)preview)].Click();
        
        public string GetUnsubLink() 
        { 
            return UnsubscribeNewsletter.GetAttribute("href"); 
        }

        public string GerFrame(EuroNewsPreviews NewsletterName)
        {
            if (NewsletterName == EuroNewsPreviews.Special)
            {
                return "This page has not preview";
            }
            return Frame(NewsletterName);
        }
        


    }
}
