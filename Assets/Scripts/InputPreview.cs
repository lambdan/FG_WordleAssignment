using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputPreview : MonoBehaviour
{
    [SerializeField] private GameObject _inputPreviewButton;
    
    private List<TMP_Text> _tileTexts = new List<TMP_Text>();
    private float _buttonSize;

    void Awake()
    {
        _buttonSize = _inputPreviewButton.GetComponent<RectTransform>().rect.width;
        GenerateTiles();
    }

    void GenerateTiles()
    {
        int howManyChars = WordleScript.Instance.HowManyChars();
        
        float buttonWidth = _inputPreviewButton.GetComponent<RectTransform>().rect.width;
        // float buttonHeight = _inputPreviewButton.GetComponent<RectTransform>().rect.height;
        
        float x_offset = -(howManyChars * buttonWidth * 0.5f);
        for (int i = 0; i < howManyChars; i++)
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
