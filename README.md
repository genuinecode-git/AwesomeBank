# Simple Banking App
[![tech](https://img.shields.io/badge/powredby-.netcore-purple.svg)](https://dotnet.microsoft.com/en-us/download) [![NUnit License](https://img.shields.io/badge/powredby-NUnit-green.svg)](https://nunit.org/) [![Mediator](https://img.shields.io/badge/powredby-MediatR-blue.svg)](https://www.nuget.org/packages/mediatr/) [![DomainDrivenDesign](https://img.shields.io/badge/powredby-DDD-red.svg)](https://en.wikipedia.org/wiki/Domain-driven_design) [![Swagger](https://img.shields.io/badge/powredby-swagger-gree.svg)](https://swagger.io/)


Simple banking system that handles operations on bank accounts. The system should be capable of the following features:
- input banking transactions
- calculate interest
- printing account statement
[Full Requirement](https://github.com/genuinecode-git/AwesomeBank/blob/dev/BankAccountQ.md)

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