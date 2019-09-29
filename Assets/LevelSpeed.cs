using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpeed : MonoBehaviour
{
    // Start is called before the first frame update
    public float levelSpeed;
    public float slowDownLevelValue;
    public float speedUpLevelValue;
    public byte cameraBackgroundColorSpeed;
    public byte speedUpCameraColorSpeed;
    public byte slowDownCameraColorSpeed;

    public const float defaultHitTimerSeconds = 0.3f;
    public float hitTimerSeconds;
    public bool hitTimerRunning;


    void Start()
    {
        InitializeLevel();
        InitializeEnemiesSpeed(levelSpeed);
        InitializeCameraBackgroundColorSpeed();
        InitializeEnemiesBackgroundColorSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        ResolveHitSpeed();
    }

    private void InitializeLevel()
    {
        levelSpeed = 0.05f;
        slowDownLevelValue = 0.02f;
        speedUpLevelValue = 0.02f;
        cameraBackgroundColorSpeed = 1;
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

    private void InitializeCameraBackgroundColorSpeed()
    {
        ChangeBackgroundColor camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ChangeBackgroundColor>();
        camera.InitializeSpeed(cameraBackgroundColorSpeed);
    }

    private void InitializeEnemiesBackgroundColorSpeed()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.GetComponent<ChangeColor>().InitializeSpeed(cameraBackgroundColorSpeed);
        }
    }

    public void SpeedUpLevel()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.GetComponent<Shrink>().SpeedUp(speedUpLevelValue);
            gameObject.GetComponent<ChangeColor>().SpeedUp(speedUpCameraColorSpeed);
        }
        GameObject.FindGameObjectWithTag("Finish").GetComponent<Shrink>().SpeedUp(speedUpLevelValue);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ChangeBackgroundColor>().SpeedUp(speedUpCameraColorSpeed);
        levelSpeed += speedUpLevelValue;
    }

    public void SlowDownLevel()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.GetComponent<Shrink>().SlowDown(slowDownLevelValue);
            gameObject.GetComponent<ChangeColor>().SlowDown(slowDownCameraColorSpeed);
        }
        GameObject.FindGameObjectWithTag("Finish").GetComponent<Shrink>().SlowDown(slowDownLevelValue);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ChangeBackgroundColor>().SlowDown(slowDownCameraColorSpeed);
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
}
