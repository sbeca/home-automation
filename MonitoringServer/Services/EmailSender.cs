using Microsoft.AspNetCore.Identity.UI.Services;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace MonitoringServer.Services;

public class EmailSender : IEmailSender
{
    private const string senderAddress = "monitoring@scottbeca.com";
    private const string configSet = "Default";

    private readonly ILogger _logger;

    public EmailSender(ILogger<EmailSender> logger)
    {
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        await Execute(subject, message, toEmail);
    }

    public async Task Execute(string subject, string message, string toEmail)
    {
        using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast1))
        {
            var sendRequest = new SendEmailRequest
            {
                Source = senderAddress,
                Destination = new Destination
                {
                    ToAddresses = new List<string> { toEmail }
                },
                Message = new Message
                {
                    Subject = new Content(subject),
                    Body = new Body
                    {
                        Html = new Content
                        {
                            Charset = "UTF-8",
                            Data = message
                        },
                        Text = new Content
                        {
                            Charset = "UTF-8",
                            Data = message
                        }
                    }
                },
                ConfigurationSetName = configSet
            };
            try
            {
                var response = await client.SendEmailAsync(sendRequest);
                _logger.LogInformation($"Email to '{toEmail}' queued successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send email to '{toEmail}'. Got exception: {ex?.ToString()}");
            }
        }
    }
}
