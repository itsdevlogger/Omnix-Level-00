using UnityEngine;
using UnityEditor;
using InteractionSystem;
using System.Text;

[InitializeOnLoad]
public class InteractionSetupChecker : MonoBehaviour
{
    private static readonly Texture ERROR_ICON = EditorGUIUtility.FindTexture("console.erroricon");
    public static readonly int PRIMARY_LAYER = LayerMask.NameToLayer("Interactable"); // This must be a single layer
    public static readonly int INTERACTION_LAYER_MASK = LayerMask.NameToLayer("Interactable"); // This can be a LayerMask or a single layer
    
    static InteractionSetupChecker()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
    }

    private static void HierarchyWindowItemOnGUI(int instanceID, Rect rect)
    {
        var target = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (target == null) return;

        if (TryGetError(target, out string error)) 
        {
            rect.width = rect.height;
            rect.x -= rect.width * 2f;
            var content = new GUIContent(ERROR_ICON, error);
            GUI.Label(rect, content);
        }
    }

    private static bool TryGetError(GameObject go, out string error)
    {
        StringBuilder builder;
        bool needLog = false;

        if (go.GetComponent<IInteractable>() != null)
        {
            builder = new StringBuilder();
            builder.AppendLine("Interactable is not poperly setup:");
        }
        else if (go.GetComponent<IInteractionFeedback>() != null || go.GetComponent<IInteractionCriteria>() != null)
        {
            builder = new StringBuilder();
            builder.AppendLine("Interactable is not poperly setup:");
            builder.AppendLine("  - Missing a Processor");
            needLog = true;
        }
        else
        {
            error = null;
            return false;
        }

        if (go.GetComponent<Collider>() == null)
        {
            builder.AppendLine($"  - Missing a Collider.");
            needLog = true;
        }

        if ((INTERACTION_LAYER_MASK & (1 << go.layer)) != 0)
        {
            builder.AppendLine($"  - Wrong Layer.");
            needLog = true;
        }

        if (needLog)
        {
            error = builder.ToString();
            return true;
        }
        else
        {
            error = null;
            return false;
        }
    }

    //[MenuItem("Tools/Check All Interactions")]
    //public static void CheckAllInteractions()
    //{
    //    int errorCount = 0;
    //    if (InteractionLayer < 0)
    //    {
    //        Debug.LogError($"Update interaction layer in {nameof(InteractionSetupChecker)}");
    //        return;
    //    }
    //    GameObject[] allGameObjects = GameObject.FindObjectsOfType<GameObject>();
    //    StringBuilder builder = new StringBuilder();

    //    foreach (GameObject go in allGameObjects)
    //    {
    //        if (go.GetComponent<IInteractionProcessor>() != null)
    //        {
    //            bool needLog = false;
    //            builder.Clear();

    //            if (go.GetComponent<Collider>() == null)
    //            {
    //                builder.AppendLine($"GameObject: {go.name}");
    //                builder.AppendLine($"    - Missing a Collider.");
    //                needLog = true;
    //                errorCount++;
    //            }

    //            if (go.layer != InteractionLayer)
    //            {
    //                if (!needLog) builder.AppendLine($"GameObject: {go.name}");

    //                builder.AppendLine($"    - Wrong Layer.");
    //                needLog = true;
    //                errorCount++;
    //            }

    //            if (needLog) Debug.LogError(builder.ToString(), go);
    //        }
    //        else if (go.GetComponent<IInteractionFeedback>() != null || go.GetComponent<IInteractionCriteria>() != null)
    //        {
    //            builder.AppendLine($"GameObject: {go.name}");
    //            builder.AppendLine($"    - Missing a Processor");
    //            Debug.LogError(builder.ToString(), go);
    //            errorCount++;
    //        }
    //    }

    //    if (errorCount > 0)
    //        Debug.LogError($"Validation completed with {errorCount} error(s). Check the console for details.");
    //    else
    //        Debug.Log("Validation completed. All interactions are properly set up!");
    //}
}
