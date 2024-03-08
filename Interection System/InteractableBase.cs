using UnityEngine;

namespace InteractionSystem
{
    public abstract class InteractableBase : MonoBehaviour
    {
        #region Fields
        [Tooltip("Text that should be shown bellow interact key (Indication to the player what they will be doing)")]
        public string interactionText;

        [SerializeField, Tooltip("What key to be used to interact with this object. If set to NONE, then uses default interaction key set in InteractionManager")]
        private KeyCode interactionKey;

        [SerializeField, Tooltip("What indications to show player when they can interact with this object")] 
        private IndicatorBase[] interactionIndicators;

        /// <summary> What key to be used to interact with this object. </summary>
        public KeyCode InteractionKey => interactionKey != KeyCode.None ? interactionKey : InteractionManager.DefaultInteractionKey;
        #endregion

        #region Functionalities
        /// <summary> Toggle highlight of this object. Indicating the player that they can interact with this object. </summary>
        internal void ToggleHighlightItem(InteractionPlayer player, bool isOn)
        {
            InteractionManager.ToggleInteractionUi(this, isOn);
            if (isOn)
            {
                foreach (IndicatorBase indicator in interactionIndicators)
                {
                    indicator.Highlight(this, player);
                }
            }
            else
            {
                foreach (IndicatorBase indicator in interactionIndicators)
                {
                    indicator.Unhighlight(this, player);
                }
            }
        }
        #endregion

        #region Abstract
        /// <summary> Callback when the player Interact with this object </summary>
        public abstract void OnInteract(InteractionPlayer player);
        #endregion

        #region Callbacks
        /// <summary> Callback when the player Un-Interacts with this object </summary>
        public virtual void OnUnInteract(InteractionPlayer player) { }

        /// <summary> Is interaction valid. </summary>
        /// <returns> true if player can interact with this object, false otherwise </returns>
        public virtual bool IsValid(InteractionPlayer player) { return true; }
        
        /// <summary> Callback when the player enters interactive range of this object </summary>
        public virtual void PrepareInteractionStart(InteractionPlayer player) { }
        
        /// <summary> Callback when the player leaves interactive range of this object </summary>
        public virtual void PrepareInteractionEnd(InteractionPlayer player) { }

        /// <summary> Should the player interact with this object in this frame. </summary>
        /// <returns> true if the interaction should begin, false otherwise. </returns>
        public virtual bool ShouldInteractInThisFrame(InteractionPlayer player) { return Input.GetKeyDown(InteractionKey); }
        #endregion
    }
}