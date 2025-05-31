---
icon: gamepad-modern
---

# BackendController

## CORE RESPONSIBILITIES

* Acts as the **backend logic entry point**, receiving task-specific instructions (`BackendPayload`) from the `BackendCoordinator`.
* Converts high-level requests into precise, execution-ready DTOs (`ServiceDTO`) that define what action the system needs to perform.
* After receiving raw results from the `ServiceHandler`, it **interprets, validates, and wraps** the response into a `BackendResponsePayload`.

## BEHAVIOR OVERVIEW

* Acts as the **interpreter and packager**—it bridges the structure of the logic request (`BackendPayload`) with the exact details needed by the execution engine (`ServiceHandler`).
* Adds any required formatting, validation, or fallback logic before finalizing the response.
* Handles both successful and failed backend operations consistently by standardizing all results as `BackendResponsePayload`s.

## SYSTEM INTEGRATION

* **Input from BackendCoordinator:** `BackendPayload` describing what kind of operation to perform
* **Output to ServiceHandler:** A `ServiceDTO` containing endpoint details, identifiers, headers, parameters, etc.
* **Input from ServiceHandler:** `DTOResponse` with raw success/failure data (e.g., response body, error message)
* **Output to BackendCoordinator:** Final `BackendResponsePayload` with structured content and metadata

## WHY IT EXISTS

The `ServiceController` provides a layer of **abstraction, translation, and standardization**:

* Keeps execution logic reusable and cleanly structured
* Makes results easier to cache and display
* Shields higher layers from dealing with raw HTTP or disk behavior
