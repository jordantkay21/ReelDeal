---
icon: arrows-to-circle
---

# BackendCoordinator

## CORE RESPONSIBILITIES

* Receives backend operation requests from the `FrontendCoordinator` in the form of `BackendPayload`s.
* Delegates execution to the appropriate `ServiceController` in the Backend Layer.
* Collects the resulting `BackendResponsePayload` and returns it up the chain to be processed and cached.

## BEHAVIOR OVERVIEW

* Acts as a **dispatch hub** for service calls such as API interactions, file reads, or system-level operations.
* Knows which service controller is responsible for which type of logic (e.g., auth, playlist, trailers).
* Passes backend results upstream and optionally signals the `Manager` to cache the result.

## SYSTEM INTEGRATION

* **Input from FrontendCoordinator:** `BackendPayload` describing a backend task
* **Output to ServiceController:** Task-specific DTOs defining exact execution logic
* **Input from ServiceController:** `BackendResponsePayload` upon completion
* **Output to FrontendCoordinator:** Final result packaged for frontend routing
* **Output to Manager (optional):** Persist important results like tokens, device codes, user data

## WHY IT EXISTS

The `BackendCoordinator` abstracts backend complexity from the rest of the system. It:

* Keeps bridge logic clean and agnostic of service-specific implementation
* Allows backend services to evolve independently
* Ensures payload structure, delegation, and response format remain consistent
