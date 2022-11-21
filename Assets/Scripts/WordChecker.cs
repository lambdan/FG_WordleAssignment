using UnityEngine;

[RequireComponent(typeof(WordleScript))]
public class WordChecker : MonoBehaviour
{ 
    public void CheckGuess(string guess)
    {
        string correct = WordleScript.Instance.TargetWord().ToUpper().Trim();
        guess = guess.ToUpper().Trim();

        for (int i = 0; i < guess.Length; i++)
        {
            if (guess[i] == correct[i])
            {
                WordleScript.Instance.AddCorrectChar(guess[i]);
            } else if (correct.Contains(guess[i]))
            {
                WordleScript.Instance.AddSemiChar(guess[i]);
            }
            else
            {
                WordleScript.Instance.AddWrongChar(guess[i]);
            }
        }
    }
}
