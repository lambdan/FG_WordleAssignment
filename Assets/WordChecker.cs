using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WordleScript))]
public class WordChecker : MonoBehaviour
{
    private WordleScript _gameManager;
    void Awake()
    {
        _gameManager = GetComponent<WordleScript>();
    }
    
    public void CheckGuess(string guess)
    {
        string correct = _gameManager.TargetWord().ToUpper().Trim();
        guess = guess.ToUpper().Trim();

        for (int i = 0; i < guess.Length; i++)
        {
            if (guess[i] == correct[i])
            {
                _gameManager.AddCorrectChar(guess[i]);
            } else if (correct.Contains(guess[i]))
            {
                _gameManager.AddSemiChar(guess[i]);
            }
            else
            {
                _gameManager.AddWrongChar(guess[i]);
            }
        }
    }
}
