using System;
using UnityEngine;

namespace InteractionSystem.Feedbacks
{
    public class FeedbackMaterialSwitch : MonoBehaviour, IInteractionFeedback
    {
        [System.Serializable]
        public class RendererMaterialSettings
        {
            public Renderer renderer;
            public Material focusMaterial;
            [NonSerialized] public Material defaultMaterial;
        }

        [SerializeField] private Renderer[] _renderers;
        [SerializeField] private Material _focusMaterial;
        [SerializeField] private RendererMaterialSettings[] _specialCases;
        private Material _defaultMaterial;

        private void Reset()
        {
            _renderers = GetComponentsInChildren<Renderer>();
        }

        private void Awake()
        {
            if (_renderers.Length > 0)
                _defaultMaterial = _renderers[0].material;

            foreach (var setting in _specialCases)
            {
                setting.defaultMaterial = setting.renderer.material;
                if (setting.focusMaterial == null)
                    Debug.LogError("Material is null for some renderers.", gameObject);
            }
        }

        void IInteractionFeedback.OnFocusGained(InteractionPlayer player)
        {
            SetMaterial(_focusMaterial);
            foreach (var setting in _specialCases)
            {
                if (setting.renderer != null)
                    setting.renderer.material = setting.focusMaterial;
            }
        }

        void IInteractionFeedback.OnBeforeInteract(InteractionPlayer player)
        {
            SetMaterial(_defaultMaterial);
            SetDefaultMaterial();
        }

        void IInteractionFeedback.OnFocusLost(InteractionPlayer player)
        {
            SetMaterial(_defaultMaterial);
            SetDefaultMaterial();
        }

        private void SetDefaultMaterial()
        {
            foreach (var setting in _specialCases)
            {
                if (setting.renderer != null)
                    setting.renderer.material = setting.defaultMaterial;
            }
        }

        private void SetMaterial(Material material)
        {
            foreach (Renderer renderer in _renderers)
            {
                if (renderer != null)
                renderer.material = material;
            }
        }
    }
}