---
icon: arrows-to-circle
---

# FrontendCoordinator

## CORE RESPONSIBILITIES

* Serves as the **decision router** for all frontend-originated actions.
* Determines whether the system can fulfill a request using locally cached data (`Manager`) or whether it must invoke backend services.
* Builds and returns structured `FrontendResponsePayload`s back to the frontend layer once resolution is complete.

## BEHAVIOR OVERVIEW

* Receives standardized `ActionPayload`s from the `FrontendController`, decodes their intent, and chooses a processing route.
* If the request targets something already stored (like user info or playlist data), it queries the `Manager` to retrieve it.
* If local data is unavailable or invalid, it forwards a `BackendPayload` to the `BackendCoordinator` for external resolution.
* Once data is retrieved (from cache or backend), it formats a `FrontendResponsePayload` for return to the frontend.

## SYSTEM INTEGRATION

* **Input from FrontendController:** `ActionPayload` containing user intent and context
* **Output to Manager:** Keyed queries for cached data or session state
* **Output to BackendCoordinator:** `BackendPayload` when local data is insufficient
* **Input from BackendCoordinator:** `BackendResponsePayload` results from executed backend logic
* **Output to FrontendController:** `FrontendResponsePayload` containing display-ready information

## WHY IT EXISTS

The `FrontendCoordinator` protects the frontend from needing to understand or manage cache logic or backend routing. It ensures:

* Optimized request handling (via early cache returns)
* Consistent routing logic for all frontend requests
* Isolation of data access strategy from the presentation layer
