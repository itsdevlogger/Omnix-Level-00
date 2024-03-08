using System;

namespace MenuManagement.Base
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class GroupReadOnly : Attribute
    {
        public readonly string[] properties;
        public GroupReadOnly(params string[] properties)
        {
            this.properties = properties;
        }
    }
}