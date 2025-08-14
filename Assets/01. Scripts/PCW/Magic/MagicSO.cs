using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Karin/MagicSO")]
public class MagicSO : ScriptableObject
{
    public string magicName;
    public string magicDescription;
    public string animationHash;
    public int Letter;
    public int Mana;
    public float CastingTime;
    public bool CanHold = false;

    [Header("Cast-Settings")]
    public float CastDecreaseValue = 1f;
    public float HoldIncreaseValue = 0.4f;
    public float CastReward = 1.5f;
    public float HoldReward = 1;

    public static string UsableLetters = "QWERASDFZXCV";

    [Header("Magic")]
    [SerializeField] Magic _magicPrefab;

    public Magic UseMagic()
    {
        return Instantiate(_magicPrefab);
    }

    public virtual Letter GetCastingLetter()
    {
        Letter result = new Letter();
        StringBuilder stringBuilder = new StringBuilder();
        int randomIndex = 0;

        for (int i = 0; i < Letter; i++)
        {
            randomIndex = Random.Range(0, UsableLetters.Length);
            stringBuilder.Append(MagicSO.UsableLetters[randomIndex]);
        }
        
        result.letter = stringBuilder.ToString();
        return result;
    }
}