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

