using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsFiller : MonoBehaviour
{
    private UserManager userManager;
    // Start is called before the first frame update
    void Start()
    {
        userManager = GameObject.Find("User").GetComponent<UserManager>();
        FillStats();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FillStats()
    {
        GameObject.Find("TutorialScore").GetComponent<Text>().text = userManager.GetLvlScore(0).ToString();
        GameObject.Find("EndlessScore").GetComponent<Text>().text = userManager.GetEndlessScore().ToString();

        for(int i=1;i<11;i++)
        {
            GameObject.Find("Lvl" + i + "Score").GetComponent<Text>().text = userManager.GetLvlScore(i).ToString();
        }

    }
}
