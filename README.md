# SmartSupplyChain Backend

A .NET backend for a smart supply chain management system designed to support inventory operations, procurement, supplier coordination, shipment tracking, and real-time business updates.

Built with C# and ASP.NET Core using a layered Clean Architecture approach, the project separates core business rules, application logic, infrastructure, and HTTP API concerns.

> **Development status:** Backend implementation is substantially complete. Testing and refinement of the AI-assisted and SignalR real-time workflows remain in progress.

## Project Overview

Supply chain operations involve multiple connected processes, including inventory management, purchasing, supplier coordination, warehouse operations, and shipment monitoring.

SmartSupplyChain aims to bring these processes together through a structured backend that supports maintainable business logic and integration with a modern frontend.

The system is being developed with extensibility in mind, allowing business capabilities to evolve without unnecessarily coupling domain logic to the API or infrastructure.

## Architecture

The solution is organized into four main projects:

```text
SmartSupplyChain-backend/
├── SmartSupplyChain.Application/
├── SmartSupplyChain.Core/
├── SmartSupplyChain.Infrastructure/
├── SmartSupplyChain.WebApi/
├── SmartSupplyChain.sln
└── README.md
```

### SmartSupplyChain.Core

Contains the core domain concepts and business rules. This layer provides the foundation for the supply chain domain and is intended to remain independent of external infrastructure.

### SmartSupplyChain.Application

Organizes application use cases and coordinates business operations between the domain and infrastructure abstractions.

### SmartSupplyChain.Infrastructure

Contains technical implementations and integrations required by the application, including persistence-related components.

### SmartSupplyChain.WebApi

Provides the HTTP API entry point through which clients communicate with the backend.

This separation supports maintainability, testability, and the independent evolution of application and infrastructure concerns.

## Technology Stack

| Area | Technology |
|---|---|
| Language | C# |
| Backend framework | ASP.NET Core |
| Architecture | Clean Architecture / layered architecture |
| Database | PostgreSQL |
| Data access | Entity Framework Core, where configured |
| Real-time communication | SignalR |
| AI-assisted workflows | Under testing |
| Source control | Git and GitHub |

The precise framework versions and additional dependencies should be documented from the solution and project files.

## Supply Chain Domain

The system's domain model covers supply chain concepts such as:

- **Companies and warehouses:** Organize business operations and warehouse locations.
- **Products and inventory:** Represent products and stock held in inventory.
- **Suppliers and purchasing:** Support supplier relationships and purchase order processing.
- **Shipments and tracking:** Represent shipment operations and shipment tracking events.
- **Drivers and vehicles:** Model transportation resources.
- **Incident reporting:** Represent operational incidents associated with supply chain activities.
- **Subscription and usage records:** Support subscription-related business concepts.

The exact workflows and business rules supported by each module should be confirmed against the implementation.

## AI and Real-Time Capabilities

### AI-assisted workflows

The project includes AI-related functionality intended to support selected supply chain operations. These workflows are currently being tested and refined.

The specific AI use cases, model integrations, configuration requirements, and supported outputs should be documented once verified against the implementation.

### Real-time updates with SignalR

SignalR is part of the project's real-time communication direction, supporting the delivery of updates to connected clients where configured.

The relevant hub endpoints, event names, authorization requirements, and client subscription behavior should be documented after the real-time workflows have been tested end to end.

## Getting Started

### Prerequisites

Install the tools required by the current solution configuration:

- Git
- A .NET SDK compatible with the project's target framework
- PostgreSQL
- Visual Studio or Visual Studio Code

Additional tools may be needed for the AI integration and frontend development.

### 1. Clone the repository

```bash
git clone https://github.com/collarfil/SmartSupplyChain-backend.git
cd SmartSupplyChain-backend
```

### 2. Restore dependencies

```bash
dotnet restore SmartSupplyChain.sln
```

### 3. Build the solution

```bash
dotnet build SmartSupplyChain.sln
```

### 4. Configure the database

Create or select a PostgreSQL database and configure the application's connection string using the configuration approach already implemented in the project.

Keep passwords, API keys, and other sensitive values outside committed source files.

### 5. Apply database migrations

Inspect the configured Entity Framework Core `DbContext`, migrations, and startup project. Apply migrations using the commands appropriate to the actual solution configuration.

### 6. Run the API

Start `SmartSupplyChain.WebApi` using the configured launch profile. Check the project's launch settings and API documentation for the correct local URL and available endpoints.

**Setup note:** The database configuration keys, target framework, migration commands, and API URL should be verified from the repository before this section is treated as complete.

## Testing

Testing should cover the system's core business behavior and its integrations.

Priority scenarios include:

- Inventory and warehouse business rules.
- Purchase order and supplier workflows.
- Shipment tracking and incident reporting.
- Database persistence and validation.
- API error handling and authorization.
- AI-assisted workflow behavior.
- SignalR connection, event delivery, and client updates.

AI and SignalR scenarios should be considered complete only after their expected behavior has been verified through suitable tests.

## Security and Reliability

Before production deployment, verify:

- Authentication and authorization for protected endpoints.
- Input validation and consistent error responses.
- Secure handling of connection strings and AI service credentials.
- Database migration and backup procedures.
- Logging and appropriate exception handling.
- Authorization for SignalR connections and messages.
- Automated tests for critical business rules.

## Current Development Focus

The immediate priorities are:

1. Complete testing of AI-assisted workflows.
2. Verify SignalR events and real-time client updates.
3. Validate the main supply chain workflows end to end.
4. Complete API and setup documentation.
5. Prepare representative screenshots and a short demonstration video.
6. Document the verified deployment procedure.

## Project Purpose

SmartSupplyChain demonstrates the design and development of a modular .NET backend for supply chain operations, with an emphasis on clean separation of concerns, domain-oriented business logic, and integration with AI-assisted and real-time capabilities.

## Author

**Collins Aigbekhai**

GitHub: [@collarfil](https://github.com/collarfil)

---

*SmartSupplyChain — connecting supply chain operations through a modular backend.*
