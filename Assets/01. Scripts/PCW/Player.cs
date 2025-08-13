using DG.Tweening;
using System;
using UnityEngine;

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

    private void Awake()
    {
        _inputReader.OnMovementInput += Movement;
    }

    private void OnDestroy()
    {
        _inputReader.OnMovementInput -= Movement;
    }

    private void Movement(Vector2 pos)
    {
        _destination = pos;
    }
}
