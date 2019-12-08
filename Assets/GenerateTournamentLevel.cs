using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateTournamentLevel : MonoBehaviour
{
    public GameObject[] gameObjects;
    private float initialScale;
    public LevelSpeed levelSpeed;
    public LevelDisplay levelDisplay;
    GameObject level;
    List<GameObject> levelObjects;
    public int objectsNumber;
    private int currentEnemy;
    public bool startLevel;

    private float[] initialAngles;

    void Start()
    {
        startLevel = false;
        initialScale = 15f;
        levelSpeed = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelSpeed>();
        levelDisplay = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelDisplay>();
        level = levelDisplay.gameObject;
        levelObjects = new List<GameObject>();
        initialAngles = new float[] { 45, -45, 90, -90, 135, -135, 180, -180 };
        currentEnemy = 0;
        Debug.Log(levelObjects);
        level.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(startLevel)
        {
            ReleaseEnemies();
        }
    }

    public List<GameObject> GenerateLevel()
    {
        for (int i = 0; i < objectsNumber; i++)
        {
            InitializeNewEnemy();
        }
        return levelObjects;
    }

    public void SetLevel(List<GameObject> level)
    {
        levelObjects = level;
    }

    public List<GameObject> getLevel()
    {
        return levelObjects;
    }

    public void StartLevel()
    {
         level.SetActive(true);
         startLevel = true;
    }

    private void ReleaseEnemies()
    {
        if(currentEnemy >= objectsNumber-1)
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

    private void InitializeNewEnemy()
    {
        float previousAngle;
        float scaleDifference;
        GameObject obj = Instantiate(gameObjects[Random.Range(0, gameObjects.Length)], new Vector3(0, 0, 0), Quaternion.identity);
        obj.SetActive(false);
        levelObjects.Add(obj);

        if (obj.tag == "GroupEnemy")
        {
            previousAngle = initialAngles[Random.Range(0, initialAngles.Length)];
            scaleDifference = obj.transform.GetChild(1).gameObject.transform.localScale.x - obj.transform.GetChild(0).gameObject.transform.localScale.x;
            Debug.Log(scaleDifference);
            for (int i = 0; i < obj.transform.childCount; i++)
            {
                GameObject temp = obj.transform.GetChild(i).gameObject;
                temp.transform.Rotate(0, 0, previousAngle + temp.transform.rotation.z);
                temp.GetComponent<Shrink>().InitializeSpeed(levelSpeed.levelSpeed);
                temp.GetComponent<ChangeColor>().InitializeSpeed(levelSpeed.backgroundColorSpeed);
                temp.transform.localScale = new Vector3(initialScale + i*scaleDifference, initialScale + i*scaleDifference);
                levelSpeed.enemies.Add(temp);
                levelDisplay.enemies.Add(temp);
            }
        }
        else
        {
            obj.transform.Rotate(0, 0, initialAngles[Random.Range(0, initialAngles.Length)]);
            obj.GetComponent<Shrink>().InitializeSpeed(levelSpeed.levelSpeed);
            obj.GetComponent<ChangeColor>().InitializeSpeed(levelSpeed.backgroundColorSpeed);
            obj.transform.localScale = new Vector3(initialScale, initialScale);
            levelSpeed.enemies.Add(obj);
            levelDisplay.enemies.Add(obj);
        }
    }
}
