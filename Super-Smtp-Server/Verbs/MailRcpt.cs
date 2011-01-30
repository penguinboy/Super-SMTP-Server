using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace SuperSmtpServer.Verbs
{
    class MailRcpt : IVerb
    {
        public void Do(Session session)
        {
            MailAddress address = new MailAddress(SmtpMailVerbUtils.ParseValue(session.Commands.Last()));
            session.Message.To.Add(address);

            session.Socket.SendString(SmtpCommandUtils.SV_OK);
        }
    }
}
