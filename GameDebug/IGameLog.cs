using UnityEngine;

namespace DebugToScreen
{
    public interface IGameLog
    {
        public int Priority { get; set; }
        public float LinesCount { get;}
        public void DrawSelf(Rect rect);
    }
}