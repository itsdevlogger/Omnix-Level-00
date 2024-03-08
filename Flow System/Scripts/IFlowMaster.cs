using System.Collections.Generic;

namespace Omnix.Flow
{
    public interface IFlowMaster
    {
        public int ChildCount { get; }
        public IEnumerable<BaseFlowStep> Children { get; }
        public BaseFlowStep this[int index] { get; }
        public void ForceEnd(BaseFlowStep step);
        #if UNITY_EDITOR
        internal void RefreshFlow();
        #endif
    }
}