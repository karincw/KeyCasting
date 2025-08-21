using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetterUI : MonoBehaviour
{
    [SerializeField] private Sprite _white;
    [SerializeField] private Sprite _black;

    public bool isComplete
    {
        set
        {
            if (value)
            {
                _image.sprite = _black;
            }
        }
    }


    private Image _image;
    private TMP_Text _text;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _text = GetComponentInChildren<TMP_Text>();
    }

    public void SetUp(string text)
    {
        _image.sprite = _white;
        _text.text = text;
    }
}