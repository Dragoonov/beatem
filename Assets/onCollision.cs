using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class onCollision : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject level;

    void Start()
    {
        level = GameObject.FindGameObjectWithTag("Level");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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
            SceneManager.LoadScene("Main_Menu");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent != null)
        {
            Destroy(collision.transform.parent.gameObject);
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
