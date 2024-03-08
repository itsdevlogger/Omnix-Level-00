using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Omnix.Flow
{
    /// <summary>
    /// Base class for one step in flow.
    /// This component will be active only when this flow step is being executed.
    /// Call <see cref="Ended"/> Inform the system when this step ends.
    /// </summary>
    public abstract class BaseFlowStep : MonoBehaviour
    {
        public static event Action<BaseFlowStep> OnStepStart; 
        public static event Action<BaseFlowStep> OnStepEnd; 

        public IFlowMaster Master { get; private set; }
        [field: SerializeField] public string ID { get; private set; }

        public virtual void Init(IFlowMaster master) => Master = master;
        protected virtual void OnEnable() => OnStepStart?.Invoke(this);
        protected virtual void OnDisable() => OnStepEnd?.Invoke(this);

        protected virtual void Reset()
        {
            #if UNITY_EDITOR
            ID = GUID.Generate().ToString();
            #endif
        }


        /// <summary> Check if the step can be started at this frame </summary>
        public abstract bool CanStart();

        /// <summary> Inform system that this step has ended. </summary>
        public void Ended()
        {
            if (Master == null)
            {
                Debug.LogError($"BrokenPipeline: {gameObject.name} ({this.GetPath()}) has no master." );
                return;
            }

            Master.ForceEnd(this);
        }
    }
}