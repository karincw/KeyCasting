

using System.Collections.Generic;

public class Letter
{
    public string letter;

    public List<string> GetLetterList()
    {
        List<string> letters = new List<string>();
        for (int i = 0; i < letter.Length; i++)
        {
            letters.Add(letter[i].ToString());
        }
        letters.Add("END");
        return letters;
    }
}