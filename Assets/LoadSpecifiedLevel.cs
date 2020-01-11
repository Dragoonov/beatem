using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSpecifiedLevel : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] gameObjects;
    public GameObject finishCircle;
    private float initialScale;
    public LevelSpeed levelSpeed;
    public LevelDisplay levelDisplay;
    public List<GameObject> levelObjects;
    private int currentEnemy;
    private float[] initialAngles;

    void Start()
    {
        initialScale = 17f;
        levelSpeed = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelSpeed>();
        levelDisplay = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelDisplay>();
        levelObjects = new List<GameObject>();
        initialAngles = new float[] { 45, -45, 90, -90, 135, -135, 180, -180 };
        currentEnemy = 0;
        //Debug.Log(levelObjects);
        //levelObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(levelObjects.Count == 0)
        {
            levelObjects = GenerateLevel();
        }
        else
        {
            ReleaseEnemies();
        }
    }

    public List<GameObject> GenerateLevel()
    {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < gameObjects.Length; i++)
        {
            list.Add(InitializeNewEnemy(i, i%initialAngles.Length));
        }
        list.Add(InitializeNewEnemy(0,0,true));
        return list;
    }

    private GameObject InitializeNewEnemy(int i, int angle, bool finish = false)
    {
        float previousAngle;
        float scaleDifference;
        GameObject obj;
        if (!finish)
        {
            obj = Instantiate(gameObjects[i], new Vector3(0, 0, 0), Quaternion.identity);
        }
        else
        {
            obj = Instantiate(finishCircle, new Vector3(0, 0, 0), Quaternion.identity);
        }
        obj.SetActive(false);
        levelObjects.Add(obj);

        if (obj.tag == "GroupEnemy")
        {
            previousAngle = initialAngles[angle];
            scaleDifference = obj.transform.GetChild(1).gameObject.transform.localScale.x - obj.transform.GetChild(0).gameObject.transform.localScale.x;
            for (int j = 0; j < obj.transform.childCount; j++)
            {
                GameObject temp = obj.transform.GetChild(j).gameObject;
                temp.transform.Rotate(0, 0, previousAngle + temp.transform.rotation.z);
                temp.GetComponent<Shrink>().InitializeSpeed(levelSpeed.levelSpeed);
                if (temp.tag != "Finish")
                {
                    temp.GetComponent<ChangeColor>().InitializeSpeed(levelSpeed.backgroundColorSpeed);
                }
                temp.transform.localScale = new Vector3(initialScale + j * scaleDifference, initialScale + j * scaleDifference);
                levelSpeed.enemies.Add(temp);
                levelDisplay.enemies.Add(temp);
            }
        }
        else
        {
            obj.transform.Rotate(0, 0, initialAngles[angle]);
            obj.GetComponent<Shrink>().InitializeSpeed(levelSpeed.levelSpeed);
            obj.GetComponent<ChangeColor>().InitializeSpeed(levelSpeed.backgroundColorSpeed);
            obj.transform.localScale = new Vector3(initialScale, initialScale);
            levelSpeed.enemies.Add(obj);
            levelDisplay.enemies.Add(obj);
        }
        return obj;
    }

    private void ReleaseEnemies()
    {
        if (currentEnemy >= levelObjects.Count)
        {
            return;
        }
        if (currentEnemy == 0)
        {
            levelObjects[currentEnemy].SetActive(true);
            currentEnemy++;
        }
        else
        {
            GameObject previousObject = levelObjects[currentEnemy - 1];
            if (previousObject != null && previousObject.tag == "GroupEnemy")
            {
                previousObject = previousObject.transform.GetChild(previousObject.transform.childCount - 1).gameObject;
            }
            if (previousObject == null || previousObject.transform.localScale.x < initialScale - 5)
            {
                levelObjects[currentEnemy].SetActive(true);
                currentEnemy++;
            }
        }
    }
}
