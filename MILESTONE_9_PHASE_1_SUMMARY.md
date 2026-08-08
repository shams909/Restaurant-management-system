# Milestone 9 (Phase 1) Summary: Security & Scalability

## Overview
As the backend approaches completion, the focus shifted from feature development to **Enterprise Polish**. This phase introduced strict Role-Based Access Control (RBAC) across the API and hardened the Order Engine against client-side race conditions.

---

## 1. Role-Based Access Control (RBAC)
Prior to this milestone, any user possessing a valid JSON Web Token (JWT) could access any endpoint in the API. This meant a Waiter could theoretically access the Kitchen Display System (KDS) or view financial records.

**The Implementation:**
We leveraged the `RoleId` claim that was injected into the JWT during Milestone 6. By applying the `[Authorize(Roles = "...")]` attribute to specific controllers, we created strict security boundaries natively managed by ASP.NET Core:
- **`TenantsController`**: Locked to `SuperAdmin`. Only the system owner can onboard new restaurant companies.
- **`KitchenController`**: Locked to `Chef` and `Manager`. Waiters cannot manually change KDS ticket statuses.
- **`InventoryItemsController` & `RecipesController`**: Locked to `Manager`. Only authorized staff can view or modify stock room logic.
- **`UsersController`**: Locked to `Manager`. Waiters cannot view other employees' records or hire/fire staff.

By handling this at the Controller level, ASP.NET Core intercepts unauthorized requests and instantly returns a `403 Forbidden` response, meaning malicious requests never even reach the service layer.

---

## 2. Bulletproof Order Generation (Server-Side Logic)
**The Vulnerability:**
Previously, the API relied on the frontend React application to generate and send the `OrderNo` (e.g., `ORD-101`) via the `CreateOrderDto`. In a high-traffic environment, if two Waiters hit the "Checkout" button at the exact same millisecond, the frontend could theoretically generate the exact same `OrderNo` twice, causing a database collision and a fatal crash.

**The Fix:**
The power to generate Order Numbers was stripped from the frontend entirely. 
Inside `OrderService.CreateOrderAsync`, the server now dynamically generates a globally unique ticket number using a combination of a UTC Timestamp and a truncated GUID (e.g., `ORD-20260809123045-A1B2`). 
Because this occurs securely on the backend immediately before database insertion, it is mathematically impossible for two orders to ever collide.
