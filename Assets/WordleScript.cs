using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(WordChecker))]
public class WordleScript : MonoBehaviour
{
    [SerializeField] private Color _wrongColor;
    [SerializeField] private Color _semiColor;
    [SerializeField] private Color _correctColor;
    [Space] 
    [SerializeField] private TMP_Text _statusText;
    [SerializeField] private TextAsset _wordsFile;
    [SerializeField] private GuessParent _guessParent;
    
    private int _charsPerWord = 5;
    private int _guessesAllowed = 5;

    private bool _gameOver;
    private int _guessesMade;
    private WordChecker _wordChecker;
    private Keyboard _keyboard;
    
    public string targetWord;
    public List<string> guesses;
    
    private List<char> _wrongChars = new List<char>();
    private List<char> _semiChars = new List<char>();
    private List<char> _correctChars = new List<char>();

    void InitializeWordDictionary()
    {
        string content = _wordsFile.text;
        string[] words = content.Split("\n");
        WordleDictionary.SetWords(words);
    }
    
    public void NewGame()
    {
        // cleanup
        _statusText.gameObject.SetActive(false);
        _wrongChars.Clear();
        _semiChars.Clear();
        _correctChars.Clear();
        guesses.Clear();
        _gameOver = false;
        _guessesMade = 0;
        _keyboard.CleanUp();
        _keyboard.EnableKeyboard();
        
        // re init
        targetWord = WordleDictionary.GetRandomWord().ToUpper();
        _guessParent.InitializeGuesses(_guessesAllowed, _charsPerWord);
        
    }

    public void MakeGuess(string guess)
    {        
        if (_gameOver)
        {
            return;
        }
        
        guess = guess.ToUpper().Trim(); // clean guess
        
        if (!WordleDictionary.IsInDictionary(guess))
        {
            _keyboard.Shake();
            return; // word not in dictionary
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
        _wordChecker.CheckGuess(guess);
        

        if (guess == targetWord.ToUpper().Trim())
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
        _keyboard.DisableKeyboard();
        
        _statusText.gameObject.SetActive(true);
        _statusText.text = "Winner!";
        _statusText.color = Colors()["correct"];
    }

    void Loser()
    {
        _gameOver = true;
        _keyboard.DisableKeyboard();
        
        _statusText.gameObject.SetActive(true);
        _statusText.text = "Loser! The word was: " + targetWord;
        _statusText.color = Colors()["wrong"];
    }
    
    void Awake()
    {
        _wordChecker = GetComponent<WordChecker>();
        _keyboard = FindObjectOfType<Keyboard>();
        InitializeWordDictionary();
    }

    void Start()
    {
        NewGame();
        
    }

    public void AddWrongChar(char c)
    {
        if (_wrongChars.Contains(c))
        {
            return;
        }
        _wrongChars.Add(c);
    }

    public void AddSemiChar(char c)
    {
        if (_semiChars.Contains(c))
        {
            return;
        }
        _semiChars.Add(c);
    }

    public void AddCorrectChar(char c)
    {
        if (_correctChars.Contains(c))
        {
            return;
        }
        _correctChars.Add(c);
    }

    public List<char> WrongChars()
    {
        return _wrongChars;
    }

    public List<char> SemiChars()
    {
        return _semiChars;
    }

    public List<char> CorrectChars()
    {
        return _correctChars;
    }

    public Dictionary<string,Color> Colors()
    {
        return new Dictionary<string, Color>()
        {
            { "correct", _correctColor },
            { "semi", _semiColor },
            { "wrong", _wrongColor }
        };
    }

    public int HowManyChars()
    {
        return _charsPerWord;
    }

    public string TargetWord()
    {
        return targetWord;
    }

    public int GuessesAllowed()
    {
        return _guessesAllowed;
    }

    public List<string> GuessesMade()
    {
        return guesses;
    }
}
