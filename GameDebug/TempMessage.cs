using UnityEngine;

namespace DebugToScreen
{
    public class TempMessage: Message
    {
        public string text;
        private float endTime;

        public TempMessage(string text, float duration) : base(text)
        {
            this.text = text;
            this.endTime = Time.time + duration;
        }

        public override void DrawSelf(Rect rect)
        {
            if (Time.time > endTime)
            {
                GameDebug.RemoveLog(this);
                return;
            }
            
            GUI.Label(rect, text, Styles.MessageStyle);
        }
    }
}