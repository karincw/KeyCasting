using UnityEngine;

public class Fireball : Magic
{
    [Header("Magic-Settings")]
    [SerializeField] private LayerMask _targetLayer;

    private bool _viewMouse;

    private void Update()
    {
        if (_viewMouse)
        {
            Vector2 mousePos = Utils.GetMousePos();
            _direction = (mousePos - (Vector2)transform.position).normalized;
            float angle = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg;
            Vector3 direction = new Vector3(0, 0, angle);
            transform.rotation = Quaternion.Euler(direction);
            transform.position = _ownerPosition + _direction;
        }
    }

    public override void CastingPhase()
    {
        _viewMouse = true;
    }
    public override void SuccessPhase()
    {
        _viewMouse = false;
        _rig2d.linearVelocity = _direction * _speed;
        _animator.Flip(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == _targetLayer.value)
        {
            if (collision.attachedRigidbody.TryGetComponent<IHitable>(out IHitable hit))
            {
                hit.Hit(_damage);
                _animator.Hit();
            }
        }
    }
}
