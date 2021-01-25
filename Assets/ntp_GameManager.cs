using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ntp_GameManager : MonoBehaviour
{
    public enum States
    {
        wait, play, levelup, dead
    }
    public static States state;

    int level;
    int score;
    int lives;

    public Text levelTxt;
    public Text scoreTxt;
    public Text livesTxt;

    public Text messageTxt;

    
    GameObject ntp_player;
    public GameObject ntp_Ennemis;
    public GameObject boom;

    Camera cam;

    public GameObject attackArea;
    float height, width;

    public GameObject ntp_waitToStart; // panel

    // Start is called before the first frame update
    void Start()
    {
        ntp_player = GameObject.FindWithTag("Player");

        messageTxt.gameObject.SetActive(false);

        cam = Camera.main;
        height = cam.orthographicSize;
        width = height * cam.aspect;

        ntp_waitToStart.gameObject.SetActive(true);
        int highscore = PlayerPrefs.GetInt("highscore");
        if (highscore > 0)
        {
            messageTxt.text = "highscore: " + highscore;
            messageTxt.gameObject.SetActive(true);
        }
        
        state = States.wait;
    }

    public void ntp_LaunchGame()
    {
        // interface
        ntp_waitToStart.gameObject.SetActive(false);
        messageTxt.gameObject.SetActive(false);

        // restaurer après une partie
        ntp_player.SetActive(true);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            Destroy(enemy);
        }

        // lancer une partie
        ntp_InitGame();
        ntp_LoadLevel();
        ntp_UpdateTexts();
    }

    void ntp_InitGame()
    {
        level = 1;
        score = 0;
        lives = 1;
    }

    void ntp_LoadLevel()
    {
        state = States.play;

        for (int i = 0; i < 2 + level; i++)
        {
            float x = Random.Range(-width, width);
            float y = Random.Range(-height, height);
            Instantiate(ntp_player, new Vector2(x, y), Quaternion.identity);
        }
    }

    void ntp_UpdateTexts()
    {
        levelTxt.text = "level: " + level;
        scoreTxt.text = "score: " + score;
        livesTxt.text = "lives: " + lives;
    }

    public void AddScore(int points)
    {
        score += points;
        ntp_UpdateTexts();
    }

    private void Update()
    {
        if (state == States.play)
        {
            ntp_EndOfLevel();
        }
    }

    void ntp_EndOfLevel()
    {
        // chercher les astéroïdes
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            StartCoroutine(ntp_LevelUp());
        }
    }

    IEnumerator ntp_LevelUp()
    {
        state = States.levelup;

        // afficher message "level up"
        messageTxt.text = "level up";
        messageTxt.gameObject.SetActive(true);

        // marquer une pause
        yield return new WaitForSecondsRealtime(3f);

        // cacher le message
        messageTxt.gameObject.SetActive(false);
        level += 1;
        ntp_LoadLevel();
        ntp_UpdateTexts();
    }

    public void ntp_KillPlayer()
    {
        StartCoroutine(ntp_PlayerAgain());
    }

    IEnumerator ntp_PlayerAgain()
    {
        state = States.dead;
        lives -= 1;
        ntp_player.SetActive(false);
        ntp_UpdateTexts();

        GameObject boomGO = Instantiate(boom, ntp_player.transform.position, Quaternion.identity);
        yield return new WaitForSecondsRealtime(1f);

        if (lives <= 0)
        {
            Destroy(boomGO);
            ntp_GameOver();
        }
        else
        {
            ntp_player.SetActive(true);
            state = States.play;
            Destroy(boomGO);
        }
    }

    void ntp_GameOver()
    {
        state = States.wait;

        int highscore = PlayerPrefs.GetInt("highscore");
        if (score > highscore)
        {
            PlayerPrefs.SetInt("highscore", score);
            messageTxt.text = "new highscore: " + score;
        }
        else
        {
            messageTxt.text = "game over\nhighscore: " + highscore;
        }

        messageTxt.gameObject.SetActive(true);
        ntp_waitToStart.gameObject.SetActive(true);
    }
}
