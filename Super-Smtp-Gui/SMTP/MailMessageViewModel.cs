using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace SuperSmtpGui.SMTP
{
    public class MailMessageViewModel
    {
        private MailMessage message;

        public MailMessageViewModel()
        {
            message = new MailMessage("from@notworking", "to@notworking", "subject", "body");
        }

        public MailMessageViewModel(MailMessage m)
        {
            this.message = m;
        }

        public string From
        {
            get
            {
                return message.From.Address;
            }
        }

        public string To
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                foreach (MailAddress add in message.To)
                {
                    builder.Append(add.Address);
                    builder.Append("; ");
                }
                return builder.ToString();
            }
        }

        public string Subject
        {
            get
            {
                return message.Subject;
            }
        }

        public string Body
        {
            get
            {
                return message.Body;
            }
        }
    }
}
