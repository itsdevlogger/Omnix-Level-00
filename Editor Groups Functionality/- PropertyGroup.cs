namespace MenuManagement.Base
{
    public static class _
    {
        public const string Events = "Events";
        public const string Unsorted = "Unsorted | Properties that don't belong to any groups";
        public const string ReadOnly = "Read Only | Should not changed these properties in the inspector";
        public const string AudioClips = "Audio | Use transitions & delay to perfectily time the laoding/unloading and sound";
        public const string EndPoints = "End Points | Define Start and End conditions for transtion";
        public const string RuntimeConstants = "Runtime Constants | Should not change these properties in Play Mode";
        public const string ConnectedObjects = "Connected Objects";
        public const string DynamicMenuProperties = "Settings (Runtime Constants) | Should not change these properties in Play Mode";
        public const string BaseTransitionProperties = "Costumise";
        public const string TransitionEditorVisualizer = "Test in Editor | (In preview, might be buggy)";

        public static void SplitName(string name, out string title, out string tooltip)
        {
            int splitter = name.LastIndexOf('|');
            if (splitter == -1 || splitter == name.Length - 1)
            {
                title = name;
                tooltip = "";
            }
            else
            {
                title = name.Substring(0, splitter).TrimEnd();
                tooltip = name.Substring(splitter, name.Length - splitter).TrimStart();
            }
        }
    }
}