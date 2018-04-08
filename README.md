# Overview
This repository contains an Azure Service Fabric application that functions as a demonstration of Service Fabric programming models and architecture.

The application consists of a stateless service hosting an ASP.Net Core application that acts as an Web Api, stateful service serving as data repository, stateless service with business logic and an actor service that emulates user's shopping cart.

## Building and deploying

[Set up your development environment](https://docs.microsoft.com/azure/service-fabric/service-fabric-get-started). You'll need version 15.1 of Visual Studio 2017 or higher installed.

This application can be built and deployed immediately using Visual Studio 2017. To deploy on the local cluster, you can simply hit F5 to debug the sample. If you'd like to try publishing it to an Azure cluster:

1. Right-click on the application project in Solution Explorer and choose Publish.
2. Sign-in to the Microsoft account associated with your Azure subscription.
3. Choose the cluster you'd like to deploy to.