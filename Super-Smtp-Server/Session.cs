using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace SuperSmtpServer
{
    public class Session
    {
        public SimpleSocket Socket;
        public event MailMessageHandler MessageRecieved;
        public void TriggerMessage(MailMessage m) { MessageRecieved(m); }

        public MailMessage Message;

        public Session(SimpleSocket socket, MailMessageHandler messageEvent)
        {
            this.Socket = socket;
            this.MessageRecieved += messageEvent;
            
            Socket.SendString(SmtpCommandUtils.SV_GREET);
        }

        public List<string> Commands = new List<string>();
        
    }
}
