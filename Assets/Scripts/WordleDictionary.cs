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
        
        // also make a hashset of the words because its faster to check if the word exists
        _wordsHashSet = new HashSet<string>(wordsArray);
    }
}