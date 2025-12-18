using System;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace test.Function;

public class TestServiceBusQueueTrigger
{
    private readonly ILogger<TestServiceBusQueueTrigger> _logger;

    public TestServiceBusQueueTrigger(ILogger<TestServiceBusQueueTrigger> logger)
    {
        _logger = logger;
    }

    [Function(nameof(TestServiceBusQueueTrigger))]
    public async Task Run(
        [ServiceBusTrigger("iris-queue", Connection = "IrisServiceBus")]
        ServiceBusReceivedMessage message,
        ServiceBusMessageActions messageActions)
    {
        _logger.LogInformation("Message ID: {id}", message.MessageId);
        _logger.LogInformation("Message Body: {body}", message.Body);
        _logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

        // Complete the message
        await messageActions.CompleteMessageAsync(message);
    }
}
