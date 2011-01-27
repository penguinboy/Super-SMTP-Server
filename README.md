# Super-Smtp-Server

## Overview
Super-Smtp Server is a tiny SMTP server that is designed to allow developers to quickly check the mail sending functions of their application are working.

### Features
- Simple GUI
- Multi-threaded, allows you to plug the server component into your own solution

### What doesn't work
- There's currently no kind of SMTP authentication available. You have to send mail without passwords and such.

## Use
1. Start the server with:

    SuperSmtpGui.exe
2. Point your mail-sending application to localhost, on port 25.
3. Send your mail, get satisfaction from the fact that it shows in server's window.

That's it.


## Slightly more complicated use
If you're unhappy with the GUI that comes with Supert-Smtp-Server, then I would suggest utilising the wonderfully simple server component of Super-Smtp-Server.

1. Clone this git repo
2. Include the 'SuperSmtpServer' project in your solution (add a reference to the project, duh)
3. Add a useing statement in your code

    using SuperSmtpServer;
4. At an appropriate time, init the server component

    var server = new SmtpServer();
5. Add an event listener to the MessageReceived event

    server.MessageRecieved += new MailMessageHandler(server_MessageRecieved);



Use the project in anyway you wish.