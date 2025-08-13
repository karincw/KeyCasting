
using UnityEngine;
using UnityEngine.InputSystem;

public class MagicMissile : Magic
{
    [SerializeField] private float guidedPower = 1;

    protected override void Update()
    {
        base.Update();

        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0f));
        
        Vector2 dir = _direction + (worldPos - (Vector2)transform.position) * guidedPower; 
        _direction = dir.normalized;

        _rig2d.linearVelocity = _direction.normalized * _speed;

        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

}