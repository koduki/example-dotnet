# example-dotnet

This is a sample .NET 9 Web API project for managing a Todo list.

## Features

* CRUD operations for Todo items.
* In-memory database using Entity Framework Core.
* Distributed tracing and metrics with OpenTelemetry.
* API documentation with OpenAPI (Swagger).
* Containerized with Docker.

## Getting Started

### Prerequisites

* .NET 9 SDK
* Docker (optional)

### Build

```sh
dotnet build
```

### Run

```sh
dotnet run
```

After running the application, you can access the OpenAPI documentation at `http://localhost:5000/openapi/v3/index.html` (the port may vary).

## API Endpoints

* `GET /api/TodoItems`: Get all todo items.
* `GET /api/TodoItems/{id}`: Get a specific todo item.
* `POST /api/TodoItems`: Create a new todo item.
