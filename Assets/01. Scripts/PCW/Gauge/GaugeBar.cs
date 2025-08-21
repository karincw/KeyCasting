using DG.Tweening;
using UnityEngine;

public class GaugeBar : MonoBehaviour
{
    [SerializeField] private Transform _gaugeBarTrm;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
    }

    public void Open(float time = 0.3f)
    {
        _canvasGroup.DOFade(1, time);
    }
    public void Close(float time = 0.3f)
    {
        _canvasGroup.DOFade(0, time);
    }

    public void SetGauge(float value)
    {
        _gaugeBarTrm.localScale = new Vector2(value, 1);
    }
}
