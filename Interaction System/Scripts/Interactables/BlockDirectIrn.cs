using UnityEngine;

namespace InteractionSystem.Interactables
{
    [ComponentInfo("Stops player from directy interacting with this object.\nForcing them to use some IrnButton that links to this object.")]
    public class BlockDirectIrn : MonoBehaviour, IInteractionCriteria, IHiddenForTrigger
    {
        public bool isBlocked = true;
        public string errorMessage;

        bool IInteractionCriteria.CanInteract(InteractionPlayer player)
        {
            if (player.CurrentTarget.gameObject != gameObject) 
                return true;

            if (isBlocked)
            {
                InteractionUiHandler.DisplayError(errorMessage);
                return false;
            }
            return true;
        }
    }
}