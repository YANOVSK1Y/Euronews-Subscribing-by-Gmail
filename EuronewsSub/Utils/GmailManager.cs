using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Aquality.Selenium;
using Aquality.Selenium.Browsers; 

namespace EuronewsSub.Utils
{
    public class GmailManager
    {
        public static List<Gmail> GetEmail(string HostEmailAddress, string Sender, string ClientSecretFilename, DateTime? MessageFromTime = null, bool OnlyUnreadMessage = false)
        {
            List<Gmail> ResultEmailList = new List<Gmail>();
            // latest message takes by default 
            if (!MessageFromTime.HasValue)
            {
                MessageFromTime = DateTime.Now;
            }
            
            GmailService GmailService = GmailHelper.GetService(ClientSecretFilename);
            
            UsersResource.MessagesResource.ListRequest ListRequest = GmailService.Users.Messages.List(HostEmailAddress);
            ListRequest.LabelIds = "INBOX";
            ListRequest.IncludeSpamTrash = false;
            if (OnlyUnreadMessage) ListRequest.Q = "is:unread";

            ListMessagesResponse emailListResponse = ListRequest.Execute();
            List<String> ALlhrefs; 

            if (emailListResponse != null && emailListResponse.Messages != null)
            {
                foreach (var email in emailListResponse.Messages)
                {
                    
                    var emailInfoRequest = GmailService.Users.Messages.Get(HostEmailAddress, email.Id);
                    var emailInfoResponse = emailInfoRequest.Execute();
                    string from = String.Empty;
                    string date = String.Empty;
                    string subject = String.Empty;
                    string To = String.Empty;

                    if (emailInfoResponse != null)
                    {
                        from = String.Empty;
                        To = String.Empty; 
                        date = String.Empty;
                        subject = String.Empty;
                        foreach (var headerItem in emailInfoResponse.Payload.Headers)
                        {
                            if (headerItem.Name == "Date")
                            {
                                date = headerItem.Value;
                            }
                            else if (headerItem.Name == "From")
                            {
                                if (!string.IsNullOrEmpty(headerItem.Value))
                                {
                                    from = headerItem.Value;
                                    string[] fromSplit = from.Split(" ");
                                    from = fromSplit[fromSplit.Length - 1];
                                    from = from.Replace("<", string.Empty);
                                    from = from.Replace(">", string.Empty);
                                }
                            }
                            else if (headerItem.Name == "Subject")
                            {
                                subject = headerItem.Value;
                            }
                            else if (headerItem.Name == "To")
                            {
                                To = headerItem.Value;
                            }
                        }
                    }

                    string MailBody = string.Empty;

                    if (emailInfoResponse.Payload.Parts == null && emailInfoResponse.Payload.Body.Data != null)
                    {
                        MailBody = emailInfoResponse.Payload.Body.Data;
                    }
                    else
                    {
                        MailBody = GmailHelper.MsgNestedParts(emailInfoResponse.Payload.Parts);
                    }

                    string ReadableText = string.Empty;
                    
                    ReadableText = GmailHelper.Base64Decode(MailBody);
                    ALlhrefs = StringManager.ExtractAllHrefs(ReadableText); 
                   

                    Gmail GMail = new Gmail();
                    GMail.Hrefs = ALlhrefs;
                    if (!string.IsNullOrEmpty(ReadableText))
                    {
                        GMail.From = from;
                        GMail.Body = ReadableText;
                        GMail.To = To;
                        GMail.DateTime = Convert.ToDateTime(date);
                    }
                    
                    if (GMail.From.ToLower().Contains(Sender.ToLower()) && GMail.DateTime > MessageFromTime) ResultEmailList.Add(GMail);

                }
            }
            return ResultEmailList;  
        }
    } 
}
