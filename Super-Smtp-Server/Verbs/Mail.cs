using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace SuperSmtpServer.Verbs
{
    class Mail : IVerb
    {
        public void Do(Session session)
        {
            session.Message = new MailMessage();

            session.Message.From = new MailAddress(SmtpMailVerbUtils.ParseValue(session.Commands.Last()));

            session.Socket.SendString(SmtpCommandUtils.SV_OK);
        }
    }
}
