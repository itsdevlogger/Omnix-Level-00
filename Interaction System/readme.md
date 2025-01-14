<style>
  .ip {
    color: #b8d7a3;
    font-style: italic;
  }

.ic {
    color: #4ec9b0;
    font-style: italic;
  }
  
  .im {
    color: #dcdcaa;
    font-style: italic;
}
</style>

## Introduction
An interactable object can be in one of four states:

| State         | Description                                                                                                                                                 |
|---------------|-------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Neutral**     | Default (fallback) state.                                                                                                                                   |
| **Pre-Focus**   | Player is looking at the object, but it is not yet ready for interaction. Handled by <span class="ip">IInteractionCriteria</span> interface.                |
| **Focused**     | The object is ready for interaction and awaiting the player's interaction input. Handled by <span class="ip">IInteractionFeedback</span> interface.         |
| **Interacting** | Actual interaction. Handled by <span class="ip">IInteractable</span> interface.                                                                             |


The system uses 3 interfaces, each managing one of the state:
1. <span class="ip">IInteractionCriteria</span> Manages **Pre-Focus** state
2. <span class="ip">IInteractionFeedback</span> Manages **Focused** state
3. <span class="ip">IInteractable</span> Manages **Interacting** state

## Execution Order
The object remains in the **Focused** state until interaction ends. This ensures the following order of execution:
1. <span class="ip">IInteractionCriteria</span>.<span class="im">CanInteract</span> Called every frame in the **Pre-Focus** state.
2. <span class="ip">IInteractionFeedback</span>.<span class="im">OnFocusGained</span> Called once when moving from **Pre-Focus** to **Focused** state.
3. <span class="ip">IInteractionFeedback</span>.<span class="im">OnBeforeInteract</span> Called once when moving from **Focused** to **Interacting** state.
4. <span class="ip">IInteractable</span>.<span class="im">OnInteractionStart</span> Called once when moving from **Focused** to **Interacting** state.
5. <span class="ip">IInteractable</span>.<span class="im">OnInteractionEnd</span> Called once when moving from **Interacting** to **Focused** state.
6. <span class="ip">IInteractionFeedback</span>.<span class="im">OnFocusLost</span> Called once when moving from **Focused** to **Neutral** state.

## Setup the System
1. Create a new physics layer called `Interactable` (This exact name is required for the <span class="ic">InteractionSetupChecker</span> utility to work)
2. Add <span class="ic">InteractionCamera</span> component to your camera
3. Add <span class="ic">InteractionPlayer</span> component to your player GameObject
4. Select `Interactable Layer` field in <span class="ic">InteractionPlayer</span> component. (Layer created in step 1)
5. Select `Start Interaction Action` and `End Interaction Action` in `InteractionPlayer` component.

## Setup Interactables 
Every interactable GameObject:
1. Must have at least 1 collider or trigger
2. Must be on the `Interactable` layer
3. Must have at least 1 component implementing <span class="ip">IInteractable</span> interface

Components implementing <span class="ip">IInteractionFeedback</span> and <span class="ip">IInteractionCriteria</span> interface are not necessary. But can be added based on your need.   

An editor utility is included to simplify this process. Right click on the GameObject and select `Setup Interaction` option. This will 
- Set the layer.
- Add <span class="ic">BoxCollider</span> if no collider found.
- Add <span class="ic">FidDisplayInfo</span> component (used to show information about the interaction), This component is almost always needed.   
You can edit this in <span class="ic">InteractionSetupWindow</span> script