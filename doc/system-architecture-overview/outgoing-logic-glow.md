# OUTGOING LOGIC GLOW

{% stepper %}
{% step %}
### [BACKEND API HANDLER](outgoing-logic-glow.md#backend-api-handler-1)

Performs raw HTTP/API requests and returns response data for further processing.
{% endstep %}

{% step %}
### [BACKEND SERVICE CONTROLLER](outgoing-logic-glow.md#backend-service-controller-1)

Processes external response data and packages it into structured DTOs.
{% endstep %}

{% step %}
### [BACKEND COORDINATOR](outgoing-logic-glow.md#backend-coordinator-1)

Receives DTOs from Service Controllers and determines outbound routing
{% endstep %}

{% step %}
### [MANAGER](outgoing-logic-glow.md#managers)

Stores outbound DTO data when instructed by a coordinator.
{% endstep %}

{% step %}
### [FRONTEND COORDINATOR](outgoing-logic-glow.md#frontend-coordinator-1)

Receives DTOs and determines how the frontend should respond.
{% endstep %}

{% step %}
### [FRONTEND CONTROLLER](outgoing-logic-glow.md#frontend-controller-1)

Converts DTOs into UI-ready state and sends instructions to Handlers.
{% endstep %}

{% step %}
### [FRONTEND HANDLER](outgoing-logic-glow.md#frontend-handler-1)

Displays data passed in from the Controller onto the screen.
{% endstep %}
{% endstepper %}

## <mark style="color:green;background-color:green;">BACKEND API HANDLER</mark>

> "Performs raw HTTP/API requests and returns response data for further processing.”

### HOW IT SENDS/RECEIVES DATA

▶️ **Returns** either a _raw response_ (e.g., JSON string) or a _simple transport model_ (e.g., `PlexTokenApiModel`) via `async Task<T>`

### RESPONSIBILITIES

* Return external data retrieved from an HTTP call (JSON, XML, etc.)
* Provide raw or minimally parsed data to the service controller
* Does not know or care how the data will be used

### IN PRACTICE (PLEX EXAMPLE)

* `PlexAPIHandler.GetAuthTokenJsonAsync(code, deviceId)`
  * Sends a GET request to `https://plex.tv/api/oauth/...`
  * Returns a raw JSON string
* OR
* `PlexAPIHandler.GetParsedTokenResultAsync(...)`
  * Returns a `PlexTokenApiModel` with `authToken` and `username`

## <mark style="color:green;background-color:green;">BACKEND SERVICE CONTROLLER</mark>

> "Processes external response data and packages it into structured DTOs."

### HOW IT SENDS/RECEIVES DATA

⬅️ **Receives** raw data or transport model from the executed method pulled from API Handler\
▶️ **Pushes a structured result DTO** via event to the `Backend Coordinator`&#x20;

### RESPONSIBILITIES

* Calls backend API Handlers using async/await to retrieve external data
* Interprets the response (e.g., success, failure, retry timeout)
* Constructs a result DTO that encapsulates the logic outcome
* **DOES NOT** store data in Managers or fire UI/UI-facing events

### IN PRACTICE (PLEX EXAMPLE)

`PlexAuthServiceController.TryPollForTokenAsync(payload)`

* Calls `PlexAPIHandler.GetAuthTokenJsonAsync(...)`
* Parses the result
* Returns a `PlexTokenResultDTO` with `Success`, `Token`, `Username`

## <mark style="color:green;background-color:green;">BACKEND COORDINATOR</mark>

> "Receives DTOs from Service Controllers and determines outbound routing."

### HOW IT SENDS/RECEIVES DATA

⬅️ **Receives a DTO via event** from `Service Controllers` \
▶️ **Pushes DTO via event** to frontend coordinators, or dispatches to result-handling systems\
▶️ **Pushes persistent data** via _public static methods_ to `managers`

### RESPONSIBILITIES

* Acts as the bridge between backend results and the frontend/UI systems
* Receives finalized DTOs from service controllers
* Emits results to appropriate frontend coordinators for UI display or session updates
* May forward to internal systems such as logging or analytics

### IN PRACTICE (PLEX EXAMPLE)

`PlexAuthCoordinatorBackend` receives a `PlexTokenResultDTO`

* Emits `OnPlexLoginSuccess(PlexTokenResultDTO)`
* Frontend Coordinator subscribes to and reacts to that event

## <mark style="color:green;background-color:green;">MANAGERS</mark>

> "Stores outbound DTO data when instructed by a coordinator."

### HOW IT SENDS/RECEIVES DATA

🚫 Does not send data forward in the outbound flow\
⬅️**Receives data via public static** methods called from `Backend Coordinators`

### RESPONSIBILITIES

* Caches result data (e.g., tokens, username) for future access
* Does not participate in control flow or decision-making
* Only updated when Coordinators explicitly pass data for persistence

### IN PRACTICE (PLEX EXAMPLE)

`PlexSessionManager.StoreToken(result.Token, result.Username)`

* Called after backend coordinator receives successful DTO

## <mark style="color:green;background-color:green;">FRONTEND COORDINATOR</mark>

> "Receives DTOs and determines how the frontend should respond."

### HOW IT SENDS/RECEIVES DATA

⬅️ **Receives DTO via event** from `Backend Coordinators`\
▶️ **PUSHES result-based events** or method calls to `frontend controllers` for UI response

### RESPONSIBILITIES

* Listens to result DTOs from backend coordinators
* Interprets the meaning of the DTO (e.g., login success, failure, retry needed)
* Decides which frontend systems should respond (e.g., update panel, show message)
* May re-route to managers or error handling flows

### IN PRACTICE (PLEX EXAMPLE)

`PlexAuthCoordinator` receives `PlexTokenResultDTO` via `OnPlexLoginSuccess`

* If success: emits `OnLoginConfirmed(username)`
* If error: emits `OnLoginFailed(message)`

## <mark style="color:green;background-color:green;">FRONTEND CONTROLLER</mark>

> "Converts result DTOs into UI-ready state and sends instructions to Handlers"

### HOW IT SENDS/RECEIVES DATA

▶️ **PUSHES final values via public methods PULLED from UI Handlers to display to user**

### **RESPONSIBILITIES**

* Interprets DTO or event data from Frontend Coordinator
* Updates UI elements like text, buttons, loading panels, error displays
* Coordinates UI animations or transitions based on results

### IN PRACTICE (PLEX EXAMPLE)

`PlexUIController.OnLoginConfirmed("Kayos")`

* Updates welcome message
* Shows logout button
* Hides connection panel

## <mark style="color:green;background-color:green;">FRONTEND HANDLER</mark>

> "Displays result data on screen using Unity UI components."

### HOW TO SEND/RECEIVE DATA

🚫 Does not emit any outbound data\
⬅️ Receives method calls from frontend controllers

### RESPONSIBILITIES

* Updates Unity UI elements with final visual feedback (text, color, visibility)
* Executes transitions or effects (e.g., fade in, success icon)
* Displays output from controller logic without interpreting the source

### IN PRACTICE (PLEX EXAMPLE)

* `WelcomeTextHandler.SetText("Welcome, Kayos!")`
* `LogoutButtonHandler.Show()`
* `ConnectionPanelHandler.Hide()`



