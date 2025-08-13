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
    [SerializeField] private LayerMask _targetLayer;

    private bool _attraction;
    private bool _followMouse;

    private void Update()
    {
        if (_followMouse)
            FollowMouse();
    }

    public override void SpawnPhase() { }
    public override void CastingPhase()
    {
        _followMouse = true;
    }
    public override void SuccessPhase()
    {
        _followMouse = false;
        StartCoroutine("AttractionCoroutine");
        StartCoroutine("DamageCoroutine");
        _attraction = true;
        _animator.Perform();
    }
    public override void HoldPhase()
    {
        _animator.Hold();
    }

    public override void FailPhase()
    {
        StopAllCoroutines();
        DestroyPhase();
    }
    public override void EndPhase()
    {
        StopAllCoroutines();
        _animator.End();
    }
    public override void DestroyPhase()
    {
        Destroy(gameObject);
    }

    private IEnumerator AttractionCoroutine()
    {
        while (_attraction)
        {
            List<Transform> targets = Physics2D.OverlapCircleAll(transform.position, _attractionAreaRadius, _targetLayer).Select(t => t.transform).ToList();
            foreach (var target in targets)
            {
                target.position += (transform.position - target.position).normalized * _attractionPower;
            }

            yield return new WaitForSeconds(_attractionCooltime);
        }
    }

    private IEnumerator DamageCoroutine()
    {
        while (true)
        {
            var targets = Physics2D.OverlapCircleAll(transform.position, _damageAreaRadius, _targetLayer);
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
        Gizmos.DrawWireSphere(transform.position, _attractionAreaRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _damageAreaRadius);
        Gizmos.color = Color.white;
    }

}
