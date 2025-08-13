
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Magic : MonoBehaviour
{
    protected Rigidbody2D _rig2d;
    protected MagicAnimator _animator;

    [SerializeField] protected int _damage;
    [SerializeField] protected int _speed;
    protected Vector2 _direction;

    protected bool _setUpEnd = false;

    protected MagicPhase _phase;

    private void Awake()
    {
        _rig2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<MagicAnimator>();
        _phase = MagicPhase.Spawn;
    }

    public void SetPositionWithDirection(Vector2 position, Vector2 direction)
    {
        transform.position = position;
        _direction = direction.normalized;
        _setUpEnd = true;
    }

    public void NextPhase(MagicPhase phase = MagicPhase.None)
    {
        if (phase == MagicPhase.None)
        {
            _phase++;
            return;
        }
        _phase = phase;
        switch (_phase)
        {
            case MagicPhase.Spawn:
                SpawnPhase();
                break;
            case MagicPhase.Casting:
                CastingPhase();
                break;
            case MagicPhase.Success:
                SuccessPhase();
                break;
            case MagicPhase.Hold:
                HoldPhase();
                break;
            case MagicPhase.End:
                EndPhase();
                break;
            case MagicPhase.Fail:
                FailPhase();
                break;
            case MagicPhase.Destroy:
                DestroyPhase();
                break;
            default:
                break;
        }
    }

    protected IEnumerator WaitUntilNextPhase(System.Func<bool> pred)
    {
        yield return new WaitUntil(pred);
        NextPhase();
    }

    protected void FollowMouse()
    {
        Vector2 screenPos = Mouse.current.position.ReadValue();
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0f));
        transform.position = worldPos;
    }

    public abstract void SpawnPhase();
    public abstract void CastingPhase();
    public abstract void SuccessPhase();
    public abstract void HoldPhase();
    public abstract void EndPhase();
    public abstract void FailPhase();
    public abstract void DestroyPhase();

}

public enum MagicPhase : int
{
    Spawn,
    Casting,
    Success,
    Hold,
    End,
    Fail,
    Destroy,
    None
}