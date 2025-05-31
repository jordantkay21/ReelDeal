---
icon: clipboard
---

# Manager

## CORE RESPONSIBILITIES

* Stores **persistent or session-bound data** that can be reused to prevent redundant backend calls.
* Acts as a passive lookup layer—only queried or written to, never responsible for routing logic.
* May include in-memory session variables, file-based cache, or serialized objects depending on implementation.

## BEHAVIOR OVERVIEW

* Data is written to the `Manager` after successful backend operations (e.g., token acquisition, server retrieval).
* When the `FrontendCoordinator` or other system layers need data, the `Manager` is consulted first.
* Matching data is returned if available; otherwise, the request continues toward the backend.

## SYSTEM INTEGRATION

* **Input from FrontendCoordinator:** Read/write requests (e.g., “is token cached?” or “save this user info”)
* **Output to FrontendCoordinator:** Cached values or null/miss flags
* **Input from BackendCoordinator:** New data to persist after backend execution
* **Output to BackendCoordinator (optional):** Session continuity data for cross-call consistency

## WHY IT EXISTS

The `Manager` enables fast response times and bandwidth optimization by avoiding unnecessary backend calls. It:

* Centralizes session data in one accessible location
* Makes the system more efficient and responsive
* Supports fallback or offline behaviors if no backend is available
