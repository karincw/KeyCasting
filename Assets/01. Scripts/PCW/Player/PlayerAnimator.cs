using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;

    public bool IsFlip => _spriteRenderer.flipX;

    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;
    protected Vector2 _moveDirection;

    protected int _walkHash = Animator.StringToHash("Walk");
    protected int _HitHash = Animator.StringToHash("Hit");
    protected int _CastingSuccessHash = Animator.StringToHash("CastingSuccess");
    protected int _CastingFailHash = Animator.StringToHash("CastingFail");

    private NavMeshAgent _navAgent;

    protected virtual void Awake()
    {
        _navAgent = GetComponentInParent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        GetMoveDirection();
    }

    private void GetMoveDirection()
    {
        Vector2 moveDirection = (_navAgent.destination - transform.position).normalized;
        _moveDirection = moveDirection;

        if (_moveDirection == Vector2.zero)
        {
            _animator.SetBool(_walkHash, false);
        }
        else
        {
            _animator.SetBool(_walkHash, true);
        }
        SetViewDirection(_moveDirection);
    }

    public void SetViewDirection(Vector2 direction)
    {
        if (direction == Vector2.zero) return;

        if (direction.x < 0)
            _spriteRenderer.flipX = true;
        else
            _spriteRenderer.flipX = false;
    }

    public void Clear()
    {
        _animator.ResetTrigger(_CastingSuccessHash);
        _animator.ResetTrigger(_CastingFailHash);
    }

    public void Hit()
    {
        _animator.SetTrigger(_HitHash);
    }

    public void CastingSuccess()
    {
        _animator.SetTrigger(_CastingSuccessHash);
    }
    public void CastingFail()
    {
        _animator.SetTrigger(_CastingFailHash);
    }


    public void SetTrigger(string hash)
    {
        _animator.SetTrigger(hash);
    }
    public void SetTrigger(int hash)
    {
        _animator.SetTrigger(hash);
    }
}
