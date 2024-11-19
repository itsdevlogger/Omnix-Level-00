// using System;
// using AndroidInputs.Modifiers;
// using UnityEditor;
// using UnityEngine;
//
// namespace AndroidInputs.CustomEditors
// {
//     [CustomPropertyDrawer(typeof(ModifiersCollection))]
//     public class ModifiersCollectionDrawer : PropertyDrawer
//     {
//         private SerializedProperty _property;
//         
//         public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//         {
//             SerializedProperty modifiers = property.FindPropertyRelative("_modifiers");
//             return EditorGUI.GetPropertyHeight(modifiers, true);
//         }
//
//         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//         {
//             SerializedProperty modifiers = property.FindPropertyRelative("_modifiers");
//             EditorGUI.PropertyField(position, modifiers, label, true);
//
//             if (modifiers.arraySize > 0)
//             {
//                 var lastElement = modifiers.GetArrayElementAtIndex(modifiers.arraySize - 1);
//                 if (lastElement.managedReferenceValue == null)
//                 {
//                     modifiers.arraySize -= 1;
//                     _property = property;
//                     InputModifierSearchProvider.Create(OnSelectEntry);
//                 }
//             }
//         }
//
//         private bool OnSelectEntry(Type type)
//         {
//             if (type != null && _property != null)
//             {
//                 SerializedProperty modifiers = _property.FindPropertyRelative("_modifiers");
//                 modifiers.arraySize += 1;
//                 var currentProperty = modifiers.GetArrayElementAtIndex(modifiers.arraySize - 1);
//                 
//                 IInputModifier newModifierInstance = (IInputModifier)Activator.CreateInstance(type);
//                 currentProperty.managedReferenceValue = newModifierInstance;
//                 currentProperty.serializedObject.ApplyModifiedProperties();
//             }
//
//             return true;
//         }
//     }
// }