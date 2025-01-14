using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace InteractionSystem.Interactables
{
    [ComponentInfo("Blocks interaction for specified amount of time after previous interaction ends.")]
    public class CriCooldownTimer : MonoBehaviour, IInteractable, IInteractionCriteria
    {
        public float cooldownTime;
        public bool isRealTime;
        public bool IsOnCooldown
        {
            get
            {
                if (_cooldownEndTime < 0) return false;

                float time = isRealTime ? Time.unscaledTime : Time.time;
                return _cooldownEndTime >= time;
            }
        }

#if UNITY_EDITOR
        [SerializeField, HideInInspector] public Component __EDITOR_ONLY_MANGED_BY__;
#endif

        private float _cooldownEndTime = -1;
        private bool _isInteracting = false;

        public bool CanInteract(InteractionPlayer player)
        {
            return !_isInteracting && !IsOnCooldown;
        }

        public void OnInteractionEnd(InteractionPlayer player)
        {
            _cooldownEndTime = isRealTime ? Time.unscaledTime : Time.time;
            _cooldownEndTime += cooldownTime;
            _isInteracting = false;
        }

        public void OnInteractionStart(InteractionPlayer player)
        {
            _isInteracting = true;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(CriCooldownTimer))]
    public class IrnCooldownEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            CriCooldownTimer cooldown = ((CriCooldownTimer)target);
            bool isManaged = cooldown.__EDITOR_ONLY_MANGED_BY__ != null;
            if (isManaged) 
            {
                GUI.enabled = false;
                EditorGUILayout.ObjectField($"Managed by", cooldown.__EDITOR_ONLY_MANGED_BY__, typeof(Component));
                EditorGUILayout.Space();
            }
            DrawDefaultInspector();
            
            GUI.enabled = true;
            if (!isManaged && cooldown.cooldownTime <= 0)
                EditorGUILayout.HelpBox("Cooldown Timer less than or equal to 0 will be egnored.", MessageType.Warning);
        }
    }
#endif
}