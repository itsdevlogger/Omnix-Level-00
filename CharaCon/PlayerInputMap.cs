//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Omnix/Level 00/CharaCon/PlayerInputMap.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Omnix.CharaCon
{
    public partial class @PlayerInputMap: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @PlayerInputMap()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputMap"",
    ""maps"": [
        {
            ""name"": ""Movement"",
            ""id"": ""fd46d50b-4216-41e9-94d7-ce3aff365cb8"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""80125db1-1f7e-4ca5-9113-209df342d097"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""13d97fbf-5fac-45ec-9bcf-8d615fb34e24"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""PassThrough"",
                    ""id"": ""516081b4-548d-4d3b-85a4-c4d3eff98220"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""da487359-d43f-40b0-9bc7-9ee438d1c7a5"",
                    ""path"": ""2DVector(mode=1)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""397c35ee-b628-45c8-af07-924c69923ad1"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""099c705d-10ac-4f32-8136-65b85dd6330f"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""e3aa9cbc-2445-4e30-b01f-cfe54930a5c9"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""51abd46b-d254-4179-b508-e06ed3f6a070"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""2cbdb40f-6544-4f68-be29-1578cf6a70a0"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""584ff710-caf3-463e-b970-2665872831ac"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""9781d4a2-e9d7-4380-bd8c-7f99112fddda"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""060b9fbf-4da1-4ba9-8945-7460823f4ed7"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""deee03d3-a4f6-4d9c-a338-d39ec5002e0b"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""320a3a18-cad4-486f-b16b-87487eca7872"",
                    ""path"": ""<Pointer>/delta"",
                    ""interactions"": """",
                    ""processors"": ""InvertVector2(invertX=false),ScaleVector2(x=0.05,y=0.05)"",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3ea787e7-dce4-4722-892c-3958dd3917e1"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""InvertVector2(invertX=false),StickDeadzone,ScaleVector2(x=300,y=300)"",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""da82ded9-5109-4bd5-8fc7-6c873419925d"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""40fe540c-3fdd-4317-adda-6acf6fff3ca4"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Basic Abilities"",
            ""id"": ""682e8066-6709-4322-b064-f17c7f0570f1"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""26892829-e3c2-4eb9-945a-fd1308a386b7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""2436740c-809c-44ec-92a8-f485dea61e80"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""30161546-abde-4d3f-a6f7-3452734f9c67"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6d728bd8-f460-4e35-afaa-017db4884b8a"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4f386b0c-4819-403c-8968-f821b36ba7fd"",
                    ""path"": ""<Keyboard>/ctrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // Movement
            m_Movement = asset.FindActionMap("Movement", throwIfNotFound: true);
            m_Movement_Move = m_Movement.FindAction("Move", throwIfNotFound: true);
            m_Movement_Look = m_Movement.FindAction("Look", throwIfNotFound: true);
            m_Movement_Sprint = m_Movement.FindAction("Sprint", throwIfNotFound: true);
            // Basic Abilities
            m_BasicAbilities = asset.FindActionMap("Basic Abilities", throwIfNotFound: true);
            m_BasicAbilities_Jump = m_BasicAbilities.FindAction("Jump", throwIfNotFound: true);
            m_BasicAbilities_Crouch = m_BasicAbilities.FindAction("Crouch", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }

        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // Movement
        private readonly InputActionMap m_Movement;
        private List<IMovementActions> m_MovementActionsCallbackInterfaces = new List<IMovementActions>();
        private readonly InputAction m_Movement_Move;
        private readonly InputAction m_Movement_Look;
        private readonly InputAction m_Movement_Sprint;
        public struct MovementActions
        {
            private @PlayerInputMap m_Wrapper;
            public MovementActions(@PlayerInputMap wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_Movement_Move;
            public InputAction @Look => m_Wrapper.m_Movement_Look;
            public InputAction @Sprint => m_Wrapper.m_Movement_Sprint;
            public InputActionMap Get() { return m_Wrapper.m_Movement; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(MovementActions set) { return set.Get(); }
            public void AddCallbacks(IMovementActions instance)
            {
                if (instance == null || m_Wrapper.m_MovementActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_MovementActionsCallbackInterfaces.Add(instance);
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Look.started += instance.OnLook;
                @Look.performed += instance.OnLook;
                @Look.canceled += instance.OnLook;
                @Sprint.started += instance.OnSprint;
                @Sprint.performed += instance.OnSprint;
                @Sprint.canceled += instance.OnSprint;
            }

            private void UnregisterCallbacks(IMovementActions instance)
            {
                @Move.started -= instance.OnMove;
                @Move.performed -= instance.OnMove;
                @Move.canceled -= instance.OnMove;
                @Look.started -= instance.OnLook;
                @Look.performed -= instance.OnLook;
                @Look.canceled -= instance.OnLook;
                @Sprint.started -= instance.OnSprint;
                @Sprint.performed -= instance.OnSprint;
                @Sprint.canceled -= instance.OnSprint;
            }

            public void RemoveCallbacks(IMovementActions instance)
            {
                if (m_Wrapper.m_MovementActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IMovementActions instance)
            {
                foreach (var item in m_Wrapper.m_MovementActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_MovementActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public MovementActions @Movement => new MovementActions(this);

        // Basic Abilities
        private readonly InputActionMap m_BasicAbilities;
        private List<IBasicAbilitiesActions> m_BasicAbilitiesActionsCallbackInterfaces = new List<IBasicAbilitiesActions>();
        private readonly InputAction m_BasicAbilities_Jump;
        private readonly InputAction m_BasicAbilities_Crouch;
        public struct BasicAbilitiesActions
        {
            private @PlayerInputMap m_Wrapper;
            public BasicAbilitiesActions(@PlayerInputMap wrapper) { m_Wrapper = wrapper; }
            public InputAction @Jump => m_Wrapper.m_BasicAbilities_Jump;
            public InputAction @Crouch => m_Wrapper.m_BasicAbilities_Crouch;
            public InputActionMap Get() { return m_Wrapper.m_BasicAbilities; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(BasicAbilitiesActions set) { return set.Get(); }
            public void AddCallbacks(IBasicAbilitiesActions instance)
            {
                if (instance == null || m_Wrapper.m_BasicAbilitiesActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_BasicAbilitiesActionsCallbackInterfaces.Add(instance);
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
            }

            private void UnregisterCallbacks(IBasicAbilitiesActions instance)
            {
                @Jump.started -= instance.OnJump;
                @Jump.performed -= instance.OnJump;
                @Jump.canceled -= instance.OnJump;
                @Crouch.started -= instance.OnCrouch;
                @Crouch.performed -= instance.OnCrouch;
                @Crouch.canceled -= instance.OnCrouch;
            }

            public void RemoveCallbacks(IBasicAbilitiesActions instance)
            {
                if (m_Wrapper.m_BasicAbilitiesActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IBasicAbilitiesActions instance)
            {
                foreach (var item in m_Wrapper.m_BasicAbilitiesActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_BasicAbilitiesActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public BasicAbilitiesActions @BasicAbilities => new BasicAbilitiesActions(this);
        public interface IMovementActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnLook(InputAction.CallbackContext context);
            void OnSprint(InputAction.CallbackContext context);
        }
        public interface IBasicAbilitiesActions
        {
            void OnJump(InputAction.CallbackContext context);
            void OnCrouch(InputAction.CallbackContext context);
        }
    }
}
