---
description: Path of Logic from the User's Interaction - Initiating a Server Request
---

# INCOMING LOGIC FLOW



{% stepper %}
{% step %}
### [Frontend Handler](incoming-logic-flow.md#frontend-handlers)

Captures or displays user input from UI components and exposes it via public methods
{% endstep %}

{% step %}
### [Frontend Controller](incoming-logic-flow.md#frontend-controller-1)

Gathers data from Handlers, validates it, and emits structured payloads via events
{% endstep %}

{% step %}
### [Frontend Coordinator](incoming-logic-flow.md#frontend-coordinator-1)

Enriches payloads with session data and decides whether to forward them to the backend
{% endstep %}

{% step %}
### [Manager](incoming-logic-flow.md#manager-1)

Stores and provides access to global, persistent session or configuration data
{% endstep %}

{% step %}
### [Backend Coordinator](incoming-logic-flow.md#backend-coordinator-1)

Receives enriched payloads, splits or routes them to appropriate service controllers
{% endstep %}

{% step %}
### [Backend Service Controller](incoming-logic-flow.md#backend-service-controller-1)

Processes business logic and orchestrates API calls through backend handlers
{% endstep %}

{% step %}
### [Backend API Handler](incoming-logic-flow.md#backend-api-handler-1)

Executes raw HTTP/API requests and parses external data responses
{% endstep %}
{% endstepper %}

## <mark style="color:green;background-color:green;">FRONTEND HANDLERS</mark>

> Gathers data from Handlers, validates it, and emits structured payloads via events

### HOW IT SENDS/RECEIVES DATA

**➡️ DATA is pulled** from `Handlers` by `Controllers` via _public methods_

### RESPONSIBILITIES

* Directly attached to a UI element like a `TMP_InputField`, `Button`, `Toggle`, or `Image`
* Gathers or displays data (e.g., username input, PIN code display)
* Exposes that data via a public method (e.g., `GetUsername()`)
* May optionally handle light UI behavior like local validation animations, field highlighting, or error outlines (visual-only feedback)
* Never owns logic  about what to _do_ with the data - it simply makes it accessible

### IN PRACTICE (PLEX EXAMPLE)

* `DeviceCodeHandler` reads the code from a `TMP_Text` and returns it via `GetDisplayedCode()`
* `ConnectButtonHandler` listens for a button click and informs the controller

## <mark style="color:green;background-color:green;">FRONTEND CONTROLLER</mark>

> Gathers data from Handlers, validates it, and emits structured payloads via events

### HOW IT SENDS/RECEIVES DATA

⬅️ **Pulls data** from Handlers\
&#xNAN;**➡️Pushes data** via `Action<T>`  to `Coordinators`

### RESPONSIBILITIES

* Centralizes and orchestrates the UI logic
* Holds references to one or more Frontend Handlers
* **Pulls** data using public methods (`handler.GetValue()`), performs validation (e.g., empty checks, format checks)
* If valid, packages data into a strongly-types payload (e.g., `DeviceCodePayload`)
* Emits that payload to a Coordinator via `Action<T>` or UnityEvent
* Does not handle enrichment or business decisions

### IN PRACTICE (PLEX EXAMPLE)

* `PlexAuthController` pulls code via`DeviceCodeHandler.GetDisplayCode()`, validates it, and emits `OnStartOAuthRequested(DeviceCodePayload)`

## <mark style="color:green;background-color:green;">FRONTEND COORDINATOR</mark>

> Enriches payloads with session data and decides whether to forward them to the backend

### HOW IT SENDS/RECEIVES DATA

⬅️ **Receives payloads via event** from `Controller`\
⬅️**Pulls extra data** from `Manager`\
&#xNAN;**➡️Pushes enriched payload** to `Backend Coordinators` via event

### RESPONSIBILITIES

