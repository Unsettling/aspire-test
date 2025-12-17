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

var serviceBus =
    builder.AddAzureServiceBus("IrisServiceBus")
        .RunAsEmulator(emulator =>
        {
            emulator.WithLifetime(ContainerLifetime.Persistent);
        });
serviceBus.AddServiceBusQueue("iris-queue");

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
