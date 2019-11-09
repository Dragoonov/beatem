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

    public List<GameObject> enemies;

    GameObject finishCircle;

    Move playerMovement;


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
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        finishCircle = GameObject.FindGameObjectWithTag("Finish");
        playerMovement = GameObject.FindGameObjectWithTag("PlayerTransparent").GetComponent<Move>();
    }

    private void InitializeEnemiesSpeed(float speed)
    {
        foreach (GameObject enemy in enemies)
        {
            if(enemy!=null)
                enemy.GetComponent<Shrink>().InitializeSpeed(speed);
        }
        finishCircle.GetComponent<Shrink>().InitializeSpeed(speed);
    }

    private void InitializeEnemiesBackgroundColorSpeed()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
                enemy.GetComponent<ChangeColor>().InitializeSpeed(backgroundColorSpeed);
        }
    }

    public void SpeedUpLevel()
    {
        if(levelSpeed + speedUpLevelValue > 0.25f)
        {
            return;
        }
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.GetComponent<Shrink>().SpeedUp(speedUpLevelValue);
                enemy.GetComponent<ChangeColor>().SpeedUp(speedUpCameraColorSpeed);
            }
        }
        finishCircle.GetComponent<Shrink>().SpeedUp(speedUpLevelValue);
        playerMovement.rotateSpeed += speedUpLevelValue * 50;
        levelSpeed += speedUpLevelValue;
    }

    public void SlowDownLevel()
    {
        if(levelSpeed - slowDownLevelValue < 0.01f)
        {
            return;
        }
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
            {
            enemy.GetComponent<Shrink>().SlowDown(slowDownLevelValue);
            enemy.GetComponent<ChangeColor>().SlowDown(slowDownCameraColorSpeed);
            }
        }
        finishCircle.GetComponent<Shrink>().SlowDown(slowDownLevelValue);
        playerMovement.rotateSpeed -= slowDownLevelValue * 50;
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
                playerMovement.Enable(true);
            }
        }
    }

    public void FireHitSpeed()
    {
        hitTimerRunning = true;
        InitializeEnemiesSpeed(0);
        playerMovement.Enable(false);
    }

    public void Finish()
    {
        finished = true;
    }

}
