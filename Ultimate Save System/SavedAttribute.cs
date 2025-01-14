using System;

namespace UltimateSaveSystem
{
    [AttributeUsage(AttributeTargets.Field)]
    public class SavedAttribute : Attribute
    {
        public string globalId;
        
        /// <summary>
        /// Constructor to be used for locally stored variable.
        /// Local variables are stored with specific GameObject.
        /// </summary>
        public SavedAttribute()
        {
        }
        
        /// <summary>
        /// Constructor to be used for globally stored variable.
        /// Global variables are associated with saved file rather than to any specific GameObject.
        /// </summary>
        public SavedAttribute(string globalId = null)
        {
            this.globalId = globalId;
        }
    }
}