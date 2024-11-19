using UnityEngine;
using InteractionSystem.Interactables;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace InteractionSystem.Feedbacks
{
    [RequireComponent(typeof(Collider))]
    public class FeedbackDisplayInfo : MonoBehaviour, IInteractionFeedback
    {
        [SerializeField] public string info = "Interact";

#if UNITY_EDITOR
        protected virtual void Reset()
        {
            var allInfos = GetComponents<FeedbackDisplayInfo>();
            if (allInfos.Length > 1)
            {
                bool value = EditorUtility.DisplayDialog("Error", $"Cannot add this component as GameObject \"{name}\" already has either \"{nameof(FeedbackDisplayInfo)}\" or \"{nameof(IrnTrigger)}\".", "Dont Add", "Replace Existing Component");
                if (value) 
                { 
                    DestroyImmediate(this); 
                }
                else
                {
                    foreach (var info in allInfos)
                    {
                        if (info != this)
                        {
                            this.info = info.info;
                            DestroyImmediate(info);
                        }
                    }
                }
                return;
            }
        }
#endif

        public virtual void OnFocusGained(InteractionPlayer player)
        {
            InteractionUiHandler.DisplayInfo(info);
        }

        public virtual void OnFocusLost(InteractionPlayer player)
        {
            InteractionUiHandler.HideInfo();
        }

        public virtual void OnBeforeInteract(InteractionPlayer player)
        {
            InteractionUiHandler.HideInfo();
        }
    }
}