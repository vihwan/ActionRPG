// GENERATED AUTOMATICALLY FROM 'Assets/2_Scripts/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Player Movement"",
            ""id"": ""a6a24b1d-329b-4ee2-bc34-48d470a21f81"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""db429b9f-4835-40b0-9faa-66e1000da784"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Camera"",
                    ""type"": ""PassThrough"",
                    ""id"": ""0f2ee329-eba1-4d49-aa3b-59c209e9b957"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Lock On Target Left"",
                    ""type"": ""Button"",
                    ""id"": ""371bd518-59c8-4df9-ae99-bc5583cf78fa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Lock On Target Right"",
                    ""type"": ""Button"",
                    ""id"": ""e2c48f33-69f8-4ec7-b74f-6d035341910c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""e7d98aff-b6da-4d93-a4f7-cd6488ec4add"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""8f4c02ce-4c3f-48d4-917a-6d6de68c614b"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""6f5a9c7f-6e55-43b0-805f-ee2cbb2d5b57"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""ed2305d8-e18d-4a1a-9bd4-7ac797e0c9af"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""20a6ded0-e064-49ca-a129-a38150a06829"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""0a140d21-8700-4b7e-b868-dccc069cf22c"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": ""StickDeadzone"",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ae20f26b-67f0-4a5c-b265-71ca25cc48a9"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": ""NormalizeVector2"",
                    ""groups"": """",
                    ""action"": ""Camera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c79e1937-e4c1-44e6-9c10-4e191fc745a1"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lock On Target Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b7d69698-ceba-40a6-be24-da19eefbe5db"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lock On Target Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5381f432-8cab-4919-91c8-7f59afb2ad32"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lock On Target Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""08262a67-66c1-47ef-9624-db97f87dacac"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lock On Target Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Player Actions"",
            ""id"": ""d9687ba5-f723-4607-8852-24979e6031a5"",
            ""actions"": [
                {
                    ""name"": ""Roll"",
                    ""type"": ""Button"",
                    ""id"": ""4577629e-9cdb-4d25-aed0-70dd4e105018"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LT"",
                    ""type"": ""Button"",
                    ""id"": ""268aaebf-7263-4416-8794-3b5c119b26b0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RB"",
                    ""type"": ""Button"",
                    ""id"": ""485403d8-8ae2-4749-bcfc-0071a901a8fa"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RT"",
                    ""type"": ""Button"",
                    ""id"": ""47dd315a-b0a1-4253-b5a7-30e2a1fdf96c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LockOn"",
                    ""type"": ""Button"",
                    ""id"": ""5a5d5a47-333f-40ed-8b5d-7394b0b2dff6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Skill_One"",
                    ""type"": ""Button"",
                    ""id"": ""4ca1f6bb-00b6-403b-b257-8834e21008a2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Skill_Two"",
                    ""type"": ""Button"",
                    ""id"": ""7d865a7f-af49-468c-8cbf-d759f98e0f3e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Skill_Three"",
                    ""type"": ""Button"",
                    ""id"": ""5d43ae3b-1767-41c8-91d4-5b6893bd31e3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Skill_Ult"",
                    ""type"": ""Button"",
                    ""id"": ""cf53e0fe-c96e-4fc2-bd00-3920edf56a44"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""A"",
                    ""type"": ""Button"",
                    ""id"": ""43426405-26a1-4fb0-b378-c1feaf6cc291"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""eec03dfd-4d63-43ac-aad3-68cd8f9f3df6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Menu"",
                    ""type"": ""Button"",
                    ""id"": ""04dce8a8-638c-4823-b1fb-16c5e5591107"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ConsumeItem"",
                    ""type"": ""Button"",
                    ""id"": ""52f37016-a1f4-4e29-aae2-0bd92691aa71"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0fccce27-aab8-4cbd-8497-bbfc8e991ec7"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d74fb747-cbb6-4bf1-9f06-c8025365fdf4"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Roll"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e0a84c41-661a-42b7-b13e-53e66df70c24"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RB"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""442d1678-9cfa-4c42-a93f-c3c393887b30"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RT"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7bed2927-e444-48d0-9e8c-e75f88b7fc86"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Skill_One"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""70962777-5cd3-4ebe-af14-33f9e9d7a3ed"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Skill_Two"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""be417b77-9d9e-46ff-b14e-72dfb3685982"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Skill_Three"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e6b035fc-336b-4e23-9b44-095efea1ce46"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Skill_Ult"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c39c9521-f98e-442f-b157-2df53e2309fb"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""A"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f5827ff9-85e2-40e1-a808-cc29574d4695"",
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
                    ""id"": ""ef3460a8-0edd-4b5a-93a7-d193bed71946"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4db0003a-be7f-439a-9e28-a16f134db62d"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Menu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7635e439-06bc-4028-9fd2-a8005c4cbb95"",
                    ""path"": ""<Gamepad>/rightStickPress"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LockOn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""433bb33d-c565-4922-8604-3d26d9c6bea8"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LockOn"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""63254c5e-70c7-4aa3-bdc8-7de53067a8a8"",
                    ""path"": ""<Keyboard>/c"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ConsumeItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e0802989-0334-4622-87e1-2d821c9ed9f8"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LT"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Player UI"",
            ""id"": ""40133e4e-f292-46d4-8572-04bb1b28a924"",
            ""actions"": [],
            ""bindings"": []
        }
    ],
    ""controlSchemes"": []
}");
        // Player Movement
        m_PlayerMovement = asset.FindActionMap("Player Movement", throwIfNotFound: true);
        m_PlayerMovement_Movement = m_PlayerMovement.FindAction("Movement", throwIfNotFound: true);
        m_PlayerMovement_Camera = m_PlayerMovement.FindAction("Camera", throwIfNotFound: true);
        m_PlayerMovement_LockOnTargetLeft = m_PlayerMovement.FindAction("Lock On Target Left", throwIfNotFound: true);
        m_PlayerMovement_LockOnTargetRight = m_PlayerMovement.FindAction("Lock On Target Right", throwIfNotFound: true);
        // Player Actions
        m_PlayerActions = asset.FindActionMap("Player Actions", throwIfNotFound: true);
        m_PlayerActions_Roll = m_PlayerActions.FindAction("Roll", throwIfNotFound: true);
        m_PlayerActions_LT = m_PlayerActions.FindAction("LT", throwIfNotFound: true);
        m_PlayerActions_RB = m_PlayerActions.FindAction("RB", throwIfNotFound: true);
        m_PlayerActions_RT = m_PlayerActions.FindAction("RT", throwIfNotFound: true);
        m_PlayerActions_LockOn = m_PlayerActions.FindAction("LockOn", throwIfNotFound: true);
        m_PlayerActions_Skill_One = m_PlayerActions.FindAction("Skill_One", throwIfNotFound: true);
        m_PlayerActions_Skill_Two = m_PlayerActions.FindAction("Skill_Two", throwIfNotFound: true);
        m_PlayerActions_Skill_Three = m_PlayerActions.FindAction("Skill_Three", throwIfNotFound: true);
        m_PlayerActions_Skill_Ult = m_PlayerActions.FindAction("Skill_Ult", throwIfNotFound: true);
        m_PlayerActions_A = m_PlayerActions.FindAction("A", throwIfNotFound: true);
        m_PlayerActions_Jump = m_PlayerActions.FindAction("Jump", throwIfNotFound: true);
        m_PlayerActions_Menu = m_PlayerActions.FindAction("Menu", throwIfNotFound: true);
        m_PlayerActions_ConsumeItem = m_PlayerActions.FindAction("ConsumeItem", throwIfNotFound: true);
        // Player UI
        m_PlayerUI = asset.FindActionMap("Player UI", throwIfNotFound: true);
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

    // Player Movement
    private readonly InputActionMap m_PlayerMovement;
    private IPlayerMovementActions m_PlayerMovementActionsCallbackInterface;
    private readonly InputAction m_PlayerMovement_Movement;
    private readonly InputAction m_PlayerMovement_Camera;
    private readonly InputAction m_PlayerMovement_LockOnTargetLeft;
    private readonly InputAction m_PlayerMovement_LockOnTargetRight;
    public struct PlayerMovementActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerMovementActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_PlayerMovement_Movement;
        public InputAction @Camera => m_Wrapper.m_PlayerMovement_Camera;
        public InputAction @LockOnTargetLeft => m_Wrapper.m_PlayerMovement_LockOnTargetLeft;
        public InputAction @LockOnTargetRight => m_Wrapper.m_PlayerMovement_LockOnTargetRight;
        public InputActionMap Get() { return m_Wrapper.m_PlayerMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerMovementActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerMovementActions instance)
        {
            if (m_Wrapper.m_PlayerMovementActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovement;
                @Camera.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCamera;
                @Camera.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCamera;
                @Camera.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCamera;
                @LockOnTargetLeft.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnLockOnTargetLeft;
                @LockOnTargetLeft.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnLockOnTargetLeft;
                @LockOnTargetLeft.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnLockOnTargetLeft;
                @LockOnTargetRight.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnLockOnTargetRight;
                @LockOnTargetRight.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnLockOnTargetRight;
                @LockOnTargetRight.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnLockOnTargetRight;
            }
            m_Wrapper.m_PlayerMovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @Camera.started += instance.OnCamera;
                @Camera.performed += instance.OnCamera;
                @Camera.canceled += instance.OnCamera;
                @LockOnTargetLeft.started += instance.OnLockOnTargetLeft;
                @LockOnTargetLeft.performed += instance.OnLockOnTargetLeft;
                @LockOnTargetLeft.canceled += instance.OnLockOnTargetLeft;
                @LockOnTargetRight.started += instance.OnLockOnTargetRight;
                @LockOnTargetRight.performed += instance.OnLockOnTargetRight;
                @LockOnTargetRight.canceled += instance.OnLockOnTargetRight;
            }
        }
    }
    public PlayerMovementActions @PlayerMovement => new PlayerMovementActions(this);

    // Player Actions
    private readonly InputActionMap m_PlayerActions;
    private IPlayerActionsActions m_PlayerActionsActionsCallbackInterface;
    private readonly InputAction m_PlayerActions_Roll;
    private readonly InputAction m_PlayerActions_LT;
    private readonly InputAction m_PlayerActions_RB;
    private readonly InputAction m_PlayerActions_RT;
    private readonly InputAction m_PlayerActions_LockOn;
    private readonly InputAction m_PlayerActions_Skill_One;
    private readonly InputAction m_PlayerActions_Skill_Two;
    private readonly InputAction m_PlayerActions_Skill_Three;
    private readonly InputAction m_PlayerActions_Skill_Ult;
    private readonly InputAction m_PlayerActions_A;
    private readonly InputAction m_PlayerActions_Jump;
    private readonly InputAction m_PlayerActions_Menu;
    private readonly InputAction m_PlayerActions_ConsumeItem;
    public struct PlayerActionsActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerActionsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Roll => m_Wrapper.m_PlayerActions_Roll;
        public InputAction @LT => m_Wrapper.m_PlayerActions_LT;
        public InputAction @RB => m_Wrapper.m_PlayerActions_RB;
        public InputAction @RT => m_Wrapper.m_PlayerActions_RT;
        public InputAction @LockOn => m_Wrapper.m_PlayerActions_LockOn;
        public InputAction @Skill_One => m_Wrapper.m_PlayerActions_Skill_One;
        public InputAction @Skill_Two => m_Wrapper.m_PlayerActions_Skill_Two;
        public InputAction @Skill_Three => m_Wrapper.m_PlayerActions_Skill_Three;
        public InputAction @Skill_Ult => m_Wrapper.m_PlayerActions_Skill_Ult;
        public InputAction @A => m_Wrapper.m_PlayerActions_A;
        public InputAction @Jump => m_Wrapper.m_PlayerActions_Jump;
        public InputAction @Menu => m_Wrapper.m_PlayerActions_Menu;
        public InputAction @ConsumeItem => m_Wrapper.m_PlayerActions_ConsumeItem;
        public InputActionMap Get() { return m_Wrapper.m_PlayerActions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActionsActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActionsActions instance)
        {
            if (m_Wrapper.m_PlayerActionsActionsCallbackInterface != null)
            {
                @Roll.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnRoll;
                @Roll.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnRoll;
                @Roll.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnRoll;
                @LT.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnLT;
                @LT.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnLT;
                @LT.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnLT;
                @RB.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnRB;
                @RB.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnRB;
                @RB.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnRB;
                @RT.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnRT;
                @RT.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnRT;
                @RT.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnRT;
                @LockOn.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnLockOn;
                @LockOn.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnLockOn;
                @LockOn.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnLockOn;
                @Skill_One.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSkill_One;
                @Skill_One.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSkill_One;
                @Skill_One.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSkill_One;
                @Skill_Two.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSkill_Two;
                @Skill_Two.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSkill_Two;
                @Skill_Two.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSkill_Two;
                @Skill_Three.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSkill_Three;
                @Skill_Three.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSkill_Three;
                @Skill_Three.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSkill_Three;
                @Skill_Ult.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSkill_Ult;
                @Skill_Ult.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSkill_Ult;
                @Skill_Ult.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnSkill_Ult;
                @A.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnA;
                @A.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnA;
                @A.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnA;
                @Jump.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnJump;
                @Menu.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnMenu;
                @Menu.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnMenu;
                @Menu.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnMenu;
                @ConsumeItem.started -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnConsumeItem;
                @ConsumeItem.performed -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnConsumeItem;
                @ConsumeItem.canceled -= m_Wrapper.m_PlayerActionsActionsCallbackInterface.OnConsumeItem;
            }
            m_Wrapper.m_PlayerActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Roll.started += instance.OnRoll;
                @Roll.performed += instance.OnRoll;
                @Roll.canceled += instance.OnRoll;
                @LT.started += instance.OnLT;
                @LT.performed += instance.OnLT;
                @LT.canceled += instance.OnLT;
                @RB.started += instance.OnRB;
                @RB.performed += instance.OnRB;
                @RB.canceled += instance.OnRB;
                @RT.started += instance.OnRT;
                @RT.performed += instance.OnRT;
                @RT.canceled += instance.OnRT;
                @LockOn.started += instance.OnLockOn;
                @LockOn.performed += instance.OnLockOn;
                @LockOn.canceled += instance.OnLockOn;
                @Skill_One.started += instance.OnSkill_One;
                @Skill_One.performed += instance.OnSkill_One;
                @Skill_One.canceled += instance.OnSkill_One;
                @Skill_Two.started += instance.OnSkill_Two;
                @Skill_Two.performed += instance.OnSkill_Two;
                @Skill_Two.canceled += instance.OnSkill_Two;
                @Skill_Three.started += instance.OnSkill_Three;
                @Skill_Three.performed += instance.OnSkill_Three;
                @Skill_Three.canceled += instance.OnSkill_Three;
                @Skill_Ult.started += instance.OnSkill_Ult;
                @Skill_Ult.performed += instance.OnSkill_Ult;
                @Skill_Ult.canceled += instance.OnSkill_Ult;
                @A.started += instance.OnA;
                @A.performed += instance.OnA;
                @A.canceled += instance.OnA;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Menu.started += instance.OnMenu;
                @Menu.performed += instance.OnMenu;
                @Menu.canceled += instance.OnMenu;
                @ConsumeItem.started += instance.OnConsumeItem;
                @ConsumeItem.performed += instance.OnConsumeItem;
                @ConsumeItem.canceled += instance.OnConsumeItem;
            }
        }
    }
    public PlayerActionsActions @PlayerActions => new PlayerActionsActions(this);

    // Player UI
    private readonly InputActionMap m_PlayerUI;
    private IPlayerUIActions m_PlayerUIActionsCallbackInterface;
    public struct PlayerUIActions
    {
        private @PlayerControls m_Wrapper;
        public PlayerUIActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputActionMap Get() { return m_Wrapper.m_PlayerUI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerUIActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerUIActions instance)
        {
            if (m_Wrapper.m_PlayerUIActionsCallbackInterface != null)
            {
            }
            m_Wrapper.m_PlayerUIActionsCallbackInterface = instance;
            if (instance != null)
            {
            }
        }
    }
    public PlayerUIActions @PlayerUI => new PlayerUIActions(this);
    public interface IPlayerMovementActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnCamera(InputAction.CallbackContext context);
        void OnLockOnTargetLeft(InputAction.CallbackContext context);
        void OnLockOnTargetRight(InputAction.CallbackContext context);
    }
    public interface IPlayerActionsActions
    {
        void OnRoll(InputAction.CallbackContext context);
        void OnLT(InputAction.CallbackContext context);
        void OnRB(InputAction.CallbackContext context);
        void OnRT(InputAction.CallbackContext context);
        void OnLockOn(InputAction.CallbackContext context);
        void OnSkill_One(InputAction.CallbackContext context);
        void OnSkill_Two(InputAction.CallbackContext context);
        void OnSkill_Three(InputAction.CallbackContext context);
        void OnSkill_Ult(InputAction.CallbackContext context);
        void OnA(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnMenu(InputAction.CallbackContext context);
        void OnConsumeItem(InputAction.CallbackContext context);
    }
    public interface IPlayerUIActions
    {
    }
}
