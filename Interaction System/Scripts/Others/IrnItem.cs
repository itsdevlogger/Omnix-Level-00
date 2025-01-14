using UnityEngine;
namespace InteractionSystem.Interactables
{
    public class IrnItem : MonoBehaviour, IInteractable
    {
        public enum Operation
        {
            Pickup,
            Drop
        }

        [SerializeField] private string _id;
        public bool _destroy;
        public Operation _operation;

        public string ID => _id;

        void IInteractable.OnInteractionEnd(InteractionPlayer player)
        {
            
        }

        void IInteractable.OnInteractionStart(InteractionPlayer player)
        {
            switch (_operation)
            {
                case Operation.Pickup:
                    player.AddToOwnedItems(_id);
                    break;
                case Operation.Drop:
                    player.RemoveFromOwnedItems(_id);
                    break;
            }
            player.EndInteraction();
            if (_destroy) Destroy(gameObject);
        }
    }
}