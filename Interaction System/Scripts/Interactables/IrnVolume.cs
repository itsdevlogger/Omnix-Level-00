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

        [SerializeField] private GameObject _target;
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
                    _player.SetFocus(_target, _forceInteraction);
                    break;
                case ActionToTake.Interact:
                    _player.StartInteraction(_target, _forceInteraction);
                    break;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_endWhenPlayerLeaves || _player == null)
                return;

            if (_player.IsInteracting == false || _player.CurrentTarget.gameObject != _target)
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
            _target = gameObject;
            //if (!_target.TryGetComponent(out IInteractionProcessor processor))
            //{
            //    EditorUtility.DisplayDialog("Error", $"Can't add \"{nameof(IrnVolume)}\" to game object \"{_target.name}\" as there are no interactable components.", "Okay");
            //    DestroyImmediate(this);
            //}
        }
#endif
    }
}