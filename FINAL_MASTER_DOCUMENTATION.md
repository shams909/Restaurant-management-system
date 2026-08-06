# Multi-Tenant & Multi-Branch Restaurant Management System (RMS)
**Enterprise SaaS Specification - Version 2.0**

## 1. Executive Summary
This document serves as the comprehensive A-to-Z blueprint for the **Restaurant Management System (RMS)**. Designed for modern enterprise scaling, this project is built to support a **Multi-Tenant (Multiple Company) & Multi-Branch SaaS model**. It utilizes a highly scalable **Clean Architecture** built on **C# .NET Core** and a dynamic **React SPA** frontend.

### Core Enhancements (Version 2)
- **Multi-Branch Hierarchy:** Supports global Companies (Tenants) that operate multiple distinct physical locations (Branches).
- **Clean Architecture:** Strict separation of concerns using the Service Pattern and Unit of Work.
- **Enterprise RBAC:** Strict Role-Based Access Control tied to a detailed permission matrix.

---

## 2. Requirements & Use Case Analysis

### 2.1 Functional Requirements
- **User Management:** Secure authentication and role-based authorization.
- **Point of Sale (POS):** Order entry, split billing, discount/coupon application, and receipt generation.
- **Inventory Management:** Real-time stock tracking per branch, supplier management, and purchase orders.
- **Menu Management:** Global menu management pushed down to individual branches.
- **Kitchen Display System (KDS):** Digital order routing and status updates.

### 2.2 System Actors & Use Case Diagrams

```mermaid
flowchart LR
    %% Actors
    SuperAdmin["Super Admin (SaaS Owner)"]
    CompanyAdmin["Company Admin (Tenant)"]
    Manager["Branch Manager"]
    Cashier["Cashier/Waiter"]

    %% SaaS Management Use Cases
    subgraph SaaS["SaaS Management"]
        UC1(["Create Tenant"])
        UC2(["Manage Subscriptions"])
    end
    
    %% Restaurant Operations Use Cases
    subgraph Ops["Restaurant Operations"]
        UC3(["Create Branch"])
        UC4(["Manage Global Menu"])
        UC5(["View Branch Reports"])
        UC6(["Take POS Orders"])
        UC7(["Manage Inventory"])
    end

    SuperAdmin --> UC1
    SuperAdmin --> UC2
    CompanyAdmin --> UC3
    CompanyAdmin --> UC4
    CompanyAdmin --> UC5
    Manager --> UC5
    Manager --> UC7
    Cashier --> UC6
```

---

## 3. Tenant Onboarding Workflow (Sequence Diagram)
To understand how the Multi-Branch architecture is initialized, below is the flow of onboarding a brand new restaurant chain into the SaaS platform.

```mermaid
sequenceDiagram
    participant SuperAdmin
    participant API
    participant Database
    
    SuperAdmin->>API: POST /api/tenants (Pizza Hut)
    API->>Database: Generate TenantId (GUID)
    Database-->>API: Success
    
    SuperAdmin->>API: POST /api/users (Create Company Admin)
    API->>Database: Save Admin User
    
    Note over API,Database: Company Admin logs in and configures branches
    
    CompanyAdmin->>API: POST /api/branches (Dhaka Branch)
    API->>Database: Save Branch with TenantId
    
    CompanyAdmin->>API: POST /api/branches (Sylhet Branch)
    API->>Database: Save Branch with TenantId
```

---

## 4. Multi-Tenant & Multi-Branch Database Architecture

To support massive scalability, we employ a hierarchical data model. A `Tenant` (Company) uses a secure **GUID** to prevent ID guessing. Physical locations are represented as `Branches` and use highly performant **Integers (INT)** for rapid SQL indexing.

```text
Tenant (e.g., Pizza Hut) - [GUID: 550e8400-e29b-41d4-a716-446655440000]
 │
 ├─ Branch (Dhaka) - [ID: 1]
 │    ├─ Orders (BranchId: 1)
 │    ├─ Employees (BranchId: 1)
 │    └─ Inventory (BranchId: 1)
 │
 └─ Branch (Sylhet) - [ID: 2]
      ├─ Orders (BranchId: 2)
      ├─ Employees (BranchId: 2)
      └─ Inventory (BranchId: 2)
```

*(Note: See the accompanying `RMS_Full_Database_Schema.md` file for the exact 20+ table data dictionary and ERD).*

---

## 5. Security & Permission Matrix

Access is strictly controlled via JWT tokens. The payload includes both the `TenantId` and the `BranchId` to ensure employees cannot view or manipulate data at other branches.

