using System;
using UnityEngine;

namespace AndroidInputs.Modifiers
{
    [Serializable]
    public class SmoothDampAdvance : IInputModifier
    {
        private const float PI = Mathf.PI;
        [SerializeField] private float _frequency = 1f;
        [SerializeField] private float _damping = 0.5f;
        [SerializeField] private float _responce = 2f;

        // V2
        private Vector2 _lastInput2;
        private Vector2 _lastOutput2;
        private Vector2 _outputVel2;
        private float _lastTime2;
        
        // float
        private float _lastInput1;
        private float _lastOutput1;
        private float _outputVel1;
        private float _lastTime1;

        private float k1 => _damping / (PI * _frequency);
        private float k2 => 1f / ((2f * PI * _frequency) * (2f * PI * _frequency));
        private float k3 => _responce * _damping / (2f * PI * _frequency);

        public float Modify(float x)
        {
            float T = Time.time - _lastTime1;
            _lastTime1 = Time.time;

            var xd = (x - _lastInput1) / T;
            _lastInput1 = x;
            _lastOutput1 = _lastOutput1 + T * _outputVel1;
            _outputVel1 = _outputVel1 + T * (x + k3 * xd - _lastOutput1 - k1 * _outputVel1) / k2;
            return _lastOutput1;
        }

        public Vector2 Modify(Vector2 x)
        {
            float T = Time.time - _lastTime2;
            _lastTime2 = Time.time;

            var xd = (x - _lastInput2) / T;
            _lastInput2 = x;
            _lastOutput2 = _lastOutput2 + T * _outputVel2;
            _outputVel2 = _outputVel2 + T * (x + k3 * xd - _lastOutput2 - k1 * _outputVel2) / k2;
            return _lastOutput2;
        }
    }
}