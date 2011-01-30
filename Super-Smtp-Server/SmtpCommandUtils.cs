using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperSmtpServer
{
    class SmtpCommandUtils
    {
        public const string SV_GREET = "220 Greetings\r\n";
        public const string SV_OK = "250 OK\r\n";
        public const string SV_INFO = "250-SSMTP Server\r\n250-PLAIN\r\n" + SV_OK;
        public const string SV_QUIT = "250 OK\r\n";
        public const string SV_DATA_OK = "354 Start mail input; end with .\r\n";
        public const string SV_UNKNOWN = "500 Unknown command\r\n";
        public const string SV_GO = "220 Go ahead";

        public const string CL_EHLO = "EHLO";
        public const string CL_HELO = "HELO";
        public const string CL_MAIL = "MAIL";
        public const string CL_MAIL_RCPT = "RCPT";
        public const string CL_DATA = "DATA";
        public const string CL_QUIT = "QUIT";
        public const string CL_STARTTLS = "STARTTLS";
    }
}
