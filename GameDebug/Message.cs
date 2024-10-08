﻿using System.Linq;
using UnityEngine;

namespace DebugToScreen
{
    public class Message : IGameLog
    {
        private string text;
        private int linesCount;

        public string Text
        {
            get => text;
            set
            {
                text = value;
                linesCount = text.Count(c => c.Equals('\n')) + 1;
            }
        }

        public int Priority { get; set; }
        public float LinesCount => linesCount;

        public Message(string text)
        {
            this.Text = text;
        }

        public virtual void DrawSelf(Rect rect)
        {
            GUI.Label(rect, text, Styles.MessageStyle);
        }
    }
}