#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SaveSystem
{
    public static class SerializationCodeGenerator
    {
        [MenuItem("Assets/Save System/Auto Gen Code", true)]
        private static bool ValidateCodeGen()
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode) return false;

            Object selectedObject = Selection.activeObject;
            if (selectedObject == null)
                return false;

            return selectedObject is MonoScript;
        }

        [MenuItem("Assets/Save System/Auto Gen Code", priority = 0)]
        [MenuItem("Save System/Regenarate Selected")]
        private static void AutoGenSelected()
        {
            Object selectedObject = Selection.activeObject;
            if (selectedObject == null)
                return;

            MonoScript ms = selectedObject as MonoScript;
            if (ms == null) return;

            Type classType = ms.GetClass();
            string folder = Directory.GetParent(AssetDatabase.GetAssetPath(selectedObject)).FullName;
            string outputPath = Path.Combine(folder, classType.Name + "Serialization.cs");
            Process(classType, outputPath);
        }

        [MenuItem("Save System/Regenarate All Code")]
        private static void AutoGenAll()
        {
            foreach (var (classType, outputPath) in GetAllClasses())
            {
                Process(classType, outputPath);
            }

            AssetDatabase.Refresh();
        }

        private static IEnumerable<(Type, string)> GetAllClasses()
        {
            string[] guids = AssetDatabase.FindAssets("t:Script");
            Type baseType = typeof(ISavable);

            foreach (var guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var ms = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
                if (ms == null) continue;

                var classType = ms.GetClass();
                if (classType == null) continue;
                if (classType.FullName.Contains("Unity")) continue;
                if (classType.IsAbstract) continue;
                if (!baseType.IsAssignableFrom(classType)) continue;


                string folder = Directory.GetParent(assetPath).FullName;
                string outputPath = Path.Combine(folder, classType.Name + "Serialization.cs");
                yield return (classType, outputPath);
            }
        }

        private static void Process(Type type, string outputPath)
        {
            var mainBuilder = new StringBuilder();
            mainBuilder.AppendLine("/*******************************************************************/");
            mainBuilder.AppendLine("/*               AUTO-GENERATED FILE. DO NOT MODIFY.               */");
            mainBuilder.AppendLine("/*******************************************************************/");
            mainBuilder.AppendLine("using UnityEngine;");
            mainBuilder.AppendLine("using SaveSystem;");
            mainBuilder.AppendLine();
            mainBuilder.AppendLine($"public partial class {type.Name}");
            mainBuilder.AppendLine("{");
            mainBuilder.AppendLine("    [SerializeField] private string _id;");
            mainBuilder.AppendLine();
            mainBuilder.AppendLine("    protected virtual void Reset()");
            mainBuilder.AppendLine("    {");
            mainBuilder.AppendLine("        _id = SirHe.GetUniqueId();");
            mainBuilder.AppendLine("    }");
            mainBuilder.AppendLine();
            mainBuilder.AppendLine("    public void SaveData()");
            mainBuilder.AppendLine("    {");
            mainBuilder.AppendLine("        var writer = new BinarySerializer();");

            StringBuilder readBuilder = new StringBuilder();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (FieldInfo field in fields)
            {
                if (ShouldSave(field))
                    AppendCode(field, readBuilder, mainBuilder, prefix: "", indents: "        ");
            }

            mainBuilder.AppendLine("        writer.Save(_id);");
            mainBuilder.AppendLine("    }");
            mainBuilder.AppendLine();

            // Generate Deserialize method
            mainBuilder.AppendLine("    public void LoadData()");
            mainBuilder.AppendLine("    {");
            mainBuilder.AppendLine("        var reader = new BinaryDeserializer(_id);");
            mainBuilder.AppendLine(readBuilder.ToString());
            mainBuilder.AppendLine("    }");
            mainBuilder.AppendLine("}");
            File.WriteAllText(outputPath, mainBuilder.ToString());
        }

        private static void AppendCode(FieldInfo field, StringBuilder reader, StringBuilder writer, string prefix, string indents)
        {
            Type fieldType = field.FieldType;
            if (TryGet(fieldType, out string writerFunction, out string readerFunction))
            {
                reader.AppendLine($"{indents}{prefix}{field.Name} = reader.Read{readerFunction}();");
                writer.AppendLine($"{indents}writer.Write{writerFunction}({prefix}{field.Name});");
                return;
            }


            if (fieldType.GetCustomAttribute<SerializableAttribute>() == null)
                return;


            var line = $"\n{indents}{{\n{indents}    // {field.Name}";
            reader.AppendLine(line);
            writer.AppendLine(line);

            string ogIndent = indents;
            indents += "    ";
            prefix += field.Name;
            if (fieldType.IsClass)
            {
                var fullName = GetFullSpecifier(fieldType);
                line = $"{indents}if({prefix} == null) {prefix} = new {fullName}();";
                reader.AppendLine(line);
                writer.AppendLine(line);
            }

            prefix += ".";
            var subFields = fieldType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo subField in subFields)
            {
                if (subField.IsPublic)
                    AppendCode(subField, reader, writer, prefix, indents);
            }

            line = $"{ogIndent}}}\n";
            reader.AppendLine(line);
            writer.AppendLine(line);
        }

        private static string GetFullSpecifier(Type type)
        {
            return type.FullName.Replace('+', '.');
        }

        private static bool TryGet(Type type, out string reader, out string writer)
        {
            writer = null;
            reader = null;

            if (type.IsEnum) writer = reader = $"Enum<{GetFullSpecifier(type)}>";
            else if (type == typeof(int)) writer = reader = "Int";
            else if (type == typeof(bool)) writer = reader = "Bool";
            else if (type == typeof(float)) writer = reader = "Float";
            else if (type == typeof(string)) writer = reader = "String";
            else if (type == typeof(DateTime)) writer = reader = "DateTime";
            else if (type == typeof(TimeSpan)) writer = reader = "TimeSpan";
            else if (type == typeof(Color)) writer = reader = "Color";
            else if (type == typeof(Vector2)) writer = reader = "Vector2";
            else if (type == typeof(Vector3)) writer = reader = "Vector3";
            else if (type == typeof(Quaternion)) writer = reader = "Quaternion";
            else if (type == typeof(int[])) writer = reader = "IntArray";
            else if (type == typeof(bool[])) writer = reader = "BoolArray";
            else if (type == typeof(float[])) writer = reader = "FloatArray";
            else if (type == typeof(string[])) writer = reader = "StringArray";
            else if (type == typeof(List<int>)) { writer = "IntArray"; reader = "IntList"; }
            else if (type == typeof(List<bool>)) { writer = "BoolArray"; reader = "BoolList"; }
            else if (type == typeof(List<float>)) { writer = "FloatArray"; reader = "FloatList"; }
            else if (type == typeof(List<string>)) { writer = "StringArray"; reader = "StringList"; }
            else return false;
            return true;
        }

        private static bool ShouldSave(FieldInfo field)
        {
            if (field.IsPublic) return true;

            var ats = field.GetCustomAttribute<SerializeField>();
            return ats != null;
        }
    }
}
#endif