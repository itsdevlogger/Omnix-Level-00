using UnityEngine;

namespace InteractionSystem.Interactables
{
    [RequireComponent(typeof(Collider))]
    [ComponentInfo("Stops player from directy interacting with this object.\nForcing them to use some IrnButton that links to this object.")]
    public class BlockDirectIrn : MonoBehaviour, IInteractionCriteria, IHiddenForTrigger
    {
        [SerializeField] private bool _isBlocked = true;
        [SerializeField] private string _errorMessage;

        bool IInteractionCriteria.CanInteract(InteractionPlayer player)
        {
            if (_isBlocked)
            {
                InteractionUiHandler.DisplayError(_errorMessage);
                return false;
            }
            return true;
        }
    }
}