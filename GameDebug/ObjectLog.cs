using System.Linq;
using UnityEngine;

namespace DebugToScreen
{
    public class ObjectLog : IGameLog
    {
        private string text;
        private float linesCount;

        public int Priority { get; set; }
        public float LinesCount => linesCount;
        
        public ObjectLog()
        {
            this.text = "";
            this.linesCount = 0f;
        }

        public void DrawSelf(Rect rect) => GUI.Label(rect, text, Styles.ObjectLogStyle);

        public void AddText(string value)
        {
            this.text += value;
            linesCount += value.Count(c => c.Equals('\n'));
        }

        public void SetText(string txt)
        {
            this.text = txt;
            this.linesCount = text.Count(((char c) => (c == '\n')));
        }
    }
}