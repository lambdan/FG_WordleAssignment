using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class GuessParent : MonoBehaviour
{
    [SerializeField] private Color _wrongColor;
    [SerializeField] private Color _semiColor;
    [SerializeField] private Color _correctColor;
    
    [SerializeField] private GameObject _guessPrefab;

    private List<GameObject> _guessObjects = new List<GameObject>();
    private List<Button> _guessObjectsButtons = new List<Button>();
    private List<TMP_Text> _guessObjectsTexts = new List<TMP_Text>();

    private int _settingCharsPerWord;

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

        float x_width = charsPerWord * 30f; // TODO read size from prefab
        float y_offset = 0;
        for (int i = 0; i < guessAmount; i++)
        {
            for (int c = 0; c < charsPerWord; c++)
            {
                float x_offset = -(x_width / 2) + (c * 30f);
                GameObject go = Instantiate(_guessPrefab, this.transform);
                go.name = "Tile " + i + "-" + c;

                Button bo = go.GetComponentInChildren<Button>();

                TMP_Text to = go.GetComponentInChildren<TMP_Text>();
                to.text = "";

                go.transform.position = new Vector3(transform.position.x + x_offset, transform.position.y + y_offset,
                    transform.position.z);

                _guessObjects.Add(go);
                _guessObjectsButtons.Add(bo);
                _guessObjectsTexts.Add(to);
            }
            y_offset -= 35;
        }

        _settingCharsPerWord = charsPerWord;
    }

    public void SetGuess(int guessNo, string guess, string correctWord)
    {
        int indexOffset = (guessNo * _settingCharsPerWord);

        for (int i = 0; i < _settingCharsPerWord; i++)
        {
            _guessObjectsTexts[indexOffset + i].text = guess[i].ToString().ToUpper();

            if (guess[i].ToString().ToLower() == correctWord[i].ToString().ToLower())
            {
                SetColor(indexOffset + i, _correctColor);
            } else if (correctWord.ToLower().Contains(guess[i].ToString().ToLower()))
            {
                SetColor(indexOffset + i, _semiColor);
            }
            else
            {
                SetColor(indexOffset + i, _wrongColor);
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
