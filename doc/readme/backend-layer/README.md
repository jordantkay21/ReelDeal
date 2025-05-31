---
icon: server
---

# BACKEND LAYER

## PURPOSE

The Backend Layer performs all final execution logic, such as:

* Handling external API requests (e.g., Plex, web services)
* Parsing files or XML/JSON responses
* Converting raw results into structured data
* Returning outcomes as `BackendResponsePayload`s for the bridge to process



## COMPONENTS

|      Component      |                                            Role                                           |
| :-----------------: | :---------------------------------------------------------------------------------------: |
| `ServiceController` | Converts BackendPayload to ServiceDTO; wraps handler output into `BackendResponsePayload` |
|   `ServiceHandler`  |         Executes actual operations such as HTTP calls, disk reads, or XML parsing         |

## DATA FLOW WITHIN THE LAYER

{% tabs %}
{% tab title="INPUT LOGIC FLOW" %}
1. **`BackendCoordinator` sends a `BackendPayload`**
   1. Routed to a specific ServiceController (e.g., PlexAuthServiceController)
2. **`ServiceController` Constructs a `ServiceDTO`**
   1. This DTO defines exactly what task the backend should perform
3. **DTO is passed to the `ServiceHandler`**
{% endtab %}

{% tab title="OUTPUT LOGIC FLOW" %}
1. **`ServiceHandler` Completes the Task**
   1. Returns the result (success or failure) in a DTOResponse
2. **`ServiceController` Interprets the Result**
   1. Wraps it into a standardized BackendResponsePayload
3. **Payload is sent back to `BackendCoordinator`**
   1. Returned to the Bridge Layer for caching and UI display
{% endtab %}
{% endtabs %}



## KEY PAYLOAD

{% tabs %}
{% tab title="BackendResponsePayload" %}
**CONSTRUCTED BY:** `ServiceController`\
**CONTAINS:** The final result of a backend operation, translated into structured form (success/failure, data payloads)\
**SENT TO:** `BackendCoordinator`, which forwards it up the bridge chain
{% endtab %}
{% endtabs %}

## KEY DTOs

{% tabs %}
{% tab title="ServiceDTO" %}
**CONSTRUCTED BY:** `ServiceController`\
**CONTAINS:** A specific set of instructions and metadata defining what backend action to perform (e.g., endpoint, parameters)\
**SENT TO:** `ServiceHandler` for execution
{% endtab %}

{% tab title="DTOResponse" %}
**CONSTRUCTED BY:** `ServiceHandler`\
**CONTAINS:** Raw result from the backend action, such as JSON/XML API responses or file contents\
**SENT TO:** `ServiceController`  for interpretation and packaging
{% endtab %}
{% endtabs %}

## IN PRACTICE

### INPUT LOGIC - Plex OAuth Login Flow <sub>(</sub><sub>_Steps 7 - 9)_</sub>

{% tabs fullWidth="false" %}
{% tab title="STEP SEVEN" %}
{% hint style="warning" %}
⬇️ **STEPS 7 - 12 \[BRIDGE LOGIC]** ⬇️
{% endhint %}

<table data-view="cards"><thead><tr><th></th><th></th></tr></thead><tbody><tr><td><strong>TRIGGER</strong></td><td><code>BackendPayload(type: RequestDeviceCode)</code> received</td></tr><tr><td><strong>PROCESS</strong></td><td><code>BackendCoordinator</code> routes it to <code>PlexAuthServiceController</code></td></tr><tr><td><strong>RESULT</strong></td><td>Payload handed off to controller</td></tr></tbody></table>

> **WHAT'S HAPPENING?**
>
> The system receives instructions to contact Plex and get a new device login code
{% endtab %}

{% tab title="STEP EIGHT" %}
<table data-view="cards"><thead><tr><th></th><th></th></tr></thead><tbody><tr><td><strong>TRIGGER</strong></td><td>Controller builds <code>ServiceDTO</code></td></tr><tr><td><strong>PROCESS</strong></td><td>Constructs request data: endpoint path, client ID, expected response format</td></tr><tr><td><strong>RESULT</strong></td><td>DTO ready for execution</td></tr></tbody></table>

> **WHAT'S HAPPENING?**
>
> A structured request object is created that tells the handler exactly what kind of request to make and how
{% endtab %}

{% tab title="STEP NINE" %}
<table data-view="cards"><thead><tr><th></th><th></th></tr></thead><tbody><tr><td><strong>TRIGGER</strong></td><td>ServiceDTO passed to PlexAPIHandler</td></tr><tr><td><strong>PROCESS</strong></td><td>Handler performs HTTP request to https://plex.tv/api/v2/pins</td></tr><tr><td><strong>RESULT</strong></td><td>Plex responds with device code, user code, and expiration</td></tr></tbody></table>

