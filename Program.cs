using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System.Net.Mail;

namespace FilledOrderMonitor.Main
{
    public class Program
    {
        protected Program() { }

        static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services.AddLogging())
                .Build();

            var logger = host.Services.GetRequiredService<ILogger<Program>>();

            var connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=trading";

            using var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                eventArgs.Cancel = true;
                cts.Cancel();
            };

            try
            {
                await ListenForNotifications(connectionString, cts.Token, logger);
            }
            catch (OperationCanceledException)
            {
                logger.LogInformation("Application ended.");
            }
        }

        private static async Task ListenForNotifications(string connectionString, CancellationToken cancellationToken, ILogger logger)
        {
            using var connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync(cancellationToken);

            using (var listenCommand = new NpgsqlCommand("LISTEN filled_orders;", connection))
            {
                await listenCommand.ExecuteNonQueryAsync(cancellationToken);
            }

            logger.LogInformation("Waiting for Order notifiacations (FILLED)...");

            connection.Notification += async (o, e) =>
            {
                logger.LogInformation("Notification received for OrderId: {OrderId}", e.Payload);
                //await SendEmailNotification(e.Payload, logger);
                logger.LogInformation("Email sent for OrderId: {OrderId}", e.Payload);
            };

            while (!cancellationToken.IsCancellationRequested)
            {
                await connection.WaitAsync(cancellationToken);
            }
        }

        private static async Task SendEmailNotification(string orderId, ILogger logger)
        {
            var fromAddress = new MailAddress("from@example.com", "Order Notification");
            var toAddress = new MailAddress("to@example.com", "Client");
            const string fromPassword = "password";
            const string subject = "Order Filled Notification";
            string body = $"Order {orderId} has been filled.";

            var smtpClient = new SmtpClient
            {
                Host = "smtp.example.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            };

            await smtpClient.SendMailAsync(message);
            logger.LogInformation("Email sent for OrderId: {OrderId}", orderId);
        }
    }
}