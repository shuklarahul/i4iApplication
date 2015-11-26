using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mail;
using System.Web;


namespace WorldRef.Models
{
    
    public class SendMailModels
    {//System.Net.Mail.Attachment' to type 'System.Web.Mail.MailAttachment

        public RegisterUserDAO GetUserDetail(string UserName)
        {
            RegisterUserDAO obj = new RegisterUserDAO();
            using ( I4IDBEntities context = new  I4IDBEntities())
            {
                var userDetail = from data in context.RegisterUsers where data.UserNo == UserName select data;
                foreach (RegisterUser item in userDetail)
                {
                    obj.UserFirstName = item.UserFirstName + " " + item.UserLastName;
                    obj.Email = item.Email;
                    obj.phone = item.phone;
                    if (obj.UserFirstName != "")
                        return obj;
                }
            }
            return obj;
        }

        public void SendMail(string toName,string toEmail, string subject,string mailBody)
        {
            try
            {

                bool fSSL = true;
                //string sHost = "smtp.zoho.com";
                string sHost ="smtp.gmail.com";// "smtpout.asia.secureserver.net";
               // mSmtpClient.Port = 80;
                int nPort =  465;
                string sUserName = "i4irevert@gmail.com";
                string sPassword = "i4ieng@123";
                string sFromName = "i4i Application";
                string sFromEmail = "i4irevert@gmail.com";
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
                sMessage += "<tr><td><table width='100%' cellspacing='0'><tr><td style='background-color: #eb5d37;' height='5'></td></tr></table> </td></tr> ";
                sMessage += "<tr><td><table width='100%' cellpadding='30' cellspacing='0'><tr><td style='border-bottom: 1px solid #e0e0e0;'>";
                sMessage += "<h1 style='margin: 0 0 20px 0; font-family: Georgia, Helvetica, Arial, sans-serif;font-size: 17px !important; font-weight: bold; font-style: italic; color: #27a9e3; text-align: left;line-height: 18px;'>";
                sMessage += "Hi " + sToName + ",<br/></h1>";
                sMessage += mailBody + "<br/><br/>" + "http://www.i4i.co.in/" + "<br/><br/>";
               
                sMessage += "<h2 style='margin: 0 0 20px 0; font-size: 14px; color: #eb5d37; text-align: left;font-weight: bold; font-family: Helvetica, Arial, sans-serif; line-height: 22px;'>";
                sMessage += "Sincerely,<br />i4i Engineering</h2></td></tr></table></td></tr></table></td></tr></table> ";

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
        public void SendMailWithAttachment(string toName, string toEmail, string subject, string mailBody,string attachment)
        {
            try
            {

                bool fSSL = true;
                //string sHost = "smtp.zoho.com";
                string sHost = "smtp.gmail.com";// "smtpout.asia.secureserver.net";
                // mSmtpClient.Port = 80;
                int nPort = 465;
                string sUserName = "i4irevert@gmail.com";
                string sPassword = "i4ieng@123";
                string sFromName = "i4i Application";
                string sFromEmail = "i4irevert@gmail.com";
                string sToName = toName;// "maaniitk@gmail.com";
                string sToEmail = toEmail;// "maaniitk@gmail.com";
                string sHeader = subject;

                string sMessage = "";
                sMessage += "<table width='100%' cellpadding='0' cellspacing='0' align='center' bgcolor='#eaedf2' style='background-color: #;'> ";
                sMessage += "<tr><td width='100%' height='20'></td> </tr>";
                sMessage += "<tr><td valign='top'><table class='BoxWrap' width='600' align='center' cellpadding='0' cellspacing='0' >";
                sMessage += "<tr><td class='RespoLogoW'><a href='http://www.i4i.co.in/' style='border: none;'> ";
                sMessage += "<img class='RespoLogo' width='200' src='http://www.i4i.co.in/wp-content/uploads/2014/06/logo.png' alt='' border='0' style='width: 50px;  display: block; border: 0px; outline: none; text-decoration: none;' /> ";
                sMessage += "</a></td><td valign='bottom'><h3 style='text-align: right; font-size: 12px; color: #a4a4a4; font-weight: normal;font-family: Helvetica, Arial, sans-serif;'>";
                sMessage += "</h3></td></tr></table></td></tr>";
                sMessage += "<tr><td width='100%' height='20'></td></tr><tr><td valign='top'><table class='BoxWrap' width='600' align='center' cellpadding='0' cellspacing='0' style='background-color: #ffffff;margin-bottom:40px;'>";
                sMessage += "<tr><td><table width='100%' cellspacing='0'><tr><td style='background-color: #eb5d37;' height='5'></td></tr></table> </td></tr> ";
                sMessage += "<tr><td><table width='100%' cellpadding='30' cellspacing='0'><tr><td style='border-bottom: 1px solid #e0e0e0;'>";
                sMessage += "<h1 style='margin: 0 0 20px 0; font-family: Georgia, Helvetica, Arial, sans-serif;font-size: 17px !important; font-weight: bold; font-style: italic; color: #27a9e3; text-align: left;line-height: 18px;'>";
                sMessage += "Hi " + sToName + ",<br/></h1>";
                sMessage += mailBody + "<br/><br/>" + "http://www.i4i.co.in/" + "<br/><br/>";

                sMessage += "<h2 style='margin: 0 0 20px 0; font-size: 14px; color: #eb5d37; text-align: left;font-weight: bold; font-family: Helvetica, Arial, sans-serif; line-height: 22px;'>";
                sMessage += "Sincerely,<br />i4i Engineering</h2></td></tr></table></td></tr></table></td></tr></table> ";

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
                MailAttachment attachFile = new MailAttachment (attachment);
                
                Mail.Attachments.Add(attachFile);
                

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
    }
}