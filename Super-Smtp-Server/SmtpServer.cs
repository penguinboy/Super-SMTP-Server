using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.Mail;
using System.Threading;
using SuperSmtpServer.Verbs;

namespace SuperSmtpServer
{

    public delegate void MailMessageHandler(MailMessage m);

    public class SmtpServer : IDisposable
    {
        public delegate void SmtpVerbHandler(Session s);
        public Dictionary<string, SmtpVerbHandler> VerbStore;


        public event MailMessageHandler MessageRecieved;
        public void OnMessageRecieved(MailMessage m)
        {
            MessageRecieved(m);
        }

        TcpListener listener;
        Thread processingThread;
        List<SimpleSocket> SocketPool;
        bool Running = true;

        public SmtpServer()
        {
            // add the standard verbs to support
            this.VerbStore = new KeyValuePair<string, SmtpVerbHandler>[] {
                new KeyValuePair<string, SmtpVerbHandler>(SmtpCommandUtils.CL_HELO, new SmtpVerbHandler(new Helo().Do)),
                new KeyValuePair<string, SmtpVerbHandler>(SmtpCommandUtils.CL_EHLO, new SmtpVerbHandler(new Ehlo().Do)),
                new KeyValuePair<string, SmtpVerbHandler>(SmtpCommandUtils.CL_MAIL, new SmtpVerbHandler(new Mail().Do)),
                new KeyValuePair<string, SmtpVerbHandler>(SmtpCommandUtils.CL_MAIL_RCPT, new SmtpVerbHandler(new MailRcpt().Do)),
                new KeyValuePair<string, SmtpVerbHandler>(SmtpCommandUtils.CL_DATA, new SmtpVerbHandler(new Data().Do)),
                new KeyValuePair<string, SmtpVerbHandler>(SmtpCommandUtils.CL_QUIT, new SmtpVerbHandler(new Quit().Do))
            }.ToDictionary(kv => kv.Key, kv => kv.Value);
            
            SocketPool = new List<SimpleSocket>();

            listener = new TcpListener(IPAddress.Any, 25);

            listener.Start();

            processingThread = new Thread(new ThreadStart(Run));
            processingThread.Start();
        }

        private void Run()
        {
            Console.WriteLine("Starting thread");
            while (Running)
            {
                if (listener.Pending())
                {
                    var sSocket = new SimpleSocket(listener.AcceptSocket());
                    SocketPool.Add(sSocket);
                    ProcessSocket(sSocket);
                    SocketPool.Remove(sSocket);
                }
                Thread.Sleep(50);
            }
        }

        public void Dispose()
        {
            Running = false;
            
            foreach (SimpleSocket s in SocketPool)
            {
                s.Close();
            }

            processingThread.Abort();
        }

        private void ProcessSocket(SimpleSocket s)
        {
            var session = new Session(s, new MailMessageHandler(OnMessageRecieved));

            while (s.Connected && Running)
            {
                ProcessCommand(session.Socket.GetNextCommand(), session);
            }
        }

        private void ProcessCommand(string commandLine, Session session)
        {
            if (commandLine != null)
            {
                var command = ParseCommand(commandLine);

                if (VerbStore.ContainsKey(command))
                {
                    session.Commands.Add(commandLine);
                    VerbStore[command].Invoke(session);
                }
                else
                {
                    session.Socket.SendString(SmtpCommandUtils.SV_UNKNOWN);
                }
            }
        }

        private string ParseCommand(string commandLine)
        {
            if (commandLine.Contains(' '))
            {
                commandLine = commandLine.Split(' ')[0];
            }
            commandLine = commandLine.TrimEnd(new char[] { '\r', '\n' });
            return commandLine;
        }
    }
}
