using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameoverScreen;
    [SerializeField] TMP_Text scoreText;
    bool gameover;
    float score;

    [SerializeField] private AudioClip[] gameoverClips;

    void Start() {
        gameover = false;
        score = 0;
    }

    public void AddScore(int addValue)
    {
        score += addValue;
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

        MusicManager.Instance.PlayOneShot(gameoverClips[UnityEngine.Random.Range(0, gameoverClips.Length)]);
    }

    public void Restart() {
        Debug.Log("SceneManager.GetActiveScene().name = " + SceneManager.GetActiveScene().name);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
