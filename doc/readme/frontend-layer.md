---
icon: palette
---

# FRONTEND LAYER

## PURPOSE

The **Frontend Layer** is the first and final point of interaction for the user. It is responsible for:

* Capturing all input events (clicks, selections, keyboard input, etc.)
* Structuring raw input into logic-compatible formats (`ActionPayload`)
* Receiving UI-ready data and updating the interface appropriately
* Reflecting system state, success/failure indicators, and feedback

## COMPONENTS

|       Component      |                                              Role                                             |
| :------------------: | :-------------------------------------------------------------------------------------------: |
|   `FrontendHandler`  |                       Captures low-level UI events and displays updates                       |
| `FrontendController` | Translates user input into logic payloads; receives results and preps them for visual display |
|                      |                                                                                               |

## DATA FLOW WITHIN THE LAYER

{% tabs %}
{% tab title="INPUT LOGIC FLOW" %}
1. **Raw UI Event Triggered**
   1. _EXAMPLE_**:** Button Clicked, Dropdown selection, text input
2. _**FrontendHandler**_**&#x20;Receives Event**
   1. Implements Unity event interfaces like `IPointerClickHandler`, `IDropdownHandler`, or invokes callbacks on other UI changes
3. _**FrontendHandler**_**&#x20;Sends Raw Data →&#x20;**_**FrontendController**_
   1. Raw data is passed via public method call or Unity event binding
   2. _EXAMPLE_**:** `OnConnectionClick()` or `OnPlaylistDropdownChanged(string playlistName)`
4.  _**FrontendController**_**&#x20;Structures into ActionPayload**

    1. Adds metadata, enum identifiers, or wrappers as needed

    > Pushes `ActionPayload` to `FrontendCoordinator` (Bridge Layer)
{% endtab %}

{% tab title="OUTPUT LOGIC FLOW" %}
1. **FrontendResponsePayload Returned**
   1. From Bridge Layer, after Manager or Backend processes request
2. **FrontendController Interprets Payload**
   1. Converts structured data into UI-friendly format (strings, sprites, status messages)
3. **FrontendController Pushes Display Data → FrontendHandler**
   1. Updates UI visuals, colors, and triggers animations or notifications
4. **FrontendHandler Renders Data**
   1. Updates button text, icon, feedback state (e.g. "Connected!", "Error: Playlist Not Found")
   2. Applies colors from style guide
{% endtab %}
{% endtabs %}

## KEY PAYLOAD

{% tabs %}
{% tab title="ActionPayload" %}
**CONSTRUCTED BY:** `FrontendController`\
**CONTAINS:** Encoded user intent and contextual data (e.g. action type, parameters, UI metadata)\
**SENT TO:** `FrontendCoordinator` in the Bridge Layer for evaluation and routing
{% endtab %}

{% tab title="FrontendResponsePayload" %}
**CONSTRUCTED BY:** `FrontendCoordinator`\
**CONTAINS:** Final response data structured for display (e.g. messages, visuals, UI states)\
**SENT TO:** `FrontendController`, which formats the content for display and passes it to `FrontendHandler`
{% endtab %}
{% endtabs %}

## IN PRACTICE

### INPUT - Plex OAuth Login Flow <sub>(</sub><sub>_Steps 1 - 3)_</sub>

{% tabs fullWidth="false" %}
{% tab title="STEP ONE" %}
{% hint style="warning" %}
⬇️ **START \[USER INPUT]** ⬇️
{% endhint %}

<table data-view="cards"><thead><tr><th></th><th></th></tr></thead><tbody><tr><td><strong>TRIGGER</strong></td><td>Player clicks the <code>CONNECT TO PLEX</code> button</td></tr><tr><td><strong>PROCESS</strong></td><td>FrontendHandler captures the click and notifies FrontendController</td></tr><tr><td><strong>RESULT</strong></td><td>FrontendController receives raw input</td></tr></tbody></table>

> WHAT'S HAPPENING?
>
> _The player initiates a login by clicking a button. The handler catches this and passes it along to the controller._
{% endtab %}

{% tab title="STEP TWO" %}
<table data-view="cards"><thead><tr><th></th><th></th></tr></thead><tbody><tr><td><strong>TRIGGER</strong></td><td>Raw input is received by controller</td></tr><tr><td><strong>PROCESS</strong></td><td><code>FrontendController</code> constructs an <code>ActionPayload(type: StartDeviceAuthFlow)</code></td></tr><tr><td><strong>RESULT</strong></td><td>ActionPayload is created and ready for processing</td></tr></tbody></table>

