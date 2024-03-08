using System;

namespace MenuManagement.Base
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class GroupRuntimeConstant : Attribute
    {
        public readonly string[] properties;
        public GroupRuntimeConstant(params string[] properties)
        {
            this.properties = properties;
        }
    }
}