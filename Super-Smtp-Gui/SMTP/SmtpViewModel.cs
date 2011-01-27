using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SuperSmtpServer;
using System.Net.Mail;
using System.ComponentModel;
using System.Windows.Threading;
using System.Threading;

namespace SuperSmtpGui.SMTP
{
    class SmtpViewModel
    {
        Thread UiThread;
        SmtpServer server;

        public ObservableCollection<MailMessage> _messages = new ObservableCollection<MailMessage>();
        public ObservableCollection<MailMessage> Messages
        {
            get
            {
                return _messages;
            }
        }

        public void Start()
        {
            UiThread = Thread.CurrentThread;
            server = new SmtpServer();
            

            server.MessageRecieved += new MailMessageHandler(server_MessageRecieved);
        }

        public void Stop()
        {
            server.Dispose();
        }

        void server_MessageRecieved(MailMessage m)
        {
            Dispatcher.FromThread(UiThread).Invoke(new MailMessageHandler(MessageRecieved), new object[] { m });
        }

        void MessageRecieved(MailMessage m)
        {
            _messages.Add(m);
        }
    }
}
