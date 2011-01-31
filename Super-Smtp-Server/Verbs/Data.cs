using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace SuperSmtpServer.Verbs
{
    class Data : IVerb
    {
        public void Do(Session session)
        {
            // for readability
            var socket = session.Socket;

            socket.SendString(SmtpCommandUtils.SV_DATA_OK);
            
            socket.CommandSeperator = "\r\n.\r\n";
            string rawMessage = socket.GetNextCommand();
            socket.CommandSeperator = "\r\n";

            string encoding = SmtpMailVerbUtils.ParseMessageValue(rawMessage, "Content-Transfer-Encoding");
            string subject = SmtpMailVerbUtils.ParseMessageValue(rawMessage, "Subject");
            string body = SmtpMailVerbUtils.ParseMessageBody(rawMessage);

            if (string.IsNullOrEmpty(body) || string.IsNullOrEmpty(subject))
            {
                socket.SendString(SmtpCommandUtils.SV_UNKNOWN);
            }
            else
            {
                session.Message.Body = body;
                session.Message.Subject = subject;

                // adjust for encoding

                if (encoding == "quoted-printable")
                {
                    // hack for quoted-printable decoding
                    // only does white space
                    session.Message.Body = session.Message.Body.Replace("=0A", "\n");
                    session.Message.Body = session.Message.Body.Replace("=0D", "\r");
                }


                // all done with getting the data
                session.TriggerMessage(session.Message);

                socket.SendString(SmtpCommandUtils.SV_OK);
            }
        }
    }
}
