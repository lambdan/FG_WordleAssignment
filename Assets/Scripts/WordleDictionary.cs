using System.Collections.Generic;
using UnityEngine;

public static class WordleDictionary
{
    private static string[] _words;
    private static HashSet<string> _wordsHashSet;

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
        return _wordsHashSet.Contains(word);
    }

    public static void SetWords(string[] wordsArray)
    {
        _words = wordsArray;

        // add words to a hashset, because checking if a hashset contains something is faster
        _wordsHashSet = new HashSet<string>();
        foreach (string s in wordsArray)
        {
            _wordsHashSet.Add(s);
        }
    }
}