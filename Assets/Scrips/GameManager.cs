using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Frogger frogger;
    private Home[] homes;
    int score;
    int live;
    int timer;
    public GameObject gameOverMenu;
    public Text scoreText;
    public Text liveText;
    public Text timeText;

    private void Awake()
    {
        homes = FindObjectsOfType<Home>();
        frogger = FindAnyObjectByType<Frogger>();
    }
    private void Start()
    {
        NewGame();
    }
    void NewGame()
    {
        gameOverMenu.SetActive(false);
        SetScore(0);
        SetLive(3);
        NewLevel();
    }
    void NewLevel()
    {
        for(int i = 0; i < homes.Length; i++)
        {
            homes[i].enabled = false;
        }
        Respawn();
    }
   
    void Respawn()
    {
        frogger.Respawn();
        StopAllCoroutines();
        StartCoroutine(Timer(30));
    }
    private IEnumerator Timer(int duration)
    {
        timer = duration;
        while (timer > 0)
        {
            yield return new WaitForSeconds(1);
            timer--;
            timeText.text = timer.ToString();
        }
        frogger.Death();
    }
    public void FrogDie()
    {
        SetLive(live - 1);
        if (live > 0)
        {
            Invoke(nameof(Respawn), 1f);
        }
        else 
        { 
            Invoke(nameof(GameOver), 1f); 
        }
    }

    void GameOver()
    {
        frogger.gameObject.SetActive(false);
        gameOverMenu.SetActive(true);
        StopAllCoroutines(); 
        StartCoroutine(PlayAgain());
    }

    private IEnumerator PlayAgain()
    {
        bool playAgain = false;
        while (!playAgain)
        {
            if (Input.GetKey(KeyCode.R))
            {
                playAgain = true;
            }
            yield return null;
        }
        NewGame();
    }

    public void AdvancedRow()
    {
        SetScore(score + 10); 
    }
    public void HomeOccupied()
    {
        frogger.gameObject.SetActive(false);
        int bonusPoint = timer * 20;
        SetScore(score + bonusPoint + 50);
        if (Clear())
        {
            SetScore(score + 1000);
            SetLive(live + 1);
            Invoke(nameof(NewLevel), 1f);
        }
        else { Respawn(); }
    }
    private bool Clear()
    {
        for (int i = 0; i < homes.Length; i++)
            if(!homes[i].enabled)
            {
                return false;
            }
        return true; 
    }

    void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString();
    }
    void SetLive(int lives)
    {
        this.live = lives;
        liveText.text = lives.ToString();
    }
    

}
