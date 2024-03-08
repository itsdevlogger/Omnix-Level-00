using System;
using System.Collections.Generic;
using Omnix.Utils;
using UnityEngine;

namespace Omnix.Examples
{
    public class OverlapConeExample : MonoBehaviour
    {
        private HashSet<Renderer> _renderers = new HashSet<Renderer>();

        [SerializeField] private Transform _cone;
        [SerializeField] private float _depth = 10f;
        [SerializeField] private float _baseRadius = 5f;

        [Header("Dont change this at runtime")] [SerializeField]
        private bool _useNonAlloc;

        [SerializeField] private int _maximumTargetsInCaseOfNonAlloc = 20;

        private Collider[] _coneHits;
        private int count;

        private void Start()
        {
            if (_useNonAlloc)
            {
                _coneHits = new Collider[_maximumTargetsInCaseOfNonAlloc];
            }
        }

        private void FixedUpdate()
        {
            _cone.localScale = new Vector3(_baseRadius, _baseRadius, _depth);
            foreach (Renderer rend in _renderers)
            {
                rend.enabled = false;
            }

            if (_useNonAlloc)
            {
                count = PhysicsUtils.OverlapCone2NonAlloc(transform.position, transform.forward, _depth, _baseRadius, _coneHits);
                if (count <= 0) return;
                for (int i = 0; i < count; i++)
                {
                    var rend = _coneHits[i].gameObject.GetComponent<Renderer>();
                    _renderers.Add(rend);
                    rend.enabled = true;
                }
            }
            else
            {
                _coneHits = PhysicsUtils.OverlapCone2(transform.position, transform.forward, _depth, _baseRadius);
                if (_coneHits.Length <= 0) return;
                for (int i = 0; i < _coneHits.Length; i++)
                {
                    var rend = _coneHits[i].gameObject.GetComponent<Renderer>();
                    _renderers.Add(rend);
                    rend.enabled = true;
                }
            }
        }

        private void OnDrawGizmos()
        {
            GizmosUtils.DrawCone(transform.position, transform.forward, _depth, _baseRadius, 20);
        }
    }
}