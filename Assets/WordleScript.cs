using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class WordleScript : MonoBehaviour
{
    [SerializeField] private TextAsset _wordsFile;
    [SerializeField] private GuessParent _guessParent;
    
    private int _charsPerWord = 5;
    private int _guessesAllowed = 5;

    private bool _gameOver;
    private int _guessesMade;
    
    public string targetWord;
    public List<string> guesses;

    void InitializeWordDictionary()
    {
        string content = _wordsFile.text;
        string[] words = content.Split("\n");
        WordleDictionary.SetWords(words);
    }
    
    void NewGame()
    {
        _gameOver = false;
        _guessesMade = 0;
        targetWord = WordleDictionary.GetRandomWord();
        _guessParent.InitializeGuesses(_guessesAllowed, _charsPerWord);
        
    }

    public void MakeGuess(string guess)
    {
        guess = guess.ToLower().Trim(); // clean guess
        
        if (_gameOver)
        {
            return;
        }

        if (guess.Length != _charsPerWord)
        {
            return;
        }

        if (guesses.Contains(guess))
        {
            return; // avoid same guess
        }
        
        _guessParent.SetGuess(_guessesMade, guess, targetWord);
        _guessesMade += 1;
        guesses.Add(guess);

        if (guess == targetWord.ToLower().Trim())
        {
            Winner();
        }
        else if (_guessesMade == _guessesAllowed)
        {
            Loser();
        }
    }

    void Winner()
    {
        _gameOver = true;
        Debug.Log("Winner!");
    }

    void Loser()
    {
        _gameOver = true;
        Debug.Log("Loser!");
    }
    
    void Awake()
    {
        InitializeWordDictionary();
    }

    void Start()
    {
        NewGame();
        
    }
}
