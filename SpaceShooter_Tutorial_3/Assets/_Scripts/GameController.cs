﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public GameObject playerExplosion;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public AudioClip victory;
    public AudioClip death;
    public AudioSource victoryMusic;
    public AudioSource deathMusic;

    public Text ScoreText;
    public Text RestartText;
    public Text GameOverText;
    public Text WinText;
    public int score;

    private bool gameOver;
    private bool restart;
    private int lives;
    private PlayerController playerController;

    void Start()
    {
        gameOver = false;
        restart = false;
        RestartText.text = "";
        GameOverText.text = "";
        WinText.text = "";
        score = 0;
        UpdateScore();
        StartCoroutine (SpawnWaves());
    }

    void Update()
    {
        if (restart)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                SceneManager.LoadScene("SpaceShooter_Main_Final");
            }
            if (Input.GetKey("escape"))
                Application.Quit();
        }
    }

    IEnumerator SpawnWaves ()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                RestartText.text = "Press 'M' for Restart";
                restart = true;
                break;
            }
        }
     }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    void UpdateScore()
    {
        ScoreText.text = "Points: " + score;
        if (score >= 100)
        {
            WinText.text = "You win! Game Created by Rhianna Horner!";
            gameOver = true;
            restart = true;
            victoryMusic.clip = victory;
            victoryMusic.Play();
        }
    }

    public void GameOver()
    {
            GameOverText.text = "Game Over...  Game Created by Rhianna Horner...";
            gameOver = true;
            KillPlayer();
            deathMusic.clip = death;
            deathMusic.Play();
    }

    void KillPlayer()
    {
        if (gameObject.CompareTag("Player"))
        {
            Instantiate(playerExplosion, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }

}
