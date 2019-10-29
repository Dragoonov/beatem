using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowerLevel : MonoBehaviour
{
    // Start is called before the first frame update
    LevelSpeed levelSpeed;

    void Start()
    {
        levelSpeed = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelSpeed>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        levelSpeed.SlowDownLevel();
    }
}
