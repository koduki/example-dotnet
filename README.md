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

## Load Testing

This project includes a k6 script for load testing.

### Prerequisites

* [k6](https://k6.io/docs/getting-started/installation/)

### Run

To run the load test, you need to set the `TARGET_URL` environment variable to the base URL of the application.

```sh
k6 run --env TARGET_URL=http://localhost:5195 tests/k6/script.js
```
