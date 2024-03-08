using System.Collections.Generic;
using UnityEngine;

namespace InteractionSystem
{
    /// <summary> Highlight an interactable by changing it's material  </summary>
    public class IndicatorMaterialChange : IndicatorBase
    {
        [SerializeField] private Material material;
        private readonly Dictionary<Renderer, Material> _oldMaterials = new Dictionary<Renderer, Material>();

        public override void Highlight(InteractableBase interactable, InteractionPlayer player)
        {
            if (_oldMaterials.Count > 0)
            {
                Unhighlight(interactable, player);
            }

            foreach (Renderer child in GetComponentsInChildren<Renderer>())
            {
                _oldMaterials.Add(child, child.material);
                child.material = material;
            }
        }

        public override void Unhighlight(InteractableBase interactable, InteractionPlayer player)
        {
            foreach (Renderer child in GetComponentsInChildren<Renderer>())
            {
                if (_oldMaterials.TryGetValue(child, out Material mat))
                {
                    child.material = mat;
                }
            }
            
            _oldMaterials.Clear();
        }
    }

}