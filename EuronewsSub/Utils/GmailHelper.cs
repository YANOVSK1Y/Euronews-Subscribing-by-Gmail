using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;
using System.Configuration;

namespace EuronewsSub.Utils
{
    public class GmailHelper
    {
        static string[] Scopes = {
            GmailService.Scope.GmailReadonly
        };
        static string ApplicationName = "Gmail API Application";

        public static GmailService GetService(string ClientSecretFileName)
        {
            UserCredential credential;
            using (FileStream stream = new FileStream(@$"Resources\ClientCredentials\{ClientSecretFileName}",
                FileMode.Open, FileAccess.Read))
            {
                String FolderPath = Convert.ToString(@"Resources\CredentialsInfo\");

                String FilePath = Path.Combine(FolderPath, "APITokenCredentials");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(FilePath, true)).Result;
            }
            
            GmailService service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            return service;
        }


        public static string MsgNestedParts(IList<MessagePart> Parts)
        {
            string str = string.Empty;
            if (Parts.Count() < 0)
            {
                return string.Empty;
            }
            else
            {
                IList<MessagePart> PlainTestMail  = Parts.Where(x => x.MimeType == "text/plain").ToList();
                IList<MessagePart> AttachmentMail = Parts.Where(x => x.MimeType == "multipart/alternative").ToList();
                IList<MessagePart> TextHtmlMail   = Parts.Where(x => x.MimeType == "text/html").ToList();

                if (TextHtmlMail.Count() > 0)
                {
                    foreach (MessagePart EachPart in TextHtmlMail)
                    {
                        if (EachPart.Parts == null)
                        {
                            if (EachPart.Body != null && EachPart.Body.Data != null)
                            {
                                str += EachPart.Body.Data;
                            }
                        }
                        else
                        {
                            return MsgNestedParts(EachPart.Parts);
                        }
                    }
                }
                if (PlainTestMail.Count() > 0)
                {
                    foreach (MessagePart EachPart in PlainTestMail)
                    {
                        if (EachPart.Parts == null)
                        {
                            if (EachPart.Body != null && EachPart.Body.Data != null)
                            {
                                str += EachPart.Body.Data;
                            }
                        }
                        else
                        {
                            return MsgNestedParts(EachPart.Parts);
                        }
                    }
                }
                if (AttachmentMail.Count() > 0)
                {
                    foreach (MessagePart EachPart in AttachmentMail)
                    {
                        if (EachPart.Parts == null)
                        {
                            if (EachPart.Body != null && EachPart.Body.Data != null)
                            {
                                str += EachPart.Body.Data;
                            }
                        }
                        else
                        {
                            return MsgNestedParts(EachPart.Parts);
                        }
                    }
                }

                return str;
            }
        }
        
        public static string Base64Decode(string Base64Test)
        {
            string EncodTxt = string.Empty;
            EncodTxt = Base64Test.Replace("-", "+");
            EncodTxt = EncodTxt.Replace("_", "/");
            EncodTxt = EncodTxt.Replace(" ", "+");
            EncodTxt = EncodTxt.Replace("=", "+");
            if (EncodTxt.Length % 4 > 0) { EncodTxt += new string('=', 4 - EncodTxt.Length % 4); }
            else if (EncodTxt.Length % 4 == 0)
            {
                EncodTxt = EncodTxt.Substring(0, EncodTxt.Length - 1);
                if (EncodTxt.Length % 4 > 0) { EncodTxt += new string('+', 4 - EncodTxt.Length % 4); }
            }

            byte[] ByteArray = Convert.FromBase64String(EncodTxt);

            return Encoding.UTF8.GetString(ByteArray);
        }
        
    }
}
