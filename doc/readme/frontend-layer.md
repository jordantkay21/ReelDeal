# FRONTEND LAYER

## PURPOSE

The **Frontend Layer** is the first and final point of interaction for the user. It is responsible for:

* Capturing all input events (clicks, selections, keyboard input, etc.)
* Structuring raw input into logic-compatible formats (`ActionPayload`)
* Receiving UI-ready data and updating the interface appropriately
* Reflecting system state, success/failure indicators, and feedback

## SUB-LAYERS

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
4. _**FrontendController**_**&#x20;Structures into ActionPayload**
   1. Adds metadata, enum identifiers, or wrappers as needed
   2. Pushes `ActionPayload` to `FrontendCoordinator` (Bridge Layer)
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
**CONSTRUCTED BY:** FrontendController\
**CONTAINS:** user intent, raw input, request type\
**SENT TO:** FrontendCoordinator
{% endtab %}

{% tab title="FrontendResponsePayload" %}
**RECEIVED BY:** FrontendController\
**CONTAINS:** final display-ready content (messages, lists, icons, etc.)\
**SENT TO:** FrontendHandler for UI rendering
{% endtab %}
{% endtabs %}

## IN PRACTICE

{% tabs %}
{% tab title="INPUT LOGIC" %}
1. Player clicks the `CONNECT TO PLEX` button
2. **FrontendHandler** _captures the click and calls_ `FrontendController.OnConnectRequest()`
3. **FrontendController** _constructs_ an `ActionPayload` with type `StartDeviceAuthFlow`
4. The `ActionPayload` is _dispatched_ to the **FrontendCoordinator** in the Bridge Layer
{% endtab %}

{% tab title="OUTPUT LOGIC" %}
> After the full system flow (device code generation, polling, token retrieval)....

1. A `FrontendResponsePayload` containing the device login state and user metadata is _returned_
2. **FrontendController** _formats this payload for UI display_: updates button label, shows user avatar, and changes border color to indicate a successful login
3. **FrontendHandler** _applies these UI changes_ using styles and fonts from the design guide to reflect the connected status
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
