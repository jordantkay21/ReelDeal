---
icon: hand
---

# FrontendHandler

## CORE RESPONSIBILITIES

* Acts as the **UI’s input/output gate**: It captures interaction events like button clicks, dropdown selections, or toggles directly from Unity’s Event System.
* **Forwards interaction context** to the `FrontendController` without modifying or interpreting the data.
* **Receives UI-ready results** from the controller and applies them visually using Unity UI components (e.g., labels, icons, progress bars, visual state indicators).

## BEHAVIOR OVERVIEW

* Exists on UI GameObjects (buttons, panels, overlays) and is often connected through Unity Inspector bindings.
* Passively listens for interaction callbacks and reacts by notifying the controller.
* Can be reused across multiple UI elements with minimal logic duplication, thanks to its focused scope.

## SYSTEM INTEGRATION

* **Input:** Direct Unity events (user interactions)
* **Output to Controller:** Structured notification that something happened (e.g., "connect button clicked")
* **Input from Controller:** A display instruction containing all visual elements to update
* **Output to Unity UI:** Assigns visuals (e.g., sets text, toggles visibility, applies status colors)

## WHY IT EXISTS

By restricting the `FrontendHandler` to rendering and event listening:

* UI can be designed and tested independently of the system’s logic.
* UI logic becomes modular, swappable, and testable without touching core logic.
* Developers and designers can iterate on layout and interactivity without understanding backend behavior.
