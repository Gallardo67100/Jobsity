# Jobsity Chatroom

Jobsity Chatroom is a .Net Core browser based chatroom that let you chat with other users and interact with a chatbot to get information about stocks.

## Installation

To get this going, we need to:

    1- Set up a RabbitMQ server
    2- Change the values of the config files on both the web app and the chat bot.

On the web application we need to set up the connection string pointing to the database, and the RabbitMQ server name.

```json
  "ConnectionStrings": {
    "JobsityChatroomContextConnection": "Server=server;Database=database;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "RabbitMQ": {
    "HostName": "localhost",
    "CommandQueueName": "JobsityCommands",
    "ResponseQueueName":  "JobsityResponses"
  }
```

On the chatbot, change the RabbitMQ server name.

```config
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <appSettings>
    <add key="RabbitMQHost" value="localhost" />
    <add key="RabbitMQCommandsQueue" value="JobsityCommands" />
    <add key="RabbitMQResponsesQueue" value="JobsityResponses" />
  </appSettings>
</configuration>
```

### Attention!

The queue names **MUST** be the same on both projects.

After setting this things up, we have to deploy both applications in a server that can communicate with the RabbitMQ server.
