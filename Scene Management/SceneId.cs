using System.Collections.Generic;

namespace Omnix.SceneManagement
{
    /// <summary> Edit this as per project </summary>
    public enum SceneId
    {
        Unknown = -10,
        Init = 0,
        LevelMaker = 10,
    }

    public static class BuildIndex
    {
        /// <summary> Put all the scenes in same order as build index. </summary>
        private static readonly (SceneId, string)[] BUILD_INDEX =
        {
          // Scene ID            Scene Asset Name
            (SceneId.Init,       "Init"),         // Scene at index 0 in Actual Build Index
            (SceneId.LevelMaker, "Level Maker"),  // Scene at index 1 in Actual Build Index
        };

        private static readonly Dictionary<SceneId, string> ID_TO_NAME;
        private static readonly Dictionary<string, SceneId> NAME_TO_ID;
        private static readonly Dictionary<SceneId, int> ID_TO_INDEX;

        static BuildIndex()
        {
            ID_TO_NAME = new Dictionary<SceneId, string>();
            ID_TO_INDEX = new Dictionary<SceneId, int>();
            NAME_TO_ID = new Dictionary<string, SceneId>();


            ID_TO_NAME.Add(SceneId.Unknown, "");
            ID_TO_INDEX.Add(SceneId.Unknown, -1);
            NAME_TO_ID.Add("", SceneId.Unknown);

            int index = 0;
            foreach (var key in BUILD_INDEX)
            {
                ID_TO_NAME.Add(key.Item1, key.Item2);
                NAME_TO_ID.Add(key.Item2, key.Item1);
                ID_TO_INDEX.Add(key.Item1, index);
                index++;
            }
        }

        /// <summary> Get the name of the scene with <see cref="SceneId"/> </summary>
        /// <remarks> Returns empty string if scene id is <see cref="SceneId.Unknown"/>, and null if its not present in <see cref="BUILD_INDEX"/> </remarks>
        public static string GetName(this SceneId sceneId)
        {
            return ID_TO_NAME[sceneId];
        }

        /// <summary> Gets the build index of the scene </summary>
        /// <remarks> Returns -1 if scene id is <see cref="SceneId.Unknown"/>, and -2 if its not present in <see cref="BUILD_INDEX"/> </remarks>
        public static int GetBuildIndex(this SceneId sceneId)
        {
            return ID_TO_INDEX[sceneId];
        }

        /// <summary> Gets the <see cref="SceneId"/> of scene with given name </summary>
        /// <remarks> Returns <see cref="SceneId.Unknown"/> if name not found in <see cref="BUILD_INDEX"/> </remarks>
        public static SceneId GetId(this string name)
        {
            if (NAME_TO_ID.TryGetValue(name, out SceneId id)) return id;
            return SceneId.Unknown;
        }

        /// <summary> Gets the <see cref="SceneId"/> of scene with given build index </summary>
        /// <remarks> Returns <see cref="SceneId.Unknown"/> if build-index is invalid </remarks>
        public static SceneId GetId(this int buildIndex)
        {
            if (buildIndex < 0 || buildIndex >= BUILD_INDEX.Length)
                return SceneId.Unknown;
            return BUILD_INDEX[buildIndex].Item1;
        }
    }
}