/* 
 * Programmers: Jack Kennedy
 * Purpose: Sends Emails
 * Inputs: email body, log file
 * Outputs: emails jauntlet95@gmail.com
 */

using UnityEngine;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class Email : MonoBehaviour
{
    #region Variables

    const string email = "jauntlet95@gmail.com";
    const string password = "crlqaqyzfzmzienz"; // actual password is StrongerPassword123! in case anyone was wondering :)

    #endregion

    #region Custom methods

    public void SendEmail(string body)
    { 
        MailMessage mail = new MailMessage();

        mail.From = new MailAddress("stupiduser@gmail.com");
        mail.To.Add(new MailAddress("jauntlet95@gmail.com"));

        mail.Subject = "Jauntlet Crash";
        mail.Body = body;

        mail.Attachments.Add(new System.Net.Mail.Attachment(Application.dataPath + " / Debug / log.txt"));

        // use gmails smtp
        SmtpClient smtp = new SmtpClient("smtp.gmail.com");
        smtp.Port = 587; // gmails port
        smtp.Timeout = 1000;
        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtp.UseDefaultCredentials = false;
        smtp.EnableSsl = true;
        // use my email and password
        smtp.Credentials = new System.Net.NetworkCredential(email, password) as ICredentialsByHost;

        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

        smtp.Send(mail);
    }

    #endregion
}