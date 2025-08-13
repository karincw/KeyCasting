using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IHitable
{
    [SerializeField] private InputReader _inputReader;

    int IHitable.Health
    {
        get => _health;
        set => _health = value;
    }
    private int _health;
    private Vector2 _destination;
    private bool moveState;

    private NavMeshAgent _navAgent;

    private void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();

        _navAgent.updateRotation = false;
        _navAgent.updateUpAxis = false;

        _inputReader.OnMovementInput += Movement;
    }

    private void OnDestroy()
    {
        _inputReader.OnMovementInput -= Movement;
    }

    private void Movement(bool state)
    {
        moveState = state;
        if (state)
        {
            StartCoroutine("MovementCoroutine");
        }
    }
    private IEnumerator MovementCoroutine()
    {
        while (moveState)
        {
            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0f));
            worldPos.z = 0f;

            _destination = worldPos;
            _navAgent.destination = _destination;
            yield return new WaitForSeconds(0.25f);
        }
    }
}
