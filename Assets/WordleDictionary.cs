using System.Collections.Generic;
using UnityEngine;

public static class WordleDictionary
{
    public static string GetRandomWord()
    {
        return _words[Random.Range(0, _words.Count)];
    }

    public static int GetWordCount()
    {
        return _words.Count;
    }

    public static bool IsInDictionary(string word)
    {
        return _words.Contains(word.ToLower());
    }

    public static void SetWords(string[] w)
    {
        // StackOverflowException if i put in all 14k words in the list below in code
        // Setting them at runtime works
        _words = new List<string>(w);
    }

    private static List<string> _words = new List<string>();


}