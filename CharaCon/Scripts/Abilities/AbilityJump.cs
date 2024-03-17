using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Omnix.CharaCon.Abilities
{
    [DefaultAbilityIndex(-2)]
    [DefaultInputHandling(InputHandling.Start, InputHandling.Ignore, InputHandling.Ignore)]
    public class AbilityJump : BaseAbility
    {
        [Space] 
        public ForceMode forceMode;
        public float force = 50f;
        public float cooldownTime = 0.1f;
        
        protected override InputAction StartAction => InputMap.BasicAbilities.Jump;

        protected override void OnInputStarted(InputAction.CallbackContext obj)
        {
            if (enabled) return;
            
            base.OnInputStarted(obj);
            Agent.AddForce(Vector3.up * force, forceMode);
            StartCoroutine(Cooldown());
        }

        private IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(cooldownTime);
            enabled = false;
        }
    }
}