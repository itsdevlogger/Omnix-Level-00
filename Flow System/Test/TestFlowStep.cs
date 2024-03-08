using UnityEngine;

namespace Omnix.Flow.Tests
{
    public class TestFlowStep : BaseFlowStep
    {
        [SerializeField] private bool _canStart = true;
        
        public override bool CanStart()
        {
            return !enabled && _canStart;
        }
    }
}