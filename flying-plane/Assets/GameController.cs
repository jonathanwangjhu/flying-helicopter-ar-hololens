using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private int score;
    private float timeRemaining;
    public Text scoreText;
    public Text timeText;
    public GameObject resetButton;
    public Text gameOverText;
    public PlaneControl planeControl;

    void Start()
    {
        score = 0;
        timeRemaining = 180f;
        UpdateScore();

        resetButton.SetActive(false);
        gameOverText.text = "";
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
        UpdateTime();
    }

    private void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTime();
        } else
        {
            gameOver("Out of time!");
        }
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

        if (seconds == 60)
        {
            seconds = 0;
            minutes += 1;
        }

        return minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void gameOver(string reason)
    {
        resetButton.SetActive(true);
        gameOverText.text = reason;
        planeControl.stopControl();
    }

    public void Reset()
    {
        SceneManager.LoadScene("flying-plane");
    }
}