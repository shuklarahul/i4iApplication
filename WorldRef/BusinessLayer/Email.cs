using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Text;
using System.Security.Cryptography;

namespace WorldRef.BusinessLayer
{
    public class Email
    {

        /// <summary>
        /// Get the username form web config
        /// </summary>
        public string EmailUserName
        {
            get { return ConfigurationManager.AppSettings["EMAIL"].ToString(); }
        }

        /// <summary>
        /// Get the email password from the Web Config.
        /// </summary>
        public string EmailPassword
        {
            get { return ConfigurationManager.AppSettings["PASSWORD"].ToString(); }
        }


        /// <summary>
        /// Common method to send the mail .
        /// All mail be send from this Email Function
        /// </summary>
        /// <param name="toName"></param>
        /// <param name="toEmail"></param>
        /// <param name="subject"></param>
        /// <param name="mailBody"></param>
        public void SendMail(string toName, string toEmail, string subject, string mailBody)
        {
            try
            {

                bool fSSL = true;
                //string sHost = "smtp.zoho.com";
                string sHost = "smtp.gmail.com";// "smtpout.asia.secureserver.net";
                // mSmtpClient.Port = 80;
                int nPort = 465;
                string sUserName = EmailUserName;
                string sPassword = EmailPassword;
                string sFromName = "i4i Application";
                string sFromEmail = EmailUserName;
                string sToName = toName;// "maaniitk@gmail.com";
                string sToEmail = toEmail;// "maaniitk@gmail.com";
                string sHeader = subject;

                string sMessage = "";
                sMessage += "<table width='100%' cellpadding='0' cellspacing='0' align='center' bgcolor='#eaedf2' style='background-color: #;'> ";
                sMessage += "<tr><td width='100%' height='20'></td> </tr>";
                sMessage += "<tr><td valign='top'><table class='BoxWrap' width='600' align='center' cellpadding='0' cellspacing='0' >";
                sMessage += "<tr><td class='RespoLogoW'><a href='http://www.i4i.co.in/' style='border: none;'> ";
                sMessage += "<img class='RespoLogo' width='200' src='http://www.i4i.co.in/wp-content/uploads/2014/06/logo.png' alt='' border='0' style='width: 50px;  display: block; border: 0px; outline: none; text-decoration: none;' /> ";
                sMessage += "</a></td><td valign='bottom'><h3 style='text-align: right; font-size: 12px; color: #a4a4a4; font-weight: normal;font-family: Helvetica, Calibri (Body), sans-serif;'>";
                sMessage += "</h3></td></tr></table></td></tr>";
                sMessage += "<tr><td width='100%' height='20'></td></tr><tr><td valign='top'><table class='BoxWrap' width='600' align='center' cellpadding='0' cellspacing='0' style='background-color: #ffffff;margin-bottom:40px;'>";
                sMessage += "<tr><td><table width='100%' cellspacing='0'><tr><td style='background-color: #f7a4a4;' height='15'></td></tr></table> </td></tr> ";
                sMessage += "<tr><td><table width='100%' cellpadding='30' cellspacing='0'><tr><td style='border-bottom: 1px solid #e0e0e0;'>";
                sMessage += "<h1 style='margin: 0 0 20px 0;font-weight: bold; font-family: Calibri;font-size: 18px !important;   color: #006699; text-align: left;line-height: 18px;'>";

                if (string.IsNullOrEmpty(sToName))
                {
                    sMessage += "Hi" + sToName + ",<br/></h1>";
                }
                else
                {
                    sMessage += "Hi " + sToName + ",<br/></h1>";

                }


                sMessage += mailBody + "<br/><br/>";

                sMessage += "<div style='margin: 0 0 -2px 0; font-size: 14px; color: #006699; text-align: left; font-family: Calibri; line-height: 22px;'>";
                sMessage += "Sincerely,</div><div style='margin: 0 0 -17px 0;font-weight: bold; font-size: 16px; color: #006699; text-align: left;font-weight: bold; font-family: Calibri; line-height: 22px;'>i4i Engineering</div><div style='margin: 0 0 -17px 0; font-size: 14px; color: #006699; text-align: left; font-family: Calibri Light; line-height: 22px;'><a href = 'http://www.i4i.co.in/'>www.i4i.co.in </a></div></td></tr></table></td></tr></table></td></tr></table> ";

                if (sToName.Length == 0)
                    sToName = sToEmail;
                if (sFromName.Length == 0)
                    sFromName = sFromEmail;

                System.Web.Mail.MailMessage Mail = new System.Web.Mail.MailMessage();
                Mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserver"] = sHost;
                Mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"] = 2;

                Mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverport"] = nPort.ToString();
                if (fSSL)
                    Mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpusessl"] = "true";

                if (sUserName.Length == 0)
                {
                    //Ingen auth
                }
                else
                {
                    Mail.Fields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"] = 1;
                    Mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendusername"] = sUserName;
                    Mail.Fields["http://schemas.microsoft.com/cdo/configuration/sendpassword"] = sPassword;
                }

                Mail.To = sToEmail;
                Mail.From = sFromEmail;
                Mail.Subject = sHeader;
                Mail.Body = sMessage;
                Mail.BodyFormat = System.Web.Mail.MailFormat.Html;

                System.Web.Mail.SmtpMail.SmtpServer = sHost;
                System.Web.Mail.SmtpMail.Send(Mail);
                Mail = null;

                //updateMailUserStatus(dr);//Updating MailSendServiceData Table to update sentStatus=true and Inserting reward Details

            }
            catch (Exception ex)
            {

                //throw new Exception(ex.Message);                
            }
        }

        /// <summary>
        /// It will generate the random password.
        /// </summary>
        /// <returns></returns>
        public string GeneratePassword()
        {
            string strPwdchar = "abcdefghijklmnopqrstuvwxyz0123456789#+@&$ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string strPwd = "";
            Random rnd = new Random();
            for (int i = 0; i <= 7; i++)
            {
                int iRandom = rnd.Next(0, strPwdchar.Length - 1);
                strPwd += strPwdchar.Substring(iRandom, 1);
            }
            return strPwd;
        }

        /// <summary>
        /// It will generate the username 
        /// </summary>
        /// <returns></returns>
        public string getUserNo()
        {
            string UserName = string.Empty;

            UserName = GetUniqueKey(7);
            using (I4IDBEntities context = new I4IDBEntities())
            {
                var isexistKey = context.RegisterUsers.Where(m => m.UserNo == UserName).FirstOrDefault();
                if (isexistKey != null)
                {
                    getUserNo();
                }
            }
            return UserName;
        }

        /// <summary>
        /// It will generate the unique key 
        /// </summary>
        /// <param name="maxSize"></param>
        /// <returns></returns>
        public static string GetUniqueKey(int maxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            data = new byte[maxSize];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(maxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
    }
}