using MenuManagement.Base;
using UnityEngine;

namespace MenuManagement.Editor
{
    public class GD_RuntimeConstant : BasePropertyGroupDrawer
    {
        public GD_RuntimeConstant() : base(_.RuntimeConstants)
        {
        }

        protected override void DrawGroup()
        {
            bool enabled = GUI.enabled;
            GUI.enabled = !Application.isPlaying;
            base.DrawGroup();
            GUI.enabled = enabled;
        }
    }
}