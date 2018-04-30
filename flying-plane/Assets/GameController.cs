using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private int score;
    private float timeRemaining;
    private bool gamePlaying;
    public Text scoreText;
    public Text timeText;
    public GameObject resetButton;
    public Text gameOverText;
    public PlaneControl planeControl;
    public Text waterGaugeText;
    public Image waterGaugeImage;

    public float startingWater = 500;

    void Start()
    {
        score = 0;
        timeRemaining = 180f;
        UpdateScore();

        gamePlaying = true;
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
        if (timeRemaining > 0f && gamePlaying)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTime();
        } else if (gamePlaying)
        {
            timeRemaining = 0f;
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

    public void reduceWaterGauge()
    {
        waterGaugeImage.fillAmount -= 1.0f / startingWater;

        if (waterGaugeImage.fillAmount == 0)
        {
            waterGaugeText.text = "Out of water!";
            planeControl.stopDroppingWater();
        }
    }

    public void gameOver(string reason)
    {
        gamePlaying = false;
        resetButton.SetActive(true);
        gameOverText.text = reason;
        planeControl.stopControl();
    }

    public void Reset()
    {
        SceneManager.LoadScene("flying-plane");
    }
}