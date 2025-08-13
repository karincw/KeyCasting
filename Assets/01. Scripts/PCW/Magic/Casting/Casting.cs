using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Casting : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GaugeBar _castingGauge;
    [SerializeField] private LetterViewer _letterViewer;

    public MagicSO currentMagic;

    private bool _isCasting = false;
    private bool _canCasting => Time.time > _lastCastingTime + castingCooltime;
    [SerializeField] private float castingCooltime = 2.5f;
    private float _lastCastingTime = -100;
    private float _castingStartTime;

    private List<string> _castingLetter;
    private int _castingIndex;

    private void Awake()
    {
        _castingLetter = new();
        _inputReader.OnCastingInput += OnCasting;
    }

    private void OnDestroy()
    {
        _inputReader.OnCastingInput -= OnCasting;
    }

    private void OnCasting()
    {
        if (!_canCasting || _isCasting) return;

        _isCasting = true;
        if (currentMagic != null)
        {
            _castingLetter = currentMagic.GetCastingLetter().GetLetterList();
            _castingIndex = 0;
            _castingStartTime = Time.time;

            StartCoroutine("CastingCoroutine");
            
            _castingGauge.Open();
            _letterViewer.Open();
            _letterViewer.SetLetters(_castingLetter);
            _castingGauge.SetGauge(1);
        }
    }

    private IEnumerator CastingCoroutine()
    {
        yield return new WaitForSeconds(currentMagic.CastingTime);

        _castingGauge.Close();
        _letterViewer.Close();
        _isCasting = false;
        _lastCastingTime = Time.time;

    }

    private void Update()
    {
        if (Keyboard.current == null) return; // 키보드 없으면 무시

        if (!_isCasting) return;

        _castingGauge.SetGauge((_castingStartTime - Time.time + currentMagic.CastingTime) / currentMagic.CastingTime);

        if (_castingLetter[_castingIndex] == "END")
        {
            _isCasting = false;
            _lastCastingTime = Time.time;
            StopCoroutine("CastingCoroutine");
            _letterViewer.Close();
            _castingGauge.Close();
            Magic magic = currentMagic.UseMagic().GetComponent<Magic>();

            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0f));
            Vector2 lookMouseDir = (worldPos - (Vector2)transform.position).normalized;

            magic.transform.position = (Vector2)transform.position + lookMouseDir;

            magic.SetDirection(lookMouseDir);
        }

        foreach (var key in Keyboard.current.allKeys)
        {
            if (key == null) continue;  // null인 키는 건너뜀

            if (key.wasPressedThisFrame) // 이번 프레임에 눌린 키 확인
            {
                Debug.Log($"Pressed Key: {key.displayName} / {key.keyCode}");
                if (_castingLetter[_castingIndex] == key.displayName)
                {
                    _letterViewer.Press(_castingIndex);
                    _castingIndex += 1;
                }
            }
        }
    }

}