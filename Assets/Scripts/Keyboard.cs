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
    
    public static Keyboard Instance { get; private set; }
    private InputPreview _inputPreview;
    private RectTransform _rt;

    private int charIndex; // = 0
    private char[] entry;

    private List<Button> _buttons = new List<Button>();

    private bool _activated;


    private char[] keyboardLayout = new char[]
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
            // Debug.Log(c);
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
        for (int i = 0; i < keyboardLayout.Length; i++)
        {
            char c = keyboardLayout[i];
            if (WordleScript.Instance.CorrectChars().Contains(c))
            {
                SetColor(i, WordleScript.Instance.Colors()["correct"]);
            }
            else if (WordleScript.Instance.SemiChars().Contains(c))
            {
                SetColor(i, WordleScript.Instance.Colors()["semi"]);
            }
            else if (WordleScript.Instance.WrongChars().Contains(c))
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
        entry = new char[WordleScript.Instance.HowManyChars()];
        for (int i = 0; i < entry.Length; i++)
        {
            _inputPreview.SetCharacter(i, ' ');
        }

        charIndex = 0;
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

        _inputPreview = FindObjectOfType<InputPreview>();
        _rt = GetComponent<RectTransform>();
        GenerateKeyboard();
    }

    void Start()
    {
        
        CleanUp();
    }

    public void PressedCharacterKey(char c)
    {
        if (charIndex >= WordleScript.Instance.HowManyChars())
        {
            return;
        }

        if (!_activated)
        {
            return;
        }

        entry[charIndex] = c;
        _inputPreview.SetCharacter(charIndex, c);
        charIndex += 1;
    }

    public void PressedEnterKey()
    {
        if (!_activated)
        {
            return;
        }
        WordleScript.Instance.MakeGuess(entry.ArrayToString());
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
    
    public void SetColor(int keyIndex, Color col)
    {
        ColorBlock cb = _buttons[keyIndex].colors;
        cb.normalColor = col;
        _buttons[keyIndex].colors = cb;
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
