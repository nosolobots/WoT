using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenuAttribute(fileName = "New Input Reader", menuName = "Input/InputReader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    private Controls _controls;
    public event Action<bool> PrimaryFireEvent;
    public event Action<Vector2> MoveEvent;
    public Vector2 AimPosition { get; private set; }

    public void OnAim(InputAction.CallbackContext context)
    {
        AimPosition = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());    
    }

    public void OnPrimaryFire(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            PrimaryFireEvent?.Invoke(true);
        }
        else if(context.canceled)
        {
            PrimaryFireEvent?.Invoke(false);
        }
    }

    private void OnEnable()
    {
        if(_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
        }
        _controls.Player.Enable();
    }
}
