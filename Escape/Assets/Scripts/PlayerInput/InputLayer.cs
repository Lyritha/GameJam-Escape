using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputLayer", menuName = "Scriptable Objects/InputLayer")]
public class InputLayer : ScriptableObject, InputSystem.IPlayerActions, InputSystem.IUIActions
{
    public UnityAction<Vector2> moveEvent;
    public UnityAction<Vector2> lookEvent;
    public UnityAction<bool> fire1Event;
    public UnityAction<bool> fire2Event;
    public UnityAction<bool> jumpEvent;



    private InputSystem inputSystem;

    private void OnEnable()
    {
        if (inputSystem == null)
        {
            inputSystem = new();
            inputSystem.Player.SetCallbacks(this);
            inputSystem.UI.SetCallbacks(this);

        }

        inputSystem.Player.Enable();
        inputSystem.UI.Enable();

        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        inputSystem.Player.Disable();
        inputSystem.UI.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        fire1Event?.Invoke(context.ReadValue<bool>());
    }

    public void OnAttackSecondary(InputAction.CallbackContext context)
    {
        fire2Event?.Invoke(context.ReadValue<float>() == 1);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {

    }

    public void OnCrouch(InputAction.CallbackContext context)
    {

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        jumpEvent?.Invoke(context.ReadValue<float>() == 1);
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {

    }

    public void OnNext(InputAction.CallbackContext context)
    {

    }

    public void OnSprint(InputAction.CallbackContext context)
    {

    }

    public void OnNavigate(InputAction.CallbackContext context)
    {

    }

    public void OnSubmit(InputAction.CallbackContext context)
    {

    }

    public void OnCancel(InputAction.CallbackContext context)
    {

    }

    public void OnPoint(InputAction.CallbackContext context)
    {

    }

    public void OnClick(InputAction.CallbackContext context)
    {

    }

    public void OnRightClick(InputAction.CallbackContext context)
    {

    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {

    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {

    }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context)
    {

    }

    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
    {

    }
}
