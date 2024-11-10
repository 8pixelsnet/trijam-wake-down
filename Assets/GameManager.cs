using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameoverScreen;
    [SerializeField] TMP_Text scoreText;
    bool gameover;
    float score;

    void Start() {
        gameover = false;
        score = 0;
    }

    void Update() {
        if (!gameover) {
            score += Time.deltaTime;
            scoreText.text = Math.Round(score, 2, MidpointRounding.AwayFromZero).ToString("0.00") + "";
        }
    }

    public bool IsGameOver() {
        return gameover;
    }

    public void GameOver() {
        gameover = true;
        gameoverScreen.SetActive(true);
    }

    public void Restart() {
        Debug.Log("SceneManager.GetActiveScene().name = " + SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
