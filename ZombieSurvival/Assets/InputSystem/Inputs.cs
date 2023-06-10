// GENERATED AUTOMATICALLY FROM 'Assets/InputSystem/Inputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Inputs : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Inputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Inputs"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""58a26bbc-c4c2-433f-8f8f-257c3e9b6393"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""5e25c508-3abb-4754-b176-2d7a70ae8f9e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""1fc153c5-70f8-4dda-82d6-64b5ed6da0c1"",
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
                    ""id"": ""558c4a73-1104-4bb9-b916-e1bc281cefcc"",
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
                    ""id"": ""9436214b-fea8-445a-b14e-0144a5e8097d"",
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
                    ""id"": ""74e5387d-9aee-4d96-8ef6-6e2456eaf68c"",
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
                    ""id"": ""c18ea34e-a199-495d-a971-d6ff236dc519"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Actions"",
            ""id"": ""c5af1cb8-9fb4-447c-80ad-a1d940ea8897"",
            ""actions"": [
                {
                    ""name"": ""InputPressedBar"",
                    ""type"": ""Button"",
                    ""id"": ""0c1e29a3-ee9a-49d0-9546-3bd52f1d54c8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""InputPressedFire"",
                    ""type"": ""Button"",
                    ""id"": ""9c88d1d7-f308-4f34-8a29-a8f398b5dd27"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e4920202-4801-4740-b0cf-03f4f065d9ba"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InputPressedBar"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4f331ec0-ef8a-46d6-972c-3448d4852de8"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InputPressedFire"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        // Actions
        m_Actions = asset.FindActionMap("Actions", throwIfNotFound: true);
        m_Actions_InputPressedBar = m_Actions.FindAction("InputPressedBar", throwIfNotFound: true);
        m_Actions_InputPressedFire = m_Actions.FindAction("InputPressedFire", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_Movement;
    public struct PlayerActions
    {
        private @Inputs m_Wrapper;
        public PlayerActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @Movement.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
                @Movement.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnMovement;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Actions
    private readonly InputActionMap m_Actions;
    private IActionsActions m_ActionsActionsCallbackInterface;
    private readonly InputAction m_Actions_InputPressedBar;
    private readonly InputAction m_Actions_InputPressedFire;
    public struct ActionsActions
    {
        private @Inputs m_Wrapper;
        public ActionsActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @InputPressedBar => m_Wrapper.m_Actions_InputPressedBar;
        public InputAction @InputPressedFire => m_Wrapper.m_Actions_InputPressedFire;
        public InputActionMap Get() { return m_Wrapper.m_Actions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ActionsActions set) { return set.Get(); }
        public void SetCallbacks(IActionsActions instance)
        {
            if (m_Wrapper.m_ActionsActionsCallbackInterface != null)
            {
                @InputPressedBar.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnInputPressedBar;
                @InputPressedBar.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnInputPressedBar;
                @InputPressedBar.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnInputPressedBar;
                @InputPressedFire.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnInputPressedFire;
                @InputPressedFire.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnInputPressedFire;
                @InputPressedFire.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnInputPressedFire;
            }
            m_Wrapper.m_ActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @InputPressedBar.started += instance.OnInputPressedBar;
                @InputPressedBar.performed += instance.OnInputPressedBar;
                @InputPressedBar.canceled += instance.OnInputPressedBar;
                @InputPressedFire.started += instance.OnInputPressedFire;
                @InputPressedFire.performed += instance.OnInputPressedFire;
                @InputPressedFire.canceled += instance.OnInputPressedFire;
            }
        }
    }
    public ActionsActions @Actions => new ActionsActions(this);
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
    }
    public interface IActionsActions
    {
        void OnInputPressedBar(InputAction.CallbackContext context);
        void OnInputPressedFire(InputAction.CallbackContext context);
    }
}
