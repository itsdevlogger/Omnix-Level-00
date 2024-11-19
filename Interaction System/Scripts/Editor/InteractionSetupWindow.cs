using InteractionSystem;
using InteractionSystem.Feedbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public class InteractionSetupWindow : EditorWindow
{
    //private class Item
    //{
    //    public Type item;
    //    public GUIContent content;
    //    public bool isSelected;
    //    public bool isExpanded = true;

    //    public Item(Type item)
    //    {
    //        this.item = item;
    //        this.content = new GUIContent(ObjectNames.NicifyVariableName(item.Name), item.FullName);
    //    }
    //}


    //private GameObject _target;
    //private List<Item> _criterias;
    //private List<Item> _feedback;
    //private List<Item> _processor;

    [MenuItem("GameObject/Interaction/Simple Setup", validate = true)]
    [MenuItem("GameObject/Interaction/Advance Setup", validate = true)]
    private static bool CheckShouldSetup()
    {
        return Selection.activeGameObject != null && Selection.gameObjects.Length == 1;
    }

    [MenuItem("GameObject/Interaction/Simple Setup")]
    private static void SetupSimple()
    {
        var gameObject = Selection.activeGameObject;
        
        if (!gameObject.TryGetComponent(out Collider _))
            gameObject.AddComponent<BoxCollider>().isTrigger = true;

        if (!gameObject.TryGetComponent(out FeedbackDisplayInfo _))
            gameObject.AddComponent<FeedbackDisplayInfo>();

        RecursiveSetLayer(gameObject.transform, 7);
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

    //[MenuItem("GameObject/Interaction/Advance Setup")]
    //private static void SetupAdvanced()
    //{
    //    SetupSimple();

    //    var window = GetWindow<InteractionSetupWindow>();
    //    window.name = "Setup Interaction";
    //    window._target = Selection.activeGameObject;
    //    window.Show();
    //}

    //private static List<Type> GetImplementations<TInterface>()
    //{
    //    // Get all types in the current assembly (can be adjusted to load from other assemblies)
    //    var types = Assembly.GetExecutingAssembly().GetTypes();
    //    var baseType = typeof(TInterface);
    //    return types
    //         .Where(type => baseType.IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
    //         .ToList();
    //}

    //private void OnEnable()
    //{
    //    _criterias = new();
    //    _feedback = new();
    //    _processor = new();
    //    var criteriasType = typeof(IInteractionCriteria);
    //    var feedbackType = typeof(IInteractionFeedback);
    //    var processorType = typeof(IInteractionProcessor);

    //    var types = AppDomain.CurrentDomain.GetAssemblies()
    //                                       .SelectMany(a => a.GetTypes())
    //                                       .Where(type => !type.IsAbstract && type.IsClass);
    //    foreach (var type in types)
    //    {
    //        if (criteriasType.IsAssignableFrom(type)) _criterias.Add(new (type));
    //        if (feedbackType.IsAssignableFrom(type)) _feedback.Add(new (type));
    //        if (processorType.IsAssignableFrom(type)) _processor.Add(new (type));
    //    }
    //}

    //private void OnGUI()
    //{
    //    EditorGUILayout.BeginHorizontal();
    //    EditorGUILayout.BeginVertical("Box");
    //    foreach (var type in _criterias)
    //    {
    //        if (type.isSelected)
    //        {
    //            type.isExpanded = EditorGUILayout.Foldout(type.isExpanded, type.content);
    //            if (type.isExpanded )
    //            {

    //            }
    //        }
    //    }
    //    EditorGUILayout.EndVertical();

    //    EditorGUILayout.BeginVertical("Box");
    //    EditorGUILayout.EndVertical();

    //    EditorGUILayout.BeginVertical("Box");
    //    EditorGUILayout.EndVertical();
    //    EditorGUILayout.EndHorizontal();
    //}
}
