using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MagicTableUI : MonoBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private RectTransform _contentsTrm;
    private Casting _casting;

    private List<MagicUI> _contents = new();

    private readonly int _moveInterval = 136;
    private int _currentIdx;

    private void Awake()
    {
        _contentsTrm.GetComponentsInChildren<MagicUI>(_contents);
        _casting = FindFirstObjectByType<Casting>();
        _currentIdx = 0;

        _inputReader.OnMagicChangeInput += HandleMagicChange;
    }

    private void OnDestroy()
    {
        _inputReader.OnMagicChangeInput -= HandleMagicChange;
    }

    private void HandleMagicChange(bool state)
    {
        if (state)
            MoveRight();
        else
            MoveLeft();
    }

    private void MoveLeft()
    {
        if (_currentIdx == 0) return;
        _contentsTrm.DOAnchorPos(_contentsTrm.anchoredPosition + new Vector2(_moveInterval, 0), 0.5f);
        _contents[_currentIdx].transform.DOKill();
        _contents[_currentIdx].transform.DOScale(0.5f, 0.5f);
        _currentIdx--;
        _contents[_currentIdx].transform.DOKill();
        _contents[_currentIdx].transform.DOScale(1, 0.5f)
            .OnComplete(() =>
            {
                _casting.currentMagicData = _contents[_currentIdx].magicData;
            });
    }
    private void MoveRight()
    {
        if (_currentIdx == _contents.Count - 1) return;
        _contentsTrm.DOAnchorPos(_contentsTrm.anchoredPosition - new Vector2(_moveInterval, 0), 0.5f);
        _contents[_currentIdx].transform.DOKill();
        _contents[_currentIdx].transform.DOScale(0.5f, 0.5f);
        _currentIdx++;
        _contents[_currentIdx].transform.DOKill();
        _contents[_currentIdx].transform.DOScale(1, 0.5f)
            .OnComplete(() =>
            {
                _casting.currentMagicData = _contents[_currentIdx].magicData;
            });
    }
}
