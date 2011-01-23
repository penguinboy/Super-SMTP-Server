Very experimental right now. There's a GUI on the way, but for now it's just a tiny little library.


## Things that work
- Bind to port 25 on localhost
- Successfully serves clients on a child thread that is spawn on instantiation.
- Successfully triggers an event when mail is recieved.

## Things to be done
- Have multiple threads
- Implement STARTTLS, AUTH, etc


## Usage
Add namespace
'using SuperSmtpServer;'

Instantiate the server with:
'SmtpServer server = new SmtpServer();'

Listen for the MessageRecieved event
'server.MessageRecieved += new MailMessageHandler(server_MessageRecieved);'

A .Net MailMessage will be return by the event.