using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSmtpServer.Verbs
{
    class Helo : IVerb
    {
        public void Do(Session session)
        {
            session.Socket.SendString(SmtpCommandUtils.SV_GREET);
        }
    }
}
