using InteractionSystem.Feedbacks;
using System.Collections.Generic;
using UnityEngine;


namespace InteractionSystem.Interactables
{
    /// <summary>
    /// Tells <see cref="IrnTrigger"/> to ignore this component.
    /// </summary>
    public interface IHiddenForTrigger
    {

    }

    [RequireComponent(typeof(Collider))]
    public class IrnTrigger : FeedbackDisplayInfo, IInteractionCriteria, IInteractionProcessor
    {
        [SerializeField] private GameObject[] _targets;
        private InteractionTarget _target;

        private void Awake()
        {
            if (InteractionTarget.TryGet(_targets, false, out InteractionTarget it))
            {
                RemoveIgnored(it.checks);
                RemoveIgnored(it.feedbacks);
                RemoveIgnored(it.handlers);
                _target = it;
            }
            else
            {
                Debug.LogError($"Did not find any interactable to brodcast to: {nameof(IrnTrigger)} component on {gameObject.name}.", gameObject);
            }
        }

        private void RemoveIgnored<T>(IEnumerable<T> target)
        {
            ((List<T>)target).RemoveAll(c => c is IHiddenForTrigger);
        }

        bool IInteractionCriteria.CanInteract(InteractionPlayer player)
        {
            return _target.CanInteract(player);
        }

        void IInteractionProcessor.OnInteractionStart(InteractionPlayer player)
        {
            // Dont change order of operations in this method
            _target.StartInteraction(player);
        }

        void IInteractionProcessor.OnInteractionEnd(InteractionPlayer player)
        {
            _target.EndInteraction(player);
        }

        public override void OnFocusGained(InteractionPlayer player)
        {
            // Dont change order of operations in this method
            base.OnFocusGained(player);
            InteractionUiHandler.AddBlocker();
            _target.GainFocus(player);
        }

        public override void OnFocusLost(InteractionPlayer player)
        {
            // Dont change order of operations in this method
            _target.LooseFocus(player);
            InteractionUiHandler.RemoveBlocker();
            base.OnFocusLost(player);
        }

        public override void OnBeforeInteract(InteractionPlayer player)
        {
            base.OnBeforeInteract(player);
        }

        public void RegisterCheck(IInteractionCriteria check)
        {
            if (check == null) return;

            var list = (List<IInteractionCriteria>)_target.checks;
            if (!list.Contains(check))
                list.Add(check);
        }

        public void RegisterFeedback(IInteractionFeedback feedback)
        {
            if (feedback == null) return;

            var list = (List<IInteractionFeedback>)_target.feedbacks;
            if (!list.Contains(feedback))
                list.Add(feedback);
        }

        public void RegisterHandler(IInteractionProcessor handler)
        {
            if (handler == null) return;

            var list = (List<IInteractionProcessor>)_target.feedbacks;
            if (!list.Contains(handler))
                list.Add(handler);
        }

        public void UnregisterCheck(IInteractionCriteria check)
        {
            if (check == null) return;

            var list = (List<IInteractionCriteria>)_target.checks;
            list.Remove(check);
        }

        public void UnregisterFeedback(IInteractionFeedback feedback)
        {
            if (feedback == null) return;

            var list = (List<IInteractionFeedback>)_target.feedbacks;
            list.Remove(feedback);
        }

        public void UnregisterHandler(IInteractionProcessor handler)
        {
            if (handler == null) return;

            var list = (List<IInteractionProcessor>)_target.feedbacks;
            list.Remove(handler);
        }
    }
}