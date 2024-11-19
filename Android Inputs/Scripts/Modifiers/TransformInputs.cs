using System;
using UnityEngine;

namespace AndroidInputs.Modifiers
{
    [Serializable]
    public class TransformInputs : IInputModifier
    {
        public AnimationCurve _curve;
        public float Modify(float input)
        {
            return _curve.Evaluate(input);
        }

        public Vector2 Modify(Vector2 input)
        {
            return new Vector2(_curve.Evaluate(input.x), _curve.Evaluate(input.y));
        }
    }
}