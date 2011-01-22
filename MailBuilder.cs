using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace SimpleSmtpServer
{
    class MailBuilder
    {
        bool MoreToParse = true;
        public MailMessage Message;

        public MailBuilder(string initialMailCommand, SimpleSocket s)
        {
            Message = new MailMessage();

            Message.From = new MailAddress(ParseValue(initialMailCommand));
            
            s.SendString(SmtpServer.CMD_OK);

            while (MoreToParse)
            {
                string command = s.GetNextCommand();

                if (command.Contains(SmtpServer.CMD_CL_MAIL_RCPT))
                {
                    MailAddress address = new MailAddress(ParseValue(command));
                    Message.To.Add(address);

                    s.SendString(SmtpServer.CMD_OK);
                }
                else if (command.Contains(SmtpServer.CMD_CL_DATA))
                {
                    s.CommandSeperator = "\r\n.\r\n";

                    s.SendString(SmtpServer.CMD_DATA_OK);

                    string rawMessage = s.GetNextCommand();

                    string subject = ParseMessageValue(rawMessage, "Subject");
                    string body = ParseMessageBody(rawMessage);

                    if (string.IsNullOrEmpty(body) || string.IsNullOrEmpty(subject))
                    {
                        throw new InvalidOperationException("Malformed message");
                    }
                    else
                    {
                        Message.Body = body;
                        Message.Subject = subject;

                        s.CommandSeperator = "\r\n";

                        MoreToParse = false;
                        s.SendString(SmtpServer.CMD_OK);
                    }
                }
                
            }
        }

        private string ParseValue(string command)
        {
            if (command.Contains(':'))
            {
                string value = command.Split(':')[1].TrimStart(new char[] { '<' }).TrimEnd(new char[] { '\r', '\n', '>' });
                return value;
            }
            else
            {
                throw new InvalidOperationException("Command does not contain a value");
            }
        }

        public string ParseMessageValue(string message, string key)
        {
            if (message.Contains(':'))
            {
                string[] lines = message.Split('\r');

                foreach (string line in lines)
                {
                    if (line.Contains(key))
                    {
                        return line.Split(':')[1];
                    }
                }

                return null;
            }
            else
            {
                throw new InvalidOperationException("Message does not contain a value");
            }
        }

        char[] newLineChars = new char[] { '\r', '\n' };

        public string ParseMessageBody(string message)
        {
            string[] lines = message.Split('\r');

            StringBuilder builder = new StringBuilder(); 
            bool messageBreak = false;
            for (int x = 0; x < lines.Length; x++)
            {
                if (messageBreak)
                {
                    builder.AppendLine(lines[x].TrimEnd(newLineChars).TrimStart(newLineChars));
                }
                else if (string.IsNullOrWhiteSpace(lines[x]))
                {
                    messageBreak = true;
                }
            }

            return builder.ToString();
        }
    }
}
