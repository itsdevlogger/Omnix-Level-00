using MenuManagement.Base;
using UnityEngine;

namespace MenuManagement.Editor
{
    public class GD_ReadOnly : BasePropertyGroupDrawer
    {
        public GD_ReadOnly() : base(_.ReadOnly)
        {
        }

        protected override void DrawGroup()
        {
            bool enabled = GUI.enabled;
            GUI.enabled = false;
            base.DrawGroup();
            GUI.enabled = enabled;
        }
    }
}