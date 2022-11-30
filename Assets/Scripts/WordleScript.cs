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

    [Header("Settings")] [Range(1, 10)] [SerializeField]
    private int _guessesAllowed = 5;

    private bool _gameOver;
    private int _charsPerWord;
    private string _targetWord;

    private HashSet<string> _guesses;
    private Dictionary<string, HashSet<char>> _characters;

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

        _characters = new Dictionary<string, HashSet<char>>();
        _characters.Add("wrong", new HashSet<char>());
        _characters.Add("semi", new HashSet<char>());
        _characters.Add("correct", new HashSet<char>());
        _guesses = new HashSet<string>();
    }

    void Start()
    {
        _guessParent.InitializeGuesses(_guessesAllowed, _charsPerWord);
        NewGame();
    }


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
        _characters["wrong"].Clear();
        _characters["semi"].Clear();
        _characters["correct"].Clear();
        _guesses.Clear();
        StatusTextScript.Instance.Hide();
        Keyboard.Instance.CleanUp();
        Keyboard.Instance.EnableKeyboard();
        _gameOver = false;

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

        if (_guesses.Contains(guess))
        {
            StatusTextScript.Instance.ShowMessage("You already guessed that!", Color.white, 2f);
            return; // avoid same guess
        }

        _guessParent.SetGuess(_guesses.Count, guess, _targetWord);
        _guesses.Add(guess);
        CheckGuess(guess);


        if (guess == _targetWord)
        {
            Winner();
        }
        else if (_guesses.Count == _guessesAllowed)
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

    public void AddChar(string set, char c)
    {
        if (_characters[set].Contains(c))
        {
            return;
        }

        _characters[set].Add(c);
    }

    public HashSet<char> GetCharSet(string set)
    {
        return _characters[set];
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
                AddChar("correct", guess[i]);
            }
            else if (_targetWord.Contains(guess[i]))
            {
                AddChar("semi", guess[i]);
            }
            else
            {
                AddChar("wrong", guess[i]);
            }
        }
    }
}