using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class onCollision : MonoBehaviour
{
    // Start is called before the first frame update
    public float playerLifes;
    GameObject level;
    FirebaseConnector connector;

    void Start()
    {
        level = GameObject.FindGameObjectWithTag("Level");
        GameObject tournament = GameObject.Find("TournamentConnector");
        if(tournament != null)
        {
            connector = tournament.GetComponent<FirebaseConnector>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerLifes < 1)
        {
            LevelDisplay levelDisplay = level.GetComponent<LevelDisplay>();
            levelDisplay.Finish();
            level.GetComponent<LevelSpeed>().Finish();
            level.GetComponent<UserInterface>().Finish();
            levelDisplay.ShowFinishPanel();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (level == null)
        {
            level = GameObject.FindGameObjectWithTag("Level");
        }
        if (other.gameObject.tag == "Enemy")
        {
            level.GetComponent<LevelDisplay>().FireHitDisplay();
            level.GetComponent<LevelSpeed>().FireHitSpeed();
            level.GetComponent<UserInterface>().score += 1f;
            level.GetComponent<AudioScript>().PlayHitSound();
            playerLifes--;
        }
        else if(other.gameObject.tag == "Finish")
        {
            LevelDisplay levelDisplay = level.GetComponent<LevelDisplay>();
            levelDisplay.Finish();
            level.GetComponent<LevelSpeed>().Finish();
            level.GetComponent<UserInterface>().Finish();
            if (connector != null)
            {
                GameObject.Find("LevelUI").SetActive(false);
                levelDisplay.ShowFinishPanel("You won!");
                connector.OnFinish();
            }
            else
            {
                levelDisplay.ShowFinishPanel();
            }
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
        if (connector != null)
        {
            connector.OnTravelObstacle();
        }
    }
}
