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
        return _wordsHashSet.Contains(word.ToLower().Trim());
    }

    public static void SetWords(string[] w)
    {
        _words = w;
        _wordsHashSet = new HashSet<string>();
        foreach (string s in w)
        {
            // checking if a HashSet contains something is faster, apparently
            _wordsHashSet.Add(s.ToLower().Trim());
        }
    }
    
    private static string[] _words;
    private static HashSet<string> _wordsHashSet;


}