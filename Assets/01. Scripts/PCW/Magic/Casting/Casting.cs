using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Casting : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GaugeBar _castingGauge;
    [SerializeField] private LetterViewer _letterViewer;

    [SerializeField] private MagicSO currentMagicData;
    private Magic currentMagic;
    private bool _canCasting => Time.time > _lastCastingTime + castingCooltime;
    [SerializeField] private float castingCooltime = 2.5f;
    private float _lastCastingTime = -100;

    private List<string> _castingLetter;
    private int _castingIndex;
    private float _castingTime;

    private bool _isCasting = false;
    private bool _isHolding = false;

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
        if (!_canCasting) return; //캐스팅 불가능시 종료
        if (_isCasting || _isHolding)           //이미 캐스팅 중일시 캐스팅 종료
        {
            EndCasting();
            return;
        }

        if (currentMagicData != null) // 마법이 있다면 마법 샐행
        {
            _castingLetter = currentMagicData.GetCastingLetter().GetLetterList(); //캐스팅 글자 받아오기
            _castingIndex = 0;
            _castingTime = currentMagicData.CastingTime;

            StartCoroutine("CastingCoroutine"); //캐스팅 시간 타이머 작동

            _castingGauge.Open();
            _letterViewer.Open();
            _letterViewer.SetLetters(_castingLetter);
            _castingGauge.SetGauge(1);   //UI 실행


            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0f));
            Vector2 lookMouseDir = (worldPos - (Vector2)transform.position).normalized;

            currentMagic = currentMagicData.UseMagic();
            currentMagic.SetPositionWithDirection((Vector2)transform.position + lookMouseDir, lookMouseDir); //마법 생성
            currentMagic.NextPhase(MagicPhase.Casting);
        }
        _isCasting = true;
        _isHolding = false;
    }

    private IEnumerator CastingCoroutine()
    {
        yield return new WaitUntil(() => _castingTime <= 0);
        EndCasting();
    }

    private void Update()
    {
        UpdateCastingGauge();
        CastingMagic();
        HoldingMagic();
        CastingKeyInput();
        HoldingKeyInput();
    }

    private void UpdateCastingGauge()
    {
        _castingTime -= Time.deltaTime;
        _castingGauge.SetGauge(_castingTime / currentMagicData.CastingTime);
    }

    private void CastingMagic()
    {
        if (!_isCasting || _isHolding) return;
        if (_castingLetter[_castingIndex] == "END")
        {
            currentMagic.NextPhase(MagicPhase.Success);

            if (currentMagicData.CanHold)
            {
                Debug.Log("새로운 주문 영창");
                _castingLetter = currentMagicData.GetCastingLetter().GetLetterList();
                _castingIndex = 0;
                _letterViewer.SetLetters(_castingLetter);
                _castingTime += 1.5f;
                _isCasting = false;
                _isHolding = true;
                currentMagic.NextPhase(MagicPhase.Hold);
            }
            else
            {
                EndCasting();
                StopCoroutine("CastingCoroutine");
            }
        }
    }
    private void HoldingMagic()
    {
        if (_isHolding && _castingLetter[_castingIndex] == "END")
        {
            _castingLetter = currentMagicData.GetCastingLetter().GetLetterList();
            _castingIndex = 0;
            _letterViewer.SetLetters(_castingLetter);
            _castingTime += 1.5f;
            _isHolding = true;
            currentMagic.NextPhase(MagicPhase.Hold);
        }
    }

    private void CastingKeyInput()
    {
        if (!_isCasting) return;
        if (Keyboard.current == null) return; // 키보드 없으면 무시
        foreach (var key in Keyboard.current.allKeys)
        {
            if (key == null) continue;  // null인 키는 건너뜀

            if (key.wasPressedThisFrame) // 이번 프레임에 눌린 키 확인
            {
                //Debug.Log($"Pressed Key: {key.displayName} / {key.keyCode}");
                if (_castingLetter[_castingIndex] == key.displayName)
                {
                    _letterViewer.Press(_castingIndex);
                    _castingIndex++;
                }
                else
                {
                    _castingTime -= 1f;
                }
            }
        }
    }
    private void HoldingKeyInput()
    {
        if (!_isHolding) return;
        if (Keyboard.current == null) return; // 키보드 없으면 무시
        foreach (var key in Keyboard.current.allKeys)
        {
            if (key == null) continue;  // null인 키는 건너뜀

            if (key.wasPressedThisFrame) // 이번 프레임에 눌린 키 확인
            {
                //Debug.Log($"Pressed Key: {key.displayName} / {key.keyCode}");
                if (_castingLetter[_castingIndex] == key.displayName)
                {
                    _letterViewer.Press(_castingIndex);
                    _castingIndex++;
                    _castingTime += 1;
                }
                else
                {
                    EndCasting();
                }
            }
        }
    }

    private void EndCasting()
    {
        _castingGauge.Close();
        _letterViewer.Close();
        _isCasting = false;
        _isHolding = false;
        _lastCastingTime = Time.time;
        currentMagic.NextPhase(MagicPhase.Fail);
    }

}