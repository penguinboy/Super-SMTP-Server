# Super-Smtp-Server

## Overview
Super-Smtp-Server is a tiny SMTP server that is designed to allow developers to quickly check the mail sending functions of their application are working.

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
You can customise the port and ip address being used by the server. All configurations are contained in the App.config file.

#### Ip Address Binding
The line below defines the Ip Address being used by Super-Smtp-Sever.
    <add key="ip" value="any"/>

- *Any* means any ip address that your computer has.
- If you want to specify an ip address, put it in standard IP for. (eg. 192.168.1.10)

#### Port Binding
The line below defines the port that Super-Smtp-Server uses.
    <add key="port" value="25"/>


## Screw your UI, I just want the server!
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