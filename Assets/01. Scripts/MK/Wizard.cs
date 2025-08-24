using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class Wizard : MonoBehaviour, IHitable
{
    int IHitable.Health
    {
        get => _health;
        set
        {
            _health = value;

            if (_health <= 0)
            {
                Die();
            }
        }
    }
    private int _health;

    private NavMeshAgent _navAgent;
    private BehaviorGraphAgent _behaviourAgent;

    [SerializeField] private GameObject _bolt;

    private void Awake()
    {
        _behaviourAgent = GetComponent<BehaviorGraphAgent>();
        _navAgent = GetComponent<NavMeshAgent>();

        _navAgent.updateRotation = false;
        _navAgent.updateUpAxis = false;
    }

    private void Die()
    {
        Debug.Log($"{this.name} Death");
    }

    private void Attack()
    {
        Debug.Log($"{this.name} Attack");
        Instantiate(_bolt, transform.position, Quaternion.identity);
    }
}