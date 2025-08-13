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
        if (!_canCasting) return; //ĳ���� �Ұ��ɽ� ����
        if (_isCasting || _isHolding)           //�̹� ĳ���� ���Ͻ� ĳ���� ����
        {
            EndCasting();
            return;
        }

        if (currentMagicData != null) // ������ �ִٸ� ���� ����
        {
            _castingLetter = currentMagicData.GetCastingLetter().GetLetterList(); //ĳ���� ���� �޾ƿ���
            _castingIndex = 0;
            _castingTime = currentMagicData.CastingTime;

            StartCoroutine("CastingCoroutine"); //ĳ���� �ð� Ÿ�̸� �۵�

            _castingGauge.Open();
            _letterViewer.Open();
            _letterViewer.SetLetters(_castingLetter);
            _castingGauge.SetGauge(1);   //UI ����


            Vector2 screenPos = Mouse.current.position.ReadValue();
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 0f));
            Vector2 lookMouseDir = (worldPos - (Vector2)transform.position).normalized;

            currentMagic = currentMagicData.UseMagic();
            currentMagic.SetPositionWithDirection((Vector2)transform.position + lookMouseDir, lookMouseDir); //���� ����
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
                Debug.Log("���ο� �ֹ� ��â");
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
                    _castingTime -= 1f;
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