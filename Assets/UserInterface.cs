using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    // Start is called before the first frame update

    Text tempoUI;
    Text scoreUI;
    LevelSpeed levelSpeed;

    public float score;

    public bool finished;

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
        }
    }

    private void InitializeLevel()
    {
        finished = false;
        tempoUI = GameObject.FindGameObjectWithTag("Tempo").GetComponent<Text>();
        scoreUI = GameObject.FindGameObjectWithTag("Time").GetComponent<Text>();
        levelSpeed = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelSpeed>();
    }

    public void CalculateUI()
    {
        score += Time.deltaTime;
        tempoUI.text = "Tempo: " + levelSpeed.levelSpeed.ToString();
        scoreUI.text = "Time: " + score.ToString("F2");
    }

    public void Finish()
    {
        finished = true;
    }
}
