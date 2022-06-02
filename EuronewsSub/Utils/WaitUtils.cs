using System;
using System.Collections.Generic;
using Aquality.Selenium.Browsers;

namespace EuronewsSub.Utils
{
    public static class WaitUtils
    {
        public static string WaitForEmail(string HostEmailAddress, string Sender, string ClientSecretFilename, int MessageWaitTime, int MessagePolimngInterval, DateTime? MessageTime, bool OnlyUnreadMessage)
        {
            string href = string.Empty;
            AqualityServices.ConditionalWait.WaitForTrue(() =>
            {
                if (!string.IsNullOrEmpty(href))
                {
                    return true;
                }
                else
                {
                    try
                    {
                        List<Gmail> gmailResList = GmailManager.GetEmail(
                            HostEmailAddress,
                            Sender,
                            ClientSecretFilename,
                            MessageFromTime: MessageTime,
                            OnlyUnreadMessage: true
                            );

                        href = gmailResList[0].Hrefs[0];
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    return false;
                }
            }, timeout: TimeSpan.FromMinutes(MessageWaitTime), pollingInterval: TimeSpan.FromSeconds(MessagePolimngInterval), message: "Failed to loading href.");
            return href; 
        }
    }
}
