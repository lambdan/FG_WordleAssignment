using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class StatusTextScript : MonoBehaviour
{
    private TMP_Text _txt;
    public static StatusTextScript Instance { get; private set; }

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

        _txt = GetComponent<TMP_Text>();
    }
    
    public void ShowMessage(string msg, Color col, float duration = 0f)
    {
        _txt.text = msg;
        _txt.color = col;
        _txt.gameObject.SetActive(true);
        if (duration > 0)
        {
            StartCoroutine(HideMessageAfter(duration));
        }
    }

    public void Hide()
    {
        _txt.gameObject.SetActive(false);
    }

    IEnumerator HideMessageAfter(float t)
    {
        yield return new WaitForSeconds(t);
        _txt.gameObject.SetActive(false);
    }
}
