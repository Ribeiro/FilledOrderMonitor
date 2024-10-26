# Order Monitor

## Description
This project is an order monitoring application developed in .NET 8 with C# and PostgreSQL that uses the `LISTEN/NOTIFY` protocol of PostgreSQL to detect status changes in an orders table. When an order is marked as `FILLED`, the application receives a notification and sends an alert email to a specified recipient.

## Features
- Monitors database events in PostgreSQL using `NOTIFY/LISTEN`.
- Automatically detects when the status of an order is changed to `FILLED`.
- Sends email notifications to configurable recipients.
  
## Technologies Used
- [.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0) with C#
- [Npgsql](https://www.npgsql.org/) for PostgreSQL communication
- PostgreSQL with `LISTEN/NOTIFY` capabilities

## Prerequisites
1. **.NET 8 SDK** - Install the .NET 8 SDK to build and run the project.
2. **PostgreSQL** - Ensure you have a running PostgreSQL database.
3. **Npgsql** - The `Npgsql` package must be installed for connecting to PostgreSQL.
4. **SMTP Server** - Required for sending emails. Check your email server credentials.

## Database Setup
Check files: table.sql, function.sql and trigger.sql and docker-compose.yml

## Important
This project tries to implements the CDC (Change Data Capture) in order to
avoid overload Postgres database using polling table for select.

## Considerations:
* Performance: Although NOTIFY/LISTEN is efficient, it is not ideal for systems that need to process thousands of events per second. For such cases, more robust systems like Kafka would be more suitable.

* Scalability: For small and medium-sized systems, this approach is perfectly viable. In large-scale scenarios, it may be necessary to consider more scalable solutions.

* Reconnection: The code should be robust enough to handle reconnections and failures in the connection to the database.

The NOTIFY/LISTEN command is generally supported by PostgreSQL, but there are some limitations when using Amazon Aurora or AWS RDS (Relational Database Service):

* RDS for PostgreSQL: RDS for PostgreSQL provides full support for NOTIFY/LISTEN, as it is practically identical to standard PostgreSQL, just managed by AWS. Therefore, you can use NOTIFY/LISTEN on RDS for PostgreSQL without any issues.

* Aurora PostgreSQL: Aurora PostgreSQL, which is a modified version of PostgreSQL for greater scalability and high availability, also supports NOTIFY/LISTEN in most cases. However, there are some important considerations:

- Multi-AZ (Multi-Availability Zone): If you are using an Aurora Multi-AZ configuration, NOTIFY notifications are local to the primary node. This means that clients connected to read replicas or standby nodes will not receive notifications. Only connections to the primary node will correctly listen for LISTEN.

- Failover: During a failover (when the primary node switches to a replica), the client needs to reconnect and reissue the LISTEN command, as the original connection will be lost.



