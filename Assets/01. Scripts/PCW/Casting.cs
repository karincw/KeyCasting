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
        if (Keyboard.current == null) return; // Ű���� ������ ����

        if (_isCasting == false) return;

        foreach (var key in Keyboard.current.allKeys)
        {
            if (key == null) continue;  // null�� Ű�� �ǳʶ�

            if (key.wasPressedThisFrame) // �̹� �����ӿ� ���� Ű Ȯ��
            {
                Debug.Log($"Pressed Key: {key.displayName} / {key.keyCode}");
                // �ʿ��� ó�� �ۼ�
            }
        }
    }

}