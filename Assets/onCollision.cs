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
        if(other.gameObject.tag == "Enemy")
        {
            GameObject.FindGameObjectWithTag("Level").GetComponent<LevelDisplay>().FireHitDisplay();
            GameObject.FindGameObjectWithTag("Level").GetComponent<LevelSpeed>().FireHitSpeed();
        }
        else if(other.gameObject.tag == "Finish")
        {
            //gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

    }
}
