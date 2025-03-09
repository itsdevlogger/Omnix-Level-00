using System;
using UnityEngine;

namespace Omnix.Damping
{
    [Serializable]
    public abstract class BaseDamper<TOut, TVel> : ISerializationCallbackReceiver
    {
        [SerializeField] private float _frequency = 2.0f;
        [SerializeField] private float _damping = 0.5f;
        [SerializeField] private float _initialResponse = 0.0f;

        public float Frequency
        {
            get => _frequency;
            set
            {
                _frequency = value;
                isDirty = true;
            }
        }

        public float Damping
        {
            get => _frequency;
            set
            {
                _damping = value;
                isDirty = true;
            }
        }

        public float InitialResponse
        {
            get => _frequency;
            set
            {
                _initialResponse = value;
                isDirty = true;
            }
        }

        [NonSerialized] protected bool isDirty = true;
        [NonSerialized] protected TOut lastTarget;
        [NonSerialized] protected TOut lastOut;
        [NonSerialized] protected TVel velocity;
        [NonSerialized] protected float k1;
        [NonSerialized] protected float k2;
        [NonSerialized] protected float k3;

        private Action<TOut, TVel, float> _computeFunction;

        protected abstract TVel GetVelocity(TOut target, float dt);
        protected abstract void Increment(TOut target, TVel targetVelocity, float dt, float k2Safe);

        protected BaseDamper()
        {
        }

        protected BaseDamper(float frequency, float damping, float initialResponse)
        {
            _frequency = frequency;
            _damping = damping;
            _initialResponse = initialResponse;
            isDirty = true;
        }

        protected void Refresh(TOut target)
        {
            // compute constants
            k1 = _damping / (Mathf.PI * _frequency);
            k2 = 1 / ((2 * Mathf.PI * _frequency) * (2 * Mathf.PI * _frequency));
            k3 = _initialResponse * _damping / (2 * Mathf.PI * _frequency);

            // initialize variables
            lastTarget = target;
            lastOut = target;
            velocity = default;
            isDirty = false;
        }

        public TOut Compute(TOut target)
        {
            TVel targetVel;
            if (isDirty)
            {
                Refresh(target);
                targetVel = default;
            }
            else
            {
                targetVel = GetVelocity(target, Time.deltaTime);
                lastTarget = target;
            }

            float dt = Time.deltaTime;
            float k2Safe = Mathf.Max(k2, (dt * dt + dt * k1) / 2f, dt * k1);
            Increment(target, targetVel, dt, k2Safe);
            return lastOut;
        }

        public TOut Compute(TOut target, float dt)
        {
            TVel targetVel;
            if (isDirty)
            {
                Refresh(target);
                targetVel = default;
            }
            else
            {
                targetVel = GetVelocity(target, dt);
                lastTarget = target;
            }

            float k2Safe = Mathf.Max(k2, (dt * dt + dt * k1) / 2f, dt * k1);
            Increment(target, targetVel, dt, k2Safe);
            return lastOut;
        }

        public void Init(TOut initialPosition)
        {
            Refresh(initialPosition);
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
#if UNITY_EDITOR
            isDirty = true;
#endif
        }
    }
}