using System.Text;
using UnityEngine;

public abstract class MagicSO : ScriptableObject
{
    public string magicName;
    public int Letter;
    public int Mana;
    public int Damage;
    public float CastingTime;

    private readonly string usableLetters = "QWERTASDFGZXCVB"; 

    public abstract GameObject UseMagic();

    public virtual Letter GetCastingLetter()
    {
        Letter result = new Letter();
        StringBuilder stringBuilder = new StringBuilder();
        int randomIndex = 0;

        for (int i = 0; i < Letter; i++)
        {
            randomIndex = Random.Range(0, usableLetters.Length);
            stringBuilder.Append(usableLetters[randomIndex]);
        }
        
        result.letter = stringBuilder.ToString();
        return result;
    }
}