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
using System.Configuration;
using System.Net;

namespace SuperSmtpGui.SMTP
{
    class SmtpViewModel : INotifyPropertyChanged
    {
        Thread UiThread;
        SmtpServer server;

        public string ServerAddress
        {
            get
            {
                if (server == null)
                {
                    return "Server starting...";
                }
                return ((server.ListeningIp == IPAddress.Any) ? "Any" : server.ListeningIp.ToString()) + " : " + server.ListeningPort;
            }
        }

        public ObservableCollection<MailMessage> _messages = new ObservableCollection<MailMessage>();
        public ObservableCollection<MailMessage> Messages
        {
            get
            {
                return _messages;
            }
        }

        public event EventHandler ServerDetailsChanged;

        public void Start()
        {
            UiThread = Thread.CurrentThread;
            server = new SmtpServer();
            this.PropertyChanged(this, new PropertyChangedEventArgs("ServerAddress"));
            
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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
