using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDisplay : MonoBehaviour
{
    public bool defaultState;

    public Color defaultEnemiesColor;
    public Color defaultPlayerColor;
    public Color defaultCenterColor;

    public const float defaultHitTimerSeconds = 0.5f;
    public float hitTimerSeconds;
    public bool hitTimerRunning;

    public Color hitCameraColor;
    public Color hitEnemiesColor;
    public Color hitPlayerColor;
    public Color hitCenterColor;
    void Start()
    {
        defaultState = true;
        defaultEnemiesColor = Color.black;
        defaultPlayerColor = Color.black;
        defaultCenterColor = Color.black;
        hitTimerSeconds = defaultHitTimerSeconds;
        hitTimerRunning = false;
        hitCameraColor = Color.red;
        hitEnemiesColor = Color.white;
        hitPlayerColor = Color.white;
        hitCenterColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        ResolveHitDisplay();
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
                GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ChangeBackgroundColor>().Enable(true);
                GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().color = defaultPlayerColor;
                GameObject.FindGameObjectWithTag("CenterDot").GetComponent<SpriteRenderer>().color = defaultCenterColor;
                hitTimerRunning = false;
                defaultState = true;
                hitTimerSeconds = defaultHitTimerSeconds;
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
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ChangeBackgroundColor>().Enable(false);
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().backgroundColor = hitCameraColor;
        GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().color = hitPlayerColor;
        GameObject.FindGameObjectWithTag("CenterDot").GetComponent<SpriteRenderer>().color = hitCenterColor;
    }
}
