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
    void Start()
    {
        InitializeLevel();
        InitializeEnemiesSpeed();
        InitializeCameraBackgroundColorSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        InitializeEnemiesSpeed();
    }

    private void InitializeLevel()
    {
        levelSpeed = 0.05f;
        slowDownLevelValue = 0.02f;
        speedUpLevelValue = 0.02f;
        cameraBackgroundColorSpeed = 1;
        speedUpCameraColorSpeed = 1;
        slowDownCameraColorSpeed = 1;
}

    private void InitializeEnemiesSpeed()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.GetComponent<Shrink>().InitializeSpeed(levelSpeed);
        }
        GameObject.FindGameObjectWithTag("Finish").GetComponent<Shrink>().InitializeSpeed(levelSpeed);
    }

    private void InitializeCameraBackgroundColorSpeed()
    {
        ChangeBackgroundColor camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ChangeBackgroundColor>();
        camera.InitializeSpeed(cameraBackgroundColorSpeed);
    }

    public void SpeedUpLevel()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.GetComponent<Shrink>().SpeedUp(speedUpLevelValue);
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
        }
        GameObject.FindGameObjectWithTag("Finish").GetComponent<Shrink>().SlowDown(slowDownLevelValue);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ChangeBackgroundColor>().SlowDown(slowDownCameraColorSpeed);
        levelSpeed -= slowDownLevelValue;
    }
}
