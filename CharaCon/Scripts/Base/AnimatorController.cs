using System;
using Omnix.CharaCon.Abilities;
using UnityEngine;

namespace Omnix.CharaCon
{
    [RequireComponent(typeof(AnimatorController))]
    public class AnimatorController : MonoBehaviour
    {
        [SerializeField] private Agent _agent;
        
        [Header("Parameters")]
        [SerializeField] private string _moveSpeedName;
        [SerializeField] private string _isGroundedName;
        [SerializeField] private string _isMovingName;
        [SerializeField] private string _abilityIndexName;
        
        private ApInt _abilityIndex;
        private ApFloat _moveSpeed;
        private ApBool _isGrounded;
        private ApBool _isMoving;

        private void Start()
        {
            var animator = GetComponent<Animator>();
            _abilityIndex = new ApInt(_abilityIndexName, animator);
            _moveSpeed = new ApFloat(_moveSpeedName, animator);
            _isGrounded = new ApBool(_isGroundedName, animator);
            _isMoving = new ApBool(_isMovingName, animator);
        }

        private void OnEnable()
        {
            BaseAbility.OnAbilityStart += OnAbilityStart;
            BaseAbility.OnAbilityStop += OnAbilityStop;
        }

        private void OnDisable()
        {
            BaseAbility.OnAbilityStart -= OnAbilityStart;
            BaseAbility.OnAbilityStop -= OnAbilityStop;
        }

        private void LateUpdate()
        {
            if (_agent.IsGrounded != _isGrounded) _isGrounded.Set(_agent.IsGrounded);
            if (_agent.IsMoving != _isMoving) _isMoving.Set(_agent.IsMoving);
            if (Math.Abs(_agent.CurrentSpeed - _moveSpeed) > 0.001f) _moveSpeed.Set(_agent.CurrentSpeed);
        }

        private void Reset()
        {
            _agent = GetComponent<Agent>();
        }
        
        private void OnAbilityStart(BaseAbility ability)
        {
            _abilityIndex.Set(ability.AbilityIndex);
        }
        
        private void OnAbilityStop(BaseAbility ability)
        {
            _abilityIndex.Set(0);
        }
    }
}