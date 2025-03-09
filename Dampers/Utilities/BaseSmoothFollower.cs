using System;
using UnityEngine;

namespace Omnix.Damping.Utils
{
    [Serializable]
    public struct Source
    {
        public Transform source;
        public Vector3 offset;

        [Range(-1f, 1f)] public float influence;
    }

    public abstract class BaseSmoothFollower : MonoBehaviour
    {
        [SerializeField] private Source[] _sources;
        [SerializeField] private bool _followX = true;
        [SerializeField] private bool _followY = true;
        [SerializeField] private bool _followZ = true;
        [SerializeField] private int _skipFrames = 0;
        [SerializeField] private Vector3Damper _damper = new Vector3Damper(1f, 1f, 0f);

        private int _frameCount = 0;
        private float _deltaTime = 0f;

        protected abstract Vector3 GetValue(Transform source);
        protected abstract void SetValue(Vector3 value);

        private void OnEnable()
        {
            _damper.Init(GetValue(transform));
        }

        private void LateUpdate()
        {
            if (_sources.Length == 0)
            {
                enabled = false;
                return;
            }

            if (_skipFrames > 0 && _frameCount != _skipFrames)
            {
                _deltaTime += Time.deltaTime;
                _frameCount++;
                return;
            }

            _frameCount = 0;
            Vector3 accumulatedPosition = Vector3.zero;
            float totalInfluence = 0f;

            foreach (var source in _sources)
            {
                if (source.source == null)
                    continue;

                Vector3 influencedPosition = GetValue(source.source) + source.offset;
                accumulatedPosition += influencedPosition * source.influence;
                totalInfluence += source.influence;
            }

            if (Mathf.Approximately(totalInfluence, 0f))
            {
                _deltaTime = 0f;
                return;
            }

            Vector3 weightedPosition = accumulatedPosition / totalInfluence;
            Vector3 desiredPosition = transform.position;

            if (_followX) desiredPosition.x = weightedPosition.x;
            if (_followY) desiredPosition.y = weightedPosition.y;
            if (_followZ) desiredPosition.z = weightedPosition.z;

            if (_skipFrames <= 0)
            {
                SetValue(_damper.Compute(desiredPosition));
            }
            else
            {
                SetValue(_damper.Compute(desiredPosition, _deltaTime));
                _deltaTime = 0f;
            }
        }
    }
}