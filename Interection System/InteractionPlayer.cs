using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace InteractionSystem
{
    public class InteractionPlayer : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// Is the player currently interacting with any object
        /// </summary>
        private bool _isInteracting = false;
        
        
        /// <summary>
        /// Objects that are being targeted for interaction.
        /// </summary>
        private readonly HashSet<InteractableBase> _targetedObjects = new HashSet<InteractableBase>();
        
        /// <summary>
        /// if <see cref="_isInteracting"/> is true, then this is the objects that player is interacting with. This set will contain exactly one element.
        /// if <see cref="_isInteracting"/> is false, then the player has confirmed interaction with all of these object and we are currently waiting for the player to choose exactly one of these 
        /// </summary>
        private readonly HashSet<InteractableBase> _interactingWith = new HashSet<InteractableBase>();

        /// <summary>
        /// All the objects to which raycast has hit in this frame
        /// </summary>
        private RaycastHit[] _raycastHits;
        private readonly List<RaycastHit> _raycastHitsList = new List<RaycastHit>();
        #endregion

        #region Unity Callbacks
        private void Start()
        {
            _raycastHits = new RaycastHit[InteractionManager.MaxSimultaneousTargets];
        }

        private void Update()
        {
            if (_isInteracting || _interactingWith.Count > 0) 
            {
                CheckInteractionOver();
                return;
            }
            
            UpdateTargets();
            UpdateInteraction();
        }
        #endregion

        #region Functionalities
        private void CheckInteractionOver()
        {
            InteractableBase item = _interactingWith.First();
            float distance = Vector3.Distance(item.transform.position, transform.position);
            if (distance > InteractionManager.InteractionRange)
            {
                if (_isInteracting)
                {
                    EndInteraction();
                }
                else
                {
                    OnAllTargetsLost();
                    InteractionManager.HideAllUi();
                }
            }
        }
        
        /// <summary>
        /// Updates the objects that are being targeted
        /// </summary>
        private void UpdateTargets()
        {
            Ray ray = InteractionCamera.Instance.ScreenPointToRay(Input.mousePosition);
            int hitCount = Physics.RaycastNonAlloc(ray, _raycastHits, InteractionManager.MaxInteractionDistance, InteractionManager.InteractableLayer);
            if (hitCount == 0)
            {
                // If raycast hits nothing, then hide the ui
                InteractionManager.HideAllUi();
                OnAllTargetsLost();
                _targetedObjects.Clear();
                _interactingWith.Clear();
            }
            else
            {
                UpdateTargetsInner(hitCount);
            }
        }

        /// <summary>
        /// Updates the target status based on RayCast
        /// </summary>
        private void UpdateTargetsInner(int hitCount)
        {
            _raycastHitsList.Clear();
            if (hitCount == 1)
            {
                _raycastHitsList.Add(_raycastHits[0]);
            }
            else
            {
                int count = 0;
                foreach (var item in _raycastHits)
                {
                    if (count >= hitCount) break;
                    count++;

                    _raycastHitsList.Add(item);
                }
                _raycastHitsList.Sort((a, b) => a.distance.CompareTo(b.distance));
            }

            HashSet<InteractableBase> cachedComponents = new HashSet<InteractableBase>();
            foreach (RaycastHit hit in _raycastHitsList)
            {
                // if (InteractionManager.BlockRaycastLayerMask == (InteractionManager.BlockRaycastLayerMask | (1 << hit.collider.gameObject.layer))) break;
                if (hit.collider.TryGetComponent(out InteractableBase component))
                {
                    cachedComponents.Add(component);
                }
                else
                {
                    if (!hit.collider.isTrigger)
                    {
                        break;
                    }
                }
            }

            // If a component was targeted in last frame, and is not found in RayCast of this frame
            // Then that target is LOST
            _targetedObjects.RemoveWhere(io => !cachedComponents.Contains(io) && OnTargetLost(io));

            // If a component was not targeted in last frame, and is found in RayCast of this frame
            // Then that target is FOUND
            foreach (InteractableBase io in cachedComponents)
            {
                if (!_targetedObjects.Contains(io))
                {
                    OnTargetFound(io);
                }
            }
        }


        /// <summary>
        /// Check if any target interaction is finalized by the player
        /// </summary>
        private void UpdateInteraction()
        {
            if (_targetedObjects.Count == 0) return;
            
            // No need to clear _interactingWith before logic,
            // Because if it has any element, then this method won't be called
            // Check Update method
            foreach (InteractableBase io in _targetedObjects)
            {
                if (io.ShouldInteractInThisFrame(this))
                {
                    _interactingWith.Add(io);
                }
            }

            int count = _interactingWith.Count;
            if (count == 1)
            {
                InteractableBase element = _interactingWith.ElementAt(0);
                OnInteract(element);
            }
            else if (count > 1)
            {
                InteractionManager.ShowMultipleInteraction(_interactingWith, OnInteract, OnAllTargetsLost);
            }
        }

        /// <summary>
        /// Include a InteractionObject object as a target.
        /// Meaning this object is ready to be interacted with. 
        /// </summary>
        private void OnTargetFound(InteractableBase other)
        {
            if (_isInteracting) return;
            if (!other.IsValid(this)) return;

            _targetedObjects.Add(other);
            other.ToggleHighlightItem(this, true);
        }

        /// <summary>
        /// Mark InteractionObject object as out-of-reach.
        /// Meaning this object is cant be interacted with.
        /// </summary>
        /// <remarks> This method does not remove object from <see cref="_targetedObjects"/></remarks>
        /// <returns> true if the object should be removed from list false wise </returns>
        private bool OnTargetLost(InteractableBase other)
        {
            if (_isInteracting) return false;

            other.ToggleHighlightItem(this, false);
            return true;
        }
        
        /// <summary>
        /// Called when the raycast hits no object and we are not interacting with anything
        /// </summary>
        private void OnAllTargetsLost()
        {
            _isInteracting = false;
            foreach (InteractableBase io in _interactingWith)
            {
                OnTargetLost(io);
            }
            foreach (InteractableBase io in _targetedObjects)
            {
                OnTargetLost(io);
            }
            
            _targetedObjects.Clear();
            _interactingWith.Clear();
        }

        /// <summary>
        /// Called when player has chosen exactly one object to interact with.
        /// </summary>
        public void OnInteract(InteractableBase interactable)
        {
            // Check if it can be interacted with
            if (!_targetedObjects.Contains(interactable) || _isInteracting) return;
            
            // Unhighlight all that we are not interacting with
            _targetedObjects.Remove(interactable);
            _interactingWith.Remove(interactable);
            OnAllTargetsLost();
            
            // Update fields
            // Must set these field before calling OnInteract. Because some object interact instantaneously
            // And call OnInteractedFinished in the same frame, which messes up the flow.
            _interactingWith.Add(interactable);
            _isInteracting = true;
            
            // Hide all the UI
            InteractionManager.HideAllUi();
            
            // Invoke Callbacks
            interactable.OnInteract(this);
        }

        /// <summary>
        /// Called to end the player's interaction with any object
        /// </summary>
        public void EndInteraction()
        {
            InteractionManager.HideAllUi();
            if (_interactingWith.Count == 0) return;

            InteractableBase interactable = _interactingWith.ElementAt(0);
            _isInteracting = false;
            _interactingWith.Clear();
            _targetedObjects.Clear();

            interactable.ToggleHighlightItem( this, false);
            interactable.OnUnInteract(this);
        }
        #endregion
    }
}