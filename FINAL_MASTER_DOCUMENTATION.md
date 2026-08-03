# Multi-Tenant Restaurant Management System (RMS)

## 1. Executive Summary
This document serves as the comprehensive A-to-Z blueprint and master documentation for the **Restaurant Management System (RMS)**. Designed to meet modern enterprise standards, this project is built to support a **Multi-Tenant (Multiple Company Support) SaaS model**, utilizing a highly scalable **Clean Architecture** built on **C# .NET Core**.

### Core Enhancements
- **Clean Architecture:** Strict separation of concerns (Domain, Application, Infrastructure, Presentation) ensuring a highly testable and maintainable codebase.
- **Multi-Tenant System:** A single instance of the software serves multiple companies/restaurants independently, using `TenantId` (CompanyId) isolation at the database level.
- **Offline-First POS:** Robust Point of Sale capabilities designed to handle high transaction volumes without interruption.

---

## 2. Requirements Analysis

### 2.1 Functional Requirements
- **User Management:** Secure authentication and role-based authorization (Admin, Manager, Cashier, Waiter, Kitchen Staff).
- **Point of Sale (POS):** Order entry, split billing, discount/coupon application, custom tipping, and receipt generation.
- **Inventory Management:** Real-time stock tracking, low stock alerts, supplier management, and purchase order generation.
- **Menu Management:** Dynamic management of categories, items, variations (sizes/addons), pricing, and availability status.
- **Table Management:** Visual floor plan with real-time table status (Available, Occupied, Reserved) and reservation management.
- **Kitchen Display System (KDS):** Digital order routing, ticket timers, and status updates (Pending, Cooking, Ready).
- **Reporting & Analytics:** Comprehensive dashboards for sales reports, tax reports, inventory consumption, and employee shift performance.
- **Shift & Cash Management:** Employee clock-in/out for payroll tracking, cash register float management, and end-of-day Z-Reports.
- **CRM & Loyalty Programs:** Customer profiles tracking order history, preferences, and a points-based loyalty reward system.
- **Third-Party Integrations:** API gateway for automatic synchronization of orders from delivery platforms (e.g., UberEats, DoorDash).

### 2.2 Non-Functional Requirements
- **Performance:** High responsiveness with fast transaction processing, especially in the POS module during peak hours.
- **Security:** Data encryption at rest and in transit, strict role-based access control (RBAC), and PCI-DSS compliance for payment gateway integration.
- **Reliability:** Automated daily data backups and robust error handling.
- **Hardware Integration:** Support for standard POS hardware including ESC/POS receipt printers, cash drawer kicks, barcode scanners, and EMV payment terminals.
- **Scalability:** Modular architecture designed to scale seamlessly as new restaurant tenants subscribe to the platform.

---

## 3. Module & Sub-Module Breakdown

The system is logically divided into distinct modules to handle different aspects of the restaurant business.

### 3.1 High-Level Module Visualization

```mermaid
mindmap
  root((RMS SaaS))
    Company Admin Module
      Subscription Management
      Branch Setup
      Staff Management
    POS Module
      Order Entry
      Billing & Checkout
      Table View
    KDS Module
      Order Queue
      Status Tracking
    Inventory Module
      Stock Tracking
      Purchase Orders
      Recipe Management
    Reporting Module
      Sales Analytics
      Tax Reports
      Employee Performance
```

### 3.2 Detailed Sub-Module Breakdown
#### 1. Tenant/Company Admin Module
- **Super Admin Sub-module:** Manage SaaS subscriptions, onboard new companies, system-wide metrics.
- **Company Profile Sub-module:** Configure tax rates, currency, receipt layouts, and business hours.
- **User & Role Sub-module:** Manage employees, assign roles (Cashier, Waiter, Kitchen), and configure RBAC.

#### 2. Point of Sale (POS) Module
- **Order Entry Sub-module:** Touch-friendly interface for item selection, applying variations/addons, and split billing.
- **Table Management Sub-module:** Visual floor plan showing real-time table statuses.
- **Payment Sub-module:** Process cash, card, and mobile payments. Generate digital or printed receipts.

#### 3. Kitchen Display System (KDS) Module
- **Ticket Routing Sub-module:** Automatically route food items to kitchen screens and beverages to the bar.
- **Status Tracking Sub-module:** Mark items as *Pending*, *Cooking*, or *Ready*.
- **Service Alerts:** Notify waitstaff via the POS or mobile devices when an order is ready for pickup.

#### 4. Inventory & Menu Management Module
- **Menu Engineering Sub-module:** Create categories, items, and dynamic pricing.
- **Stock Tracking Sub-module:** Real-time deduction of inventory based on sold items (using mapped recipes/ingredients).
- **Supplier & PO Sub-module:** Manage supplier details and generate Purchase Orders.

#### 5. Analytics & Reporting Module
- **Sales Dashboard:** Visual charts for daily/weekly/monthly revenue and top-selling items.
- **Operational Reports:** Inventory consumption reports, Z-Reports (End of Day), and tax summaries.

