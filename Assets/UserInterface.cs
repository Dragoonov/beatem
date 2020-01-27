using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    // Start is called before the first frame update

    Text tempoUI;
    Text scoreUI;
    Text lifesUI;
    LevelSpeed levelSpeed;
    onCollision playerCollision;

    public float score;

    public bool finished;

    private float slowTempoSum;
    private float mediumTempoSum;
    private float fastTempoSum;

    void Start()
    {
        InitializeLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if(!finished)
        {
            CalculateUI();
            AddTimeToSums();
        }
    }

    private void AddTimeToSums()
    {
        if (levelSpeed.levelSpeed < 0.08f)
        {
            slowTempoSum += Time.deltaTime;
        }
        else if (levelSpeed.levelSpeed >= 0.08f && levelSpeed.levelSpeed < 0.15f)
        {
            mediumTempoSum += Time.deltaTime;
        }
        else if (levelSpeed.levelSpeed >= 0.15f)
        {
            fastTempoSum += Time.deltaTime;
        }
    }

    private void InitializeLevel()
    {
        finished = false;
        tempoUI = GameObject.FindGameObjectWithTag("Tempo").GetComponent<Text>();
        scoreUI = GameObject.FindGameObjectWithTag("Time").GetComponent<Text>();
        lifesUI = GameObject.FindGameObjectWithTag("Lifes").GetComponent<Text>();
        levelSpeed = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelSpeed>();
        playerCollision = GameObject.FindGameObjectWithTag("Player").GetComponent<onCollision>();
    }

    public void CalculateUI()
    {
        score += Time.deltaTime;
        tempoUI.text = "Tempo: " + levelSpeed.levelSpeed.ToString();
        scoreUI.text = "Time: " + score.ToString("F2");
        lifesUI.text = "Lives: " + playerCollision.playerLifes;
    }


    public void Finish()
    {
        finished = true;
        if(GetComponent<LevelDisplay>().lvlNumber != 11)
        {
            score = 100 - score;
        }
        score += slowTempoSum * 0.1f + mediumTempoSum * 1 + fastTempoSum * 5;
    }
}
