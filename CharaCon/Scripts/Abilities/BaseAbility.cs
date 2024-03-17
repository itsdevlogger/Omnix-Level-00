using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Omnix.CharaCon.Abilities
{
    public enum InputHandling
    {
        Ignore,
        Toggle,
        Start,
        Stop,
    }

    /// <remarks> set <see cref="BaseAbility.enabled"/> = true if the ability is being used, otherwise set <see cref="BaseAbility.enabled"/> = false </remarks>
    public abstract class BaseAbility : MonoBehaviour
    {
        public static event Action<BaseAbility> OnAbilityStart;
        public static event Action<BaseAbility> OnAbilityStop;

        // @formatter:off
        [Header("References")]
        [SerializeField]                                       private Agent _agent;
        [SerializeField]                                       private Animator _agentAnimator;
        
        [Space, Header("Settings")]
        [SerializeField, Tooltip(_.ABILITY_INDEX)]             private int _abilityIndex;
        [SerializeField, Tooltip(_.ABILITY_INPUT_START_HANDLING)] public InputHandling inputStartedHandling;
        [SerializeField, Tooltip(_.ABILITY_INPUT_START_HANDLING)] public InputHandling inputPerformedHandling;
        [SerializeField, Tooltip(_.ABILITY_INPUT_CANCEL_HANDLING)]   public InputHandling inputCanceledHandling;
        

        public int AbilityIndex               => _abilityIndex;
        public Agent Agent                    => _agent;
        public Camera MainCamera              => AgentCamera.Current;
        public Animator Animator              => _agentAnimator;
        public PlayerInputMap InputMap        => AgentInput.Instance.InputMap;
        public CharacterController Controller => _agent.Controller;
        // @formatter:on


        /// <returns> Input action that will start this ability </returns>
        [NotNull]
        protected abstract InputAction StartAction { get; }

        protected void HandleKeyEvent(InputHandling mode)
        {
            switch (mode)
            {
                case InputHandling.Ignore:
                    break;
                case InputHandling.Toggle:
                    enabled = !enabled;
                    break;
                case InputHandling.Start:
                    enabled = true;
                    break;
                case InputHandling.Stop:
                    enabled = false;
                    break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        // @formatter:off
        protected virtual void OnInputStarted(InputAction.CallbackContext obj) => HandleKeyEvent(inputStartedHandling);
        protected virtual void OnInputPerformed(InputAction.CallbackContext obj) => HandleKeyEvent(inputPerformedHandling);
        protected virtual void OnInputCanceled(InputAction.CallbackContext obj) => HandleKeyEvent(inputCanceledHandling);
        protected virtual void OnEnable() => OnAbilityStart?.Invoke(this);
        protected virtual void OnDisable() => OnAbilityStop?.Invoke(this);
        // @formatter:on

        protected virtual void Awake()
        {
            InputAction action = StartAction;
            action.started += OnInputStarted;
            action.performed += OnInputPerformed;
            action.canceled += OnInputCanceled;
            StartAction.Enable();
        }

        protected virtual void OnDestroy()
        {
            InputAction action = StartAction;
            action.started -= OnInputStarted;
            action.performed -= OnInputPerformed;
            action.canceled -= OnInputCanceled;
        }

        protected virtual void Reset()
        {
            _agent = GetComponentInParent<Agent>();
            if (_agent == null)
            {
                #if UNITY_EDITOR
                EditorUtility.DisplayDialog("Agent Not Found", "Ability must be added as a child of Agent.", "Okay");
                #else
                Debug.LogError("Ability must be added as a child of Agent.");
                #endif

                DestroyImmediate(this);
                return;
            }

            _agentAnimator = _agent.GetComponent<Animator>();
            enabled = false;

            foreach (var attribute in GetType().GetCustomAttributes(typeof(Attribute), true))
            {
                switch (attribute)
                {
                    case DefaultAbilityIndex abilityIndex:
                        _abilityIndex = abilityIndex.value;
                        break;
                    case DefaultInputHandling kdh:
                        inputStartedHandling = kdh.start;
                        inputPerformedHandling = kdh.performed;
                        inputCanceledHandling = kdh.canceled;
                        break;
                }
            }
        }
    }
}