using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    [SerializeField] private Color _defaultColor; 
    [SerializeField] private GameObject _characterKeyPrefab;
    [SerializeField] private GameObject _enterKeyPrefab;
    [SerializeField] private GameObject _backspaceKeyPrefab;
    [SerializeField] private InputPreview _inputPreview;
    
    public static Keyboard Instance { get; private set; }
    private RectTransform _rt;
    private int _charIndex; // = 0
    private char[] _entry;
    private bool _activated;
    private List<Button> _buttons = new List<Button>();
    private bool _shaking;

    private char[] _keyboardLayout = new char[]
    {
        'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P',
        'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L',
        'Z', 'X', 'C', 'V', 'B', 'N', 'M'
    };

    private Rect _rect;
    private Vector2 _characterKeySize;
    private Vector2 _backspaceKeySize;
    private Vector2 _enterKeySize;

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
        
        _rt = GetComponent<RectTransform>();
        
        // cache size of panel
        _rect = _rt.rect;
        
        // cache sizes of keys
        _characterKeySize = _characterKeyPrefab.GetComponent<RectTransform>().rect.size;
        _backspaceKeySize = _backspaceKeyPrefab.GetComponent<RectTransform>().rect.size;
        _enterKeySize = _enterKeyPrefab.GetComponent<RectTransform>().rect.size; 
        
        GenerateKeyboard();
    }

    void Start()
    {
        CleanUp();
    }

    void OnGUI()
    {
        // get input from physical keyboard
        if (Event.current.isKey && Event.current.type == EventType.KeyDown)
        {
            if (Event.current.keyCode != KeyCode.None)
            {
                InterpretKeyboardInput(Event.current.keyCode);
            }
        }
    }

    void InterpretKeyboardInput(KeyCode kc)
    {
        if (kc == KeyCode.Backspace)
        {
            PressedBackspaceKey();
        } else if (kc == KeyCode.Return)
        {
            PressedEnterKey();
        }
        else
        {
            char c = kc.ToString()[0];
            if (_keyboardLayout.Contains(c))
            {
                PressedCharacterKey(c);
            }
        }
        
    }
    
    void GenerateKeyboard()
    {
        float startX = transform.position.x - _rect.size.x * 0.5f + _characterKeySize.x;
        float startY = transform.position.y + (_rect.size.y * 0.5f) - _characterKeySize.y;

        float xOffset = 0;
        float yOffset = 0;

        foreach (char c in _keyboardLayout)
        {
            char letter = c;
            
            GameObject go = Instantiate(_characterKeyPrefab, transform);
            go.name = c + " key";
            go.transform.position = new Vector2(startX + xOffset, startY + yOffset);
            go.GetComponentInChildren<TMP_Text>().text = letter.ToString();
            go.GetComponent<Button>().onClick.AddListener(() => { PressedCharacterKey(letter); });
            _buttons.Add(go.GetComponent<Button>());

            if (c == 'P')
            {
                // new row
                xOffset = 0;
                yOffset -= _characterKeySize.y;
            }
            else if (c == 'L')
            {
                // add backspace button
                xOffset += _backspaceKeySize.x;
                GameObject backspaceKeyGO = Instantiate(_backspaceKeyPrefab, transform);
                backspaceKeyGO.name = "Backspace key";
                backspaceKeyGO.transform.position = new Vector2(startX + xOffset, startY + yOffset);
                backspaceKeyGO.GetComponent<Button>().onClick.AddListener(PressedBackspaceKey);

                // new row
                yOffset -= _characterKeySize.y;
                xOffset = 0;
            }
            else if (c == 'M')
            {
                // last letter: add enter key
                xOffset += _enterKeySize.x - _characterKeySize.x;
                GameObject enterKeyGO = Instantiate(_enterKeyPrefab, transform);
                enterKeyGO.name = "Enter key";
                enterKeyGO.transform.position = new Vector2(startX + xOffset, startY + yOffset);
                enterKeyGO.GetComponent<Button>().onClick.AddListener(PressedEnterKey);
            }
            else
            {
                xOffset += _characterKeySize.x;
            }
        }
    }

    void UpdateButtonColors()
    {
        for (int i = 0; i < _keyboardLayout.Length; i++)
        {
            char c = _keyboardLayout[i];
            if (WordleScript.Instance.GetCharSet("correct").Contains(c))
            {
                SetColor(i, WordleScript.Instance.Colors()["correct"]);
            }
            else if (WordleScript.Instance.GetCharSet("semi").Contains(c))
            {
                SetColor(i, WordleScript.Instance.Colors()["semi"]);
            }
            else if (WordleScript.Instance.GetCharSet("wrong").Contains(c))
            {
                SetColor(i, WordleScript.Instance.Colors()["wrong"]);
            }
        }
    }

    public void DisableKeyboard()
    {
        _activated = false;
    }

    public void EnableKeyboard()
    {
        _activated = true;
    }

    public void Shake()
    {
        StartCoroutine(KeyboardShake(0.75f));
    }

    public void CleanUp()
    {
        ClearEntry();
        // clear keyboard colors
        for (int i = 0; i < _buttons.Count; i++)
        {
            SetColor(i, _defaultColor);
        }
    }
    
    void ClearEntry()
    {
        _entry = new char[WordleScript.Instance.HowManyChars()];
        for (int i = 0; i < _entry.Length; i++)
        {
            _inputPreview.SetCharacter(i, ' ');
        }
        _charIndex = 0;
    }
    
    public void PressedCharacterKey(char c)
    {
        if (_charIndex >= WordleScript.Instance.HowManyChars())
        {
            return;
        }

        if (!_activated)
        {
            return;
        }

        _entry[_charIndex] = c;
        _inputPreview.SetCharacter(_charIndex, c);
        _charIndex += 1;
    }

    public void PressedEnterKey()
    {
        if (!_activated)
        {
            return;
        }
        WordleScript.Instance.MakeGuess(_entry.ArrayToString());
        UpdateButtonColors();
        ClearEntry();
    }

    public void PressedBackspaceKey()
    {
        if (!_activated)
        {
            return;
        }

        _charIndex -= 1;
        if (_charIndex <= 0)
        {
            _charIndex = 0;
        }
        
        _inputPreview.SetCharacter(_charIndex, ' ');
    }
    
    public void SetColor(int keyIndex, Color col)
    {
        ColorBlock cb = _buttons[keyIndex].colors;
        cb.normalColor = col;
        _buttons[keyIndex].colors = cb;

    }

    IEnumerator KeyboardShake(float duration)
    {
        if (_shaking)
        {
            yield break;
        }
        
        _shaking = true;
        Vector3 startPos = transform.position;
        float timeDone = Time.time + duration;

        bool lastRight = false;
        
        while (Time.time < timeDone)
        {
            if (lastRight)
            {
                transform.position = startPos + Vector3.left * Random.Range(0, 5f);
                lastRight = false;
            }
            else
            {
                transform.position = startPos + Vector3.right * Random.Range(0, 5f);
                lastRight = true;
            }

            yield return new WaitForSeconds(duration/20f);
        }
        transform.position = startPos;
        _shaking = false;
    }
}