---

## 4. System Workflows

### 4.1 Standard Workflow: Order to Payment
```mermaid
sequenceDiagram
    participant Waiter/Cashier
    participant POS System
    participant Kitchen (KDS)
    participant Inventory/Reporting
    
    Waiter/Cashier->>POS System: Enter Order
    POS System->>Kitchen (KDS): Route Order Ticket
    Kitchen (KDS)-->>POS System: Status: Cooking
    Kitchen (KDS)-->>POS System: Status: Ready
    POS System-->>Waiter/Cashier: Notify Service Ready
    Waiter/Cashier->>POS System: Generate Bill & Process Payment
    POS System->>Inventory/Reporting: Deduct Stock & Update Sales
```

### 4.2 Third-Party Delivery Workflow (e.g., UberEats)
```mermaid
sequenceDiagram
    participant Delivery App
    participant Integrations Gateway
    participant POS System
    participant Kitchen (KDS)
    
    Delivery App->>Integrations Gateway: New Order Placed
    Integrations Gateway->>POS System: Inject Order into Queue
    POS System->>Kitchen (KDS): Auto-Route Ticket
    Kitchen (KDS)-->>POS System: Status: Ready
    POS System-->>Integrations Gateway: Update Order Status
    Integrations Gateway-->>Delivery App: Notify Driver for Pickup
```

---

## 5. System Architecture (Clean Architecture)

The system is designed using the **Clean Architecture** pattern in **C# .NET Core**. Dependencies flow inwards toward the Domain layer, ensuring business logic is completely isolated from UI frameworks or database technologies.

### 5.1 Architecture Flow
```mermaid
graph TD
    subgraph PresentationLayer [Presentation Layer]
        UI["Web API (ASP.NET Core)"]
        Blazor["Blazor / React SPA"]
    end
    
    subgraph InfrastructureLayer [Infrastructure Layer]
        EFCore["EF Core Repository"]
        Identity["Auth / Identity"]
        External["Third-Party Integrations"]
    end
    
    subgraph ApplicationLayer [Application Layer]
        CQRS["MediatR (Commands/Queries)"]
        Interfaces["Interfaces / DTOs"]
    end
    
    subgraph DomainLayer [Domain Layer]
        Entities["Core Entities (Models)"]
        Exceptions["Domain Exceptions"]
    end

    UI --> CQRS
    Blazor --> UI
    EFCore -.-> Interfaces
    Identity -.-> Interfaces
    External -.-> Interfaces
    CQRS --> Entities
    Interfaces --> Entities
```

### 5.2 Visual Studio Solution Folder Structure
```text
RestaurantManagementSystem.sln
│
├── 1. RMS.Domain (Class Library)
│   ├── Common/              # Base Entity classes
│   ├── Entities/            # Tenant, User, Order, MenuItem, Table
│   ├── Enums/               # OrderStatus, RoleType
│   └── Exceptions/          # Domain-specific errors
│
├── 2. RMS.Application (Class Library)
│   ├── Interfaces/          # IApplicationDbContext, ITenantService
│   ├── DTOs/                # OrderResponseDto, CreateUserRequestDto
│   ├── Features/            # CQRS Pattern (Organized by feature)
│   │   ├── Orders/          # Commands (CreateOrder) & Queries
│   │   └── Menu/            # Commands (AddMenuItem)
│   └── Behaviors/           # MediatR validation pipeline behaviors
│
├── 3. RMS.Infrastructure (Class Library)
│   ├── Persistence/         # ApplicationDbContext, Migrations
│   │   └── Configurations/  # FluentAPI Entity configurations
│   ├── Services/            # TenantResolutionService (Reads TenantId from JWT)
│   └── Authentication/      # JwtTokenGenerator setup
│
└── 4. RMS.Api (Web API Project - Startup)
    ├── Controllers/         # API Endpoints (e.g. OrdersController)
    ├── Middlewares/         # Global Exception Handling, Tenant Middleware
    └── Program.cs           # Dependency Injection setup
```

---

## 6. Multi-Tenant Database Design

To support multiple companies from a single database, we use **Database-Level Multi-Tenancy** (Row-Level Security). Every core table includes a `TenantId` (or `CompanyId`) column.

### 6.1 Database Entity Relationship (ER) Diagram

```mermaid
erDiagram
    TENANT ||--o{ USER : "employs"
    TENANT ||--o{ MENU_CATEGORY : "owns"
    TENANT ||--o{ TABLE : "manages"
    TENANT ||--o{ ORDER : "processes"
    TENANT ||--o{ INVENTORY : "tracks"
    
    USER ||--o{ ORDER : "takes"
    ROLE ||--o{ USER : "has"
    TABLE ||--o{ ORDER : "placed at"
    ORDER ||--|{ ORDER_ITEM : "contains"
    
    MENU_CATEGORY ||--|{ MENU_ITEM : "categorizes"
    MENU_ITEM ||--o{ ORDER_ITEM : "ordered as"

    TENANT {
        uniqueidentifier Id PK
        string CompanyName
        string Subdomain
    }
    USER {
        uniqueidentifier Id PK
        uniqueidentifier TenantId FK
        string Username
        int RoleId FK
    }
    ORDER {
        uniqueidentifier Id PK
        uniqueidentifier TenantId FK
        uniqueidentifier TableId FK
        decimal TotalAmount
        string Status
    }
```

