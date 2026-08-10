# Milestone 11: Future-Proofing with Custom Exceptions

## The Problem (The "Boy Who Cried Wolf")
In the early stages of this project, whenever a user did something wrong (like trying to order a burger without enough stock in the inventory), we used standard C# exceptions:
`throw new Exception("Insufficient Stock!");`

**The Analogy:**
Imagine you own a restaurant. A waiter runs into the office and screams, *"PROBLEM!"*
As the Manager (the Middleware), you have no idea what is happening. Is the problem *"A customer dropped their fork"* (a minor user error), or is the problem *"The kitchen is literally on fire"* (a catastrophic server crash)? 

Because the waiter just yelled a generic "Problem!" (`System.Exception`), the Middleware had to guess how to handle it using a hacky trick (`if exception.GetType() == typeof(Exception)`). 

## The Enterprise Solution (Custom Exceptions)
In Enterprise Architecture, we don't let our code yell generic problems. We create specific, named alarm bells.

We created a custom class called `BadRequestException` that inherits from the base `Exception` class. 
Now, when a user tries to order without stock, we write:
`throw new BadRequestException("Insufficient Stock!");`

**The Analogy:**
Now, the waiter runs into the office and specifically yells, *"CUSTOMER DROPPED A FORK!"* 

The Manager (the Middleware) instantly knows this is a **400 Bad Request** (a minor, expected user error). 
If the waiter ever yells the generic *"PROBLEM!"* (`System.Exception`) again, the Manager instantly knows it must be a real system failure (like a Database Timeout or a Null Reference) and correctly returns a **500 Internal Server Error**.

## What We Changed in the Code
1. **Created `BadRequestException.cs`**: A dedicated class to represent business rule violations.
2. **Updated `ExceptionMiddleware.cs`**: 
   ```csharp
   // The Middleware no longer has to guess. It explicitly checks for our custom alarm bell!
   if (exception is BadRequestException)
   {
       context.Response.StatusCode = StatusCodes.Status400BadRequest;
   }
   else
   {
       context.Response.StatusCode = StatusCodes.Status500InternalServerError;
   }
   ```

By doing this, we eliminated a fragile "hack" and replaced it with type-safe, enterprise-grade error routing.
