using System.Collections.Generic;
using UnityEngine;

public class WordleScript : MonoBehaviour
{
    [SerializeField] private Color _wrongColor;
    [SerializeField] private Color _semiColor;
    [SerializeField] private Color _correctColor;
    [Space] [SerializeField] private TextAsset _wordsFile;
    [SerializeField] private GuessParent _guessParent;

    public static WordleScript Instance { get; private set; }

    [Header("Settings")]
    [Range(1,10)][SerializeField] private int _guessesAllowed = 5;

    private bool _gameOver;
    private int _guessesMade;
    private int _charsPerWord;
    private string _targetWord;

    private List<string> guesses = new List<string>();
    private List<char> _wrongChars = new List<char>();
    private List<char> _semiChars = new List<char>();
    private List<char> _correctChars = new List<char>();

    void InitializeWordDictionary()
    {
        string[] words = _wordsFile.text.Split("\n");
        
        // trim words (to get rid of newlines etc.) and convert to uppercase
        for (int i = 0; i < words.Length; i++)
        {
            words[i] = words[i].Trim().ToUpper();
        }
        _charsPerWord = words[0].Length; // set word length
        
        // give the words to the dictionary
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
        _targetWord = WordleDictionary.GetRandomWord().ToUpper();
        Debug.Log("targetWord: " + _targetWord);
        _guessParent.Clear();
    }

    public void MakeGuess(string guess)
    {
        if (_gameOver)
        {
            return;
        }
        
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

        _guessParent.SetGuess(_guessesMade, guess, _targetWord);
        _guessesMade += 1;
        guesses.Add(guess);
        CheckGuess(guess);


        if (guess == _targetWord)
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
        StatusTextScript.Instance.ShowMessage("Winner!", Color.green);
    }

    private void Loser()
    {
        _gameOver = true;
        Keyboard.Instance.DisableKeyboard();
        StatusTextScript.Instance.ShowMessage("Loser! The word was: " + _targetWord, Color.red);
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

        InitializeWordDictionary();
    }

    void Start()
    {
        _guessParent.InitializeGuesses(_guessesAllowed, _charsPerWord);
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

    public Dictionary<string, Color> Colors()
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

    void CheckGuess(string guess)
    {
        for (int i = 0; i < guess.Length; i++)
        {
            if (guess[i] == _targetWord[i])
            {
                AddCorrectChar(guess[i]);
            }
            else if (_targetWord.Contains(guess[i]))
            {
                AddSemiChar(guess[i]);
            }
            else
            {
                AddWrongChar(guess[i]);
            }
        }
    }
}