| Feature / Action | Company Admin | Branch Manager | Cashier | Waiter | Kitchen |
| :--- | :---: | :---: | :---: | :---: | :---: |
| **Manage Subscriptions** | ✅ | ❌ | ❌ | ❌ | ❌ |
| **Create New Branches** | ✅ | ❌ | ❌ | ❌ | ❌ |
| **Edit Global Menu** | ✅ | ❌ | ❌ | ❌ | ❌ |
| **Manage Inventory** | ✅ | ✅ | ❌ | ❌ | ❌ |
| **View End-of-Day Z-Report** | ✅ | ✅ | ❌ | ❌ | ❌ |
| **Void/Refund Order** | ✅ | ✅ | ❌ | ❌ | ❌ |
| **Process Payment** | ✅ | ✅ | ✅ | ❌ | ❌ |
| **Create POS Order** | ✅ | ✅ | ✅ | ✅ | ❌ |
| **Update KDS Status** | ❌ | ❌ | ❌ | ❌ | ✅ |

---

## 6. System Architecture (Clean Architecture)

```mermaid
graph TD
    subgraph PresentationLayer [Presentation Layer]
        UI["React SPA (Frontend)"]
        API["Web API (REST Controllers)"]
    end
    
    subgraph InfrastructureLayer [Infrastructure Layer]
        EFCore["EF Core (ApplicationDbContext)"]
        UoW["Unit of Work & Repositories"]
        Identity["Auth / Identity (JWT)"]
    end
    
    subgraph ApplicationLayer [Application Layer]
        Services["Business Services (OrderService, MenuService)"]
        Interfaces["Interfaces (IUnitOfWork, IRepository)"]
        DTO["Data Transfer Objects (DTOs)"]
    end
    
    subgraph DomainLayer [Domain Layer]
        Entities["Core Entities (Models)"]
        Exceptions["Domain Exceptions"]
    end

    UI --> API
    API --> Services
    EFCore -.-> Interfaces
    UoW -.-> Interfaces
    Services --> UoW
    Services --> Entities
    UoW --> Entities
```

---

## 7. REST API Design (Endpoints)

The system adheres to strict RESTful design principles. Below are the primary endpoints exposed by the .NET Core API.

### Authentication & Core
- `POST /api/v1/auth/login` (Returns JWT)
- `GET /api/v1/auth/me` (Returns User Profile & Permissions)
- `POST /api/v1/tenants` (Super Admin Only)
- `POST /api/v1/branches` (Company Admin Only)

### Menu & POS
- `GET /api/v1/menus` (Fetches global menu for the tenant)
- `GET /api/v1/branches/{branchId}/tables` (Fetches floor plan)
- `POST /api/v1/branches/{branchId}/orders` (Submit a new POS ticket)
- `PUT /api/v1/orders/{orderId}/status` (Update status to Paid/Void)

### Kitchen & Inventory
- `GET /api/v1/branches/{branchId}/kds/active-tickets` (Long-polling for Kitchen displays)
- `PUT /api/v1/kds/tickets/{ticketId}/status` (Mark cooking/ready)
- `GET /api/v1/branches/{branchId}/inventory/low-stock`

---

## 8. Deployment Architecture

To handle high traffic during peak restaurant hours, the system will be deployed to a scalable cloud environment.

- **Frontend:** The React SPA will be compiled into static assets and hosted on a global CDN (e.g., **Vercel** or **AWS S3/CloudFront**) for instant load times.
- **Backend API:** The .NET Core API will be containerized using **Docker** and deployed to **Azure App Services** (or AWS ECS), allowing horizontal scaling.
- **Database:** A managed instance of **Azure SQL Database** (or Amazon RDS for SQL Server) will handle the intense transactional loads, configured with automated daily backups.

---

## 9. 8-Week Agile Implementation Plan

### 9.1 Agile Gantt Chart
```mermaid
gantt
    title RMS 8-Week Enterprise Roadmap
    dateFormat  YYYY-MM-DD
    axisFormat  W%W
    
    section Foundation (Backend)
    Architecture & Core DB Setup    :a1, 2026-08-01, 7d
    Multi-Branch & Identity Auth    :a2, after a1, 7d
    
    section API & Business Logic
    UoW & Core Business Services    :b1, after a2, 7d
    Inventory & External Integrations:b2, after b1, 7d
    
    section React Frontend
    React Foundation & Admin UI     :c1, after b2, 7d
    POS Dashboard & Kitchen (KDS)   :c2, after c1, 7d
    
    section QA & Go-Live
    Unit & Integration Testing      :d1, after c2, 7d
    UAT & Cloud Deployment          :d2, after d1, 7d
```
