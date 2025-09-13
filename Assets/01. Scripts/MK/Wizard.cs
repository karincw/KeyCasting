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
    [SerializeField] private int _health = 10;

    private NavMeshAgent _navAgent;
    private BehaviorGraphAgent _behaviorAgent;

    [SerializeField] private Bolt _bolt;
    [SerializeField] private WizardStateChange _wizardStateChange;

    private WizardStateChange _stateChangeEvent;

    private void Awake()
    {
        _behaviorAgent = GetComponent<BehaviorGraphAgent>();
        _navAgent = GetComponent<NavMeshAgent>();

        _navAgent.updateRotation = false;
        _navAgent.updateUpAxis = false;

        _behaviorAgent.enabled = false;
    }

    void Start()
    {
        _stateChangeEvent = _wizardStateChange.Clone() as WizardStateChange;

        if (_behaviorAgent.GetVariable("StateChange", out BlackboardVariable<WizardStateChange> variable))
        {
            variable.Value = _stateChangeEvent;
        }

        _behaviorAgent.enabled = true;
    }

    public void RandomStateChange()
    {
        WizardState[] states = (WizardState[])System.Enum.GetValues(typeof(WizardState));
        int randomIndex = Random.Range(0, states.Length);
        WizardState state = states[randomIndex];
        _wizardStateChange.SendEventMessage(state);
    }

    public void Attack()
    {
        var player = FindAnyObjectByType<Player>();
        Bolt bolt = Instantiate(_bolt, transform.position, Quaternion.identity).GetComponent<Bolt>();

        var target = (player.transform.position - transform.position).normalized;
        target += new Vector3(0, 0.1f, 0);
        bolt.SetDirection(target);
    }

    public Vector3 MoveTarget()
    {
        Vector3 randomTarget = new Vector3(UnityEngine.Random.Range(-10f, 10f), UnityEngine.Random.Range(-10f, 10f), 0);
        Vector3 target = transform.position + randomTarget;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(target, out hit, 20.0f, NavMesh.AllAreas))
        {
            target = hit.position;
        }

        return target;
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

            hitable.Hit(1);
        }
    }
}