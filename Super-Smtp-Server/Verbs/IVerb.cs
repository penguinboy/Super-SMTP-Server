using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSmtpServer.Verbs
{
    interface IVerb
    {
        void Do(Session session);
    }
}
