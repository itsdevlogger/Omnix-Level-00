using System;

namespace MenuManagement.Base
{
    [AttributeUsage(AttributeTargets.Method)]
    public class InspectorButton : Attribute
    {
        public readonly string text;

        public InspectorButton()
        {
        }

        public InspectorButton(string text)
        {
            this.text = text;
        }
    }
}