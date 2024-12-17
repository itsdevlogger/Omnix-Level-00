﻿using UnityEngine;
namespace InteractionSystem.Interactables
{
    public class CriHasItem : MonoBehaviour, IInteractionCriteria
    {
        [SerializeField, Tooltip("If direct referance is possible, use this")] private IrnItem _key;
        [SerializeField, Tooltip("If direct referance is not possible, use this")] private string _keyId;
        [SerializeField] private string _errorMessage;

        private string KeyID
        {
            get
            {
                if (_key != null) return _key.ID;
                return _keyId;
            }
        }

        bool IInteractionCriteria.CanInteract(InteractionPlayer player)
        {
            if (!player.DoesOwnsItem(KeyID))
            {
                InteractionUiHandler.DisplayError(_errorMessage);
                return false;
            }

            return true;
        }
    }
}