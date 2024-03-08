using System;
using System.Collections;
using UnityEngine;

namespace InteractionSystem
{
    /// <summary> Highlight an interactable by animating change in its transform. Untested. </summary>
    public class IndicatorTransformChange : IndicatorBase
    {
        private enum BlendMode
        {
            Ignore,
            OffsetLocal,
            OffsetGlobal,
            SetLocal,
            SetGlobal
        }

        private class Wrapper
        {
            public Vector3 minPos;
            public Vector3 maxPos;
            public Quaternion minRot;
            public Quaternion maxRot;
            public Vector3 minSca;
            public Vector3 maxSca;
            public Action<Vector3> posSetter;
            public Action<Quaternion> rotSetter;
            public Action<Vector3> scaSetter;
        }


        [SerializeField] private Vector3 position;
        [SerializeField] private BlendMode positionBlendMode;

        [SerializeField] private Vector3 rotation;
        [SerializeField] private BlendMode rotationBlendMode;

        [SerializeField] private Vector3 scale;
        [SerializeField] private BlendMode scaleBlendMode;

        [SerializeField] private float animationDuration;
        [SerializeField] private bool loopAnimation;

        private Vector3 _originalPosition;
        private Quaternion _originalRotation;
        private Vector3 _originalScale;
        private bool _animating = false;
        private bool _areObjectOriginalValuesSet = true;

        public override void Highlight(InteractableBase interactable, InteractionPlayer player)
        {
            StopAllCoroutines();
            Transform trans = interactable.transform;
            if (_areObjectOriginalValuesSet)
            {
                _originalPosition = trans.position;
                _originalRotation = trans.rotation;
                _originalScale = trans.localScale;
            }

            var args = new Wrapper();
            switch (positionBlendMode)
            {
                case BlendMode.Ignore:
                    args.minPos = Vector3.zero;
                    args.maxPos = Vector3.zero;
                    args.posSetter = _ => { };
                    break;
                case BlendMode.OffsetLocal:
                    args.minPos = trans.localPosition;
                    args.maxPos = args.minPos + position;
                    args.posSetter = v3 => trans.localPosition = v3;
                    break;
                case BlendMode.OffsetGlobal:
                    args.minPos = trans.position;
                    args.maxPos = args.minPos + position;
                    args.posSetter = v3 => trans.position = v3;
                    break;
                case BlendMode.SetLocal:
                    args.minPos = trans.localPosition;
                    args.maxPos = position;
                    args.posSetter = v3 => trans.localPosition = v3;
                    break;
                case BlendMode.SetGlobal:
                    args.minPos = trans.position;
                    args.maxPos = position;
                    args.posSetter = v3 => trans.position = v3;
                    break;
            }

            switch (rotationBlendMode)
            {
                case BlendMode.Ignore:
                    args.minRot = Quaternion.identity;
                    args.maxRot = Quaternion.identity;
                    args.rotSetter = _ => { };
                    break;
                case BlendMode.OffsetLocal:
                    args.minRot = trans.localRotation;
                    args.maxRot = Quaternion.Euler(Quaternion.Euler(rotation) * args.minRot.eulerAngles);
                    args.rotSetter = q => trans.localRotation = q;
                    break;
                case BlendMode.OffsetGlobal:
                    args.minRot = trans.rotation;
                    args.maxRot = Quaternion.Euler(Quaternion.Euler(rotation) * args.minRot.eulerAngles);
                    args.rotSetter = q => trans.rotation = q;
                    break;
                case BlendMode.SetLocal:
                    args.minRot = trans.localRotation;
                    args.maxRot = Quaternion.Euler(rotation);
                    args.rotSetter = q => trans.localRotation = q;
                    break;
                case BlendMode.SetGlobal:
                    args.minRot = trans.rotation;
                    args.maxRot = Quaternion.Euler(rotation);
                    args.rotSetter = q => trans.rotation = q;
                    break;
            }

            switch (scaleBlendMode)
            {
                case BlendMode.Ignore:
                    args.minSca = Vector3.zero;
                    args.maxSca = Vector3.zero;
                    args.scaSetter = _ => { };
                    break;
                case BlendMode.OffsetLocal:
                case BlendMode.OffsetGlobal:
                    args.minSca = trans.localScale;
                    args.maxSca = args.minSca + scale;
                    args.scaSetter = v3 => trans.localScale = v3;
                    break;
                case BlendMode.SetLocal:
                case BlendMode.SetGlobal:
                    args.minSca = trans.localScale;
                    args.maxSca = scale;
                    args.scaSetter = v3 => trans.localScale = v3;
                    break;
            }

            StartCoroutine(Animator(args));
        }

        public override void Unhighlight(InteractableBase interactable, InteractionPlayer player)
        {
            StopAllCoroutines();
            _animating = false;
            StartCoroutine(Animator(interactable.transform));
        }
        
        private IEnumerator Animator(Wrapper vals)
        {
            _areObjectOriginalValuesSet = false;
            if (loopAnimation == false)
            {
                foreach (object obj in ForwardPass(vals, animationDuration))
                {
                    yield return obj;
                }

                yield break;
            }


            float duration = animationDuration * 0.5f;
            while (_animating)
            {
                foreach (var obj in ForwardPass(vals, duration))
                {
                    yield return obj;
                }

                foreach (var obj in ReversePass(vals, duration))
                {
                    yield return obj;
                }
            }
        }

        private IEnumerator Animator(Transform trans)
        {
            float timeElapsed = 0;

            Vector3 minPos = trans.position;
            Quaternion minRot= trans.rotation;
            Vector3 minSca = trans.localScale;

            while (timeElapsed < 0.25f)
            {
                timeElapsed += Time.deltaTime;
                float tValue = timeElapsed * 4f;
                trans.position = Vector3.Lerp(minPos, _originalPosition, tValue);
                trans.rotation = Quaternion.Lerp(minRot, _originalRotation, tValue);
                trans.localScale = Vector3.Lerp(minSca, _originalScale, tValue);
                yield return null;
            }

            trans.position = _originalPosition;
            trans.rotation = _originalRotation;
            trans.localScale = _originalScale;
            _areObjectOriginalValuesSet = true;
        }

        private IEnumerable ForwardPass(Wrapper vals, float duration)
        {
            float timeElapsed = 0f;
            while (timeElapsed < duration)
            {
                timeElapsed += Time.deltaTime;
                float tVal = timeElapsed % duration;
                vals.posSetter(Vector3.Lerp(vals.minPos, vals.maxPos, tVal));
                vals.rotSetter(Quaternion.Lerp(vals.minRot, vals.maxRot, tVal));
                vals.scaSetter(Vector3.Lerp(vals.minSca, vals.maxSca, tVal));
                yield return null;
            }
        }

        private IEnumerable ReversePass(Wrapper vals, float duration)
        {
            float timeElapsed = duration;
            while (timeElapsed > 0f)
            {
                timeElapsed -= Time.deltaTime;
                float tVal = timeElapsed % duration;
                vals.posSetter(Vector3.Lerp(vals.minPos, vals.maxPos, tVal));
                vals.rotSetter(Quaternion.Lerp(vals.minRot, vals.maxRot, tVal));
                vals.scaSetter(Vector3.Lerp(vals.minSca, vals.maxSca, tVal));
                yield return null;
            }
        }
    }
}