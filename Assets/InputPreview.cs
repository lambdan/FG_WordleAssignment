using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputPreview : MonoBehaviour
{
    [SerializeField] private GameObject _inputPreviewButton;
    
    private List<TMP_Text> _tileTexts = new List<TMP_Text>();
    private WordleScript _gameManager;
    private RectTransform _rt;
    private float _buttonSize;

    void Awake()
    {
        _rt = GetComponent<RectTransform>();
        _gameManager = FindObjectOfType<WordleScript>();
        _buttonSize = _inputPreviewButton.GetComponent<RectTransform>().rect.width;
        GenerateTiles();
    }

    void GenerateTiles()
    {
        float x_offset = -(_gameManager.HowManyChars() * _buttonSize * 0.5f);
        for (int i = 0; i < _gameManager.HowManyChars(); i++)
        {
            GameObject go = Instantiate(_inputPreviewButton, transform);
            go.transform.position = new Vector2(transform.position.x + x_offset, transform.position.y);
            x_offset += _buttonSize;

            TMP_Text to = go.GetComponentInChildren<TMP_Text>();
            to.text = "";
            _tileTexts.Add(to);
        }
    }

    public void SetCharacter(int idx, char c)
    {
        if (_tileTexts.Count > 0)
        {
            _tileTexts[idx].text = c.ToString();
        }
        
    }
    
    
}
