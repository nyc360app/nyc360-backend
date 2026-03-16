# NYC360.Backend Project Documentation

This document provides a detailed overview of the `NYC360.Backend` project, covering its core specifics, implemented features, technology stack, architectural patterns, and coding conventions.

## Project Overview

The `NYC360.Backend` project serves as the robust backend service for a platform primarily focused on the New York City (NYC) ecosystem. Its main purpose is to manage and facilitate various interactions related to community engagement, event organization, content posting, and comprehensive user management. The application provides a scalable and maintainable foundation for a dynamic user experience.

## Core Technologies and Frameworks

*   **Language:** C#
*   **Framework:** .NET 10 (specifically ASP.NET Core for API development)
*   **Object-Relational Mapper (ORM):** Entity Framework Core (used for data access and persistence, managing database interactions and migrations).
*   **Caching:** Redis (integrated for distributed caching, enhancing performance and scalability).
*   **Email Services:** MailKit (utilized for sending emails, supporting various email-related functionalities).
*   **Authentication & Authorization:** JSON Web Tokens (JWT) for secure authentication, with support for Refresh Tokens and Role-Based Access Control (RBAC). OAuth2 integration, specifically with Google, is also supported.
*   **Database:** (Likely SQL Server, PostgreSQL, or another relational database, inferred from Entity Framework Core usage and migration capabilities, though not explicitly stated in directory structure).

## Architecture In-Depth

The project adheres to a **Clean Architecture** (also known as Onion Architecture) style. This ensures a strict dependency rule where inner layers (Domain) have no knowledge of outer layers (Infrastructure, API).

### 1. Domain Layer (`NYC360.Domain`)
**The Core.** Encapsulates the enterprise business logic and database schema definitions. It has *no external dependencies*.

#### Entity Modules
Entities are grouped into folders representing the main functional areas:
*   **Communities:** `Community`, `CommunityMember`.
*   **Events:** `Event`, `EventAttendee`.
*   **Forums:** `Forum`, `Question`, `Answer`.
*   **Housing:** `HouseListing`, `HousingRequest`, and authorization workflow entities.
*   **Locations:** Location data and categorization.
*   **Posts:** Content posts, comments, and interactions.
*   **Professions:** User profession entities.
*   **Support:** Support ticket system.
*   **Tags:** Taxonomy system.
*   **Topics:** Topic organization.
*   **User:** Core user entities, profiles (`StudentProfile`, `WorkerProfile`), and verification docs.
*   **Core:** Shared entities like `RssFeedSource` and `RefreshToken`.

#### Key Components
*   **Enums:** Strong typing for model properties (e.g., `DocumentType`, `FlagStatus`, `SocialPlatform`).
*   **DTOs:** Internal Data Transfer Objects for moving data without exposing raw entities.
*   **Wrappers:** `StandardResponse` and `PagedResponse` for consistent API structure.
*   **Constants:** System-wide constant values.

### 2. Application Layer (`NYC360.Application`)
**The Orchestrator.** Contains application-specific business rules and use cases. It implements the **CQRS** pattern using **MediatR**.

#### Architecture & Patterns
*   **CQRS (Command Query Responsibility Segregation):** Operations are strictly split into Commands (writes) and Queries (reads).
*   **Mediator Pattern:** Use cases are encapsulated as Requests and handled by independent `IRequestHandler` implementations, promoting loose coupling.
*   **Validation:** **FluentValidation** is used to validate all incoming requests/commands.
*   **Mapping:** **AutoMapper** is used to map Domain entities to DTOs.
*   **Abstractions:** Defines interfaces (`Contracts`) that the Infrastructure layer must implement, such as `IEmailService` and repositories.

#### Structure
*   **Features:** Organized by feature slice (e.g., `Communities`, `Housing`). Inside each feature, you find `Commands` and `Queries` folders.
*   **Contracts:** Interfaces for Persistence (`IUnitOfWork`, Repositories) and Infrastructure services.
*   **Common:** Shared behaviors and exceptions.

### 3. Infrastructure Layer (`NYC360.Infrastructure`)
**The Implementation.** Implements the interfaces defined in the Application layer and connects to external resources.

