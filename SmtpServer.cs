using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.Mail;
using System.Threading;

namespace SuperSmtpServer
{

    public delegate void MailMessageHandler(MailMessage m);

    public class SmtpServer : IDisposable
    {
        public const string CMD_GREET = "220 Greetings\r\n";
        public const string CMD_OK = "250 OK\r\n";
        public const string CMD_INFO = "250-SSMTP Server\r\n250-PLAIN\r\n" + CMD_OK;
        public const string CMD_QUIT = "250 OK\r\n";
        public const string CMD_DATA_OK = "354 Start mail input; end with .\r\n";
        public const string CMD_UNKNOWN = "500 Unknown command\r\n";
        public const string CMD_GO = "220 Go ahead";

        public const string CMD_CL_EHLO = "EHLO";
        public const string CMD_CL_HELO = "HELO";
        public const string CMD_CL_MAIL = "MAIL FROM:";
        public const string CMD_CL_MAIL_RCPT = "RCPT TO:";
        public const string CMD_CL_DATA = "DATA";
        public const string CMD_CL_QUIT = "QUIT";
        public const string CMD_CL_STARTTLS = "STARTTLS";


        public event MailMessageHandler MessageRecieved;

        TcpListener listener;
        Thread processingThread;
        List<SimpleSocket> SocketPool;
        bool Running = true;

        public SmtpServer()
        {
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
            byte[] b = new byte[1];

            s.SendString(CMD_GREET);

            while (s.Connected && Running)
            {
                ProcessCommand(s.GetNextCommand(), s);
            }
        }

        private void ProcessCommand(string command, SimpleSocket s)
        {
            if (command.Contains(CMD_CL_EHLO))
            {
                s.SendString(CMD_INFO);
            }
            else if (command.Contains(CMD_CL_HELO))
            {
                s.SendString(CMD_OK);
            }
            else if (command.Contains(CMD_CL_MAIL))
            {
                MailBuilder builder = new MailBuilder(command, s);
                if (this.MessageRecieved != null)
                {
                    this.MessageRecieved(builder.Message);
                }
            }
            else if (command.Contains(CMD_CL_QUIT))
            {
                s.SendString(CMD_OK);
                s.Close();
            }
            else
            {
                Console.WriteLine(command);
                s.SendString(CMD_UNKNOWN);
            }
        }
    }
}
