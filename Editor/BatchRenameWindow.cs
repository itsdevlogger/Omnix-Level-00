using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using Rect = UnityEngine.Rect;

namespace Omnix.Editor
{
    public class RenameVariable
    {
        private string _name;
        private float _start;
        private float _step = 1f;
        private float _value;

        public RenameVariable(string name) => _name = name;
        public void Compile() => _value = _start - _step;

        public string Get()
        {
            _value += _step;
            return _value.ToString(CultureInfo.InvariantCulture);
        }

        public void Draw()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label($"     {_name}     ");
            GUILayout.Label("Start: ");
            _start = EditorGUILayout.FloatField(GUIContent.none, _start);
            EditorGUILayout.Space();
            GUILayout.Label("Step: ");
            _step = EditorGUILayout.FloatField(GUIContent.none, _step);
            EditorGUILayout.EndHorizontal();
        }
    }

    public class OldBatchRenameWindow : EditorWindow
    {
        private const string MENU_NAME_ONE = "Tools/Batch Rename";
        private const string MENU_NAME_TWO = "GameObject/Tools/Batch Rename";
        private const string COMMAND = " %F2";

        private static readonly Regex VARIABLE_REGEX = new Regex(@"\{(.*?)\}");
        private static readonly Color TOGGLE_BUTTON_ACTIVE_COLOR = new Color(0.584f, 0.8f, 0.749f);
        private static readonly Color TOGGLE_BUTTON_INACTIVE_COLOR = new Color(0.812f, 0.812f, 0.812f);

        public enum CaseFormatting
        {
            DoNothing,
            LowerCase,
            UpperCase,
            SentenceCase,
            TitleCase
        }

        private string _findWhat = "";
        private string _replaceWith = "";
        private bool _addPrefix = false;
        private bool _addSuffix = false;
        private string _prefix = "";
        private string _suffix = "";
        private bool _caseSensitive = false;
        private bool _useRegex = false;
        private bool _matchAllOccurrences = true;
        private bool _nicefyNames = false;
        private CaseFormatting _caseFormatting = CaseFormatting.DoNothing;

        private List<Object> _selectedObjects = new List<Object>();
        private List<string> _originalNames = new List<string>();
        private List<string> _renamedNames = new List<string>();
        private readonly Dictionary<string, RenameVariable> _renameVariables = new();

        private Vector2 _scrollPosition;
        private GUIStyle _boxStyle;
        private GUIStyle _bigButtonStyle;
        private Regex _findRegex;
        private Func<string, string> _caseFormatter;

        [MenuItem(MENU_NAME_ONE, validate = true)]
        [MenuItem(MENU_NAME_TWO, validate = true)]
        private static bool Check() => Selection.gameObjects.Length > 0;

        [MenuItem(MENU_NAME_ONE + COMMAND)]
        [MenuItem(MENU_NAME_TWO)]
        private static void ShowWindow()
        {
            var window = GetWindow<OldBatchRenameWindow>("Batch Rename");
            window.InitializeWithSelection();
        }

        private void InitializeWithSelection()
        {
            _selectedObjects = Selection.objects.ToList();
            Compile();
            UpdateNames();
        }

        private void OnGUI()
        {
            if (_boxStyle == null)
                _boxStyle = "box";

            // Top Area
            DrawSettings();
            DrawPreview();
        }

        private void DrawSettings()
        {
            if (_bigButtonStyle == null)
            {
                _bigButtonStyle = new GUIStyle(EditorStyles.toolbarButton);
                _bigButtonStyle.fixedHeight = 30f;
            }

            EditorGUILayout.BeginVertical(_boxStyle);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.BeginHorizontal();
            _findWhat = EditorGUILayout.TextField("Find What", _findWhat);
            _useRegex = EditorGUILayout.ToggleLeft("Use Regex", _useRegex, GUILayout.Width(100f));
            EditorGUILayout.EndHorizontal();
            _replaceWith = EditorGUILayout.TextField("Replace With", _replaceWith);

            // Prefix
            DrawToggleField("Add Prefix", ref _addPrefix, ref _prefix);
            DrawToggleField("Add Suffix", ref _addSuffix, ref _suffix);

            _caseFormatting = (CaseFormatting)EditorGUILayout.EnumPopup("Case Formatting", _caseFormatting);

            if (_renameVariables.Count > 0)
            {
                GUILayout.Space(20f);
                EditorGUILayout.BeginVertical(_boxStyle);
                GUILayout.Label("Rename Variables:");
                foreach (RenameVariable variable in _renameVariables.Values)
                    variable.Draw();
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.BeginHorizontal();
            _nicefyNames = ToggleButton("Nicefy Names", _nicefyNames);
            _caseSensitive = ToggleButton("Case Sensitive", _caseSensitive);
            // _useRegex = ToggleButton("Use Regex", _useRegex);
            _matchAllOccurrences = ToggleButton("Match All Occurrences", _matchAllOccurrences);
            EditorGUILayout.EndHorizontal();

            bool needUpdate = EditorGUI.EndChangeCheck();

            GUILayout.Space(5f);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Apply", _bigButtonStyle, GUILayout.Height(30f)))
            {
                PerformRename();
            }

            if (GUILayout.Button("Apply and Close", _bigButtonStyle, GUILayout.Height(30f)))
            {
                PerformRename();
                Close();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.EndVertical();

            GUILayout.Space(5f);
            Object obj = EditorGUILayout.ObjectField(null, typeof(Object), true, GUILayout.Height(30f));
            if (obj != null && !_selectedObjects.Contains(obj))
            {
                _selectedObjects.Add(obj);
                needUpdate = true;
            }

            if (needUpdate)
            {
                Compile();
                UpdateNames();
            }
        }

        private static void DrawToggleField(string label, ref bool variable, ref string text)
        {
            Rect rect = EditorGUILayout.GetControlRect(true);
            var width = rect.width;

            rect.width = EditorGUIUtility.labelWidth;
            variable = EditorGUI.ToggleLeft(rect, label, variable);
            GUI.enabled = variable;

            rect.x += rect.width;
            rect.width = width - rect.width;
            text = EditorGUI.TextField(rect, GUIContent.none, text);
            GUI.enabled = true;
        }

        private void DrawPreview()
        {
            if (_originalNames.Count == 0) return;

            float x = 20;
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
            Rect headerRect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight * 1.4f);
            var firstRect = new Rect(headerRect);
            firstRect.height = (firstRect.height + EditorGUIUtility.standardVerticalSpacing + 2f) * (_originalNames.Count + 1);
            EditorGUI.DrawRect(firstRect, new Color(0f, 0f, 0f, 0.2f));
            firstRect.x += firstRect.width * 0.5f - x;
            firstRect.width = 2f;
            EditorGUI.DrawRect(firstRect, new Color(0f, 0f, 0f, 0.5f));
            DrawTwoLabels(headerRect, "Current", "Renamed");


            for (int i = 0; i < _originalNames.Count; i++)
            {
                var rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight * 1.4f);
                DrawTwoLabels(rect, _originalNames[i], _renamedNames[i]);
            }

            EditorGUILayout.EndScrollView();
            return;

            void DrawTwoLabels(Rect rect, string labelOne, string labelTwo)
            {
                var lineRect = new Rect(rect);
                lineRect.y += lineRect.height;
                lineRect.height = 2f;
                EditorGUI.DrawRect(lineRect, new Color(0f, 0f, 0f, 0.5f));

                rect.x += x;
                rect.width = rect.width / 2f - 2.5f * x;
                EditorGUI.LabelField(rect, labelOne);

                rect.x += rect.width + 2f * x;
                EditorGUI.LabelField(rect, labelTwo);
                GUILayout.Space(2f);
            }
        }

        private bool ToggleButton(string text, bool toggle)
        {
            var color = GUI.backgroundColor;
            GUI.backgroundColor = toggle ? TOGGLE_BUTTON_ACTIVE_COLOR : TOGGLE_BUTTON_INACTIVE_COLOR;
            if (GUILayout.Button(text, _bigButtonStyle, GUILayout.Height(25f))) toggle = !toggle;
            GUI.backgroundColor = color;
            return toggle;
        }

        private void Compile()
        {
            string finding;
            if (_useRegex) finding = _findWhat;
            else finding = Regex.Escape(_findWhat);

            if (_caseSensitive) _findRegex = new Regex(finding);
            else _findRegex = new Regex(finding, RegexOptions.IgnoreCase);

            HashSet<string> removedNames = _renameVariables.Keys.ToHashSet();
            foreach (Match match in VARIABLE_REGEX.Matches(_replaceWith))
            {
                string varName = match.Groups[1].Value;
                if (_renameVariables.TryGetValue(varName, out var variable)) variable.Compile();
                else
                {
                    var renameVariable = new RenameVariable(varName);
                    renameVariable.Compile();
                    _renameVariables[varName] = renameVariable;
                }

                removedNames.Remove(varName);
            }

            foreach (string removedName in removedNames)
            {
                _renameVariables.Remove(removedName);
            }

            switch (_caseFormatting)
            {
                case CaseFormatting.LowerCase:
                    _caseFormatter = text => text.ToLower();
                    break;
                case CaseFormatting.UpperCase:
                    _caseFormatter = text => text.ToUpper();
                    break;
                case CaseFormatting.SentenceCase:
                    _caseFormatter = ToSentenceCase;
                    break;
                case CaseFormatting.TitleCase:
                    _caseFormatter = ToTitleCase;
                    break;
                default:
                    _caseFormatter = text => text;
                    break;
            }
        }

        private void PerformRename()
        {
            Compile();

            int undoGroupIndex = Undo.GetCurrentGroup();
            Undo.IncrementCurrentGroup();
            Undo.SetCurrentGroupName($"Old Batch Renamed {_selectedObjects.Count} Objects");

            for (int i = 0; i < _selectedObjects.Count; i++)
            {
                var obj = _selectedObjects[i];
                if (obj == null) continue;

                string newName = GetRenamedName(_originalNames[i]);
                Undo.RecordObject(obj, "Rename Object");
                obj.name = newName;
            }

            Undo.CollapseUndoOperations(undoGroupIndex);

            Compile();
            UpdateNames();
        }

        private void UpdateNames()
        {
            _originalNames.Clear();
            _renamedNames.Clear();
            int index = 1;
            foreach (var obj in _selectedObjects)
            {
                if (obj == null) continue;

                string originalName = obj.name;
                _originalNames.Add(originalName);
                _renamedNames.Add(GetRenamedName(originalName));
                index++;
            }
        }

        private static string ToSentenceCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            input = input.ToLower(CultureInfo.InvariantCulture);
            return char.ToUpper(input[0], CultureInfo.InvariantCulture) + input.Substring(1);
        }

        private static string ToTitleCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(input.ToLower());
        }

        private string GetRenamedName(string originalName)
        {
            string replaceWith = _replaceWith;
            foreach (KeyValuePair<string, RenameVariable> variable in _renameVariables)
                replaceWith = replaceWith.Replace($"{{{variable.Key}}}", variable.Value.Get());

            string replaced;
            if (_matchAllOccurrences) replaced = _findRegex.Replace(originalName, replaceWith);
            else replaced = _findRegex.Replace(originalName, replaceWith, 1);

            if (_nicefyNames) replaced = ObjectNames.NicifyVariableName(replaced);
            if (_addPrefix) replaced = _prefix + replaced;
            if (_addSuffix) replaced += _suffix;
            return _caseFormatter(replaced);
        }
    }
}