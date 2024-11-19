// using AndroidInputs.Modifiers;
// using UnityEditor;
// using UnityEngine;
//
// namespace AndroidInputs.CustomEditors
// {
//     [CustomPropertyDrawer(typeof(IInputModifier), true)]
//     public class InputModifierDrawer : PropertyDrawer
//     {
//         public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
//         {
//             return EditorGUI.GetPropertyHeight(property, true);
//         }
//         
//         public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//         {
//             GUIContent content= label;
//             if (property.propertyType == SerializedPropertyType.ManagedReference)
//             {
//                 var propertyType = property.type;
//                 string typeName = propertyType.Substring(17, propertyType.Length - 18);
//                 content = new GUIContent(ObjectNames.NicifyVariableName(typeName));
//             }
//             EditorGUI.PropertyField(position, property, content, true);
//         }
//     }
// }