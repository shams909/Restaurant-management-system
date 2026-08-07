# Milestone 3 Summary: Enterprise Security & Data Transfer Objects (DTOs)

## Overview
This document summarizes the technical achievements completed in Milestone 3 of the Restaurant Management System (RMS). The primary objective of this phase was to secure the application against data exposure and hardcoded credential vulnerabilities using industry-standard enterprise patterns.

---

## 1. Environment Variable Security (`.env`)
A critical security audit revealed that the database connection string was temporarily hardcoded in `ApplicationDbContext.cs` and `appsettings.json`, which poses a massive security risk if pushed to public source control.

**What we did:**
- We implemented the `DotNetEnv` package to load configuration from a local `.env` file.
- The connection string was completely removed from the C# source code.
- We updated `.gitignore` to explicitly ban `*.env` files from being tracked by Git.
- In `Program.cs`, we securely injected the database connection at runtime using `Environment.GetEnvironmentVariable()`.

**Why it matters:** 
This ensures that our production database credentials will never be leaked to GitHub or exposed to automated scraping bots, adhering to strict DevSecOps best practices.

---

## 2. Data Transfer Objects (DTOs)
Prior to this milestone, our API endpoints accepted and returned raw Domain Entities (e.g., `Tenant`). This is a security vulnerability known as "Over-Posting" or "Mass Assignment," where malicious actors can inject data into restricted columns (like `Id` or `IsAdmin`).

**What we did:**
- We implemented **DTOs (Data Transfer Objects)** in the `RMS.Application` layer (e.g., `TenantDto`, `CreateTenantDto`).
- `CreateTenantDto` strictly enforces exactly which fields the API will accept (excluding sensitive fields like `Id` and `CreatedAt`).

**Why it matters:** 
The API is now completely insulated. Swagger UI correctly reflects the safe schemas, and our internal database structures are hidden from the public internet.

---

## 3. AutoMapper Integration & Version 13 Syntax
To prevent writing hundreds of lines of boilerplate code to manually map `CreateTenantDto` to `Tenant`, we integrated **AutoMapper**.

**Challenge Faced & Overcome:**
We encountered a dependency injection compilation error (`CS1503`) when attempting to register AutoMapper in `Program.cs`. 
Upon investigation, we identified that **AutoMapper version 13.0** completely overhauled its Dependency Injection syntax. It removed the `AutoMapper.Extensions.Microsoft.DependencyInjection` package and deprecated the `Assembly[]` registration method.

**The Solution:**
We successfully implemented the modern AutoMapper 13 lambda configuration syntax to explicitly register the `MappingProfile`:
```csharp
builder.Services.AddAutoMapper(cfg => 
{
    cfg.AddProfile<RMS.Application.Mappings.MappingProfile>();
});
```

This successfully resolved the DI pipeline error and allowed our `TenantsController` to seamlessly convert DTOs to Entities and vice versa.
