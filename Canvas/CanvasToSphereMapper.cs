using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Omnix.Monos
{
    /// <summary> Maps canvas position to position on edge of a sphere in world position </summary>
    public class CanvasToSphereMapper : MonoBehaviour
    {
        // @formatter:off
        private enum Quadrant { Positive, Negative }

        [Header("Canvas Bonds")] 
        [SerializeField] private Vector2 _canvasMin;
        [SerializeField] private Vector2 _canvasMax;

        [Header("World Space")] 
        [SerializeField] private Transform _target;
        [SerializeField] private Transform _circleCenter;
        [SerializeField] private float _circleRadius;
        [SerializeField] private Quadrant _zQuadrant;
        [SerializeField] private Vector2 _minAngle;
        [SerializeField] private Vector2 _maxAngle;

        [Header("Controls")] 
        [SerializeField] private bool _lookAtCenter;
        [SerializeField] private float _reactSpeed;
        [SerializeField, Range(0.01f, 0.51f)] private float _visualsResolution = 0.1f;
        
        private Vector2 _currentPos = new Vector2(0.5f, 0.5f);
        private Vector2 _targetPos = new Vector2(0.5f, 0.5f);
        // @formatter:on

        
        private void LateUpdate()
        {
            if (Vector2.Distance(_currentPos, _targetPos) > 0.1f)
            {
                _currentPos = Vector2.Lerp(_currentPos, _targetPos, Time.deltaTime * _reactSpeed);
                UpdatePosition();
            }
        }
        
        private static float ConvertRange(float value, float oldMin, float oldMax, float newMin, float newMax)
        {
            if (Math.Abs(oldMin - oldMax) < 0.001f) return value;
            return ((value - oldMin) * (newMax - newMin) / (oldMax - oldMin)) + newMin;
        }

        private Vector3 CanvasToWorldPoint(float currentX, float currentY)
        {
            Vector3 center = _circleCenter.position;

            float xAngle = ConvertRange(currentX, _canvasMin.x, _canvasMax.x, _minAngle.x, _maxAngle.x) * Mathf.Deg2Rad;
            float xDif = _circleRadius * Mathf.Cos(xAngle);

            float yAngle = ConvertRange(currentY, _canvasMin.y, _canvasMax.y, _minAngle.y, _maxAngle.y) * Mathf.Deg2Rad;
            float yDif = _circleRadius * Mathf.Cos(yAngle);

            float zDif = _circleRadius * _circleRadius - xDif * xDif - yDif * yDif;
            if (zDif < 0) zDif = 0;
            float zWorld = _zQuadrant switch
            {
                Quadrant.Positive => center.z + Mathf.Sqrt(zDif),
                Quadrant.Negative => center.z - Mathf.Sqrt(zDif),
                _ => throw new ArgumentOutOfRangeException()
            };
            return new Vector3(center.x - xDif, center.y - yDif, zWorld);
        }

        private void UpdatePosition()
        {
            _target.position = CanvasToWorldPoint(_currentPos.x, _currentPos.y);
            if (_lookAtCenter) _target.LookAt(_circleCenter.position);
        }

        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (_circleCenter == null || _target == null) return;
            
            
            bool isNotSelected = Selection.activeGameObject != gameObject && Selection.activeTransform != _target && Selection.activeTransform != _circleCenter;
            if (isNotSelected || _visualsResolution < 0.01f || _visualsResolution > 0.5f) return;

            Vector3 lastPoint;
            Gizmos.color = Color.blue;
            for (float i = 0f; i <= 1f; i += _visualsResolution)
            {
                lastPoint = CanvasToWorldPoint(0f, i);
                for (float j = _visualsResolution; j <= 1f; j += _visualsResolution)
                {
                    Vector3 currentPoint = CanvasToWorldPoint(j, i);
                    Gizmos.DrawLine(lastPoint, currentPoint);
                    lastPoint = currentPoint;
                }
            }

            Gizmos.color = Color.green;
            for (float i = 0f; i <= 1f; i += _visualsResolution)
            {
                lastPoint = CanvasToWorldPoint(i, 0f);
                for (float j = _visualsResolution; j <= 1f; j += _visualsResolution)
                {
                    Vector3 currentPoint = CanvasToWorldPoint(i, j);
                    Gizmos.DrawLine(lastPoint, currentPoint);
                    lastPoint = currentPoint;
                }
            }

            Gizmos.color = Color.white;
        }
        #endif
        
        public void SetCircleRadius(float r)
        {
            _circleRadius = r;
            UpdatePosition();
        }

        public void SetX(float x)
        {
            _targetPos.x = x;
        }

        public void SetY(float y)
        {
            _targetPos.y = y;
        }
    }
}