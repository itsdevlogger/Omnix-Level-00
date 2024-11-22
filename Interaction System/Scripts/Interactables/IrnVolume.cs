using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace InteractionSystem.Interactables
{
    [RequireComponent(typeof(Collider))]
    [ComponentInfo("Starts the interaction as soon as player enters the\nvolume defined by trigger on this object")]
    public class IrnVolume : MonoBehaviour
    {
        public enum ActionToTake
        {
            Focus,
            Interact
        }

        [SerializeField] private ActionToTake _action;
        [SerializeField] private bool _forceInteraction;
        [SerializeField] private bool _endWhenPlayerLeaves = true;

        private InteractionPlayer _player;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out _player))
                return;

            switch (_action)
            {
                case ActionToTake.Focus:
                    _player.SetFocus(gameObject, _forceInteraction);
                    break;
                case ActionToTake.Interact:
                    _player.StartInteraction(gameObject, _forceInteraction);
                    break;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_endWhenPlayerLeaves || _player == null)
                return;

            if (_player.IsInteracting == false || _player.CurrentTarget.gameObject != gameObject)
                return;

            if (other.TryGetComponent(out InteractionPlayer otherPlayer) == false)
                return;
            
            if (otherPlayer != _player) 
                return;
            
            switch (_action)
            {
                case ActionToTake.Focus:
                    _player.ClearFocus();
                    break;
                case ActionToTake.Interact:
                    _player.EndInteraction();
                    break;
            }
        }


#if UNITY_EDITOR
        private void Reset()
        {
            if (!gameObject.TryGetComponent(out IInteractionProcessor processor))
            {
                EditorUtility.DisplayDialog("Error", $"Can't add \"{nameof(IrnVolume)}\" to game object \"{gameObject.name}\" as there are no interactable components.", "Okay");
                DestroyImmediate(this);
            }
        }
#endif
    }
}