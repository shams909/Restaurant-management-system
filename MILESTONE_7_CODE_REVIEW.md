# Milestone 7 Code Review: Hardening the Engine

## Overview
After successfully building the Multi-Tenant Order Engine, the system underwent a Senior-level Code Review (via AI Assistants). Three critical edge-case vulnerabilities were identified and resolved, elevating the codebase from a "working prototype" to "production-ready software."

---

## 1. The Broken Token Fallback (Security Fix)
**The Vulnerability:** 
The Entity Framework Global Query Filter used an OR operator: `BranchId == CurrentBranchId || CurrentBranchId == 0`. If a user presented a corrupted JWT token, the system failed to parse the Branch ID, defaulted to `0`, and bypassed the security filter entirely, exposing all restaurant data.

**The Fix:**
The `|| CurrentBranchId == 0` fallback was explicitly removed. In SaaS security, authorization must "fail closed" (deny all access) rather than "fail open" (grant all access) when encountering bad data.

## 2. The Negative Stock Glitch (Business Logic Fix)
**The Vulnerability:** 
The inventory deduction math (`CurrentStock -= amountUsed`) blindly subtracted ingredients. If an order requested more ingredients than were physically available in the stock room, the database would save a negative stock quantity (e.g., `-5 Burger Patties`).

**The Fix:**
An aggressive Guard Clause was implemented in the `OrderService`. Before any math is executed, the system verifies `if (CurrentStock < amountUsed)`. If triggered, the system throws a fatal exception, immediately aborting the HTTP request and preventing the database from saving an impossible reality.

## 3. The Ghost Transactions (Data Integrity Fix)
**The Vulnerability:**
The `InventoryTransaction` records were successfully generated to log ingredient usage, but they were missing the `TenantId`. Because Global Query Filters hide any data that doesn't match the active `TenantId`, these transactions were silently disappearing from all database queries.

**The Fix:**
The `ICurrentUserService` (the security bouncer) was injected directly into the `OrderService`. When building the receipt, the `TenantId` is securely extracted from the user's token and stamped onto the transaction, ensuring the receipt is visible to the correct restaurant manager.
