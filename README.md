# Simple Banking App
[![Build](https://github.com/genuinecode-git/AwesomeBank/actions/workflows/dotnet-desktop.yml/badge.svg?branch=master)](https://github.com/genuinecode-git/AwesomeBank/actions/workflows/dotnet-desktop.yml) [![tech](https://img.shields.io/badge/powredby-.netcore-purple.svg)](https://dotnet.microsoft.com/en-us/download) [![NUnit License](https://img.shields.io/badge/powredby-NUnit-green.svg)](https://nunit.org/) [![Mediator](https://img.shields.io/badge/powredby-MediatR-blue.svg)](https://www.nuget.org/packages/mediatr/) [![DomainDrivenDesign](https://img.shields.io/badge/powredby-DDD-red.svg)](https://en.wikipedia.org/wiki/Domain-driven_design) [![Swagger](https://img.shields.io/badge/powredby-swagger-gree.svg)](https://swagger.io/) [![SeriLog](https://img.shields.io/badge/powredby-serilog-orange.svg)](https://serilog.net/)


Simple banking system that handles operations on bank accounts. The system should be capable of the following features:
- input banking transactions
- calculate interest
- printing account statement
[Full Requirement](https://github.com/genuinecode-git/AwesomeBank/blob/dev/BankAccountQ.md)


Project Build on GitHub and Workflow Configured [Build](https://github.com/genuinecode-git/AwesomeBank/actions)

[Build Artifacts](https://github.com/genuinecode-git/AwesomeBank/releases)

Project was designed on showcase the thinking pattern of the developer when new project starts on following considerations.
- Scalability
- Performance
- Security
- Readability
- Relaibilty
- Platform Indipendnace
- Adoptabilty of code
- Post Handover (support , bug fixing)


## How to Run

- Clone the repository.
- Open in Visual studio and run the **'AwesomeBank.Console'** project
- In VsCode , Open terminal and navigate to **'AwesomeBank.Console'** folder use **dotnet run**


## Project Structure
| Name      | Description |
| ----------- | ----------- |
| AwesomeBank.Domain      | Domain Objects.       |
| AwesomeBank.Infrastructure   | Infranstrcuture logics (ex: Concurrent Memory /DB Opertaions) .       |
|AwesomeBank.API| Created for futuristic purspective. Interaction between Infranstrcuture is implimented here. |
|AwesomeBank.Console| Presntation layer. |
|AwesomeBank.Test| All Unit tests are here.|

## Design Patterns and Methods Used
- Domain Driven Design
- Repository Pattern
- Mediator Pattern
- Unit Of Work
- CQRS
- SOLID
- TDD
- Dependancy Injection

## Assumptions
- Interst will not be added to balance when account have older transactions than statement month. (As per given data set)
- User will not use withdraw transaction if the amount is not availble in account balance.
- User will input InterstRules and Transactions before check for Statement. (for best experince )
- In future application will use to run on multiple clients by connecting to the API (API Created for futureproof)
- In future application will interact with Database. (Infranstrcure layer is configurable for any Database type)
- Application is deigned for support Horizontal and vertical scaling if required.
- Application will grow when new changes and requirements comes.
