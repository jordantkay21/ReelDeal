---
icon: bridge
---

# BRIDGE LAYER

## PURPOSE

The **BRIDGE LAYER** is the logic router and data mediator. It determines:

* Whether a request can be resolved with cached data (via `Manager`)
* If backend execution is needed (via BackendCoordinator)
* How to return finalized results back to the frontend in structured format

> This layer isolates business logic decisions and centralizes control without exposing raw backend or UI code



## COMPONENTS

|       Component       |                                        Role                                       |
| :-------------------: | :-------------------------------------------------------------------------------: |
| `FrontendCoordinator` |          Evaluates frontend requests and decides between cache or backend         |
|       `Manager`       |          Stores and retrieves locally cached state, session, or user data         |
|  `BackendCoordinator` | Forwards service logic requests to the backend layer and handles response routing |

## DATA FLOW WITHIN THE LAYER

{% tabs %}
{% tab title="INPUT LOGIC FLOW" %}
1. **FrontendController Sends ActionPayload**
   1. Routed to `FrontendCoordinator`
2. **FrontendCoordinator Evaluates the Payload**
   1. Uses payload type to determine logic path
   2. Checks `Manager` if data might be cached (e.g. device token, server list)
3. **If Cache Hit:**
   1. Builds and returns a `FrontendResponsePayload` immediately
4. **If Cache Miss or New Request:**
   1. Builds a `BackendPayload` and sends to `BackendCoordinator`
5. **BackendCoordinator Forwards Payload to Backend Layer**
   1. Manages transfer and response collection from backend services
{% endtab %}

{% tab title="OUTPUT LOGIC FLOW" %}
1. **BackendCoordinator Receives BackendResponsePayload from&#x20;**_**Backend Layer**_
2. **Writes to Manager (if applicable)**
   1. _EXAMPLE:_ store plex token, username, auth status, etc.
3. **Wraps Data into a FrontendResponsePayload**
4. **Returns It to Frontend Layer**
{% endtab %}
{% endtabs %}

## KEY PAYLOAD

{% tabs %}
{% tab title="BackendPayload" %}
**CONSTRUCTED BY:** `FrontendCoordinator`\
**CONTAINS:** The backend operation request details (e.g. request device code, poll login status)\
**SENT TO:** `BackendCoordinator` for routing to the backend service logic
{% endtab %}

{% tab title="FrontendResponsePayload" %}
**CONSTRUCTED BY:** `FrontendCoordinator`\
**CONTAINS:** Fully structured, UI-ready data including response messages, status indicators, or fetched content\
**SENT TO:** `FrontendController` for display formatting and forwarding to `FrontendHandler`
{% endtab %}
{% endtabs %}

## IN PRACTICE

### INPUT LOGIC - Plex OAuth Login Flow <sub>(</sub><sub>_Steps 4 - 6)_</sub>

{% tabs fullWidth="false" %}
{% tab title="STEP FOUR" %}
{% hint style="warning" %}
⬇️ **STEPS 1 - 3 \[FRONTEND LOGIC]** ⬇️
{% endhint %}

<table data-view="cards"><thead><tr><th></th><th></th></tr></thead><tbody><tr><td><strong>TRIGGER</strong></td><td><code>ActionPayload(type: StartDeviceAuthFlow)</code> is received</td></tr><tr><td><strong>PROCESS</strong></td><td><code>FrontendCoordinator</code> checks <code>Manager</code> for an existing valid device code</td></tr><tr><td><strong>RESULT</strong></td><td>No valid device code found</td></tr></tbody></table>

> **WHAT'S HAPPENING?**
>
> _The system checks if it already has a login code saved, but it doesn’t._
{% endtab %}

{% tab title="STEP FIVE" %}
<table data-view="cards"><thead><tr><th></th><th></th></tr></thead><tbody><tr><td><strong>TRIGGER</strong></td><td>No cached data available</td></tr><tr><td><strong>PROCESS</strong></td><td><code>FrontendCoordinator</code> constructs a <code>BackendPayload</code> requesting a new device code</td></tr><tr><td><strong>RESULT</strong></td><td><code>BackendPayload</code> sent to <code>BackendCoordinator</code></td></tr></tbody></table>

> **WHAT'S HAPPENING?**
>
> _Since no cached data exists, it asks the backend to get a new login code from Plex._
{% endtab %}

{% tab title="STEP SIX" %}
<table data-view="cards"><thead><tr><th></th><th></th></tr></thead><tbody><tr><td><strong>TRIGGER</strong></td><td><code>BackendPayload</code> is forwarded</td></tr><tr><td><strong>PROCESS</strong></td><td><code>BackendCoordinator</code> dispatches the request to the backend (PlexAuthServiceController → PlexAPIHandler)</td></tr><tr><td><strong>RESULT</strong></td><td>Backend begins contacting Plex to get a device login code</td></tr></tbody></table>

