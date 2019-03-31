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
        m_gameplay_Pause = m_gameplay.GetAction("Pause");
        // Menu Navigation
        m_MenuNavigation = asset.GetActionMap("Menu Navigation");
        m_MenuNavigation_PointAction = m_MenuNavigation.GetAction("Point Action");
        m_MenuNavigation_MoveAction = m_MenuNavigation.GetAction("Move Action");
        m_MenuNavigation_SubmitAction = m_MenuNavigation.GetAction("Submit Action");
        m_MenuNavigation_CancelAction = m_MenuNavigation.GetAction("Cancel Action");
        m_MenuNavigation_LeftClickAction = m_MenuNavigation.GetAction("Left Click Action");
        m_MenuNavigation_MiddleClickAction = m_MenuNavigation.GetAction("Middle Click Action");
        m_MenuNavigation_RightClickAction = m_MenuNavigation.GetAction("Right Click Action");
        m_MenuNavigation_ScrollWheelAction = m_MenuNavigation.GetAction("Scroll Wheel Action");
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
        m_gameplay_Pause = null;
        if (m_MenuNavigationActionsCallbackInterface != null)
        {
            MenuNavigation.SetCallbacks(null);
        }
        m_MenuNavigation = null;
        m_MenuNavigation_PointAction = null;
        m_MenuNavigation_MoveAction = null;
        m_MenuNavigation_SubmitAction = null;
        m_MenuNavigation_CancelAction = null;
        m_MenuNavigation_LeftClickAction = null;
        m_MenuNavigation_MiddleClickAction = null;
        m_MenuNavigation_RightClickAction = null;
        m_MenuNavigation_ScrollWheelAction = null;
        m_Initialized = false;
    }
    public void SetAsset(InputActionAsset newAsset)
    {
        if (newAsset == asset) return;
        var gameplayCallbacks = m_GameplayActionsCallbackInterface;
        var MenuNavigationCallbacks = m_MenuNavigationActionsCallbackInterface;
        if (m_Initialized) Uninitialize();
        asset = newAsset;
        gameplay.SetCallbacks(gameplayCallbacks);
        MenuNavigation.SetCallbacks(MenuNavigationCallbacks);
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
    private InputAction m_gameplay_Pause;
    public struct GameplayActions
    {
        private PlayerInput m_Wrapper;
        public GameplayActions(PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement { get { return m_Wrapper.m_gameplay_Movement; } }
        public InputAction @Weapon1 { get { return m_Wrapper.m_gameplay_Weapon1; } }
        public InputAction @Weapon2 { get { return m_Wrapper.m_gameplay_Weapon2; } }
        public InputAction @Jump { get { return m_Wrapper.m_gameplay_Jump; } }
        public InputAction @Dodge { get { return m_Wrapper.m_gameplay_Dodge; } }
        public InputAction @Pause { get { return m_Wrapper.m_gameplay_Pause; } }
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
                Pause.started -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                Pause.performed -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
                Pause.cancelled -= m_Wrapper.m_GameplayActionsCallbackInterface.OnPause;
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
                Pause.started += instance.OnPause;
                Pause.performed += instance.OnPause;
                Pause.cancelled += instance.OnPause;
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
    // Menu Navigation
    private InputActionMap m_MenuNavigation;
    private IMenuNavigationActions m_MenuNavigationActionsCallbackInterface;
    private InputAction m_MenuNavigation_PointAction;
    private InputAction m_MenuNavigation_MoveAction;
    private InputAction m_MenuNavigation_SubmitAction;
    private InputAction m_MenuNavigation_CancelAction;
    private InputAction m_MenuNavigation_LeftClickAction;
    private InputAction m_MenuNavigation_MiddleClickAction;
    private InputAction m_MenuNavigation_RightClickAction;
    private InputAction m_MenuNavigation_ScrollWheelAction;
    public struct MenuNavigationActions
    {
        private PlayerInput m_Wrapper;
        public MenuNavigationActions(PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @PointAction { get { return m_Wrapper.m_MenuNavigation_PointAction; } }
        public InputAction @MoveAction { get { return m_Wrapper.m_MenuNavigation_MoveAction; } }
        public InputAction @SubmitAction { get { return m_Wrapper.m_MenuNavigation_SubmitAction; } }
        public InputAction @CancelAction { get { return m_Wrapper.m_MenuNavigation_CancelAction; } }
        public InputAction @LeftClickAction { get { return m_Wrapper.m_MenuNavigation_LeftClickAction; } }
        public InputAction @MiddleClickAction { get { return m_Wrapper.m_MenuNavigation_MiddleClickAction; } }
        public InputAction @RightClickAction { get { return m_Wrapper.m_MenuNavigation_RightClickAction; } }
        public InputAction @ScrollWheelAction { get { return m_Wrapper.m_MenuNavigation_ScrollWheelAction; } }
        public InputActionMap Get() { return m_Wrapper.m_MenuNavigation; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled { get { return Get().enabled; } }
        public InputActionMap Clone() { return Get().Clone(); }
        public static implicit operator InputActionMap(MenuNavigationActions set) { return set.Get(); }
        public void SetCallbacks(IMenuNavigationActions instance)
        {
            if (m_Wrapper.m_MenuNavigationActionsCallbackInterface != null)
            {
                PointAction.started -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnPointAction;
                PointAction.performed -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnPointAction;
                PointAction.cancelled -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnPointAction;
                MoveAction.started -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnMoveAction;
                MoveAction.performed -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnMoveAction;
                MoveAction.cancelled -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnMoveAction;
                SubmitAction.started -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnSubmitAction;
                SubmitAction.performed -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnSubmitAction;
                SubmitAction.cancelled -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnSubmitAction;
                CancelAction.started -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnCancelAction;
                CancelAction.performed -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnCancelAction;
                CancelAction.cancelled -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnCancelAction;
                LeftClickAction.started -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnLeftClickAction;
                LeftClickAction.performed -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnLeftClickAction;
                LeftClickAction.cancelled -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnLeftClickAction;
                MiddleClickAction.started -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnMiddleClickAction;
                MiddleClickAction.performed -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnMiddleClickAction;
                MiddleClickAction.cancelled -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnMiddleClickAction;
                RightClickAction.started -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnRightClickAction;
                RightClickAction.performed -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnRightClickAction;
                RightClickAction.cancelled -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnRightClickAction;
                ScrollWheelAction.started -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnScrollWheelAction;
                ScrollWheelAction.performed -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnScrollWheelAction;
                ScrollWheelAction.cancelled -= m_Wrapper.m_MenuNavigationActionsCallbackInterface.OnScrollWheelAction;
            }
            m_Wrapper.m_MenuNavigationActionsCallbackInterface = instance;
            if (instance != null)
            {
                PointAction.started += instance.OnPointAction;
                PointAction.performed += instance.OnPointAction;
                PointAction.cancelled += instance.OnPointAction;
                MoveAction.started += instance.OnMoveAction;
                MoveAction.performed += instance.OnMoveAction;
                MoveAction.cancelled += instance.OnMoveAction;
                SubmitAction.started += instance.OnSubmitAction;
                SubmitAction.performed += instance.OnSubmitAction;
                SubmitAction.cancelled += instance.OnSubmitAction;
                CancelAction.started += instance.OnCancelAction;
                CancelAction.performed += instance.OnCancelAction;
                CancelAction.cancelled += instance.OnCancelAction;
                LeftClickAction.started += instance.OnLeftClickAction;
                LeftClickAction.performed += instance.OnLeftClickAction;
                LeftClickAction.cancelled += instance.OnLeftClickAction;
                MiddleClickAction.started += instance.OnMiddleClickAction;
                MiddleClickAction.performed += instance.OnMiddleClickAction;
                MiddleClickAction.cancelled += instance.OnMiddleClickAction;
                RightClickAction.started += instance.OnRightClickAction;
                RightClickAction.performed += instance.OnRightClickAction;
                RightClickAction.cancelled += instance.OnRightClickAction;
                ScrollWheelAction.started += instance.OnScrollWheelAction;
                ScrollWheelAction.performed += instance.OnScrollWheelAction;
                ScrollWheelAction.cancelled += instance.OnScrollWheelAction;
            }
        }
    }
    public MenuNavigationActions @MenuNavigation
    {
        get
        {
            if (!m_Initialized) Initialize();
            return new MenuNavigationActions(this);
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
    void OnPause(InputAction.CallbackContext context);
}
public interface IMenuNavigationActions
{
    void OnPointAction(InputAction.CallbackContext context);
    void OnMoveAction(InputAction.CallbackContext context);
    void OnSubmitAction(InputAction.CallbackContext context);
    void OnCancelAction(InputAction.CallbackContext context);
    void OnLeftClickAction(InputAction.CallbackContext context);
    void OnMiddleClickAction(InputAction.CallbackContext context);
    void OnRightClickAction(InputAction.CallbackContext context);
    void OnScrollWheelAction(InputAction.CallbackContext context);
}
