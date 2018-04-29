using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int score;
    private int timeRemaining;
    public Text scoreText;
    public Text timeText;
    public GameObject resetButton;
    public Text crashText;

    void Start()
    {
        score = 0;
        timeRemaining = 300;
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

    private void FixedUpdate()
    {
        timeRemaining--;
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
        return (timeRemaining / 60).ToString() + ":" + (timeRemaining % 60).ToString();
    }

    public void crash()
    {
        resetButton.SetActive(true);
        crashText.text = "You have crashed!";
    }
}