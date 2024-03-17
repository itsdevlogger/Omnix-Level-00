using System;
using UnityEngine;


namespace Omnix.CharaCon
{
    [DefaultExecutionOrder(-100)]
    [RequireComponent(typeof(CharacterController))]
    public class Agent : MonoBehaviour
    {
        public static Agent LocalAgent { get; private set; }
        
        // @formatter:off
        [Header("Movement")] 
        [Tooltip(_.MOVE_SPEED)]                               public float moveSpeed = 2.0f;
        
        [Space, Header("Physics")] 
        [Tooltip(_.MASS)]                                     public float mass = -15.0f;
        [Tooltip(_.GRAVITY)]                                  public float gravity = -15.0f;
        [Tooltip(_.SPEED_CHANGE_RATE)]                        public float speedChangeRate = 10.0f;
        [Tooltip(_.ROTATION_SMOOTH_TIME)] [Range(0f, 0.3f)]   public float rotationSmoothTime = 0.12f;
        
        [Space, Header("Ground Check")] 
        [Tooltip(_.GROUND_LAYERS)]                            public LayerMask groundLayers;
        [Tooltip(_.GROUNDED_OFFSET)]                          public float groundOffset = -0.14f;
        [Tooltip(_.GROUNDED_RADIUS)]                          public float groundCheckRadius = 0.28f;
        // @formatter:on

        public bool IsMoving { get; private set; } = false;
        public bool IsGrounded { get; private set; } = true;
        public float CurrentSpeed { get; private set; }
        private CharacterController _controller;
        private Vector3 _currentVelocity = Vector3.zero;

        // intermediates (Used for calculations) 
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _terminalVelocity = 53.0f;

        public CharacterController Controller => _controller;
        public PlayerInputMap InputMap => AgentInput.Instance.InputMap;

        private void Awake()
        {
            LocalAgent = this;
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            Gravity();
            GroundedCheck();
            Move();
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (IsGrounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            Vector3 position = transform.position;
            position.y -= groundOffset;
            Gizmos.DrawSphere(position, groundCheckRadius);
        }

        private void GroundedCheck()
        {
            // set sphere position, with offset
            Vector3 position = transform.position;
            position.y -= groundOffset;
            IsGrounded = Physics.CheckSphere(position, groundCheckRadius, groundLayers, QueryTriggerInteraction.Ignore);
        }

        private void Move()
        {
            float targetSpeed;
            if (AgentInput.Instance.Move == Vector2.zero)
            {
                IsMoving = false;
                targetSpeed = 0.0f;
            }
            else
            {
                IsMoving = true;
                targetSpeed = moveSpeed;
            }

            // a reference to the players current horizontal velocity
            Vector3 controllerVelocity = _controller.velocity;
            float currentHorizontalSpeed = new Vector3(controllerVelocity.x, 0.0f, controllerVelocity.z).magnitude;
            float speedOffset = 0.1f;

            // accelerate or decelerate to target speed
            if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                CurrentSpeed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed, Time.deltaTime * speedChangeRate);
                CurrentSpeed = Mathf.Round(CurrentSpeed * 1000f) / 1000f;
            }
            else
            {
                CurrentSpeed = targetSpeed;
            }

            // if there is a move input rotate player when the player is moving
            if (AgentInput.Instance.Move != Vector2.zero)
            {
                Vector3 inputDirection = new Vector3(AgentInput.Instance.Move.x, 0.0f, AgentInput.Instance.Move.y).normalized;
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + AgentCamera.Current.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, rotationSmoothTime);
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }


            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
            targetDirection.Normalize();
            targetDirection *= CurrentSpeed;
            _controller.Move((targetDirection + _currentVelocity) * Time.deltaTime);
        }

        private void Gravity()
        {
            if (IsGrounded)
            {
                if (_currentVelocity.y < 0.0f)
                {
                    _currentVelocity.y = -2f;
                }
            }
            else if (_currentVelocity.y < _terminalVelocity)
            {
                _currentVelocity.y += gravity * Time.deltaTime;
            }
        }


        public void AddForce(Vector3 force, ForceMode forceMode = ForceMode.Force)
        {
            switch (forceMode)
            {
                case ForceMode.Force:
                    _currentVelocity += force * (Time.deltaTime / mass);
                    break;
                case ForceMode.Acceleration:
                    _currentVelocity += force * Time.deltaTime;
                    break;
                case ForceMode.Impulse:
                    _currentVelocity += force / mass;
                    break;
                case ForceMode.VelocityChange:
                    _currentVelocity += force;
                    break;
            }
        }
    }
}