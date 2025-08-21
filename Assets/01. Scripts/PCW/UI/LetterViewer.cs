using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class LetterViewer : MonoBehaviour
{
    [SerializeField] private List<LetterUI> _letterUI;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
    }

    public void Open(float time = 0.3f)
    {
        _canvasGroup.DOComplete();
        _canvasGroup.DOFade(1, time);
    }
    public void Close(float time = 0.3f)
    {
        _canvasGroup.DOFade(0, time);
    }

    public void SetLetters(List<string> letters)
    {
        int letterLen = letters.Count - 1;
        for (int i = 0; i < letterLen; i++)
        {
            _letterUI[i].gameObject.SetActive(true);
            _letterUI[i].SetUp(letters[i]);
        }
        for (int i = letterLen; i < _letterUI.Count; i++)
        {
            _letterUI[i].gameObject.SetActive(false);
        }
    }

    public void Press(int index)
    {
        _letterUI[index].isComplete = true;
    }
}
