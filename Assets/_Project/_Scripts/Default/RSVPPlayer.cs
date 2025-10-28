using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RSVPPlayer : MonoBehaviour
{
    [Header("References")]
    public TextAsset textFile;

    [Header("Settings")]
    public float wordsPerMinute = 180f; // typical reading speed
    public bool loop = false;
    [SerializeField] int wpmIncrementValue;

    List<string> words;
    int currentIndex = 0;
    bool isPlaying = false;
    Coroutine playRoutine;

    void Start()
    {
        words = FileLoader.LoadWordsFromFile(textFile);
        if (words.Count > 0)
            HUD.Instance.SetWordDisplay(words[0]);

        HUD.Instance.SetWordsPerMinute($"{wordsPerMinute:F0} WPM");
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        // * Space: toggle play/pause
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isPlaying)
                Pause();
            else
                Play();
        }

        // * Scroll wheel: adjust speed (±20 WPM per scroll tick)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            int direction = scroll > 0 ? 1 : -1;
            wordsPerMinute += direction * wpmIncrementValue;
            wordsPerMinute = Mathf.Clamp(wordsPerMinute, 0f, 1000f);
            HUD.Instance.SetWordsPerMinute($"{wordsPerMinute:F0} WPM");
        }

        // * Manual navigation when paused
        if (!isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
                ShowNextWord();
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                ShowPreviousWord();
        }
    }

    void Play()
    {
        if (words.Count == 0) return;

        isPlaying = true;
        playRoutine = StartCoroutine(PlayWords());
    }

    void Pause()
    {
        isPlaying = false;
        if (playRoutine != null)
            StopCoroutine(playRoutine);
    }

    IEnumerator PlayWords()
    {
        while (isPlaying)
        {
            HUD.Instance.SetWordDisplay(words[currentIndex]);

            currentIndex++;
            if (currentIndex >= words.Count)
            {
                if (loop)
                    currentIndex = 0;
                else
                {
                    Pause();
                    yield break;
                }
            }

            // Stop instantly if 0 wpm
            while (wordsPerMinute <= 0)
            {
                if (!isPlaying) yield break; // stop if user paused manually
                yield return null;
                continue;
            }

            // Wait dynamically — responsive to speed changes
            float elapsed = 0f;
            while (elapsed < 60f / Mathf.Max(wordsPerMinute, 1f))
            {
                if (!isPlaying)
                    yield break;

                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }

    void ShowNextWord()
    {
        if (words.Count == 0) return;

        currentIndex++;
        if (currentIndex >= words.Count)
        {
            if (loop)
                currentIndex = 0;
            else
                currentIndex = words.Count - 1;
        }

        HUD.Instance.SetWordDisplay(words[currentIndex]);
    }

    void ShowPreviousWord()
    {
        if (words.Count == 0) return;

        currentIndex--;
        if (currentIndex < 0)
        {
            if (loop)
                currentIndex = words.Count - 1;
            else
                currentIndex = 0;
        }

        HUD.Instance.SetWordDisplay(words[currentIndex]);
    }
}
