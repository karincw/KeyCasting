using UnityEngine;

public class IcePillar : Magic
{
    [Header("Magic-Settings")]
    [SerializeField] private float _damageAreaRadius = 2;
    [SerializeField] private Vector3 _damageAreaOffset;
    [SerializeField] private LayerMask _targetLayer;

    private bool _followMouse;

    private void Update()
    {
        if (_followMouse)
        {
            transform.position = Utils.GetMousePos();
        }
    }

    public override void CastingPhase()
    {
        _followMouse = true;
    }
    public override void SuccessPhase()
    {
        _followMouse = false;
        _animator.Perform();
        Attack();
    }

    private void Attack()
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
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + _damageAreaOffset, _damageAreaRadius);
        Gizmos.color = Color.white;
    }

}
