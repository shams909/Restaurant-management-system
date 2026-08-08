# Milestone 7 (Phase 2) Summary: The Restaurant Order Engine

## Overview
This document summarizes the completion of Phase 2 of Milestone 7. The primary objective was to transition the `OrderService` from a basic CRUD (Create, Read, Update, Delete) operation into a true **Business Logic Engine** that automates the complex workflows of a commercial restaurant.

---

## 1. The Problem: "Dumb" CRUD Services
Before this phase, the frontend application was responsible for calculating the total cost of an order, and the backend simply saved whatever number the frontend sent. 
Additionally, placing an order did absolutely nothing to the restaurant's inventory levels, rendering the backend useless for stock management.

### Major Flaws Identified:
- **Security Vulnerability:** A malicious user could manipulate the HTTP POST request to set the `GrandTotal` of a $100 meal to $0.01, and the API would accept it blindly.
- **Missing Domain Logic:** The kitchen had no idea how many raw ingredients (e.g., burger patties, buns) were being consumed, leading to inaccurate stock data.
- **No Traceability:** If stock levels changed, managers had no paper trail to verify why.

## 2. The Solution: Automated Engine Workflows
The `OrderService.CreateOrderAsync` method was completely rewritten to intercept the raw order items from the client and perform all mathematical calculations on the secure server.

### Step-by-Step Workflow:
1. **Server-Side Pricing:** 
   The engine now loops through the requested items, queries the database for the *true* `BasePrice` of each item, and mathematically computes the `GrandTotal`. The frontend's pricing calculations are completely ignored.
   
2. **KDS Integration:** 
   Every order item is automatically stamped with `KdsStatus = "Pending"`. This ensures the item is instantly routed to the Kitchen Display System without any manual intervention.

3. **Recipe Traversal & Inventory Deduction:**
   For every menu item ordered, the engine queries the `Recipe` table to determine exactly which raw ingredients are required to cook it (e.g., 1 Burger = 1 Patty + 1 Bun). It calculates the `QuantityUsed` and automatically deducts it from the `InventoryItems` stock level.

4. **The Ledger (InventoryTransactions):**
   To prevent "ghost" stock changes, the engine creates an `InventoryTransaction` receipt for every deduction. This creates an immutable, timestamped ledger that a manager can review to see exactly when and why an ingredient left the stock room.

---

## 3. Results & Architecture Maturity
The API is no longer just a database wrapper; it is the "Brain" of the restaurant. By centralizing the business logic inside the Application layer (Clean Architecture), any platform (Web, iOS, Android, POS Terminal) can hit the API, and the backend will guarantee that the math is correct, the stock is deducted, and the kitchen is notified.
