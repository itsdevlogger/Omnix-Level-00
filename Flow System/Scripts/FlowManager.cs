using System.Collections.Generic;
using UnityEngine;

namespace Omnix.Flow
{
    [DefaultExecutionOrder(-100)]
    public class FlowManager : MonoBehaviour, IFlowMaster
    {
        private static FlowManager instance;

        public static FlowManager Instance
        {
            get
            {
                if (instance == null) instance = new GameObject("FlowManager").AddComponent<FlowManager>();
                return instance;
            }
        }

        [SerializeField] private bool _startInAwake;
        [SerializeField] private List<BaseFlowStep> _flowSteps;

        public BaseFlowStep CurrentStep { get; private set; }
        public int CurrentStepIndex { get; private set; }

        public IEnumerable<BaseFlowStep> Children => _flowSteps;
        public BaseFlowStep this[int index] => _flowSteps[index];
        public int ChildCount => _flowSteps.Count;


        private void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("Multiple instances of Flow Manager.");
                Destroy(gameObject);
                return;
            }

            instance = this;
            Init();
            if (_startInAwake) StartFlow();
        }

        private void Init()
        {
            foreach (BaseFlowStep step in _flowSteps)
            {
                step.Init(this);
                step.enabled = false;
            }
        }

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
                Debug.Log("Reached end of the flow. Call StartFlow to start over");
                return;
            }

            CurrentStep = _flowSteps[CurrentStepIndex];
            if (CurrentStep.CanStart()) CurrentStep.enabled = true;
            else StartNextStep();
        }

        /// <summary> Starts the flow from the beginning </summary>
        public void StartFlow()
        {
            CurrentStepIndex = -1;
            StartNextStep();
        }

        /// <summary> Abrupt end the flow </summary>
        public void EndFlow()
        {
            if (CurrentStep) CurrentStep.enabled = false;
            CurrentStepIndex = _flowSteps.Count;
            CurrentStep = null;
        }

        /// <summary>
        /// End the current step in the master-flow.
        /// If using <see cref="SubFlow"/> or <see cref="ParallelSubFlow"/> or <see cref="Branch"/> don't call this method,
        /// Use <see cref="BaseFlowStep.Ended"/> instead
        /// </summary>
        public void ForceEnd(BaseFlowStep step)
        {
            Debug.Log($"FlowManager Ending: {step}");
            if (CurrentStep == step)
            {
                StartNextStep();
            }
            else
            {
                Debug.LogError($"Cannot end step {step} ({step.GetPath()}).");
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

            foreach (IFlowMaster master in GetComponentsInChildren<IFlowMaster>(true))
            {
                if (master is FlowManager) continue;
                
                master.RefreshFlow();
            }
        }
        #endif
    }
}