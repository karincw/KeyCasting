using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Casting : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;

    public bool _isCasting = false;

    private void Awake()
    {
        _inputReader.OnCastingInput += OnCasting;
    }

    private void OnDestroy()
    {
        _inputReader.OnCastingInput -= OnCasting;
    }

    private void OnCasting(bool state)
    {
        _isCasting = state;
    }

    private void Update()
    {
        if (Keyboard.current == null) return; // 키보드 없으면 무시

        if (_isCasting == false) return;

        foreach (var key in Keyboard.current.allKeys)
        {
            if (key == null) continue;  // null인 키는 건너뜀

            if (key.wasPressedThisFrame) // 이번 프레임에 눌린 키 확인
            {
                Debug.Log($"Pressed Key: {key.displayName} / {key.keyCode}");
                // 필요한 처리 작성
            }
        }
    }

}