> **WHAT'S HAPPENING?**
>
> _The system sends a request behind the scenes to Plex asking for a new device login code._

{% hint style="warning" %}
&#x20;⬇️ **STEPS 7 - 9 \[BACKEND LOGIC] ⬇️**
{% endhint %}
{% endtab %}
{% endtabs %}

### OUTPUT LOGIC - Plex OAuth Login Flow <sub>(</sub><sub>_Steps 13 - 15)_</sub>

{% tabs fullWidth="false" %}
{% tab title="STEP THIRTEEN" %}
{% hint style="warning" %}
⬇️ **STEPS 10 - 12 \[BACKEND LOGIC]** ⬇️
{% endhint %}

<table data-view="cards"><thead><tr><th></th><th></th></tr></thead><tbody><tr><td><strong>TRIGGER</strong></td><td>Backend finishes fetching login code</td></tr><tr><td><strong>PROCESS</strong></td><td><code>BackendCoordinator</code> receives <code>BackendResponsePayload</code> and passes it back to <code>FrontendCoordinator</code></td></tr><tr><td><strong>RESULT</strong></td><td>Device code is returned along with a user code and expiry time</td></tr></tbody></table>

> **WHAT'S HAPPENING?**
>
> _Plex sends back a special login code and user code that the player can use to connect their account._
{% endtab %}

{% tab title="STEP FOURTEEN" %}
<table data-view="cards"><thead><tr><th></th><th></th></tr></thead><tbody><tr><td><strong>TRIGGER</strong></td><td>Response is handled and cached</td></tr><tr><td><strong>PROCESS</strong></td><td><code>FrontendCoordinator</code> stores the data in <code>Manager</code> and wraps it into a <code>FrontendResponsePayload</code></td></tr><tr><td><strong>RESULT</strong></td><td><code>FrontendResponsePayload</code> returned to <code>FrontendController</code></td></tr></tbody></table>

> **WHAT'S HAPPENING?**
>
> _The system saves this info for reuse and prepares a message with the login info for the UI._
{% endtab %}

{% tab title="STEP FIFTEEN" %}
<table data-view="cards"><thead><tr><th></th><th></th></tr></thead><tbody><tr><td><strong>TRIGGER</strong></td><td><code>BackendPayload</code> is forwarded</td></tr><tr><td><strong>PROCESS</strong></td><td><code>BackendCoordinator</code> dispatches the request to the backend (PlexAuthServiceController → PlexAPIHandler)</td></tr><tr><td><strong>RESULT</strong></td><td>Backend begins contacting Plex to get a device login code</td></tr></tbody></table>

> **WHAT'S HAPPENING?**
>
> _The system sends a request behind the scenes to Plex asking for a new device login code._

{% hint style="warning" %}
&#x20;⬇️ **STEPS 16 - 17 \[FRONTEND LOGIC] ⬇️**
{% endhint %}
{% endtab %}
{% endtabs %}

## COMMUNICATION SUMMARY

{% tabs %}
{% tab title="INCOMING COMMUNICATION" %}
<table><thead><tr><th width="181" align="center">SOURCE</th><th width="181" align="center">TARGET</th><th width="152" align="center">DATA TYPE</th><th align="center">PURPOSE</th></tr></thead><tbody><tr><td align="center">FrontendController</td><td align="center">FrontendCoordinator</td><td align="center">ActionPayload</td><td align="center">User-triggered request payload</td></tr><tr><td align="center">FrontendCoordinator</td><td align="center">Manager</td><td align="center">Raw key/value query</td><td align="center">Check for cached data</td></tr><tr><td align="center">FrontendCoordinator</td><td align="center">BackendCoordinator</td><td align="center">BackendPayload</td><td align="center">Request backend operation if no local match</td></tr></tbody></table>
{% endtab %}

{% tab title="OUTPUT COMMUNICATION" %}
<table><thead><tr><th width="178" align="center">SOURCE</th><th width="180" align="center">TARGET</th><th width="162" align="center">DATA TYPE</th><th align="center">PURPOSE</th></tr></thead><tbody><tr><td align="center">BackendCoordinator</td><td align="center">ServiceController</td><td align="center">ServiceDTO</td><td align="center">Pass payload to backend layer</td></tr><tr><td align="center">BackendCoordinator</td><td align="center">FrontendCoordinator</td><td align="center">BackendResponsePayload → <code>FrontendResponsePayload</code> </td><td align="center">Final data packaging for frontend</td></tr></tbody></table>
{% endtab %}
{% endtabs %}

## WHY IT EXISTS

The Bridge Layer:

* Prevents the frontend from having to manage data decisions or backend logic
* Enables smart caching and re-use of existing session state
* Isolates routing logic to make the system maintainable, testable, and extensible

