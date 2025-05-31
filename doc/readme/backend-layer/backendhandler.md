---
icon: hand
---

# BackendHandler

## CORE RESPONSIBILITIES

* Executes the **actual backend operation**, such as:
  * Making HTTP API calls (e.g., Plex OAuth endpoints)
  * Reading or writing to disk
  * Parsing XML/JSON responses
  * Authenticating users or retrieving remote metadata
* Returns a raw `DTOResponse` which the `ServiceController` will format.

## BEHAVIOR OVERVIEW

* Acts as the **execution engine** of your backend—it doesn’t decide what to do, it just executes exactly what the `ServiceDTO` tells it to do.
* Operates using asynchronous methods (e.g., `await HttpClient.GetAsync(...)`) or coroutines, depending on implementation.
* Responsible for handling request exceptions (timeouts, 404s, network issues), but not interpreting them.

## SYSTEM INTEGRATION

* **Input from ServiceController:** A structured `ServiceDTO` containing task details
* **Output to ServiceController:** A `DTOResponse` with raw response data or error state
* **Accesses external systems:** APIs, file system, databases, cloud endpoints

## WHY IT EXISTS

The `ServiceHandler` decouples **low-level logic** from every other layer. This ensures:

* All backend work is done in a single, testable location
* Backend tasks are consistent, centralized, and easy to audit
* Side effects (I/O, network) are isolated from controllers and coordinators
