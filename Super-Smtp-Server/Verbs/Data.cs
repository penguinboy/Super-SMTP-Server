using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

                // all done with getting the data
                session.TriggerMessage(session.Message);

                socket.SendString(SmtpCommandUtils.SV_OK);
            }
        }
    }
}
