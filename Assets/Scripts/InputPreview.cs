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
        float xOffset = 0f;
        
        for (int i = 0; i < howManyChars; i++)
        {
            GameObject go = Instantiate(_inputPreviewButton, transform);
            go.transform.position = new Vector2(startX + xOffset, transform.position.y);
            
            Debug.Log(go.transform.position); // wtf?
            
            TMP_Text to = go.GetComponentInChildren<TMP_Text>();
            to.text = "";
            _tileTexts.Add(to);

            xOffset += buttonWidth;
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, 5f);
    }

    public void SetCharacter(int idx, char c)
    {
        if (_tileTexts.Count > 0)
        {
            _tileTexts[idx].text = c.ToString();
        }
        
    }
    
    
}
