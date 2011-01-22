using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Net.Mail;

namespace SimpleSmtpServer
{
    public class SmtpServer
    {
        public const string CMD_GREET = "220 Greetings\n\r";
        public const string CMD_OK = "250 OK\n\r";
        public const string CMD_INFO = "250-SSMTP Server\n\r250-PLAIN\n\r" + CMD_OK;
        public const string CMD_QUIT = "250 OK\n\r";
        public const string CMD_DATA_OK = "354 Start mail input; end with .\n\r";
        public const string CMD_UNKNOWN = "500 Unknown command\n\r";

        public const string CMD_CL_EHLO = "EHLO";
        public const string CMD_CL_HELO = "HELO";
        public const string CMD_CL_MAIL = "MAIL FROM:";
        public const string CMD_CL_MAIL_RCPT = "RCPT TO:";
        public const string CMD_CL_DATA = "DATA";
        public const string CMD_CL_QUIT = "QUIT";

        public SmtpServer()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 25);

            listener.Start();

            ProcessSocket(new SimpleSocket(listener.AcceptSocket()));
        }


        bool Connected = true;
        private void ProcessSocket(SimpleSocket s)
        {
            byte[] b = new byte[1];

            s.SendString(CMD_GREET);

            while (s.Connected)
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
            }
            else if (command.Contains(CMD_CL_QUIT))
            {
                s.SendString(CMD_OK);
                s.Close();
            }
            else
            {
                s.SendString(CMD_UNKNOWN);
            }
        }
    }
}
