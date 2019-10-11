using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject tempoUI;
    GameObject scoreUI;

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
        tempoUI = GameObject.FindGameObjectWithTag("Tempo");
        scoreUI = GameObject.FindGameObjectWithTag("Time");
    }

    public void CalculateUI()
    {
        score += Time.deltaTime;
        tempoUI.GetComponent<Text>().text = "Tempo: " + GameObject.FindGameObjectWithTag("Level").GetComponent<LevelSpeed>().levelSpeed.ToString();
        scoreUI.GetComponent<Text>().text = "Time: " + score.ToString("F2");
    }

    public void Finish()
    {
        finished = true;
    }
}
