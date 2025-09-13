using UnityEngine;

public class Bolt : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _lifetime = 2f;
    private float _lifeTimer = 0f;

    private void Update()
    {
        _lifeTimer += Time.deltaTime;
        if (_lifeTimer >= _lifetime)
        {
            Die();
        }

        transform.Translate(Vector3.right * Time.deltaTime * _speed);
    }

    public void SetDirection(Vector3 dir)
    {
        transform.right = dir;
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var hitable = collision.GetComponentInParent<IHitable>();

            if (hitable == null) return;

            hitable.Hit(_damage);
            Die();
        }
    }
}