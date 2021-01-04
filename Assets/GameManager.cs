using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
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
    
    GameObject player;
    public GameObject asteroid; // le grand prefab
    public GameObject boom;
    public GameObject waitToStart; // panel
    
    Camera cam;
    float height, width;

    // Start is called before the first frame update
    void Start()
    {
        messageTxt.gameObject.SetActive(false);

        player = GameObject.FindWithTag("Player");

        cam = Camera.main;
        height = cam.orthographicSize;
        width = height * cam.aspect;

        waitToStart.gameObject.SetActive(true);

        int highscore = PlayerPrefs.GetInt("highscore");
        if(highscore > 0)
        {
            messageTxt.text = "HIGHSCORE : " + highscore;
            messageTxt.gameObject.SetActive(true);
        }
        state = States.wait; 
    }

    public void LaunchGame()
    {
        waitToStart.gameObject.SetActive(false);

        // restaurer après une partie
        player.SetActive(true);
        messageTxt.gameObject.SetActive(false);
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemys)
        {
            Destroy(enemy);
        }

        // lancer une partie
        InitGame();
        LoadLevel();
    }

    void LoadLevel()
    {
        state = States.play;

        // instancier 3 asteroids (selon le niveau)
        // - savoir ce qu'est un asteroid (public, prefab ...)
        // - faire une boucle 1 à 3
        // instancier un asteroid : dans les limites de l'écran
        for(int i = 0; i < 3; i++)
        {
            float x = Random.Range(-width, width);
            float y = Random.Range(-height, height);
            Instantiate(asteroid, new Vector2(x, y), Quaternion.identity);
        }
        UpdateTexts();
    }

    void InitGame()
    {
        level = 1;
        score = 0;
        lives = 5;
    }

    void UpdateTexts()
    {
        levelTxt.text = "LEVEL : " + level;
        scoreTxt.text = "SCORE : " + score;
        livesTxt.text = "LIVES : " + lives;
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateTexts();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == States.play)
        {
            EndOfLevel();
        }
    }

    void EndOfLevel()
    {
        // chercher les asteroids
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        if(enemys.Length == 0)
        {
            StartCoroutine(LevelUp());
        }
    }

    IEnumerator LevelUp()
    {
        state = States.levelup;
        // afficher message "level up"
        messageTxt.text = "LEVEL UP";
        messageTxt.gameObject.SetActive(true);
        // marquer une pause
        yield return new WaitForSecondsRealtime(3f);
        // cacher le message
        messageTxt.gameObject.SetActive(false);
        level += 1;
        LoadLevel();
    }

    public void KillPlayer()
    {
        StartCoroutine(PlayerAgain());
    }

    IEnumerator PlayerAgain()
    {
        state = States.dead;

        GameObject boomGO = Instantiate(boom, player.transform.position, Quaternion.identity);
        
        lives -= 1;
        player.SetActive(false);
        UpdateTexts();
        if(lives <= 0)
        {
            yield return new WaitForSecondsRealtime(2f);
            Destroy(boomGO);
            GameOver();
        }
        else
        {
            yield return new WaitForSecondsRealtime(3f);
            Destroy(boomGO);
            player.SetActive(true);
            state = States.play;
        }
    }

    void GameOver()
    {
        state = States.wait;

        int highscore = PlayerPrefs.GetInt("highscore");
        if(score > highscore)
        {
            PlayerPrefs.SetInt("highscore", score);
            messageTxt.text = "NEW HIGHSCORE : " + score;
        }
        else
        {
            messageTxt.text = "GAME OVER\nHIGHSCORE : "+ highscore;
        }
        
        messageTxt.gameObject.SetActive(true);
        waitToStart.gameObject.SetActive(true);
    }
}
