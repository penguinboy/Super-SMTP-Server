using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace SimpleSmtpServer
{
    class SimpleSocket
    {
        Encoding Encoding = ASCIIEncoding.ASCII;
        Socket Socket;

        public string CommandSeperator = "\r\n";
        public bool Connected = true;


        public SimpleSocket(Socket s)
        {
            this.Socket = s;
        }

        public void Close()
        {
            this.Socket.Close();
            this.Socket.Dispose();
            this.Connected = false;
        }

        public void SendString(string s)
        {
            Console.WriteLine("Sending: " + s);
            this.Socket.Send(this.Encoding.GetBytes(s));
        }

        public int Receive(byte[] b)
        {
            return this.Socket.Receive(b);
        }

        public string GetNextCommand()
        {
            string currentCommand = "";
            byte[] b = new byte[1];

            while (this.Receive(b) == 1)
            {
                string c = ASCIIEncoding.ASCII.GetString(b);

                currentCommand += c;

                Console.Write(c);

                if (currentCommand.Contains(CommandSeperator))
                {
                    return currentCommand;
                }
            }
            
            throw new InvalidOperationException("Connection terminated before command seperator finished the current command");
        }
    }
}
