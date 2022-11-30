using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuessParent : MonoBehaviour
{
    public class GuessTile
    {
        public GameObject gameObj;
        public Button buttonObj;
        public TMP_Text textObj;
    }
    
    
    [SerializeField] private GameObject _guessTilePrefab;
    private int _settingCharsPerWord;
    private Rect _tileRect;
    private Color _defaultTileColor;

    private Dictionary<int, Dictionary<int, GuessTile>> _tiles;

    void Awake()
    {
        _tileRect = _guessTilePrefab.GetComponent<RectTransform>().rect;
        _defaultTileColor = _guessTilePrefab.GetComponent<Button>().colors.disabledColor;
    }
    
    public void Clear()
    {
        for (int i = 0; i < _tiles.Count; i++)
        {
            for (int j = 0; j < _tiles[i].Count; j++)
            {
                _tiles[i][j].textObj.text = "";
                SetColor(_tiles[i][j].buttonObj, _defaultTileColor);
            }
        }
    }
    
    public void InitializeGuesses(int guessAmount, int charsPerWord)
    {
        _tiles = new Dictionary<int, Dictionary<int, GuessTile>>();

        float buttonWidth = _tileRect.width;
        float buttonHeight = _tileRect.height;
        float y_offset = 0;

        float startX = transform.position.x - (0.5f * charsPerWord * buttonWidth);
        
        for (int i = 0; i < guessAmount; i++)
        {
            Dictionary<int, GuessTile> row = new Dictionary<int, GuessTile>();

            for (int c = 0; c < charsPerWord; c++)
            {
                GuessTile tile = new GuessTile();
                
                
                tile.gameObj = Instantiate(_guessTilePrefab, this.transform);
                tile.gameObj.name = "Tile " + i + "-" + c;

                tile.buttonObj = tile.gameObj.GetComponentInChildren<Button>();

                tile.textObj = tile.gameObj.GetComponentInChildren<TMP_Text>();
                tile.textObj.text = "";

                tile.gameObj.transform.position = new Vector3(startX + buttonWidth * c + (buttonWidth*0.5f), transform.position.y + y_offset,
                    transform.position.z);
                
                row.Add(c, tile);
            }
            y_offset -= buttonHeight;

            _tiles.Add(i, row);
        }

        _settingCharsPerWord = charsPerWord;
    }

    public void SetGuess(int guessNo, string guess, string correctWord)
    {
        for (int i = 0; i < _settingCharsPerWord; i++)
        {
            _tiles[guessNo][i].textObj.text = guess[i].ToString().ToUpper();

            if (guess[i].ToString().ToUpper() == correctWord[i].ToString().ToUpper())
            {
                SetColor(_tiles[guessNo][i].buttonObj, WordleScript.Instance.Colors()["correct"]);
            } 
            else if (correctWord.ToUpper().Contains(guess[i].ToString().ToUpper()))
            {
                SetColor(_tiles[guessNo][i].buttonObj, WordleScript.Instance.Colors()["semi"]);
            }
            else
            {
                SetColor(_tiles[guessNo][i].buttonObj, WordleScript.Instance.Colors()["wrong"]);
            }
        }
    }

    public void SetColor(Button buttonObj, Color col)
    {
        ColorBlock cb = buttonObj.colors;
        cb.disabledColor = col;
        buttonObj.colors = cb;
    }
}
