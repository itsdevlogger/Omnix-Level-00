using System.Collections.Generic;
using UnityEngine;

namespace Omnix.Flow
{
    public class SubFlow : BaseFlowStep, IFlowMaster
    {
        [SerializeField] private List<BaseFlowStep> _flowSteps;

        public BaseFlowStep CurrentStep { get; private set; }
        public int CurrentStepIndex { get; private set; }
        public bool SubFlowStarted { get; private set; }

        public IEnumerable<BaseFlowStep> Children => _flowSteps;
        public BaseFlowStep this[int index] => _flowSteps[index];
        public int ChildCount => _flowSteps.Count;

        /// <summary> End current step and start next step in the flow </summary>
        private void StartNextStep()
        {
            if (CurrentStep != null)
            {
                CurrentStep.enabled = false;
            }

            CurrentStepIndex++;
            if (CurrentStepIndex >= _flowSteps.Count)
            {
                CurrentStep = null;
                Ended();
                return;
            }

            CurrentStep = _flowSteps[CurrentStepIndex];
            if (CurrentStep.CanStart()) CurrentStep.enabled = true;
            else StartNextStep();
        }

        /// <summary> Starts the flow from the beginning </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            SubFlowStarted = true;
            CurrentStepIndex = -1;
            StartNextStep();
        }

        /// <summary> Abrupt end the flow </summary>
        protected override void OnDisable()
        {
            base.OnDisable();
            if (CurrentStep) CurrentStep.enabled = false;

            CurrentStep = null;
            CurrentStepIndex = -1;
            SubFlowStarted = false;
        }

        public override bool CanStart()
        {
            return !SubFlowStarted;
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

        /// <summary>
        /// End the current step in the master-flow.
        /// If using <see cref="SubFlow"/> or <see cref="ParallelSubFlow"/> or <see cref="Branch"/> don't call this method,
        /// Use <see cref="BaseFlowStep.Ended"/> instead
        /// </summary>
        public void ForceEnd(BaseFlowStep step)
        {
            if (CurrentStep == step)
            {
                StartNextStep();
            }
            else
            {
                Debug.LogError($"Cannot end step {step} (type: {step.GetType().FullName}).");
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