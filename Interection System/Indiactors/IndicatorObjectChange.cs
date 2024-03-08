using UnityEngine;

namespace InteractionSystem
{
    /// <summary> Highlight an interactable by activating and deactivating visual gameObjects </summary>
    public class IndicatorObjectChange : IndicatorBase
    {
        [SerializeField] private GameObject normalObject;
        [SerializeField] private GameObject highlightedObject;

        private void Start()
        {
            Unhighlight(null, null);
        }

        public override void Highlight(InteractableBase interactable, InteractionPlayer player)
        {
            normalObject.SetActive(false);
            highlightedObject.SetActive(true);
        }

        public override void Unhighlight(InteractableBase interactable, InteractionPlayer player)
        {
            normalObject.SetActive(true);
            highlightedObject.SetActive(false);
        }
    }
}