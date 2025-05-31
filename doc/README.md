---
icon: sitemap
---

# Data Lifecycle Documentation

**PRIMARY FLOW STRUCTURE:** Splits data flow into _**Inbound Logic**_ (🔵 Blue) and _**Outbound Logic**_ (🟢 Green), following it as it logic is initiated via `frontend` (🟠 Orange) and pushed through to the `backend` (🟣 Purple) and back to the `frontend` to display results back to the user.



## OVERVIEW

> This system separates logic into **three clean tiers**, each with specific responsibilities, data formats, and flow behaviors. Below is a complete outline of each tier, including key components, payload types, DTOs, and an **"In Practice"** example to illustrate how data moves through the layer.

{% tabs %}
{% tab title="FRONTEND LAYER" %}
**PURPOSE:** \
Captures raw user interaction and displays results

**CONTAINS:**

* `FrontendHandler` - **triggers** and **receives** visual updates
* `FrontendController` - **formats data** for logic and UI

**KEY PAYLOAD(S)**

* `ActionPayload` - Encapsulates user intent captured by the frontend and prepares it for routing through the bridge layer

**IN PRACTICE**

1. Player clicks "CONNECTION" button → `FrontendHandler` triggers event
2. Controller wraps into `ActionPayload`
3. Once logic is complete system-wide, `FrontendResponsePayload` is returned to controller → handler updates button visuals, text, or feedback indicators
{% endtab %}

{% tab title="BRIDGE LAYER" %}
**PURPOSE:** \
Coordinates all mid-flow routing, caching, and decision-making

**CONTAINS:**

* `FrontendCoordinator` - **routes ActionPayloads**, chooses local vs backend
* `Manager` - **caches** reusable session or gameplay **data**
* `BackendCoordinator` - **sends requests** to service logic and **collects results**

**KEY PAYLOAD(S)**

* `BackendPayload` - Represents a backend oepration request that could not be fulfilled by local cache or manager data
* `FrontendResponsePayload` - Contains formatted, display-ready data returned to the frontend for UI updates or feedback

**IN PRACTICE**

1. `FrontendCoordinator` receives an `ActionPayload` and checks `Manager` for cached data
2. If cached data exists → wraps it in `FrontendResponsePayload` and returns immediately
3. If not → emits a `BackendPayload` to `BackendCoordinator`, which will forward it to backend for fulfillment
4. On backend success, result is cached via `Manager` and sent back to frontend via `FrontendResponsePayload`
{% endtab %}

{% tab title="BACKEND LAYER" %}
**PURPOSE:** \
Executes actual backend operations and prepares responses

**CONTAINS:**

* `ServiceController` - **builds DTOs** and wraps responses
* `ServiceHandler` - **handles real API calls**, file I/O, and parsing

**KEY PAYLOAD(S)**

* `BackendResponsePayload` - Wraps the result of a backend operation (success or failure) in a structured format ready to return to the bridge layer

**KEY DTO(S)**

* `ServiceDTO` - A purpose-built instruction object created from a _BackendPayload_ that defines exactly what backend operation to execute&#x20;
* `DTOResponse` - A raw result or failure returned by the backend logic (e.g. API call or file read) that is passed back up the stack to be structured into a formal response

**IN PRACTICE**

* ServiceController receives a BackendPayload and constructs a specific ServiceDTO (e.g. for fetching a Plex trailer)
* `ServiceHandler` uses the `ServiceDTO` to perform the actual API request or file operation
* The result is returned as a `DTOResponse` (success or error), which is wrapped into a `BackendResponsePayload` and sent back to the BackendCoordinator
* From there, the result continues to flow back through the Bridge Layer and ultimately updates the UI
{% endtab %}
{% endtabs %}

