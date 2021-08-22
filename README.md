# Welcome to Phone Book MicroService Application :iphone:
## :information_source: Info 
The application provides simple crud transactions and a report with .net core microservice architecture  

## :rocket: Usage
You can download or clone a copy of the project up for running on your local machine for development and testing purposes.  

You can run the application directly from within **Visual Studio.**  

First right-click on the solution file and select **Set Startup Projects**, and configure all microservices projects to either start as desired. 
When you run the project from within **Visual Studio**, all two projects will start up.  
+ The Report microservice will be running on port **59449** and you can view the API documentation at [Report Service](http://localhost:59449/swagger)
+ The Person microservice will be running on port **61514** and you can view the API documentation at [Person Service](http://localhost:61514/swagger)

### Requirements
+ **Mongo :** MongoDB driver
+ **RabbitMQ :** Message queue 

They can be run with docker. 

## :notebook: Whats Including In This Repository
The project has implemented below features over the repository.

**Microservices which includes :**
+ ASP.NET Core Web API application
+ REST API principles, CRUD operations
+ MongoDB database connection and containerization

**Microservices Communication :**
+ Async Microservices Communication with RabbitMQ Message-Broker Service
+ Using MassTransit for abstraction over RabbitMQ Message-Broker System

**Unit Test**
+ Unit Testing with xUnit
+ Moq Mocking Framework with xUnit for testing microservice api

 ## :boy: Author 
 + **Github :** [@eyupyaray](https://github.com/eyupyaray)
 
