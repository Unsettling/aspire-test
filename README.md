# Demo for minimum Aspire project

## Introduction

How to create a minimum Aspire project that uses Service Bus and Event Hub emulators with a Function App.

## Setup

Assuming the dotnet CLI is already installed.

```bash
  mkdir test; cd test
  dotnet new install Aspire.ProjectTemplates
  dotnet new aspire-starter
  dotnet new gitignore
  dotnet sln migrate
  dotnet dev-certs https --trust
  dotnet run --project .\test.AppHost\test.AppHost.csproj
```

## Add Integrations

Assuming the Aspire CLI is already installed.

```bash
  aspire update --self
  aspire update
  aspire add azure-servicebus
  aspire add azure-eventhubs
```

## Update AppHost

Update the `AppHost.cs` to add Event Hubs and Service Bus emulators.

## Add Function App

```bash
winget install Microsoft.Azure.FunctionsCoreTools
aspire add azure-functions
mkdir test.Function; cd .\test.Function\
func new --template "QueueTrigger" --name "TestServiceBusQueueTrigger" --worker-runtime dotnet-isolated
cd ..; dotnet sln add .\test.Function\test_Function.csproj
```

