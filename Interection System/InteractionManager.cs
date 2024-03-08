using System;
using System.Collections.Generic;
using UnityEngine;

namespace InteractionSystem
{
    public class InteractionManager : MonoBehaviour
    {
        #region Static Fields
        private static InteractionManager _instance;
        public static int MaxSimultaneousTargets => _instance.maxSimultaneousTargets;
        public static float MaxInteractionDistance => _instance.maxInteractionDistance;
        public static float InteractionRange => _instance.interactionRange;
        public static LayerMask InteractableLayer => _instance.interactableLayer;
        public static LayerMask BlockRaycastLayerMask => _instance.blockRaycastLayerMask;
        public static KeyCode DefaultInteractionKey => _instance.defaultInteractionKey;
        #endregion

        #region Fields
        [SerializeField] private Camera playerCamera;
        
        [Header("Settings")]
        [SerializeField] private int maxSimultaneousTargets = 3;
        [SerializeField, Tooltip("Max distance to which player can start interaction")] private float maxInteractionDistance;
        [SerializeField, Tooltip("Auto-end interaction when player to interactable distance is more than range.")] private float interactionRange;
        [SerializeField] private LayerMask interactableLayer;
        [SerializeField] private LayerMask blockRaycastLayerMask;
        [SerializeField] private KeyCode defaultInteractionKey;

        [Header("Prefabs")]
        [SerializeField] private InteractionUiElement keyPrefab;
        [SerializeField] private InteractionUiElement elementPrefab;
        
        [Header("UI Parents")]
        [SerializeField] private Transform keysParent;
        [SerializeField] private Transform elementsParent;
        
        private Action _mimCancelCallback;                   // mim: Multiple Interactions Menu
        private Action<InteractableBase> _mimConfirmCall;    // mim: Multiple Interactions Menu
        private Dictionary<InteractableBase, InteractionUiElement> _activeElements;
        private bool _showingMultipleInteractions;
        #endregion

        #region Unity Callback
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            
        }

        private void Start()
        {
            _activeElements = new Dictionary<InteractableBase, InteractionUiElement>();
            _showingMultipleInteractions = false;
        }

        private void Update()
        {
            if (!_showingMultipleInteractions) return;
            if (Input.GetMouseButtonDown(0))
            {
                HideAllUi();
                _mimCancelCallback?.Invoke();
                return;
            }

            bool hideAll = false;
            foreach (var element in _activeElements)
            {
                if (element.Value.CheckConfirmation())
                {
                    _mimConfirmCall?.Invoke(element.Key);
                    hideAll = true;
                    break;
                }
            }

            if (hideAll)
            {
                HideAllUi();
            }
        }
        #endregion

        #region Functionalities
        /// <summary> Finds interaction (with active UI button) that has given keycode as activation key </summary>
        private InteractionUiElement GetInteractionOfSameKey(KeyCode keyCode)
        {
            foreach (var pair in _activeElements)
            {
                if (pair.Key.InteractionKey == keyCode) return pair.Value;
            }
            return null;
        }
        
        /// <summary> Show UI for activation key for the give interactable </summary>
        private static void ShowInteractionKey(InteractableBase interactable)
        {
            if (_instance._activeElements.ContainsKey(interactable))
                return;

            InteractionUiElement element = _instance.GetInteractionOfSameKey(interactable.InteractionKey);
            if (element == null) element = Instantiate(_instance.keyPrefab, _instance.keysParent);
            
            element.AddInteractable(interactable);
            _instance._activeElements.Add(interactable, element);
        }

        /// <summary> Hide UI for activation key for the give interactable </summary>
        private static void HideInteractionKey(InteractableBase interactable)
        {
            if (!_instance._activeElements.ContainsKey(interactable)) return;
            _instance._activeElements[interactable].RemoveInteractable(interactable);
            _instance._activeElements.Remove(interactable);
        }

        /// <summary> Hide all active UI </summary>
        public static void HideAllUi()
        {
            foreach (InteractionUiElement element in _instance._activeElements.Values)
            {
                Destroy(element.gameObject);
            }
            _instance._activeElements.Clear();
            _instance._showingMultipleInteractions = false;
        }

        /// <summary> Show list view for all the interactableElements with a number key associated with each of them </summary>
        public static void ShowMultipleInteraction(IEnumerable<InteractableBase> interactableElements, Action<InteractableBase> confirmCallback, Action cancelCallback)
        {
            HideAllUi();
            int index = 1;

            foreach (InteractableBase interactable in interactableElements)
            {
                if (index > 9) break;

                InteractionUiElement element = Instantiate(_instance.elementPrefab, _instance.elementsParent);
                element.SetupForOne(interactable, index);
                _instance._activeElements.Add(interactable, element);
                index++;
            }

            _instance._mimConfirmCall = confirmCallback;
            _instance._mimCancelCallback = cancelCallback;
            _instance._showingMultipleInteractions = true;
        }
        
        /// <summary> Toggle the key-indication ui for a specific interactable  </summary>
        public static void ToggleInteractionUi(InteractableBase io, bool value)
        {
            if (value) ShowInteractionKey(io);
            else HideInteractionKey(io);
        }
        #endregion
    }
}