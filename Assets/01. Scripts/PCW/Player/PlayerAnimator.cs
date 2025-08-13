using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.U2D.Animation;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;

    [Header("AgentAnimator-Settings")]
    [SerializeField] protected SpriteLibraryAsset _side;
    [SerializeField] protected SpriteLibraryAsset _front;
    [SerializeField] protected SpriteLibraryAsset _back;

    protected Animator _animator;
    protected SpriteLibrary _spreteLibrary;
    protected SpriteRenderer _spriteRenderer;
    protected Vector2 _moveDirection;

    protected int _walkHash = Animator.StringToHash("Walk");

    private NavMeshAgent _navAgent;

    protected virtual void Awake()
    {
        _navAgent = GetComponentInParent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _spreteLibrary = GetComponent<SpriteLibrary>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        GetMoveDirection();
        SetAnimationDirection();
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
    }

    protected void SetAnimationDirection()
    {
        if (_moveDirection == Vector2.zero) return;

        if (Vector2.Dot(Vector2.up, _moveDirection) >= 0.9f)
        {
            _spreteLibrary.spriteLibraryAsset = _front;
        }
        else if (Vector2.Dot(Vector2.down, _moveDirection) >= 0.9f)
        {
            _spreteLibrary.spriteLibraryAsset = _back;
        }
        else
        {
            _spreteLibrary.spriteLibraryAsset = _side;
            if (_moveDirection.x < 0)
                _spriteRenderer.flipX = true;
            else
                _spriteRenderer.flipX = false;

        }
        _spreteLibrary.RefreshSpriteResolvers();
    }
}
