# Milestone 1 Summary: Backend Foundation & Database Deployment

## Overview
This document summarizes the technical achievements completed in Milestone 1 of the Restaurant Management System (RMS). The primary objective was to establish a robust, highly scalable **Clean Architecture** backend in .NET Core and successfully deploy a Code-First database to a remote SQL Server.

---

## 1. Clean Architecture Implementation
We successfully scaffolded the foundational layers of the application, strictly adhering to Clean Architecture principles to ensure the system is modular and maintainable.

### A. The Domain Layer (`RMS.Domain`)
**Purpose:** The absolute center of the architecture. It contains pure business logic and has zero dependencies on databases or web frameworks.
- **What was done:** We manually coded 12 essential C# Entities representing the entire SaaS restaurant model.
- **Key Entities Created:**
  - `Tenant` (Uses a secure `GUID` primary key for multi-tenant SaaS isolation).
  - `Branch` (Allows a single Tenant to operate multiple physical locations).
  - `User`, `Order`, `MenuCategory`, `MenuItem`, `Table`, `InventoryItem`, etc.

### B. The Infrastructure Layer (`RMS.Infrastructure`)
**Purpose:** Acts as the bridge between the pure C# Domain and external systems (like SQL Server).
- **What was done:** We installed Entity Framework Core and configured the `ApplicationDbContext`.
- **Key Technical Achievement (Database Schema Isolation):** During deployment, we discovered that another student/partner had accidentally pushed their "Hospital Management" tables into our shared database. If we proceeded normally, our code would have crashed or deleted their work. To solve this, we overrode the `OnModelCreating` method in Entity Framework to enforce a custom default schema (`rms`). This enterprise-level trick ensured all our tables were generated as `rms.Orders`, `rms.Tenants`, etc., perfectly isolating our data from the accidental pushes of other people without causing any data loss.

### C. The API Layer (`RMS.Api`)
**Purpose:** The entry point of the application. It hosts the Kestrel web server and will eventually serve RESTful JSON endpoints to the React frontend.
- **What was done:** We configured `RMS.Api` as the Startup Project.
- **Security:** We successfully injected the remote SQL Server credentials into the `appsettings.json` file.
- **Cleanup:** We removed all default Microsoft boilerplate code (like `WeatherForecast`) to ensure a perfectly clean slate for our upcoming Application Layer development.

---

## 2. Database Deployment (Code-First Migrations)
Instead of manually clicking around in SSMS to build tables, we utilized an enterprise **Code-First** strategy.

1. **Migration Generation:** We ran `Add-Migration InitialCreate` to compile our C# Entities into raw SQL instructions.
2. **Remote Execution:** We ran `Update-Database`, which successfully connected to the remote SQL Server across the internet and instantly built all 12 tables within the `rms` schema.

---

## Next Steps (Milestone 2)
With the database physically deployed and the Domain securely mapped, the next milestone will focus on the **Application Layer**.
- Implementing the **Unit of Work** and **Repository Patterns**.
- Building Business Services (e.g., `OrderService`).
- Creating the first REST API Controllers to perform CRUD operations on the remote database via Swagger.
