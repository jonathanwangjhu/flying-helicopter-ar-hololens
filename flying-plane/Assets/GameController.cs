using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int score;
    private float timeRemaining;
    public Text scoreText;
    public Text timeText;
    public GameObject resetButton;
    public Text crashText;

    void Start()
    {
        score = 0;
        timeRemaining = 180f;
        UpdateScore();

        resetButton.SetActive(false);
        crashText.text = "";
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
        UpdateTime();
    }

    private void Update()
    {
        timeRemaining -= Time.deltaTime;
        UpdateTime();
    }

    void UpdateScore()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    void UpdateTime()
    {
        timeText.text = "Time: " + timeRemainingToString();
    }

    private string timeRemainingToString()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.RoundToInt(timeRemaining % 60f);
        string formatedSeconds = seconds.ToString();

        if (seconds == 60)
        {
            seconds = 0;
            minutes += 1;
        }

        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void crash()
    {
        resetButton.SetActive(true);
        crashText.text = "You have crashed!";
    }
}