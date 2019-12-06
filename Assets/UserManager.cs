using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public float[] lvlsUnlocked;
    public float[] lvlsScore;
    public float endlessScore;
    public static UserManager instance;
    // Start is called before the first frame update
    void Start()
    {
        //ResetScores();
    }


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            LoadState();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ResetScores()
    {
        for (int i = 0; i < 11; i++)
        {
            lvlsUnlocked[i] = -1;
            PlayerPrefs.SetFloat("lvl" + i + "Score", 0);
            PlayerPrefs.SetFloat("lvl" + i + "Unlocked", -1);
            PlayerPrefs.SetFloat("endlessScore", 0);
            lvlsScore[i] = 0;
            endlessScore = 0;
        }
        lvlsUnlocked[0] = 1;
        SaveState();
    }

    public float GetLvlScore(int lvl)
    {
        return lvlsScore[lvl];
    }

    public float GetEndlessScore()
    {
        return endlessScore;
    }

    public bool isUnlocked(int lvl)
    {
        return lvlsUnlocked[lvl] > 0;
    }

    public void UnlockLvl(int lvl)
    {
        lvlsUnlocked[lvl] = 1;
    }

    private void LoadState()
    {
        lvlsUnlocked = new float[11];
        lvlsScore = new float[11];

        for (int i=0;i<11;i++)
        {
            lvlsUnlocked[i] = PlayerPrefs.GetFloat("lvl" + i + "Unlocked", -1);
            lvlsScore[i] = PlayerPrefs.GetFloat("lvl" + i + "Score", 0);
        }
        lvlsUnlocked[0] = 1;

        endlessScore = PlayerPrefs.GetFloat("endlessScore", 0);
    }

    public void UpdateUnlocked(string key, float value, int levelNumber)
    {
        if(lvlsUnlocked[levelNumber] < value)
        {
            PlayerPrefs.SetFloat(key, value);
        }
    }

    public void UpdateScore(string key, float value, int levelNumber)
    {
        value = (float)Math.Round(value, 2);
        Debug.Log(value);
        if(levelNumber <= 10) //IF CLASSIC LEVELS
        {
            if(lvlsScore[levelNumber] > value || lvlsScore[levelNumber] < 0.5)
            {
                PlayerPrefs.SetFloat(key, value);
                lvlsScore[levelNumber] = value;
            }
        }
        else
        {
            if(endlessScore < value)
            {
                PlayerPrefs.SetFloat("endlessScore", value);
                endlessScore = value;
            }
        }
    }

    private void SaveState()
    {
        PlayerPrefs.Save();
    }
}
