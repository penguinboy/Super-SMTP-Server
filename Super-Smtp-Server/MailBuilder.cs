using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace SuperSmtpServer
{
    class SmtpMailVerbUtils
    {
        public static string ParseValue(string command)
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

        public static string ParseMessageValue(string message, string key)
        {
            if (message.Contains(':'))
            {
                string[] lines = message.Split('\r');

                for (int x = 0; x < lines.Length; x++)
                {
                    if (lines[x].Contains(key))
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.Append(lines[x].Split(':')[1]);

                        int nextLine = x + 1;
                        while (nextLine != lines.Length && lines[nextLine].StartsWith("\n "))
                        {
                            builder.Append(lines[nextLine].Substring(1));
                            nextLine++;
                        }

                        return builder.ToString();
                    }
                }

                return null;
            }
            else
            {
                throw new InvalidOperationException("Message does not contain a value");
            }
        }

        private static char[] newLineChars = new char[] { '\r', '\n' };
        private static char[] endBodyChars = new char[] { '\r', '\n', '.' };
        public static string ParseMessageBody(string message)
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

            return builder.ToString().TrimEnd(endBodyChars);
        }
    }
}
