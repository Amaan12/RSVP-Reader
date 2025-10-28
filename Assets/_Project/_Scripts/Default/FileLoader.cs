using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class FileLoader : MonoBehaviour
{
    public static List<string> LoadWordsFromFile(TextAsset textFile)
    {
        List<string> words = new List<string>();

        if (textFile == null)
        {
            Debug.LogError("Text file is null!");
            return words;
        }

        // Split by any whitespace or punctuation except apostrophes (so contractions stay intact)
        string[] splitWords = Regex.Split(textFile.text, @"\s+");

        foreach (string word in splitWords)
        {
            string trimmed = word.Trim();
            if (!string.IsNullOrEmpty(trimmed))
                words.Add(trimmed);
        }

        return words;
    }
}
