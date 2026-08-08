# Milestone 8 Code Review: Securing the KDS

## Overview
Following the completion of the Kitchen Display System (KDS) and Payment Engine, the system underwent a secondary code review. Two critical vulnerabilities were discovered related to Entity Framework's data retrieval methods and relational data tracking. Both vulnerabilities were patched to ensure strict Multi-Tenant Data Isolation.

---

## 1. The Kitchen Data Leak (Missing Tenant Boundaries)
**The Vulnerability:** 
The `KitchenService` was designed to query the `OrderItems` table for all "Pending" food items. However, the `OrderItem` entity lacked a `BranchId` column, and the `ApplicationDbContext` did not have a Global Query Filter applied to it. 
As a result, a Chef logging into the system at Restaurant A would receive pending tickets from Restaurant B, resulting in a severe cross-tenant data leak.

**The Fix:**
- **Denormalization:** The `BranchId` was added directly to the `OrderItem` table, and the `OrderService` was updated to stamp the correct `BranchId` upon creation.
- **The Invisible Wall:** A new Global Query Filter (`builder.Entity<OrderItem>().HasQueryFilter(e => e.BranchId == CurrentBranchId);`) was applied to the database context, ensuring the KDS only returns tickets for the authenticated chef's specific kitchen.

---

## 2. The Entity Framework `FindAsync` Bypass
**The Vulnerability:**
The `GenericRepository` utilized Microsoft's built-in `_dbSet.FindAsync(id)` method to retrieve records by their Primary Key. 
A known architectural flaw in Entity Framework Core is that `FindAsync` accesses the local in-memory change tracker before executing SQL, which causes it to completely ignore Global Query Filters. A malicious user could theoretically guess an ID belonging to another tenant and `FindAsync` would bypass the security filter and return the data.

**The Fix:**
The `FindAsync` method was entirely removed from the codebase. It was replaced with `_dbSet.FirstOrDefaultAsync(e => EF.Property<object>(e, "Id").Equals(id))`. Because `FirstOrDefaultAsync` evaluates as a standard LINQ query, it correctly honors the Global Query Filters, sealing the vulnerability across every single entity in the database simultaneously.
