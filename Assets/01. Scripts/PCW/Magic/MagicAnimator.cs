using UnityEngine;

[RequireComponent (typeof(Animator))]
public class MagicAnimator : MonoBehaviour
{
    protected Animator _animator;
    protected SpriteRenderer _spriteRenderer;

    protected int _PerformHash = Animator.StringToHash("Perform");
    protected int _HoldHash = Animator.StringToHash("Hold");
    protected int _EndHash = Animator.StringToHash("End");
    protected int _HitHash = Animator.StringToHash("Hit");

    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Flip(bool state)
    {
        _spriteRenderer.flipX = state;
    }

    public void Perform()
    {
        _animator.SetTrigger(_PerformHash);
    }
    public void Hold()
    {
        _animator.SetTrigger(_HoldHash);
    }
    public void End()
    {
        _animator.SetTrigger(_EndHash);
    }
    public void Hit()
    {
        _animator.SetTrigger(_HitHash);
    }
}
