using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSpeed : MonoBehaviour
{
    // Start is called before the first frame update
    public float levelSpeed;
    public float slowDownLevelValue;
    public float speedUpLevelValue;
    public byte speedUpCameraColorSpeed;
    public byte slowDownCameraColorSpeed;
    public byte backgroundColorSpeed = 1;

    public const float defaultHitTimerSeconds = 0.3f;
    public float hitTimerSeconds;
    public bool hitTimerRunning;

    public bool finished;


    void Start()
    {
        InitializeLevel();
        InitializeEnemiesSpeed(levelSpeed);
        InitializeEnemiesBackgroundColorSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        if(!finished)
        {
        ResolveHitSpeed();
        }
    }

    private void InitializeLevel()
    {
        levelSpeed = 0.05f;
        slowDownLevelValue = 0.015f;
        speedUpLevelValue = 0.015f; 
        speedUpCameraColorSpeed = 1;
        slowDownCameraColorSpeed = 1;
        hitTimerRunning = false;
        hitTimerSeconds = defaultHitTimerSeconds;
}

    private void InitializeEnemiesSpeed(float speed)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.GetComponent<Shrink>().InitializeSpeed(speed);
        }
        GameObject.FindGameObjectWithTag("Finish").GetComponent<Shrink>().InitializeSpeed(speed);
    }

    private void InitializeEnemiesBackgroundColorSpeed()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.GetComponent<ChangeColor>().InitializeSpeed(backgroundColorSpeed);
        }
    }

    public void SpeedUpLevel()
    {
        if(levelSpeed + speedUpLevelValue > 0.25f)
        {
            return;
        }
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.GetComponent<Shrink>().SpeedUp(speedUpLevelValue);
            gameObject.GetComponent<ChangeColor>().SpeedUp(speedUpCameraColorSpeed);
        }
        GameObject.FindGameObjectWithTag("Finish").GetComponent<Shrink>().SpeedUp(speedUpLevelValue);
        GameObject.FindGameObjectWithTag("PlayerTransparent").GetComponent<Move>().rotateSpeed += speedUpLevelValue * 50;
        levelSpeed += speedUpLevelValue;
    }

    public void SlowDownLevel()
    {
        if(levelSpeed - slowDownLevelValue < 0.01f)
        {
            return;
        }
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.GetComponent<Shrink>().SlowDown(slowDownLevelValue);
            gameObject.GetComponent<ChangeColor>().SlowDown(slowDownCameraColorSpeed);
        }
        GameObject.FindGameObjectWithTag("Finish").GetComponent<Shrink>().SlowDown(slowDownLevelValue);
        GameObject.FindGameObjectWithTag("PlayerTransparent").GetComponent<Move>().rotateSpeed -= slowDownLevelValue * 50;
        levelSpeed -= slowDownLevelValue;
    }

    private void ResolveHitSpeed()
    {
        if (hitTimerRunning)
        {
            hitTimerSeconds -= Time.smoothDeltaTime;
            if (hitTimerSeconds < 0)
            {
                hitTimerRunning = false;
                hitTimerSeconds = defaultHitTimerSeconds;
                InitializeEnemiesSpeed(levelSpeed);
                GameObject.FindGameObjectWithTag("PlayerTransparent").GetComponent<Move>().Enable(true);
            }
        }
    }

    public void FireHitSpeed()
    {
        hitTimerRunning = true;
        InitializeEnemiesSpeed(0);
        GameObject.FindGameObjectWithTag("PlayerTransparent").GetComponent<Move>().Enable(false);
    }

    public void Finish()
    {
        finished = true;
    }

}
