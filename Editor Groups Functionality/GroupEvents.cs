using System;

namespace MenuManagement.Base
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class GroupEvents : Attribute
    {
        public readonly string[] properties;
        public GroupEvents(params string[] properties)
        {
            this.properties = properties;
        }
    }
}