> **WHAT'S HAPPENING?**
>
> A network call is made to Plex's servers, and they return the needed codes for device login

{% hint style="warning" %}
&#x20;⬇️ **STEPS 10 - 12 \[BACKEND LOGIC] ⬇️**
{% endhint %}
{% endtab %}
{% endtabs %}

### OUTPUT LOGIC - Plex OAuth Login Flow <sub>(</sub><sub>_Steps 10 - 12)_</sub>

{% tabs fullWidth="false" %}
{% tab title="STEP TEN" %}
{% hint style="warning" %}
⬇️ **STEPS 7 - 9 \[BACKEND LOGIC]** ⬇️
{% endhint %}

<table data-view="cards"><thead><tr><th></th><th></th></tr></thead><tbody><tr><td><strong>TRIGGER</strong></td><td>Handler returns DTOResponse</td></tr><tr><td><strong>PROCESS</strong></td><td>DTO contains raw response data (status code, payload JSON/XML)</td></tr><tr><td><strong>RESULT</strong></td><td>DTO passed back to controller</td></tr><tr><td></td><td></td></tr></tbody></table>

> **WHAT'S HAPPENING?**
>
> _The raw data Plex gave us is returned to the controller for parsing and formatting_
{% endtab %}

{% tab title="STEP ELEVEN" %}
<table data-view="cards"><thead><tr><th></th><th></th></tr></thead><tbody><tr><td><strong>TRIGGER</strong></td><td>Controller wraps it into <code>BackendResponsePayload</code></td></tr><tr><td><strong>PROCESS</strong></td><td>Extracts device code, user code, and expiration; applies status = success</td></tr><tr><td><strong>RESULT</strong></td><td>Structured response ready to return</td></tr></tbody></table>

> **WHAT'S HAPPENING?**
>
> _The raw API data is cleaned up, validated, and packaged so it can be used by the BRIDGE LAYER_
{% endtab %}

{% tab title="STEP TWELVE" %}
<table data-view="cards"><thead><tr><th></th><th></th></tr></thead><tbody><tr><td><strong>TRIGGER</strong></td><td>BackendResponsePayload returned to BackendCoordinator</td></tr><tr><td><strong>PROCESS</strong></td><td>Sent to Bridge Layer for caching and final routing to the frontend</td></tr><tr><td><strong>RESULT</strong></td><td>Frontend eventually displays device login instructions</td></tr></tbody></table>

> **WHAT'S HAPPENING?**
>
> _The response moves back up the chain - eventually reaching the UI where the player sees the login code and instruction message_

{% hint style="warning" %}
&#x20;⬇️ **STEPS 13 - 15 \[BRIDGE LOGIC] ⬇️**
{% endhint %}
{% endtab %}
{% endtabs %}

## COMMUNICATION SUMMARY

{% tabs %}
{% tab title="INCOMING COMMUNICATION" %}
<table><thead><tr><th width="181" align="center">SOURCE</th><th width="181" align="center">TARGET</th><th width="152" align="center">DATA TYPE</th><th align="center">PURPOSE</th></tr></thead><tbody><tr><td align="center">BackendCoordinator</td><td align="center">ServiceController</td><td align="center">BackendPayload</td><td align="center">Request for backend execution</td></tr><tr><td align="center">ServiceController</td><td align="center">ServiceHandler</td><td align="center">ServiceDTO</td><td align="center">Structured instruction for what logic to perform</td></tr></tbody></table>
{% endtab %}

{% tab title="OUTPUT COMMUNICATION" %}
<table><thead><tr><th width="178" align="center">SOURCE</th><th width="180" align="center">TARGET</th><th width="162" align="center">DATA TYPE</th><th align="center">PURPOSE</th></tr></thead><tbody><tr><td align="center">ServiceHandler</td><td align="center">ServiceController</td><td align="center">DTOResponse</td><td align="center">Raw result of backend operation</td></tr><tr><td align="center">ServiceController</td><td align="center">BackendCoordinator</td><td align="center">BackendResponsePayload</td><td align="center">Final formatted response ready for bridge return</td></tr></tbody></table>
{% endtab %}
{% endtabs %}

## WHY IT EXISTS

The Backend Layer is where real-world execution happens. It's important because:

* It **decouples execution logic** from both UI and flow-control, keeping the architecture clean and modular.
* It ensures that **external requests (like APIs)** are handled in a centralized, testable location.
* It allows every backend interaction to be **standardized through DTOs**, ensuring clear contracts between intent (payload) and execution (handler).
* It enables full reuse and mocking of services—so alternate backends or offline stubs can easily plug in.
* It isolates side effects, keeping your Bridge and Frontend layers deterministic and state-safe.

&#x20;