#### Modules
*   **Persistence:**
    *   **Context:** `ApplicationDbContext` (Entity Framework Core).
    *   **Repositories:** Implementations of repository interfaces.
    *   **Migrations:** Database schema changes.
    *   **Seeders:** Initial data population.
*   **Identity:** Services for JWT generation, password hashing, and auth.
*   **Background Services:** Hosts `IHostedService` implementations for long-running tasks like RSS discovery.
*   **External Services:**
    *   **Email:** MailKit/SMTP integration.
    *   **Storage:** File storage implementation.
    *   **Cache:** Redis distributed caching.
    *   **RSS:** Feed parsing and processing services.
    *   **OAuth:** Google authentication handlers.
    *   **Stripe:** Payment processing integration.

### 4. API Layer (`NYC360.API`)
**The Entry Point.** Handles HTTP requests using the **REPR (Request-Endpoint-Response)** pattern with **FastEndpoints**.

#### Key Responsibilities
*   **Endpoints:** Routes are defined as individual classes (e.g., `CreateCommunityEndpoint`) rather than grouped Controllers, ensuring single responsibility.
*   **Authentication & Authorization:** Configures JWT Bearer auth and role-based permissions.
*   **Swagger/OpenAPI:** Auto-generated API documentation.
*   **Extensions:** Centralized service registration (linking all layers via `ServiceCollectionExtensions`).

## Key Implemented Features

The project incorporates a variety of features spanning multiple functional domains:

*   **User Management:**
    *   User Registration (Regular Users and Organizations)
    *   User Login and Logout
    *   Password Management (Reset, Change Password)
    *   Email Confirmation
    *   Two-Factor Authentication (2FA)
    *   User Roles and Permissions

*   **Authentication & Authorization:**
    *   JWT Token Generation and Validation
    *   Refresh Token Mechanism
    *   Role-Based Access Control (RBAC)
    *   Google OAuth Integration

*   **Community Management:** Functionality for creating, managing, and interacting with communities.

*   **Event Management:** Capabilities for creating, listing, and managing events.

*   **Content Management:**
    *   Posting Content (e.g., articles, updates)
    *   Commenting on Posts
    *   Managing Post Categories

*   **Flagging System:**
    *   Mechanism for flagging inappropriate content or users.
    *   Flag Reason Types and Statuses.

*   **Location Services:** Management and categorization of locations (`LocationType`).

*   **RSS Integration:**
    *   RSS Feed Source Management.
    *   Background Service for RSS Feed Discovery and Processing.

*   **Caching:** A generic caching service with a Redis-backed implementation for frequently accessed data.

*   **Email Services:** Sending transactional and notification emails.

*   **Utility Services:**
    *   Slug Generation for user-friendly URLs.
    *   Local Storage Service for handling file storage.

## Code Structure and Style

The codebase follows idiomatic C# and .NET coding conventions, promoting readability and consistency:

*   **Consistent Naming Conventions:**
    *   PascalCase for class names, method names, properties, and public fields.
    *   camelCase for method parameters and local variables.
*   **Folder-by-Feature/Module Organization:** Code is organized logically by feature or module, especially within the `Endpoints` (API) and `Features` (Application) layers, enhancing discoverability and maintainability.
*   **Extension Methods for Configuration:** Extensive use of extension methods (e.g., `AddInfrastructureServices`, `AddApplicationServices`) to encapsulate and manage service registrations and other configurations, leading to cleaner startup code.
*   **Asynchronous Programming:** Widespread adoption of `async` and `await` keywords for I/O-bound operations to ensure responsiveness and scalability.
*   **Standardized API Responses:** Use of generic wrapper classes like `StandardResponse<T>` and `PagedResponse<T>` to provide consistent API response formats, including success data, metadata, and error details.
*   **Code Documentation:** (Assumed) Use of XML documentation comments for public APIs to aid understanding and maintainability.
*   **Error Handling:** Centralized error handling mechanisms, often utilizing middleware or custom exception filters, to provide consistent error responses to clients.
*   **Code Linting and Formatting:** (Assumed) Adherence to a consistent code style enforced by tools like EditorConfig or Roslyn analyzers.
