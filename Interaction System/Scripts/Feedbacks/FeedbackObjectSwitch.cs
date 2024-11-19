using UnityEngine;

namespace InteractionSystem.Feedbacks
{
    public class FeedbackObjectSwitch : MonoBehaviour, IInteractionFeedback
    {
        [SerializeField] private GameObject defaultObject;
        [SerializeField] private GameObject focusObject;

        private void Awake()
        {
            SetFocusState(false);
        }

        private void SetFocusState(bool inFocus)
        {
            if (defaultObject != null) defaultObject.SetActive(!inFocus);
            if (focusObject != null) focusObject.SetActive(inFocus);
        }

        void IInteractionFeedback.OnFocusLost(InteractionPlayer player)
        {
            SetFocusState(false);
        }

        void IInteractionFeedback.OnBeforeInteract(InteractionPlayer player)
        {
            SetFocusState(false);
        }

        void IInteractionFeedback.OnFocusGained(InteractionPlayer player)
        {
            SetFocusState(true);
        }
    }
}