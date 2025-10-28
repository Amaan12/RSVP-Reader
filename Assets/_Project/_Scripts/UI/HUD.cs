using UnityEngine;
using TMPro;

public class HUD : Singleton<HUD>
{
    [SerializeField] TextMeshProUGUI wordDisplay;
    [SerializeField] TextMeshProUGUI wordsPerMinute;

    public void SetWordDisplay(string text)
    {
        wordDisplay.text = text;
    }
    
    public void SetWordsPerMinute(string text)
    {
        wordsPerMinute.text = text;
    }
}
