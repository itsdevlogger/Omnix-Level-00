using UnityEngine;
namespace InteractionSystem.Interactables
{
    public class IrnItem : MonoBehaviour, IInteractionProcessor
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

        void IInteractionProcessor.OnInteractionEnd(InteractionPlayer player)
        {
            
        }

        void IInteractionProcessor.OnInteractionStart(InteractionPlayer player)
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