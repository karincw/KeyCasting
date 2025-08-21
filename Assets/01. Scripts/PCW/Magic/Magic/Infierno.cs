using System.Collections;
using UnityEngine;

public class Infierno : Magic
{
    [Header("Magic-Settings")]
    [SerializeField] private float _damageCooltime;
    [SerializeField] private float _damageAreaRadius = 2;
    [SerializeField] private Vector3 _damageAreaOffset;
    [SerializeField] private LayerMask _targetLayer;

    [SerializeField] private float _followMoveSpeed;

    private bool _followMouse;

    private void Update()
    {
        if (_followMouse)
        {
            Vector3 mousePos = Utils.GetMousePos();
            Vector2 movementDirection = (mousePos - transform.position).normalized;
            transform.Translate(movementDirection * _followMoveSpeed);
        }
    }

    public override void CastingPhase()
    {
        _followMouse = true;
    }
    public override void SuccessPhase()
    {
        StartCoroutine("DamageCoroutine");
        _animator.Perform();
    }
    public override void HoldPhase()
    {
        _animator.Hold();
    }

    public override void FailPhase()
    {
        _followMouse = false;
        base.FailPhase();
    }

    public override void EndPhase()
    {
        base.EndPhase();
    }

    private IEnumerator DamageCoroutine()
    {
        while (true)
        {
            var targets = Physics2D.OverlapCircleAll(transform.position + _damageAreaOffset, _damageAreaRadius, _targetLayer);
            foreach (var target in targets)
            {
                if (target.attachedRigidbody.TryGetComponent<IHitable>(out IHitable hit))
                {
                    hit.Hit(_damage);
                    Destroy(gameObject);
                }
            }
            yield return new WaitForSeconds(_damageCooltime);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + _damageAreaOffset, _damageAreaRadius);
        Gizmos.color = Color.white;
    }

}
