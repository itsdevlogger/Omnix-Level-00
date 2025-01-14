using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace InteractionSystem
{
    public class InteractionTarget
    {
        public static event Action<InteractionPlayer, InteractionTarget> OnFocusGained;
        public static event Action<InteractionPlayer, InteractionTarget> OnFocusLost;
        public static event Action<InteractionPlayer, InteractionTarget> OnInteractionStart;
        public static event Action<InteractionPlayer, InteractionTarget> OnInteractionEnd;

        public readonly GameObject gameObject;
        public readonly Transform transform;
        public readonly IEnumerable<IInteractionCriteria> checks;
        public readonly IEnumerable<IInteractionFeedback> feedbacks;
        public readonly IEnumerable<IInteractable> handlers;
        public bool invokeEvents;
        
        private HashSet<IInteractionCriteria> _readyToInteract;

        /// <summary>
        /// Use <see cref="TryGet(GameObject, bool, out InteractionTarget)"/> or <see cref="TryGet(IEnumerable{GameObject}, bool, out InteractionTarget)"/>
        /// </summary>
        public InteractionTarget(
            GameObject gameObject,
            IEnumerable<IInteractionCriteria> checks,
            IEnumerable<IInteractionFeedback> feedbacks,
            IEnumerable<IInteractable> handlers,
            bool invokeEvents = true
            )
        {
            this.gameObject = gameObject;
            this.transform = gameObject.transform;
            this.invokeEvents = invokeEvents;
            this._readyToInteract = new HashSet<IInteractionCriteria>();

            this.checks = checks;
            this.feedbacks = feedbacks;
            this.handlers = handlers;
        }

        public bool CanInteract(InteractionPlayer player)
        {
            foreach (var check in checks)
            {
                if (check == null) continue;
                if (_readyToInteract.Contains(check)) continue;

                if (check.CanInteract(player)) _readyToInteract.Add(check);
                else return false;
            }

            return true;
        }

        public void GainFocus(InteractionPlayer player)
        {
            _readyToInteract.Clear();

            try
            {
                foreach (var component in feedbacks)
                {
                    component?.OnFocusGained(player);
                }

                if (invokeEvents) 
                    OnFocusGained?.Invoke(player, this);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Got error in during callback, see the next log.");
                Debug.LogException(ex);
            }
        }

        public void LooseFocus(InteractionPlayer player)
        {
            try
            {
                foreach (var component in feedbacks)
                {
                    component?.OnFocusLost(player);
                }

                if (invokeEvents)
                    OnFocusLost?.Invoke(player, this);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Got error in during callback, see the next log.");
                Debug.LogException(ex);
            }
        }

        public void StartInteraction(InteractionPlayer player)
        {
            try
            {
                foreach (var component in feedbacks)
                {
                    component?.OnBeforeInteract(player);
                }

                foreach (var component in handlers)
                {
                    component?.OnInteractionStart(player);
                }

                if (invokeEvents) 
                    OnInteractionStart?.Invoke(player, this);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Got error in during callback, see the next log.");
                Debug.LogException(ex);
            }
        }

        public void EndInteraction(InteractionPlayer player)
        {
            try
            {
                foreach (var component in handlers)
                {
                    component?.OnInteractionEnd(player);
                }

                if (invokeEvents) 
                    OnInteractionEnd?.Invoke(player, this);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Got error in during callback, see the next log.");
                Debug.LogException(ex);
            }
        }

        public static bool TryGet(GameObject target, bool invokeEvents, out InteractionTarget interactionTarget)
        {
            if (target == null)
            {
                interactionTarget = null;
                return false;
            }

            var h = target.GetComponents<IInteractable>();
            if (h.Length == 0)
            {
#if UNITY_EDITOR
                if (InteractionPlayer.EditorOnlyLogIncorrectlySetupInteractions)
                {
                    var checks = target.GetComponents<IInteractionCriteria>();
                    var feedbacks = target.GetComponents<IInteractionFeedback>();
                    if (checks.Length > 0) Debug.LogError($"Game Object \"{target.name}\" has \"{nameof(IInteractionCriteria)}\" but no \"{nameof(IInteractable)}\". The object will be ignored.");
                    if (feedbacks.Length > 0) Debug.LogError($"Game Object \"{target.name}\" has \"{nameof(IInteractionFeedback)}\" but no \"{nameof(IInteractable)}\". The object will be ignored.");
                }
#endif
                interactionTarget = null;
                return false;
            }

            var c = target.GetComponents<IInteractionCriteria>();
            var f = target.GetComponents<IInteractionFeedback>();
            interactionTarget = new InteractionTarget(target, c, f, h, invokeEvents);
            return true;
        }

        public static bool TryGet(IEnumerable<GameObject> targets, bool invokeEvents, out InteractionTarget interactionTarget)
        {
            var checks = new List<IInteractionCriteria>();
            var feedbacks = new List<IInteractionFeedback>();
            var handlers = new List<IInteractable>();
            GameObject mainTarget = null;

            foreach (var target in targets)
            {
                if (target == null) continue;

                if (mainTarget == null) mainTarget = target;
                checks.AddRange(target.GetComponents<IInteractionCriteria>());
                feedbacks.AddRange(target.GetComponents<IInteractionFeedback>());
                handlers.AddRange(target.GetComponents<IInteractable>());
            }

            if (mainTarget == null || handlers.Count == 0)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var target in targets)
                {
                    stringBuilder.Append("\n - " + target.name);
                }
                var targs = stringBuilder.ToString();
                if (checks.Count > 0) Debug.LogError($"Given Game Objects have \"{nameof(IInteractionCriteria)}\" but no \"{nameof(IInteractable)}\". The objects will be ignored.\nObjects:{targs}");
                if (feedbacks.Count > 0) Debug.LogError($"Game Objects {mainTarget} has {nameof(IInteractionFeedback)} but no {nameof(IInteractable)}. The object will be ignored.\nObjects:{targs}");
                interactionTarget = null;
                return false;
            }

            interactionTarget = new InteractionTarget(mainTarget, checks, feedbacks, handlers, invokeEvents);
            return true;
        }
    }
}
