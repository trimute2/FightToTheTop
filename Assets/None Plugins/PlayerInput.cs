// GENERATED AUTOMATICALLY FROM 'Assets/None Plugins/PlayerInput.inputactions'

using System;
using UnityEngine;
using UnityEngine.Experimental.Input;


[Serializable]
public class PlayerInput : InputActionAssetReference
{
    public PlayerInput()
    {
    }
    public PlayerInput(InputActionAsset asset)
        : base(asset)
    {
    }
    private bool m_Initialized;
    private void Initialize()
    {
        // gameplay
        m_gameplay = asset.GetActionMap("gameplay");
        m_gameplay_Movement = m_gameplay.GetAction("Movement");
        m_gameplay_Weapon1 = m_gameplay.GetAction("Weapon1");
        m_gameplay_Weapon2 = m_gameplay.GetAction("Weapon2");
        m_gameplay_Jump = m_gameplay.GetAction("Jump");
        m_gameplay_Dodge = m_gameplay.GetAction("Dodge");
        m_Initialized = true;
    }
    private void Uninitialize()
    {
        if (m_GameplayActionsCallbackInterface != null)
        {
            gameplay.SetCallbacks(null);
        }
        m_gameplay = null;
        m_gameplay_Movement = null;
        m_gameplay_Weapon1 = null;
        m_gameplay_Weapon2 = null;
        m_gameplay_Jump = null;
        m_gameplay_Dodge = null;
        m_Initialized = false;
    }
    public void SetAsset(InputActionAsset newAsset)
    {
        if (newAsset == asset) return;
        var gameplayCallbacks = m_GameplayActionsCallbackInterface;
        if (m_Initialized) Uninitialize();
        asset = newAsset;
        gameplay.SetCallbacks(gameplayCallbacks);
    }
    public override void MakePrivateCopyOfActions()
    {
        SetAsset(ScriptableObject.Instantiate(asset));
    }
    // gameplay
    private InputActionMap m_gameplay;
    private IGameplayActions m_GameplayActionsCallbackInterface;
    private InputAction m_gameplay_Movement;
    private InputAction m_gameplay_Weapon1;
    private InputAction m_gameplay_Weapon2;
    private InputAction m_gameplay_Jump;
    private InputAction m_gameplay_Dodge;
    public struct GameplayActions
    {
        private PlayerInput m_Wrapper;
        public GameplayActions(PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement { get { return m_Wrapper.m_gameplay_Movement; } }
        public InputAction @Weapon1 { get { return m_Wrapper.m_gameplay_Weapon1; } }
        public InputAction @Weapon2 { get { return m_Wrapper.m_gameplay_Weapon2; } }
        public InputAction @Jump { get { return m_Wrapper.m_gameplay_Jump; } }
        public InputAction @Dodge { get { return m_Wrapper.m_gameplay_Dodge; } }
        public InputActionMap Get() { return m_Wrapper.m_gameplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(GameplayActions set) { return set.Get(); }
        public void SetCallbacks(IGameplayActions instance)
        {
            if (m_Wrapper.m_GameplayActionsCallbackInterface != null)
            {
                Movement.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                Movement.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                Movement.cancelled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnMovement;
                Weapon1.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWeapon1;
                Weapon1.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWeapon1;
                Weapon1.cancelled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWeapon1;
                Weapon2.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWeapon2;
                Weapon2.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWeapon2;
                Weapon2.cancelled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnWeapon2;
                Jump.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                Jump.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                Jump.cancelled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnJump;
                Dodge.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDodge;
                Dodge.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDodge;
                Dodge.cancelled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnDodge;
            }
            m_Wrapper.m_GameplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                Movement.started += instance.OnMovement;
                Movement.performed += instance.OnMovement;
                Movement.cancelled += instance.OnMovement;
                Weapon1.started += instance.OnWeapon1;
                Weapon1.performed += instance.OnWeapon1;
                Weapon1.cancelled += instance.OnWeapon1;
                Weapon2.started += instance.OnWeapon2;
                Weapon2.performed += instance.OnWeapon2;
                Weapon2.cancelled += instance.OnWeapon2;
                Jump.started += instance.OnJump;
                Jump.performed += instance.OnJump;
                Jump.cancelled += instance.OnJump;
                Dodge.started += instance.OnDodge;
                Dodge.performed += instance.OnDodge;
                Dodge.cancelled += instance.OnDodge;
            }
        }
    }
    public GameplayActions @gameplay
    {
        get
        {
            if (!m_Initialized) Initialize();
            return new GameplayActions(this);
        }
    }
    private int m_KeyboardAndMouseSchemeIndex = -1;
    public InputControlScheme KeyboardAndMouseScheme
    {
        get

        {
            if (m_KeyboardAndMouseSchemeIndex == -1) m_KeyboardAndMouseSchemeIndex = asset.GetControlSchemeIndex("Keyboard And Mouse");
            return asset.controlSchemes[m_KeyboardAndMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get

        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.GetControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
}
public interface IGameplayActions
{
    void OnMovement(InputAction.CallbackContext context);
    void OnWeapon1(InputAction.CallbackContext context);
    void OnWeapon2(InputAction.CallbackContext context);
    void OnJump(InputAction.CallbackContext context);
    void OnDodge(InputAction.CallbackContext context);
}
