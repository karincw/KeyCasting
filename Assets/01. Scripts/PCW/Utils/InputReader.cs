using System;
using System.Collections;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;
using static Controls;

[CreateAssetMenu(menuName = "SO/InputReader")]
public class InputReader : ScriptableObject, IInGameActions
{
    private Controls _controls;
    public Controls Controls => _controls;

    public Action<string> OnCastingKeyInput;
    public Action<bool> OnCastingInput;
    public Action<Vector2> OnMovementInput;

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
        if(context.performed)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0f));
            worldPos.z = 0f;
            OnMovementInput?.Invoke(worldPos);
        }
    }

    public void OnCasting(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnCastingInput?.Invoke(true);
        }
        if (context.canceled)
        {
            OnCastingInput?.Invoke(false);
        }
    }
}
