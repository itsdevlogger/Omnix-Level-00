using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;



public static class SceneIndexGenerator
{
    private const string SCRIPT_TEMPLATE = @"/*********************************************/
/******** THIS CODE IS AUTO GENERATED ********/
/*********************************************/

using UnityEngine; 
using System.Collections.Generic;

public enum SceneId
{
        [Tooltip(""Previous Scene in Build Index"")]
        PreviousScene = -2,

        [Tooltip(""Next Scene in Build Index"")]
        NextScene = -1,

        [Tooltip(""Invalid Scene/Unknown Scene/Null Scene"")]
        Unknown = 0,

SCENE_ID_ENTRIES
}


public static class BI
{
    public static readonly (SceneId, string)[] BUILD_INDEX = new (SceneId, string)[]
    {
BUILD_INDEX_ENTRIES
    };

    public static readonly Dictionary<SceneId, string> ID_TO_NAME;
    public static readonly Dictionary<string, SceneId> NAME_TO_ID;
    public static readonly Dictionary<SceneId, int> ID_TO_INDEX;

    static BI()
    {
        ID_TO_NAME = new Dictionary<SceneId, string>();
        ID_TO_INDEX = new Dictionary<SceneId, int>();
        NAME_TO_ID = new Dictionary<string, SceneId>();

        int index = -3;
        foreach ((SceneId id, string name) in BUILD_INDEX)
        {
            ID_TO_NAME.Add(id, name);
            NAME_TO_ID.Add(name, id);
            ID_TO_INDEX.Add(id, index);
            index++;
        }
    }
}";

    private const string INDEX_DATA_RESOURCE_NAME = "SceneIndexData";
    private const string INDEX_DATA_RESOURCE_PATH = "Assets/Editor/Resources";
    private const string SCRIPT_GUID = "a5bea2d20a604267829ad47212707b57";

    private static SceneIndexData LoadOrCreateSceneIndex()
    {
        SceneIndexData indexData = Resources.Load<SceneIndexData>(INDEX_DATA_RESOURCE_NAME);
        if (indexData == null)
        {
            indexData = ScriptableObject.CreateInstance<SceneIndexData>();
            if (!Directory.Exists(INDEX_DATA_RESOURCE_PATH))
            {
                Directory.CreateDirectory(INDEX_DATA_RESOURCE_PATH);
                AssetDatabase.ImportAsset(INDEX_DATA_RESOURCE_PATH);
            }
            AssetDatabase.CreateAsset(indexData, INDEX_DATA_RESOURCE_PATH + "/" + INDEX_DATA_RESOURCE_NAME + ".asset");
            AssetDatabase.SaveAssets();
        }
        
        return indexData;
    }
    
    public static string ConvertToPascalCase(string input)
    {
        TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
        string titleCase = textInfo.ToTitleCase(input);
        return titleCase.Replace(" ", "");
    }
    
    [MenuItem("Scene Management/Rebuild Index")]
    public static void RebuildSceneIndex()
    {
        var indexData = LoadOrCreateSceneIndex();
        Dictionary<SceneAsset, SceneEntry> sceneDictionary = indexData.sceneEntries.ToDictionary(entry => entry.asset, entry => entry);
        
        var scenesInBuild = EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => AssetDatabase.LoadAssetAtPath<SceneAsset>(scene.path))
            .ToList();

        HashSet<SceneAsset> completeList = scenesInBuild.Concat(sceneDictionary.Keys).ToHashSet();
        List<SceneEntry> sceneEntries = new List<SceneEntry>();
        HashSet<string> sceneIdEnumEntries = new HashSet<string>();
        int sceneId = sceneDictionary.Values.Select(sc => sc.enumValue).DefaultIfEmpty(0).Max() + 1;
        
        foreach (SceneAsset scene in completeList)
        {
            if (sceneDictionary.TryGetValue(scene, out var entry) == false)
            {
                var enumEntry = ConvertToPascalCase(Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(scene)));
                entry = new SceneEntry(scene, enumEntry, sceneId);
                sceneDictionary.Add(scene, entry);
                sceneId++;
            }

            if (sceneIdEnumEntries.Contains(entry.enumName))
                throw new Exception("Build index contains multiple scenes with the same name, Scene Manager does not know hot to rectify this.");
            
            sceneIdEnumEntries.Add(entry.enumName);
            sceneEntries.Add(entry);
        }

        // Generate enum file
        string scriptPath = AssetDatabase.GUIDToAssetPath(SCRIPT_GUID);
        if (string.IsNullOrEmpty(scriptPath))
            throw new Exception($"Invalid GUID: {SCRIPT_GUID}. Cannot find asset path.");

        sceneEntries.Sort((s1, s2) => s1.enumValue.CompareTo(s2.enumValue));
        
        string sceneIdEntries = string.Join("\n", sceneEntries.Select(s => $"        {s.enumName} = {s.enumValue},"));
        string buildIndexEntries = string.Join(",\n", scenesInBuild.Select(scene =>
        {
            var info = sceneDictionary[scene];
            var sceneName = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(scene));
            return $"        (SceneId.{info.enumName}, \"{sceneName}\")";
        }));
        
        string generatedCode = SCRIPT_TEMPLATE.Replace("SCENE_ID_ENTRIES", sceneIdEntries).Replace("BUILD_INDEX_ENTRIES", buildIndexEntries);
        File.WriteAllText(scriptPath, generatedCode);
        AssetDatabase.Refresh();

        // Persist updated list
        indexData.sceneEntries = sceneEntries;
        EditorUtility.SetDirty(indexData);
        AssetDatabase.SaveAssets();
        Debug.Log("Scene Index Updated.");
    }
}
