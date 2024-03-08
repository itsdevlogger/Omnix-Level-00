using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
namespace InteractionSystem
{
    public class InteractionUiElement : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI interactionTitleText;
        [SerializeField] private TextMeshProUGUI interactionKeyText;

        private readonly HashSet<InteractableBase> _managedIntractables = new HashSet<InteractableBase>();
        private KeyCode _checkCode;

        private void Awake()
        {
            interactionTitleText.text = "";
            interactionKeyText.text = "";
        }
        
        /// <summary> Update title for this UI Element, assuming that this element manages multiple interactable </summary>
        private void UpdateAllInteractableTitles()
        {
            if (interactionTitleText == null) return;
            
            StringBuilder builder = new StringBuilder();
            foreach (InteractableBase interactable in _managedIntractables)
            {
                string text = interactable.interactionText;
                if (!string.IsNullOrEmpty(text)) builder.AppendLine(text);
            }
            interactionTitleText.text = builder.ToString();
        }
        
        /// <summary>
        /// Setup this UI element assuming the given interactable is the only interactable managed by this key
        /// Used to display listview of Multiple-Interactions
        /// </summary>
        public void SetupForOne(InteractableBase interactable, int index)
        {
            if (interactionTitleText != null)
            {
                string text = interactable.interactionText;
                if (string.IsNullOrEmpty(text)) interactionTitleText.text = "Interact";
                else interactionTitleText.text = text;
            }

            if (interactionKeyText != null)
            {
                interactionKeyText.text = $"Press {index}: ";
                _checkCode = (KeyCode)(48 + index);
            }
        }

        /// <summary> Add interactable to managed list. Assumes that all the intractable in managed list have same activation key. </summary>
        public void AddInteractable(InteractableBase interactable)
        {
            _managedIntractables.Add(interactable);
            UpdateAllInteractableTitles();

            if (interactionKeyText != null)
            {
                interactionKeyText.text = interactable.InteractionKey.ToString();
                _checkCode = KeyCode.None;
            }
        }

        /// <summary> Remove interactable from managed list. Assumes that all the intractable in managed list have same activation key. </summary>
        public void RemoveInteractable(InteractableBase interactable)
        {
            if (interactable == null) return;
            
            _managedIntractables.Remove(interactable);
            if (_managedIntractables.Count == 0)
            {
                Destroy(gameObject);
            }
            else
            {
                UpdateAllInteractableTitles();
            }
        }

        /// <summary> Check if the required key is pressed. </summary>
        public bool CheckConfirmation()
        {
            return _checkCode != KeyCode.None && Input.GetKeyUp(_checkCode);
        }
    }
}