using UnityEngine;
using UnityEngine.InputSystem;

public static class Utils
{
    public static Vector2 GetMousePos()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0f));
        return worldPos;
    }
}