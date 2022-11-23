using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WordChecker))]
public class WordleScript : MonoBehaviour
{
    [SerializeField] private Color _wrongColor;
    [SerializeField] private Color _semiColor;
    [SerializeField] private Color _correctColor;
    [Space]
    [SerializeField] private TextAsset _wordsFile;
    [SerializeField] private GuessParent _guessParent;

    public static WordleScript Instance { get; private set; }

    [SerializeField] private int _charsPerWord = 5;
    [SerializeField] private int _guessesAllowed = 6;

    private bool _gameOver;
    private int _guessesMade;
    private WordChecker _wordChecker;

    private string targetWord;
    
    private List<string> guesses = new List<string>();
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
        StatusTextScript.Instance.Hide();
        _wrongChars.Clear();
        _semiChars.Clear();
        _correctChars.Clear();
        guesses.Clear();
        _gameOver = false;
        _guessesMade = 0;
        Keyboard.Instance.CleanUp();
        Keyboard.Instance.EnableKeyboard();
        
        // re init
        targetWord = WordleDictionary.GetRandomWord().ToUpper();
        Debug.Log("targetWord: " + targetWord);
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
            Keyboard.Instance.Shake();
            StatusTextScript.Instance.ShowMessage("Word not in dictionary", Color.white, 2f);
            return; // word not in dictionary
        }

        if (guess.Length != _charsPerWord)
        {
            return;
        }
        
        if (guesses.Contains(guess))
        {
            StatusTextScript.Instance.ShowMessage("You already guessed that!", Color.white, 2f);
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

    private void Winner()
    {
        _gameOver = true;
        Keyboard.Instance.DisableKeyboard();
        StatusTextScript.Instance.ShowMessage("Winner!", Colors()["correct"]);
    }

    private void Loser()
    {
        _gameOver = true;
        Keyboard.Instance.DisableKeyboard();
        StatusTextScript.Instance.ShowMessage("Loser! The word was: " + targetWord, Colors()["wrong"]);
    }
    
    void Awake()
    {
         if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        
        _wordChecker = GetComponent<WordChecker>();
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
