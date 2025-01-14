using InteractionSystem.Feedbacks;
using UnityEditor;
using UnityEngine;

public class InteractionSetupWindow : EditorWindow
{
    [MenuItem("GameObject/Setup Interaction", validate = true)]
    private static bool CheckShouldSetup()
    {
        return Selection.activeGameObject != null && Selection.gameObjects.Length == 1;
    }

    [MenuItem("GameObject/Setup Interaction")]
    private static void SetupSimple()
    {
        var gameObject = Selection.activeGameObject;
        
        if (!gameObject.TryGetComponent(out Collider _))
            gameObject.AddComponent<BoxCollider>().isTrigger = true;

        if (!gameObject.TryGetComponent(out FidDisplayInfo _))
            gameObject.AddComponent<FidDisplayInfo>();

        RecursiveSetLayer(gameObject.transform, InteractionSetupChecker.PRIMARY_LAYER);
        EditorUtility.SetDirty(gameObject.transform);
    }

    private static void RecursiveSetLayer(Transform transform, int layer)
    {
        if (transform == null) return;

        transform.gameObject.layer = layer;
        foreach (Transform child in transform)
        {
            RecursiveSetLayer(child, layer);
        }
    }
}
