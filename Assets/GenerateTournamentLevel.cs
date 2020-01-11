using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTournamentLevel : MonoBehaviour
{
    public GameObject[] gameObjects;
    public GameObject finishCircle;
    private float initialScale;
    public LevelSpeed levelSpeed;
    public LevelDisplay levelDisplay;
    GameObject levelObject;
    public List<GameObject> levelObjects;
    public int objectsNumber;
    private int currentEnemy;
    public bool startLevel;

    private float[] initialAngles;

    void Start()
    {
        startLevel = false;
        initialScale = 17f;
        levelSpeed = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelSpeed>();
        levelDisplay = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelDisplay>();
        levelObject = levelDisplay.gameObject;
        levelObjects = new List<GameObject>();
        initialAngles = new float[] { 45, -45, 90, -90, 135, -135, 180, -180 };
        currentEnemy = 0;
        //Debug.Log(levelObjects);
        levelObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(startLevel)
        {
            ReleaseEnemies();
        }
    }

    public List<ObstacleDataWrapper> GenerateLevel()
    {
        List<ObstacleDataWrapper> list = new List<ObstacleDataWrapper>();
        for (int i = 0; i < objectsNumber-1; i++)
        {
            list.Add(InitializeNewEnemy());
        }
        list.Add(InitializeNewEnemy(true));
        return list;
    }

    public void SetLevel(List<ObstacleDataWrapper> level)
    {
        for (int i = 0; i < objectsNumber-1; i++)
        {
            InitializeNewEnemy(false, level[i]);
        }
        InitializeNewEnemy(true);
    }

    public List<GameObject> getLevel()
    {
        return levelObjects;
    }

    public void StartLevel()
    {
         levelObject.SetActive(true);
         startLevel = true;
    }

    private void ReleaseEnemies()
    {
        if(currentEnemy >= objectsNumber)
        {
            return;
        }
        if(currentEnemy == 0)
        {
            levelObjects[currentEnemy].SetActive(true);
            currentEnemy++;
        }
        else
        {
            GameObject previousObject = levelObjects[currentEnemy - 1];
            if(previousObject != null && previousObject.tag == "GroupEnemy")
            {
                previousObject = previousObject.transform.GetChild(previousObject.transform.childCount - 1).gameObject;
            }
            if(previousObject == null || previousObject.transform.localScale.x < initialScale - 5)
            {
                levelObjects[currentEnemy].SetActive(true);
                currentEnemy++;
            }
        }
    }

    private ObstacleDataWrapper InitializeNewEnemy(bool finish = false, ObstacleDataWrapper model = null)
    {
        ObstacleDataWrapper randoms;
        int randomAngle;
        int randomObject;
        if (model == null)
        {
            randomAngle = UnityEngine.Random.Range(0, initialAngles.Length);
            randomObject = UnityEngine.Random.Range(0, gameObjects.Length);
            randoms = new ObstacleDataWrapper(randomAngle, randomObject);
        }
        else
        {
            randoms = model;
            randomAngle = model.angle;
            randomObject = model.obj;
        }
        float previousAngle;
        float scaleDifference;
        GameObject obj;
        if(!finish)
        {
            obj = Instantiate(gameObjects[randomObject], new Vector3(0, 0, 0), Quaternion.identity);
        }
        else
        {
            obj = Instantiate(finishCircle, new Vector3(0, 0, 0), Quaternion.identity);
        }
        obj.SetActive(false);
        levelObjects.Add(obj);

        if (obj.tag == "GroupEnemy")
        {
            previousAngle = initialAngles[randomAngle];
            scaleDifference = obj.transform.GetChild(1).gameObject.transform.localScale.x - obj.transform.GetChild(0).gameObject.transform.localScale.x;
            //Debug.Log(scaleDifference);
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                GameObject temp = obj.transform.GetChild(i).gameObject;
                temp.transform.Rotate(0, 0, previousAngle + temp.transform.rotation.z);
                temp.GetComponent<Shrink>().InitializeSpeed(levelSpeed.levelSpeed);
                if(temp.tag != "Finish")
                {
                    temp.GetComponent<ChangeColor>().InitializeSpeed(levelSpeed.backgroundColorSpeed);
                }
                temp.transform.localScale = new Vector3(initialScale + i*scaleDifference, initialScale + i*scaleDifference);
                levelSpeed.enemies.Add(temp);
                levelDisplay.enemies.Add(temp);
            }
        }
        else
        {
            obj.transform.Rotate(0, 0, initialAngles[randomAngle]);
            obj.GetComponent<Shrink>().InitializeSpeed(levelSpeed.levelSpeed);
            obj.GetComponent<ChangeColor>().InitializeSpeed(levelSpeed.backgroundColorSpeed);
            obj.transform.localScale = new Vector3(initialScale, initialScale);
            levelSpeed.enemies.Add(obj);
            levelDisplay.enemies.Add(obj);
        }
        return randoms;
    }
}
