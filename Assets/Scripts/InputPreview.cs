using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputPreview : MonoBehaviour
{
    [SerializeField] private GameObject _inputPreviewButton;
    private List<TMP_Text> _tileTexts = new List<TMP_Text>();
    private Rect _tileRect;

    void Awake()
    {
        _tileRect = _inputPreviewButton.GetComponent<RectTransform>().rect;
        GenerateTiles();
    }

    void GenerateTiles()
    {
        int howManyChars = WordleScript.Instance.HowManyChars();
        float buttonWidth = _tileRect.size.x;

        float startX = transform.position.x - (0.5f * howManyChars * buttonWidth);
        for (int i = 0; i < howManyChars; i++)
        {
            GameObject go = Instantiate(_inputPreviewButton, transform);
            go.transform.position = new Vector2(startX + buttonWidth * i + (buttonWidth * 0.5f), transform.position.y);

            TMP_Text to = go.GetComponentInChildren<TMP_Text>();
            to.text = "";
            _tileTexts.Add(to);
        }
    }

    public void SetCharacter(int idx, char c)
    {
        _tileTexts[idx].text = c.ToString();
    }
}