using System;

namespace MenuManagement.Base
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class GroupProperties : Attribute
    {
        public readonly string groupName;
        public readonly string tooltip;
        public readonly string[] properties;

        public GroupProperties(string nameAndContent, params string[] properties)
        {
            this.properties = properties;
            _.SplitName(nameAndContent, out groupName, out tooltip);
        }
    }
}