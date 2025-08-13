
using UnityEngine;

public class Magic : MonoBehaviour
{
    protected Rigidbody2D _rig2d;
    protected int _damage;
    protected int _speed;
    protected Vector2 _direction;

    protected bool _setUpEnd = false;

    public void SetUp(int damage, int speed)
    {
        _rig2d = GetComponent<Rigidbody2D>();
        _damage = damage;
        _speed = speed;
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction.normalized;
        _setUpEnd = true;
    }

    protected virtual void Update()
    {
        if (!_setUpEnd) return;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_setUpEnd) return;
        if (collision.attachedRigidbody.TryGetComponent<IHitable>(out IHitable hit))
        {
            hit.Hit(_damage);
            Destroy(gameObject);
        }
    }
}