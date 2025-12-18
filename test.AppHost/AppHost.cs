var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.test_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

//builder.AddAzureContainerAppEnvironment("env");

var eventHubs =
    builder.AddAzureEventHubs("event-hubs")
        .RunAsEmulator(emulator =>
        {
            emulator.WithLifetime(ContainerLifetime.Persistent);
        });
eventHubs.AddHub("messages");

// The name given to the AddAzureServiceBus method must match the name expected by
// the 'Producer' function trigger's 'Connection' parameter.
var serviceBus =
    builder.AddAzureServiceBus("IrisServiceBus")
        .RunAsEmulator(emulator =>
        {
            emulator.WithLifetime(ContainerLifetime.Persistent);
        });
// The name for the queue must be in the local.settings.json.
serviceBus.AddServiceBusQueue("iris-queue");

var functions = builder.AddAzureFunctionsProject<Projects.test_Function>("functions");

builder.AddProject<Projects.test_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithReference(serviceBus)
    .WaitFor(serviceBus)
    .WithReference(eventHubs)
    .WaitFor(eventHubs);

builder.Build().Run();
