using System.Collections;
using System.Collections.Generic;
using TMPro;
using TMPro.EditorUtilities;
using UnityEngine;

public class GuessParent : MonoBehaviour
{
    [SerializeField] private GameObject _guessPrefab;

    private List<GameObject> _guessObjects = new List<GameObject>();
    private List<TMP_Text> _guessObjectsTexts = new List<TMP_Text>();

    private void CleanUp()
    {
        foreach (GameObject go in _guessObjects)
        {
            Destroy(go);
        } 
        foreach (TMP_Text to in _guessObjectsTexts)
        {
            Destroy(to);
        }
        
        _guessObjects.Clear();
        _guessObjectsTexts.Clear();
    }
    
    public void InitializeGuesses(int amount)
    {
        CleanUp();

        for (int i = 0; i < amount; i++)
        {
            GameObject go = Instantiate(_guessPrefab, this.transform);
            go.name = "GUESS " + i;

            TMP_Text to = go.GetComponent<TMP_Text>();
            to.text = "GUESS " + i;
            //TODO hide inactive
            
            float y_offset = i * -50f;
            go.transform.position = new Vector3(transform.position.x, transform.position.y + y_offset, transform.position.z);
            
            _guessObjects.Add(go);
            _guessObjectsTexts.Add(to);
        }
    }

    public void SetGuess(int index, string guess)
    {
        _guessObjectsTexts[index].text = guess;
    }
}
