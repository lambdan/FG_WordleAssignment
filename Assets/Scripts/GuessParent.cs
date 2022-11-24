using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuessParent : MonoBehaviour
{
    [SerializeField] private GameObject _guessPrefab;

    private List<GameObject> _guessObjects = new List<GameObject>();
    private List<Button> _guessObjectsButtons = new List<Button>();
    private List<TMP_Text> _guessObjectsTexts = new List<TMP_Text>();
    private int _settingCharsPerWord;
    private Rect _tileRect;

    void Awake()
    {
        _tileRect = _guessPrefab.GetComponent<RectTransform>().rect;
    }
    
    private void CleanUp()
    {
        foreach (GameObject go in _guessObjects)
        {
            Destroy(go);
        } 
        foreach (Button bo in _guessObjectsButtons)
        {
            Destroy(bo);
        }
        foreach (TMP_Text to in _guessObjectsTexts)
        {
            Destroy(to);
        }
        
        _guessObjects.Clear();
        _guessObjectsButtons.Clear();
        _guessObjectsTexts.Clear();
    }
    
    public void InitializeGuesses(int guessAmount, int charsPerWord)
    {
        CleanUp();

        float buttonWidth = _tileRect.width;
        float buttonHeight = _tileRect.height;
        float y_offset = 0;

        float startX = transform.position.x - (0.5f * charsPerWord * buttonWidth);
        
        for (int i = 0; i < guessAmount; i++)
        {
            for (int c = 0; c < charsPerWord; c++)
            {
                GameObject go = Instantiate(_guessPrefab, this.transform);
                go.name = "Tile " + i + "-" + c;

                Button bo = go.GetComponentInChildren<Button>();

                TMP_Text to = go.GetComponentInChildren<TMP_Text>();
                to.text = "";

                go.transform.position = new Vector3(startX + buttonWidth * c + (buttonWidth*0.5f), transform.position.y + y_offset,
                    transform.position.z);

                _guessObjects.Add(go);
                _guessObjectsButtons.Add(bo);
                _guessObjectsTexts.Add(to);
            }
            y_offset -= buttonHeight;
        }

        _settingCharsPerWord = charsPerWord;
    }

    public void SetGuess(int guessNo, string guess, string correctWord)
    {
        int indexOffset = (guessNo * _settingCharsPerWord);

        for (int i = 0; i < _settingCharsPerWord; i++)
        {
            _guessObjectsTexts[indexOffset + i].text = guess[i].ToString().ToUpper();

            if (guess[i].ToString().ToUpper() == correctWord[i].ToString().ToUpper())
            {
                SetColor(indexOffset + i, WordleScript.Instance.Colors()["correct"]);
            } else if (correctWord.ToUpper().Contains(guess[i].ToString().ToUpper()))
            {
                SetColor(indexOffset + i, WordleScript.Instance.Colors()["semi"]);
            }
            else
            {
                SetColor(indexOffset + i, WordleScript.Instance.Colors()["wrong"]);
            }
        }
    }

    public void SetColor(int idx, Color col)
    {
        ColorBlock cb = _guessObjectsButtons[idx].colors;
        cb.disabledColor = col;
        _guessObjectsButtons[idx].colors = cb;
    }
}
