using System;
using UnityEngine;

namespace AndroidInputs.Modifiers
{
    [Serializable]
    public class SmoothDamp : IInputModifier
    {
        public float smoothTime = 0.08f;
        public float maxSpeed = float.MaxValue;

        private float _velFloat;
        private Vector2 _vel3;

        public float Modify(float input) => Mathf.SmoothDamp(0f, input, ref _velFloat, smoothTime, maxSpeed);
        public Vector2 Modify(Vector2 input) => Vector2.SmoothDamp(Vector2.zero, input, ref _vel3, smoothTime, maxSpeed);
    }
}