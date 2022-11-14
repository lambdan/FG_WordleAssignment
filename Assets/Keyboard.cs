using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Keyboard : MonoBehaviour
{
    [SerializeField] private Color _defaultColor;
    [SerializeField] private GameObject characterKey;
    [SerializeField] private GameObject enterKey;
    [SerializeField] private GameObject backspaceKey;

    private WordleScript _gameManager;
    private InputPreview _inputPreview;
    private RectTransform _rt;

    private int charIndex = 0;
    private char[] entry;

    private List<Button> _buttons = new List<Button>();

    private bool _activated;


    private List<char> keyboardLayout = new List<char>()
    {
        'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P',
        'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L',
        'Z', 'X', 'C', 'V', 'B', 'N', 'M'
    };

    void GenerateKeyboard()
    {
        float start_x = _rt.rect.width / 2.5f;
        float start_y = _rt.rect.height / 3f;

        float x_offset = -start_x;
        float y_offset = +start_y;

        foreach (char c in keyboardLayout)
        {
            Debug.Log(c);
            char letter = c;
            GameObject go = Instantiate(characterKey, transform);
            go.transform.position = new Vector2(transform.position.x + x_offset, transform.position.y + y_offset);
            go.GetComponentInChildren<TMP_Text>().text = letter.ToString();

            go.GetComponent<Button>().onClick.AddListener(() => { PressedCharacterKey(letter); });

            _buttons.Add(go.GetComponent<Button>());

            if (c == 'P')
            {
                // new row
                x_offset = -start_x;
                y_offset -= 32f;
            }
            else if (c == 'L')
            {
                x_offset += 32f;
                GameObject backspaceKeyGO = Instantiate(backspaceKey, transform);
                backspaceKeyGO.transform.position =
                    new Vector2(transform.position.x + x_offset, transform.position.y + y_offset);
                backspaceKeyGO.GetComponent<Button>().onClick.AddListener(PressedBackspaceKey);

                y_offset -= 32f;
                x_offset = -start_x;
            }
            else if (c == 'M')
            {
                x_offset += 64f;
                GameObject enterKeyGO = Instantiate(enterKey, transform);
                enterKeyGO.transform.position =
                    new Vector2(transform.position.x + x_offset, transform.position.y + y_offset);
                enterKeyGO.GetComponent<Button>().onClick.AddListener(PressedEnterKey);
            }
            else
            {
                x_offset += 32f;
            }
        }
    }

    void UpdateButtonColors()
    {
        for (int i = 0; i < keyboardLayout.Count; i++)
        {
            char c = keyboardLayout[i];
            if (_gameManager.CorrectChars().Contains(c))
            {
                SetColor(i, _gameManager.Colors()["correct"]);
            }
            else if (_gameManager.SemiChars().Contains(c))
            {
                SetColor(i, _gameManager.Colors()["semi"]);
            }
            else if (_gameManager.WrongChars().Contains(c))
            {
                SetColor(i, _gameManager.Colors()["wrong"]);
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
        entry = new char[_gameManager.HowManyChars()];
        for (int i = 0; i < entry.Length; i++)
        {
            _inputPreview.SetCharacter(i, ' ');
        }

        charIndex = 0;
    }
    
    void Awake()
    {
        _gameManager = FindObjectOfType<WordleScript>();
        _inputPreview = FindObjectOfType<InputPreview>();
        _rt = GetComponent<RectTransform>(); 
        GenerateKeyboard();

        CleanUp();
        
    }

    public void PressedCharacterKey(char c)
    {
        if (charIndex >= _gameManager.HowManyChars())
        {
            return;
        }

        if (!_activated)
        {
            return;
        }

        entry[charIndex] = c;
        
        // Debug.Log(c);
        _inputPreview.SetCharacter(charIndex, c);
        charIndex += 1;
    }

    public void PressedEnterKey()
    {
        if (!_activated)
        {
            return;
        }
        _gameManager.MakeGuess(entry.ArrayToString());
        UpdateButtonColors();
        ClearEntry();
    }

    public void PressedBackspaceKey()
    {
        if (!_activated)
        {
            return;
        }

        charIndex -= 1;
        if (charIndex <= 0)
        {
            charIndex = 0;
        }
        
        _inputPreview.SetCharacter(charIndex, ' ');
    }
    
    public void SetColor(int idx, Color col)
    {
        ColorBlock cb = _buttons[idx].colors;
        //cb.disabledColor = col;
        cb.normalColor = col;
        _buttons[idx].colors = cb;
    }

    IEnumerator KeyboardShake(float duration)
    {
        Vector3 startPos = transform.position;
        float timeStarted = Time.time;

        bool lastRight = false;
        
        while (Time.time < (timeStarted + duration))
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
    }
}
