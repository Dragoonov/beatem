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

    public const float defaultHitTimerSeconds = 0.4f;
    public float hitTimerSeconds;
    public bool hitTimerRunning;

    public Color hitBackgroundColor;
    public Color hitEnemiesColor;
    public Color hitPlayerColor;
    public Color hitCenterColor;

    public float rotateSpeed;
    public float switchRotationDeltaSeconds;
    private float time;
    private int rotateDirection;

    byte color1, color2, color3;
    bool ascending;
    public byte changeColorSpeed;

    GameObject[] backgrounds;

    public bool finished;

    public float fieldOfView;

    GameObject centerCircle;
    public float maxCircleRadius;
    public float minCircleRadius;
    private float transformToRadius;
    public float pulsateSpeed;

    public List<GameObject> enemies;
    SpriteRenderer playerRenderer;
    SpriteRenderer centerDotRenderer;
    LevelSpeed levelSpeed;

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
        rotateDirection = 1;
        fieldOfView = 120;
        centerCircle = GameObject.FindGameObjectWithTag("CenterDot");
        maxCircleRadius = 1.20f;
        minCircleRadius = 0.5f;
        transformToRadius = maxCircleRadius;
        pulsateSpeed = 0.01f;
        enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
        playerRenderer = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        centerDotRenderer = GameObject.FindGameObjectWithTag("CenterDot").GetComponent<SpriteRenderer>();
        levelSpeed = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelSpeed>();
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
            ChangeCameraFieldOfView();
            PulsateCenterDot();
        }
    }

    private void ResolveHitDisplay()
    {
        if (hitTimerRunning)
        {
            hitTimerSeconds -= Time.smoothDeltaTime;
            if (hitTimerSeconds < 0)
            {
                foreach (GameObject enemy in enemies)
                {
                    if(enemy!=null)
                        enemy.GetComponent<ChangeColor>().ChangeObjectColor(defaultEnemiesColor);
                }
                playerRenderer.color = defaultPlayerColor;
                centerDotRenderer.color = defaultCenterColor;
                hitTimerRunning = false;
                defaultState = true;
                hitTimerSeconds = defaultHitTimerSeconds;
            }
        }
    }

    private void UpdateRotateSpeed()
    {
        rotateSpeed = levelSpeed.levelSpeed * 10;
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
            foreach (GameObject enemy in enemies)
            {
                if (enemy != null)
                    enemy.GetComponent<ChangeColor>().UpdateColors();
            }
        }
    }

    private void PulsateCenterDot()
    {
        ResolvePulsateSpeed();
        float centerRadius = centerCircle.transform.localScale.y;
        float lerpResult = Mathf.Lerp(centerRadius, transformToRadius, pulsateSpeed);
        centerCircle.transform.localScale = new Vector3(lerpResult,lerpResult,lerpResult);
        if(centerRadius >= maxCircleRadius-0.01f)
        {
            transformToRadius = minCircleRadius;
        }
        else if (centerRadius <= minCircleRadius+0.01f)
        {
            transformToRadius = maxCircleRadius;
        }
    }

    private void ResolvePulsateSpeed()
    {
        if (levelSpeed.levelSpeed < 0.08f)
        {
            pulsateSpeed = 0.01f;
        }
        else if (levelSpeed.levelSpeed >= 0.08f && levelSpeed.levelSpeed < 0.15f)
        {
            pulsateSpeed = 0.05f;
        }
        else if (levelSpeed.levelSpeed >= 0.15f)
        {
            pulsateSpeed = 0.1f;
        }
    }

    public void FireHitDisplay()
    {
        hitTimerRunning = true;
        defaultState = false;
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null)
                enemy.GetComponent<ChangeColor>().ChangeObjectColor(hitEnemiesColor);
        }
        camera.backgroundColor = hitBackgroundColor;
        backgrounds[0].GetComponent<SpriteRenderer>().color = hitBackgroundColor;
        backgrounds[1].GetComponent<SpriteRenderer>().color = hitBackgroundColor;
        playerRenderer.color = hitPlayerColor;
        centerDotRenderer.color = hitCenterColor;
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
                rotateDirection = -rotateDirection;
            }
            camera.transform.Rotate(0, 0, rotateSpeed*rotateDirection);
        }
    }

    private void ChangeCameraFieldOfView()
    {
        if (levelSpeed.levelSpeed < 0.08f && camera.fieldOfView != 120f)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 120f, 0.05f);
        }
        else if (levelSpeed.levelSpeed >= 0.08f && levelSpeed.levelSpeed < 0.15f && camera.fieldOfView != 130f)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 130f, 0.05f);
        }
        else if (levelSpeed.levelSpeed >= 0.15f && camera.fieldOfView != 140f)
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, 140f, 0.05f);
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
