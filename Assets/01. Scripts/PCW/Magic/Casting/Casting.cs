using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Casting : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private GaugeBar _castingGauge;
    [SerializeField] private LetterViewer _letterViewer;

    public MagicSO currentMagicData;

    private Magic currentMagic;
    private PlayerAnimator _playerAnimator;

    private List<string> _castingLetter;
    private int _castingIndex;

    [SerializeField] private float castingCooltime = 2.5f;
    private float _lastCastingTime = -100;
    private float _castingTime;

    private bool _canCasting => Time.time > _lastCastingTime + castingCooltime;
    private bool _isCasting = false;
    private bool _isHolding = false;

    private void Awake()
    {
        _castingLetter = new();
        _playerAnimator = GetComponentInChildren<PlayerAnimator>();
        _inputReader.OnCastingInput += OnCasting;
    }

    private void OnDestroy()
    {
        _inputReader.OnCastingInput -= OnCasting;
    }

    private void OnCasting()
    {
        if (!_canCasting) return; //ĳ���� �Ұ��ɽ� ����
        if (_isCasting)           //�̹� ĳ���� ���Ͻ� ĳ���� ����
        {
            EndCasting(MagicPhase.Destroy);
            _playerAnimator.CastingFail();
            return;
        }
        if (_isHolding)
        {
            EndCasting(MagicPhase.End);
            _playerAnimator.CastingFail();
            return;
        }

        if (currentMagicData != null) // ������ �ִٸ� ���� ����
        {
            _playerAnimator.Clear();
            _castingLetter = currentMagicData.GetCastingLetter().GetLetterList(); //ĳ���� ���� �޾ƿ���
            _castingIndex = 0;
            _castingTime = currentMagicData.CastingTime;

            StartCoroutine("CastingCoroutine"); //ĳ���� �ð� Ÿ�̸� �۵�

            _castingGauge.Open();
            _letterViewer.Open();
            _letterViewer.SetLetters(_castingLetter);
            _castingGauge.SetGauge(1);   //UI ����

            
            Vector2 lookMouseDir = (Utils.GetMousePos() - (Vector2)transform.position).normalized;

            currentMagic = currentMagicData.UseMagic();
            currentMagic.SetPositionWithDirection((Vector2)transform.position + lookMouseDir, lookMouseDir); //���� ����
            currentMagic.Flip(_playerAnimator.IsFlip);
            currentMagic.NextPhase(MagicPhase.Casting);

            _playerAnimator.SetTrigger(currentMagicData.animationHash);
            _isCasting = true;
            _isHolding = false;
        }
    }

    private IEnumerator CastingCoroutine()
    {
        yield return new WaitUntil(() => _castingTime <= 0);
        EndCasting(MagicPhase.Fail);
        _playerAnimator.CastingFail();
    }

    private void Update()
    {
        LookUpMouse();
        UpdateCastingGauge();
        CastingMagic();
        HoldingMagic();
        CastingKeyInput();
        HoldingKeyInput();
    }

    private void LookUpMouse()
    {
        if (!(_isCasting || _isHolding)) return;
        Vector3 mousePos = Utils.GetMousePos();
        _playerAnimator.SetViewDirection((mousePos - transform.position).normalized );
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
            currentMagic.Flip(_playerAnimator.IsFlip);
            _playerAnimator.CastingSuccess();

            if (currentMagicData.CanHold)
            {
                _castingLetter = currentMagicData.GetCastingLetter().GetLetterList();
                _castingIndex = 0;
                _letterViewer.SetLetters(_castingLetter);
                _castingTime += currentMagicData.CastReward;
                _isCasting = false;
                _isHolding = true;
                currentMagic.NextPhase(MagicPhase.Hold);
            }
            else
            {
                EndCasting(MagicPhase.End);
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
            _castingTime += currentMagicData.HoldReward;
            _isHolding = true;
            currentMagic.NextPhase(MagicPhase.Hold);
        }
    }

    private void CastingKeyInput()
    {
        if (!_isCasting) return;
        if (Keyboard.current == null) return; // Ű���� ������ ����
        foreach (var key in Keyboard.current.allKeys)
        {
            if (key == null) continue;  // null�� Ű�� �ǳʶ�

            if (key.wasPressedThisFrame) // �̹� �����ӿ� ���� Ű Ȯ��
            {
                //Debug.Log($"Pressed Key: {key.displayName} / {key.keyCode}");
                if (_castingLetter[_castingIndex] == key.displayName)
                {
                    _letterViewer.Press(_castingIndex);
                    _castingIndex++;
                }
                else
                {
                    _castingTime -= currentMagicData.CastDecreaseValue;
                }
            }
        }
    }
    private void HoldingKeyInput()
    {
        if (!_isHolding) return;
        if (Keyboard.current == null) return; // Ű���� ������ ����
        foreach (var key in Keyboard.current.allKeys)
        {
            if (key == null) continue;  // null�� Ű�� �ǳʶ�

            if (key.wasPressedThisFrame) // �̹� �����ӿ� ���� Ű Ȯ��
            {
                //Debug.Log($"Pressed Key: {key.displayName} / {key.keyCode}");
                if (_castingLetter[_castingIndex] == key.displayName)
                {
                    _letterViewer.Press(_castingIndex);
                    _castingIndex++;
                    _castingTime += currentMagicData.HoldIncreaseValue;
                }
                else
                {
                    EndCasting(MagicPhase.Fail);
                    _playerAnimator.CastingFail();
                }
            }
        }
    }

    private void EndCasting(MagicPhase phase)
    {
        _castingGauge.Close();
        _letterViewer.Close();
        _isCasting = false;
        _isHolding = false;
        _lastCastingTime = Time.time;
        currentMagic.NextPhase(phase);
        StopAllCoroutines();
    }

}