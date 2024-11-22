using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace InteractionSystem.Interactables
{
    [ComponentInfo("Blocks interaction for specified amount of time after previous interaction ends.")]
    [RequireComponent(typeof(Collider))]
    public class IrnCooldown : MonoBehaviour, IInteractionProcessor, IInteractionCriteria
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
    [CustomEditor(typeof(IrnCooldown))]
    public class IrnCooldownEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            IrnCooldown cooldownScript = (IrnCooldown)target;
            if (cooldownScript.cooldownTime <= 0)
            {
                EditorGUILayout.HelpBox("Cooldown Timer must be greater than 0.", MessageType.Error);
            }
        }
    }
#endif
}