### 6.2 Data Dictionary (Core Tables)

| Table Name | Description | Key Columns |
| :--- | :--- | :--- |
| **Tenants** | The root table identifying different restaurant businesses. | `Id` (PK), `CompanyName`, `Subdomain`, `CreatedAt` |
| **Users** | Employees belonging to a specific tenant. | `Id` (PK), `TenantId` (FK), `Username`, `PasswordHash`, `RoleId` |
| **MenuItems** | Food and beverages sold by a tenant. | `Id` (PK), `TenantId` (FK), `Name`, `Price`, `IsAvailable` |
| **Orders** | Customer orders placed at the restaurant. | `Id` (PK), `TenantId` (FK), `TableId` (FK), `UserId` (FK), `TotalAmount`, `Status` |

### 6.3 Implementation: EF Core Global Query Filters
To ensure data isolation so Restaurant A never sees Restaurant B's data, we apply a global filter in the `ApplicationDbContext`. The `TenantId` is extracted from the user's JWT token via the `ITenantService`.
```csharp
protected override void OnModelCreating(ModelBuilder builder)
{
    // Automatically filter all queries so users only see their company's data
    builder.Entity<Order>().HasQueryFilter(e => e.TenantId == _tenantService.TenantId);
    builder.Entity<MenuItem>().HasQueryFilter(e => e.TenantId == _tenantService.TenantId);
}
```

---

## 7. Conceptual UI Wireframes

1. **POS Dashboard (Front of House):**
   - **Left Panel:** Grid of category buttons (e.g., Starters, Mains, Desserts, Beverages) with colorful icons.
   - **Center Panel:** Responsive grid of menu items corresponding to the selected category.
   - **Right Panel:** The active order ticket displaying selected items, quantities, and prices. Includes a summary section for Subtotal, Tax, Discount, and a prominent "Pay / Checkout" button.
   
2. **Kitchen Display System (KDS):**
   - **Kanban-style Grid:** Displaying active tickets in columns (Pending, In Progress, Ready).
   - **Ticket Details:** Each ticket prominently shows the Table Number, Waiter Name, Elapsed Time (color-coded red if delayed), and the list of items with special dietary notes.

3. **Admin Dashboard (Back Office):**
   - **Top Overview:** KPIs like Today's Sales, Active Orders, and Low Stock Alerts.
   - **Sidebar Navigation:** Expandable menu for Reports, User Management, Menu Engineering, Inventory control, and System Settings.

---

## 8. Technology Stack Summary

| Layer/Component | Technology |
| :--- | :--- |
| **Backend Framework** | C# .NET Core (Web API) |
| **Architecture** | Clean Architecture, CQRS (MediatR) |
| **Database** | Microsoft SQL Server |
| **ORM** | Entity Framework Core (EF Core) |
| **Authentication** | ASP.NET Core Identity & JWT |
| **Multi-Tenancy** | Database-level (TenantId column + EF Core Query Filters) |
| **Frontend Options** | Blazor WebAssembly / React.js / WPF |

---

## 9. Step-by-Step Implementation Roadmap

### **Phase 1: Foundation (The Base Structure)**
1. Create the 4 projects in Visual Studio (`Domain`, `Application`, `Infrastructure`, `Api`).
2. Set up Project References correctly adhering to Clean Architecture.
3. Create the Domain Entities with `TenantId` properties.

### **Phase 2: Database & Multi-Tenancy**
1. Set up `ApplicationDbContext` in Infrastructure.
2. Implement the `ITenantService` and the EF Core Global Query Filters.
3. Generate the first Entity Framework Migration and update the SQL Server database.

### **Phase 3: Authentication & Security**
1. Implement ASP.NET Core Identity for users.
2. Build the Login endpoint to generate JWT Tokens (ensuring `TenantId` and `Role` are inside the payload).
3. Test Multi-Tenancy: Login as Restaurant A, create an item. Login as Restaurant B, verify you cannot see Restaurant A's item.

### **Phase 4: Core Business Logic (CQRS)**
1. Build the **Menu Module** (Create/Read/Update Menu Items).
2. Build the **Table Management Module** (Add tables, change status).
3. Build the **POS Order Module** (Creating an Order, linking Order Items, calculating totals).

### **Phase 5: Frontend Integration**
1. Connect the chosen frontend (Blazor, React, or WPF) to the API endpoints.
2. Build the POS Interface, Kitchen Display System (KDS), and Admin Dashboards.
