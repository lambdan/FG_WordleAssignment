using TMPro;
using UnityEngine;

public class WordCountDisplay : MonoBehaviour
{
    private TMP_Text _txt;

    void Awake()
    {
        _txt = GetComponent<TMP_Text>();
    }

    void Start()
    {
        _txt.text = "Words in Dictionary: " + WordleDictionary.GetWordCount().ToString();
    }
}