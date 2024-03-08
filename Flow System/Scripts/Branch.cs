using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Omnix.Flow
{
    public class Branch : BaseFlowStep, IFlowMaster
    {
        [SerializeField] private List<BaseFlowStep> _choices;

        public IEnumerable<BaseFlowStep> Children => _choices;
        public BaseFlowStep this[int index] => _choices[index];
        public int ChildCount => _choices.Count;

        public BaseFlowStep Active { get; private set; }

        public override bool CanStart()
        {
            return _choices.Any(choice => choice.CanStart());
        }

        public override void Init(IFlowMaster master)
        {
            base.Init(master);
            foreach (BaseFlowStep step in _choices)
            {
                step.Init(this);
                step.enabled = false;
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            MakeChoice();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Active = null;
            foreach (BaseFlowStep choice in _choices)
            {
                choice.enabled = false;
            }
        }

        private void MakeChoice()
        {
            foreach (BaseFlowStep choice in _choices)
            {
                if (choice.CanStart())
                {
                    Active = choice;
                    choice.enabled = true;
                    return;
                }
            }

            Debug.LogWarning($"Cannot find a valid choice for {this.GetPath()}.");
            Ended();
        }

        public void ForceEnd(BaseFlowStep step)
        {
            if (Active == step)
            {
                step.enabled = false;
                Ended();
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
            _choices = new List<BaseFlowStep>();
            foreach (Transform child in transform)
            {
                if (child.TryGetComponent(out BaseFlowStep step))
                {
                    _choices.Add(step);
                    step.enabled = false;
                    step.gameObject.SetActive(true);
                }
            }
        }
        #endif
    }
}