using UnityEditor;
using UnityEngine;

namespace Omnix.Editor
{
    public class OmnixLayout
    {
        private Rect _inner;
        private Rect _totalRect;
        private int _horizontal;
        private bool _endHorizontal;
        private Vector2 _space;

        public OmnixLayout(Rect totalPosition, float xSpace = 0f, float ySpace = 0f)
        {
            _space = new Vector2(xSpace, ySpace);
            _horizontal = 0;
            _inner = totalPosition;
            _totalRect = new Rect(totalPosition);

            _inner.height = EditorGUIUtility.singleLineHeight;
            _inner.y -= _inner.height;
        }

        public void BeginHorizontal(int propertyCount, GUIContent label = null)
        {
            _inner.y += _inner.height;
            _horizontal = propertyCount;

            if (label == null || string.IsNullOrEmpty(label.text))
            {
                _inner.width = (_inner.width / propertyCount) - _space.x;
                _inner.x -= _inner.width;
            }
            else
            {
                _inner.width = EditorGUIUtility.labelWidth;
                EditorGUI.PrefixLabel(_inner, label);
                _inner.width = (_totalRect.width - _inner.width) / propertyCount;
                _inner.x += EditorGUIUtility.labelWidth - _inner.width;
            }
        }

        public void SpaceHorizontal(float pixels)
        {
            _inner.x += pixels;
            _inner.width -= pixels;
        }

        public void SpaceVertical(float linesCount)
        {
            _inner.y += linesCount * EditorGUIUtility.singleLineHeight;
        }

        public void Increment()
        {
            if (_horizontal > 0)
            {
                _inner.x += _inner.width + _space.x;
                _horizontal--;
                _endHorizontal = _horizontal == 0;
                return;
            }

            _inner.y += _inner.height + _space.y;
            if (_endHorizontal == false) return;

            _endHorizontal = false;
            _inner.x = _totalRect.x;
            _inner.width = _totalRect.width;
        }

        public Rect ControlledRect(float linesCount)
        {
            float height = linesCount * EditorGUIUtility.singleLineHeight;
            float innerHeight = _inner.height + _space.y;
            _inner.y += innerHeight;
            var output = new Rect(_inner)
            {
                height = height
            };
            _inner.y += height - innerHeight;
            return output;
        }

        public static implicit operator Rect(OmnixLayout p)
        {
            p.Increment();
            return p._inner;
        }
    }
}
