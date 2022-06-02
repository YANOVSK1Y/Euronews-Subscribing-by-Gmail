using NUnit.Framework;
using Aquality.Selenium.Browsers;
using System.Threading;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using EuronewsSub.Utils;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using EuronewsSub.Forms.Pages;
using EuronewsSub.Forms;
using System;
using System.Collections.Generic;

namespace EuronewsSub
{
    public class Tests
    {

        protected Browser Browser;
        protected MainPage MainPage;
        protected NewslettersPage NewslettersPage;
        protected EmailForm EmailForm;
        protected ConfirmPage ConfirmPage;
        protected UnsubPage UnsubPage; 
        JObject ConfigFile;
        JObject TestDataFile;

        [SetUp]
        public void Setup()
        {
            Browser = AqualityServices.Browser;
            MainPage = new MainPage();
            ConfirmPage = new ConfirmPage(); 
            NewslettersPage = new NewslettersPage();
            EmailForm = new EmailForm();
            UnsubPage = new UnsubPage(); 
            ConfigFile = FileReader.ReadFile(@"Resources/config.json");
            TestDataFile = FileReader.ReadFile(@"Resources/TestData.json");
        }

        [Test]
        public void Test1()
        {
            
            Browser.GoTo(ConfigFile.GetValue("StartUrl").ToString());
            
            Assert.IsTrue(MainPage.State.IsExist, "Main page is not exist");
            MainPage.AcceptPrivacy();
            MainPage.OpenNewslettersPage();
            Assert.IsTrue(NewslettersPage.State.IsExist, "Newsletters page is not exist");

            var SubmiteIndex = NewslettersPage.SubmitOnNewsletter();
            EmailForm.EnterEmail(TestDataFile.GetValue("UserPost").ToString());
            EmailForm.SubmitEmail();

            string SubmitionHref = NewslettersPage.GetSubmitionUrl(
                        TestDataFile.GetValue("UserPost").ToString(), 
                        "Euronews", 
                        ConfigFile.GetValue("ClientSecretFileName").ToString(),
                        MessageWaitTime: 2, 
                        MessagePolimngInterval: 1
                    );
            
            Browser.GoTo(SubmitionHref);

            Assert.IsTrue(ConfirmPage.State.IsExist, "Confirm page is not exist");
            ConfirmPage.BackToTheCite();
            Assert.IsTrue(MainPage.State.IsExist, "Main page is not exist");
            MainPage.OpenNewslettersPage();

            Assert.IsTrue(NewslettersPage.State.IsExist, "Newsletters page is not exist");

            NewslettersPage.SeePreview(SubmiteIndex);

            Browser.Driver.SwitchTo().Frame(((int)SubmiteIndex));

            string UnsubLink = NewslettersPage.GetUnsubLink();
            
            Browser.GoTo(UnsubLink);
            
            Assert.IsTrue(UnsubPage.State.IsExist, "Unsub page is not exist");

            UnsubPage.EnterEmail(TestDataFile.GetValue("UserPost").ToString());

            UnsubPage.SubmitEmail();

            string UnsubMitionUrl = NewslettersPage.GetSubmitionUrl(
                        TestDataFile.GetValue("UserPost").ToString(),
                        "Euronews",
                        ConfigFile.GetValue("ClientSecretFileName").ToString(),
                        MessageWaitTime: 1,
                        MessagePolimngInterval: 1
                    );
            Assert.IsEmpty(UnsubMitionUrl, "ERROR. Have some unsub url from message");


        }
        
        
        [TearDown]
        public void TearDown()
        {
            AqualityServices.Browser.Quit();
        }
    }
}