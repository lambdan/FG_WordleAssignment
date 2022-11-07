using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WordleDictionary
{
    public static string GetRandomWord()
    {
        return _words[Random.Range(0, _words.Length)];
    }

    public static int GetWordCount()
    {
        return _words.Length;
    }

    public static bool IsInDictionary(string word)
    {
        // _words.Contains always returns false?
        foreach (string w in _words)
        {
            if (w.ToLower().Trim() == word.ToLower().Trim())
            {
                return true;
            }
        }
        return false;
    }

    public static void SetWords(string[] w)
    {
        _words = w;
    }
    
    private static string[] _words;


}