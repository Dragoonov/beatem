using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject level = GameObject.FindGameObjectWithTag("Level");
        if (other.gameObject.tag == "Enemy")
        {
            level.GetComponent<LevelDisplay>().FireHitDisplay();
            level.GetComponent<LevelSpeed>().FireHitSpeed();
            level.GetComponent<UserInterface>().score += 1f;
            level.GetComponent<AudioScript>().PlayHitSound();
        }
        else if(other.gameObject.tag == "Finish")
        {
            level.GetComponent<LevelDisplay>().Finish();
            level.GetComponent<LevelSpeed>().Finish();
            level.GetComponent<UserInterface>().Finish();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }
}
