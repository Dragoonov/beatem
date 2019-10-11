using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDisplay : MonoBehaviour
{
    public bool defaultState;

    Camera camera;

    public Color defaultEnemiesColor;
    public Color defaultPlayerColor;
    public Color defaultCenterColor;

    public const float defaultHitTimerSeconds = 0.3f;
    public float hitTimerSeconds;
    public bool hitTimerRunning;

    public Color hitBackgroundColor;
    public Color hitEnemiesColor;
    public Color hitPlayerColor;
    public Color hitCenterColor;

    public float rotateSpeed;
    public float switchRotationDeltaSeconds;
    private float time;

    byte color1, color2, color3;
    bool ascending;
    public byte changeColorSpeed;

    GameObject[] backgrounds;

    public bool finished;

    void Start()
    {
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        defaultState = true;
        defaultEnemiesColor = Color.black;
        defaultPlayerColor = Color.black;
        defaultCenterColor = Color.black;
        hitTimerSeconds = defaultHitTimerSeconds;
        hitTimerRunning = false;
        hitBackgroundColor = Color.red;
        hitEnemiesColor = Color.white;
        hitPlayerColor = Color.white;
        hitCenterColor = Color.white;
        finished = false;
        rotateSpeed = 0.5f;
        switchRotationDeltaSeconds = 5;
        ascending = true;
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        color1 = color3 = color2 = 50;
        backgrounds = GameObject.FindGameObjectsWithTag("Background");
        changeColorSpeed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(!finished)
        {
            ResolveHitDisplay();
            ChangeEnemiesBackgroundColor();
            ChangeCameraRotation();
            ChangeBackgroundColor();
            UpdateColors();
            UpdateRotateSpeed();
        }
    }

    private void ResolveHitDisplay()
    {
        if (hitTimerRunning)
        {
            hitTimerSeconds -= Time.smoothDeltaTime;
            if (hitTimerSeconds < 0)
            {
                GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject gameObject in gameObjects)
                {
                    gameObject.GetComponent<ChangeColor>().ChangeObjectColor(defaultEnemiesColor);
                }
                GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().color = defaultPlayerColor;
                GameObject.FindGameObjectWithTag("CenterDot").GetComponent<SpriteRenderer>().color = defaultCenterColor;
                hitTimerRunning = false;
                defaultState = true;
                hitTimerSeconds = defaultHitTimerSeconds;
            }
        }
    }

    private void UpdateRotateSpeed()
    {
        rotateSpeed = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelSpeed>().levelSpeed * 10;
    }

    private void ChangeBackgroundColor()
    {
        if(defaultState)
        {
            camera.backgroundColor = new Color32(color1, color2, color3, 255);
            backgrounds[0].GetComponent<SpriteRenderer>().color = new Color32(color1, color2, color3, 255);
            backgrounds[1].GetComponent<SpriteRenderer>().color = new Color32(Convert.ToByte(255 - color1), Convert.ToByte(255 - color2), Convert.ToByte(255 - color3), 255);
        }
    }

    private void ChangeEnemiesBackgroundColor()
    {
        if(defaultState)
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.GetComponent<ChangeColor>().UpdateColors();
            }
        }
    }

    public void FireHitDisplay()
    {
        hitTimerRunning = true;
        defaultState = false;
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject gameObject in gameObjects)
        {
            gameObject.GetComponent<ChangeColor>().ChangeObjectColor(hitEnemiesColor);
        }
        camera.backgroundColor = hitBackgroundColor;
        backgrounds[0].GetComponent<SpriteRenderer>().color = hitBackgroundColor;
        backgrounds[1].GetComponent<SpriteRenderer>().color = hitBackgroundColor;
        GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().color = hitPlayerColor;
        GameObject.FindGameObjectWithTag("CenterDot").GetComponent<SpriteRenderer>().color = hitCenterColor;
    }

    public void Finish()
    {
        finished = true;
    }

    private void ChangeCameraRotation()
    {
        if(defaultState)
        {
            time += Time.deltaTime;
            if (time > switchRotationDeltaSeconds)
            {
                time = 0;
                switchRotationDeltaSeconds = UnityEngine.Random.Range(5, 15);
                rotateSpeed = -rotateSpeed;
            }
            camera.transform.Rotate(0, 0, rotateSpeed);
        }
    }

    private void UpdateColors()
    {
        if(ascending)
        {
            color1 += changeColorSpeed;
            color2 += changeColorSpeed;
            color3 += changeColorSpeed;
        }
        else
        {
            color1 -= changeColorSpeed;
            color2 -= changeColorSpeed;
            color3 -= changeColorSpeed;
        }
        if(color1>=200 || color1 <=50)
        {
            ascending = !ascending;
        }
    }
}
