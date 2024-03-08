using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Omnix.Flow.Tests
{
    public class FlowUiBuilder : MonoBehaviour
    {
        [SerializeField] private float _indentSpace;
        [SerializeField] private FlowItemPrefab _prefab;
        [SerializeField] private Transform _parent;

        private Dictionary<BaseFlowStep, FlowItemPrefab> _instances = new Dictionary<BaseFlowStep, FlowItemPrefab>();
        [SerializeField]private List<BaseFlowStep> _currentSteps;
        private int _currentIndentLevel;
        public float Indent => _currentIndentLevel * _indentSpace;

        private void Start()
        {
            DestroyAllInstances();
            foreach (BaseFlowStep step in GetChildrenRecursive(FlowManager.Instance, 0))
            {
                FlowItemPrefab instance = Instantiate(_prefab, _parent);
                instance.Setup(step, this);
                _instances.Add(step, instance);
            }
            
            BaseFlowStep.OnStepStart += OnStepStart;
            BaseFlowStep.OnStepEnd += OnStepEnd;
        }

        private void OnDestroy()
        {
            BaseFlowStep.OnStepStart -= OnStepStart;
            BaseFlowStep.OnStepEnd -= OnStepEnd;
        }

        public void Refresh()
        {
            DestroyAllInstances();
            if (_currentSteps != null) _currentSteps.Clear();
            Start();
        }
        
        private void DestroyAllInstances()
        {
            if (_instances.Count == 0) return;
            foreach (KeyValuePair<BaseFlowStep, FlowItemPrefab> pair in _instances)
            {
                Destroy(pair.Value.gameObject);
            }

            _instances.Clear();
        }

        private IEnumerable<BaseFlowStep> GetChildrenRecursive(IFlowMaster target, int indent)
        {
            foreach (BaseFlowStep child in target.Children)
            {
                _currentIndentLevel = indent;
                yield return child;

                if (child is not IFlowMaster master) continue;

                foreach (BaseFlowStep data in GetChildrenRecursive(master, indent + 1))
                {
                    yield return data;
                }
            }
        }

        private void OnStepStart(BaseFlowStep step)
        {
            FlowItemPrefab ui = _instances.GetValueOrDefault(step);
            if (ui != null) ui.ToggleForFlowItem.isOn = true;
            if (step is not IFlowMaster) _currentSteps.Add(step);
        }

        private void OnStepEnd(BaseFlowStep step)
        {
            FlowItemPrefab ui = _instances.GetValueOrDefault(step);
            if (ui != null) ui.ToggleForFlowItem.isOn = false;
            _currentSteps.Remove(step);
        }

        public void EndCurrent()
        {
            if (_currentSteps == null || _currentSteps.Count == 0)
            {
                _currentSteps = new List<BaseFlowStep>();
                FlowManager.Instance.StartFlow();
                return;
            }

            foreach (BaseFlowStep step in _currentSteps.ToList())
            {
                Debug.Log($"Ending step: {step} with master: {step.Master}");
                step.Ended();
            }
        }
    }
}