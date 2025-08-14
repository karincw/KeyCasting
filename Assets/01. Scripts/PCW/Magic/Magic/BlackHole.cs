using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlackHole : Magic
{
    [Header("Attraction-Settings")]
    [SerializeField] private float _attractionPower = 1;
    [SerializeField] private float _attractionCooltime = 0.5f;
    [SerializeField] private float _attractionAreaRadius = 7;

    [Header("Magic-Settings")]
    [SerializeField] private float _damageCooltime;
    [SerializeField] private float _damageAreaRadius = 2;
    [SerializeField] private Vector3 _damageAreaOffset;
    [SerializeField] private LayerMask _targetLayer;

    [SerializeField] private float _followMoveSpeed;

    private bool _attraction;
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
        _followMouse = false;
        _attraction = true;
        StartCoroutine("AttractionCoroutine");
        StartCoroutine("DamageCoroutine");
        _animator.Perform();
    }
    public override void HoldPhase()
    {
        _animator.Hold();
    }

    private IEnumerator AttractionCoroutine()
    {
        while (_attraction)
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
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + _damageAreaOffset, _attractionAreaRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + _damageAreaOffset, _damageAreaRadius);
        Gizmos.color = Color.white;
    }

}
