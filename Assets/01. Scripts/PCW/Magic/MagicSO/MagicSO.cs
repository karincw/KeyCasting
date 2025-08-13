using System.Text;
using UnityEngine;

public abstract class MagicSO : ScriptableObject
{
    public string magicName;
    public string magicDescription;
    public string animationHash;
    public int Letter;
    public int Mana;
    public float CastingTime;
    public bool CanHold = false;

    public static string UsableLetters = "QWERASDFZXCV";  

    public abstract Magic UseMagic();

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