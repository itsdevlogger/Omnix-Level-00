// using System;
// using System.Collections.Generic;
// using System.Linq;
// using AndroidInputs.Modifiers;
// using UnityEditor.Experimental.GraphView;
// using UnityEngine;
//
// namespace AndroidInputs.CustomEditors
// {
//     public class InputModifierSearchProvider : ScriptableObject, ISearchWindowProvider
//     {
//         private static readonly List<SearchTreeEntry> SEARCH_TREE;
//
//         private Func<Type, bool> _onSelect;
//
//         static InputModifierSearchProvider()
//         {
//             var modifierTypes = AppDomain.CurrentDomain.GetAssemblies()
//                 .SelectMany(assembly => assembly.GetTypes())
//                 .Where(type => typeof(IInputModifier).IsAssignableFrom(type) && !type.IsAbstract && !type.IsInterface);
//
//             SEARCH_TREE = new List<SearchTreeEntry>
//             {
//                 new SearchTreeGroupEntry(new GUIContent("Select Modifier"), 0)
//             };
//
//             foreach (var type in modifierTypes)
//             {
//                 SEARCH_TREE.Add(new SearchTreeEntry(new GUIContent(type.Name))
//                 {
//                     userData = type,
//                     level = 1
//                 });
//             }
//         }
//
//         public static InputModifierSearchProvider Create(Func<Type, bool> onSelect)
//         {
//             var current = CreateInstance<InputModifierSearchProvider>();
//             current._onSelect = onSelect;
//             SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), current);
//             return current;
//         }
//
//         public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context) => SEARCH_TREE;
//
//         public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context) => _onSelect.Invoke((Type)entry.userData);
//     }
// }