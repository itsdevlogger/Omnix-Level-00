using System.Collections.Generic;
using UnityEngine;

namespace Omnix.Flow
{
    public class ParallelSubFlow : BaseFlowStep, IFlowMaster
    {
        [SerializeField] private List<BaseFlowStep> _flowSteps;
        private HashSet<BaseFlowStep> _endedSteps;
        private bool _subFlowStarted = false;

        public IEnumerable<BaseFlowStep> Children => _flowSteps;
        public BaseFlowStep this[int index] => _flowSteps[index];
        public int ChildCount => _flowSteps.Count;

        public override bool CanStart()
        {
            return !_subFlowStarted;
        }

        public override void Init(IFlowMaster master)
        {
            base.Init(master);
            foreach (BaseFlowStep step in _flowSteps)
            {
                step.Init(this);
                step.enabled = false;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _endedSteps = new HashSet<BaseFlowStep>();
            _subFlowStarted = true;
            foreach (BaseFlowStep step in _flowSteps)
            {
                if (step.CanStart())
                    step.enabled = true;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _subFlowStarted = false;
            _endedSteps = null;
            
            foreach (BaseFlowStep step in _flowSteps)
            {
                step.enabled = false;
            }
        }

        public void ForceEnd(BaseFlowStep step)
        {
            if (_flowSteps.Contains(step) == false)
            {
                Debug.LogError($"Cannot end step {step} (type: {step.GetType().FullName}).");
                return;
            }

            step.enabled = false;
            if (_endedSteps == null)
            {
                Ended();
                return;
            }
            
            _endedSteps.Add(step);
            if (_endedSteps.Count == _flowSteps.Count)
            {
                Ended();
            }
        }

        #if UNITY_EDITOR
        [ContextMenu("Refresh Flow (Editor Only)")]
        void IFlowMaster.RefreshFlow()
        {
            _flowSteps = new List<BaseFlowStep>();
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out BaseFlowStep step))
                {
                    _flowSteps.Add(step);
                    step.enabled = false;
                    step.gameObject.SetActive(true);
                }
            }
        }
        #endif
    }
}