* Subscribes to controller events
* Gathers additional context or enriches data from Manager (e.g., session ID, device ID, app version)
* Decides whether the system has enough data to proceed or needs to requery
* Once payload is ready, emits to the backend using another event system

### IN PRACTICE

* `PlexAuthCoordinator` listens for `OnStartOAuthRequested`, formats the `DeviceCodePayload` with available information retrieved from `PlexSessionManager` , and fires `BackendPlexAuthCoordinator.RequestTokenFlow`.

## <mark style="color:green;background-color:green;">MANAGER</mark>

> “Stores and provides access to global, persistent session or configuration data.”

### HOW IT SENDS/RECEIVES DATA

⬅️ **Pullable by** `Frontend` \
⚠️**Pullable by `Backend`**, but _**only when necessary**_. All data should be _pulled from `frontend`_ first and then _passed to `backend`_

### RESPONSIBILITIES

* Central access point for shared data needed across both frontend and backend layers
* Can store authentication tokens, device IDs, user profiles, app versions, or cached API responses
* Acts as a temporary in-memory session or global state cache
* Should not contain any logic about network communication, validation, or business flow

### IN PRACTICE (PLEX EXAMPLE)

* `PlexSessionManager` stores the `current Token`, `Username`, and `DeviceID`

## <mark style="color:green;background-color:green;">BACKEND COORDINATOR</mark>

> “Receives enriched payloads, splits or routes them to appropriate service controllers.”

### **How It Sends/Receives Data**:

⬅️ **Receives enriched payloads via event** from `Frontend Coordinators`\
▶️ **Pushes payloads** to `Service Controllers` via events

### RESPONSIBILITIES

* Receives fully prepared payloads from frontend
* Orchestrates business-level flow across backend systems
* Determines which service controllers need to be activated and can split the payload to multiple destinations if necessary
* May sequence requests or batch/parallelize them across service layers

### IN PRACTICE (PLEX EXAMPLE)

* `PlexAuthCoordinatorBackend` gets pushed the `DeviceCodePayload` via `RequestTokenFlow()` and dispatches it to `PlexAuthServiceController.StartTokenPolling`

## <mark style="color:green;background-color:green;">BACKEND SERVICE CONTROLLER</mark>

> “Processes business logic and orchestrates API calls through backend handlers.”

### HOW IT SENDS/RECEIVES DATA:

⬅️ **Receives payload via event** from `Backend Coordinator`\
⬅️ **Pulls data** via `await` from `API Handlers`

### RESPONSIBILITIES

* Owns flow-specific logic such as polling, retries, conditional branching
* Pulls static API methods from the `Handler` layer to execute actual HTTP logic
* Aggregates API responses and reacts (e.g., store result, retry if error)

### IN PRACTICE (PLEX EXAMPLE)

* `PlexAuthServiceController` starts an `aync Task<T>` that polls Plex every 5 seconds using `PlexAPIHandler.TryGetAuthToken`, then returns a `PlexTokenResultDTO`.

## <mark style="color:green;background-color:green;">BACKEND API HANDLER</mark>

> “Executes raw HTTP/API requests and parses external data responses.”

### HOW IT SENDS/RECEIVES DATA

⬅️ **Receives primitive parameters or raw payloads (e.g., code, deviceId)** via method call\
▶️ **Returns either raw strings or simple transport models (not business DTOs)** via `async Task<T>`

### RESPONSIBILITIES

* Fully static class - no instantiation, no MonoBehaviour
* Lowest layer of abstraction - focuses purely on external communication
* Performs raw I/O: builds HTTP requests, sets headers, parses or returns raw response content.
* Deserializes data (e.g., JSON → DTOs) and wraps them in a result object
* Contains **no business logic -** it just sends and receives

### IN PRACTICE (PLEX EXAMPLE)

* `PlexAPIHandler.GetAuthTokenJsonAsync(string code, string deviceId)`:
  * **Sends** a GET request to `https://plex.tv/api/oauth/...`
  * **Returns** a raw JSON string.