> WHAT'S HAPPENING?
>
> _The controller converts the click into a structured instruction for the rest of the system to act on._
{% endtab %}

{% tab title="STEP THREE" %}
<table data-view="cards"><thead><tr><th></th><th></th></tr></thead><tbody><tr><td><strong>TRIGGER</strong></td><td>Payload is sent to the Bridge Layer</td></tr><tr><td><strong>PROCESS</strong></td><td><code>ActionPayload</code> is dispatched to the <code>FrontendCoordinator</code></td></tr><tr><td><strong>RESULT</strong></td><td>System begins processing request</td></tr></tbody></table>

> WHAT'S HAPPENING?
>
> _The structured login request is forwarded to the next layer to check cache or contact Plex._

{% hint style="warning" %}
⬇️ **STEPS 4 - 6 \[FRONTEND LOGIC] ⬇️**
{% endhint %}
{% endtab %}
{% endtabs %}

### OUTPUT - Plex OAuth Login Flow

{% tabs fullWidth="false" %}
{% tab title="STEP SIXTEEN" %}
{% hint style="warning" %}
⬇️ **STEPS 13 - 15 \[BRIDGE LOGIC]** ⬇️
{% endhint %}

<table data-view="cards"><thead><tr><th></th><th></th></tr></thead><tbody><tr><td><strong>TRIGGER</strong></td><td><code>FrontendResponsePayload</code> is returned to controller</td></tr><tr><td><strong>PROCESS</strong></td><td>Controller formats it (e.g., updates text, avatar, status color)</td></tr><tr><td><strong>RESULT</strong></td><td>Formatted UI-ready content is sent to <code>FrontendHandler</code></td></tr></tbody></table>

> WHAT'S HAPPENING?
>
> Once Plex responds and the Bridge Layer processes the result, the controller updates everything the player will see.
{% endtab %}

{% tab title="STEP SEVENTEEN" %}
<table data-view="cards"><thead><tr><th></th><th></th></tr></thead><tbody><tr><td><strong>TRIGGER</strong></td><td>UI is updated</td></tr><tr><td><strong>PROCESS</strong></td><td><code>FrontendHandler</code> applies the visual updates</td></tr><tr><td><strong>RESULT</strong></td><td>Button shows connected status, maybe with username and green indicator</td></tr></tbody></table>

> WHAT'S HAPPENING?
>
> The player now sees that they’re successfully connected to Plex with their username and a visual confirmation.

{% hint style="warning" %}
**⬇️ END \[USER DISPLAY] ⬇️**
{% endhint %}
{% endtab %}
{% endtabs %}

## COMMUNICATION SUMMARY

{% tabs %}
{% tab title="INCOMING COMMUNICATION" %}
<table><thead><tr><th width="167" align="center">SOURCE</th><th width="184" align="center">TARGET</th><th width="133" align="center">DATA TYPE</th><th align="center">PURPOSE</th></tr></thead><tbody><tr><td align="center">FrontendHandler</td><td align="center">FrontendController</td><td align="center">Raw Input</td><td align="center">Capture user intent</td></tr><tr><td align="center">FrontendController</td><td align="center">FrontendCoordinator</td><td align="center">ActionPayload</td><td align="center">Structured request to be processed</td></tr></tbody></table>
{% endtab %}

{% tab title="OUTBOUND COMMUNICATION" %}
<table><thead><tr><th width="167" align="center">SOURCE</th><th width="180" align="center">TARGET</th><th width="165" align="center">DATA TYPE</th><th align="center">PURPOSE</th></tr></thead><tbody><tr><td align="center">FrontendController</td><td align="center">FrontendCoordinator</td><td align="center">FrontendResponsePayload</td><td align="center">Final response for rendering </td></tr><tr><td align="center">FrontendController</td><td align="center">FrontendHandler</td><td align="center">Formatted UI content</td><td align="center">Rendered data, visuals, or feedback</td></tr></tbody></table>
{% endtab %}
{% endtabs %}

## WHY IT EXISTS

The **Frontend Layer** abstracts UI complexity from the logic system. It ensures that:

* Input and output are clearly separated
* No backend logic leaks into UI code
* The user receives fast and consistent visual feedback
* Payloads remain traceable and testable throughout the system
