using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSmtpServer.Verbs
{
    class Quit : IVerb
    {
        public void Do(Session session)
        {
            session.Socket.SendString(SmtpCommandUtils.SV_OK);
            session.Socket.Close();
        }
    }
}
