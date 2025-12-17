using System;
using Azure.Storage.Queues.Models;
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
    public void Run([QueueTrigger("myqueue-items", Connection = "")] QueueMessage message)
    {
        _logger.LogInformation("C# Queue trigger function processed: {messageText}", message.MessageText);
    }
}