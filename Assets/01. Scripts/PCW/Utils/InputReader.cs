using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(menuName = "SO/InputReader")]
public class InputReader : ScriptableObject, IInGameActions
{
    private Controls _controls;
    public Controls Controls => _controls;

    public Action<string> OnCastingKeyInput;
    public Action OnCastingInput;
    public Action<bool> OnMovementInput;
    public Action<bool> OnMagicChangeInput;

    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.InGame.SetCallbacks(this);
        }

        _controls.InGame.Enable();
    }
    private void OnDisable()
    {
        _controls.InGame.Disable();
    }

    public IEnumerator InputPauseCoroutine(float time)
    {
        _controls.InGame.Enable();
        yield return new WaitForSeconds(time);
        _controls.InGame.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnMovementInput?.Invoke(true);
        }
        else if (context.canceled)
        {
            OnMovementInput?.Invoke(false);
        }
    }

    public void OnCasting(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnCastingInput?.Invoke();
        }
    }

    public void OnMagicChange(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            float value = context.ReadValue<float>();
            OnMagicChangeInput?.Invoke(value > 0);
        }
    }
}
