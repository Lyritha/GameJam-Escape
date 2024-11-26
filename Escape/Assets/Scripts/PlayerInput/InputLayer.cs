using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputLayer", menuName = "Scriptable Objects/InputLayer")]
public class InputLayer : ScriptableObject, InputSystem.IPlayerActions, InputSystem.IUIActions
{
    public UnityAction<Vector2> moveEvent;
    public UnityAction<Vector2> lookEvent;
    public UnityAction<bool> freeLookEvent;
    public UnityAction<bool> fire1Event;
    public UnityAction<bool> fire2Event;
    public UnityAction<bool> jumpEvent;
    public UnityAction<bool> sprintEvent;
    public UnityAction<bool> interactEvent;
    public UnityAction<bool> crouchEvent;


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



    //Player

    //invokes a vector2 event
    private void InvokeEvent(InputAction.CallbackContext context, UnityAction<Vector2> eventAction)
        => eventAction?.Invoke(context.ReadValue<Vector2>());

    //invokes a bool event, converting the float to bool
    private void InvokeEvent(InputAction.CallbackContext context, UnityAction<bool> eventAction)
        => eventAction?.Invoke(context.ReadValue<float>() == 1);


    public void OnMove(InputAction.CallbackContext context) => InvokeEvent(context, moveEvent);
    public void OnLook(InputAction.CallbackContext context) => InvokeEvent(context, lookEvent);
    public void OnAttack(InputAction.CallbackContext context) => InvokeEvent(context, fire1Event);
    public void OnAttackSecondary(InputAction.CallbackContext context) => InvokeEvent(context, fire2Event);
    public void OnInteract(InputAction.CallbackContext context) => InvokeEvent(context, interactEvent);
    public void OnCrouch(InputAction.CallbackContext context) => InvokeEvent(context, crouchEvent);
    public void OnJump(InputAction.CallbackContext context) => InvokeEvent(context, jumpEvent);
    public void OnSprint(InputAction.CallbackContext context) => InvokeEvent(context, sprintEvent);
    public void OnFreeLook(InputAction.CallbackContext context) => InvokeEvent(context, freeLookEvent);





    //UI


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
