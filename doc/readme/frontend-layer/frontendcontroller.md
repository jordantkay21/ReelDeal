---
icon: gamepad-modern
---

# FrontendController

## CORE RESPONSIBILITIES

* Serves as the **logic orchestrator** for the UI layer.
* Translates raw, unstructured input from the handler into a normalized internal format (i.e., `ActionPayload`) which the system can understand and route through the Bridge Layer.
* Receives complete logic responses from the system (`FrontendResponsePayload`) and translates them into simplified, formatted UI data for the handler to display.

## BEHAVIOR OVERVIEW

* Performs basic validation and categorization of frontend requests (e.g., distinguish between "connect to Plex" vs. "select playlist").
* May enrich input with context before forwarding (e.g., timestamps, current session state).
* Filters and formats returned data before rendering, ensuring handlers don’t need to interpret anything.

## SYSTEM INTEGRATION

* **Input from Handler:** Raw interaction data (e.g., string identifiers, enum choices)
* **Output to Coordinator:** Structured request payloads (`ActionPayload`) for logic execution
* **Input from Coordinator:** `FrontendResponsePayload` containing display-ready data and status info
* **Output to Handler:** Clean, stylized UI update packages ready for rendering (e.g., "display username + avatar in green")

## WHY IT EXISTS

The `FrontendController` enforces a **clean break between interface and logic**, enabling:

* Standardized data flow through payloads
* Logical traceability for UI interactions
* Greater reuse and consistency across different parts of the UI
