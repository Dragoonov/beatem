using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEndlessEnemies : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject[] gameObjects;
    private float initialScale;
    public LevelSpeed levelSpeed;
    public LevelDisplay levelDisplay;
    GameObject lastObject;

    private float[] initialAngles;

    void Start()
    {
        initialScale = 15f;
        levelSpeed = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelSpeed>();
        levelDisplay = GameObject.FindGameObjectWithTag("Level").GetComponent<LevelDisplay>();
        initialAngles = new float[] { 45, -45, 90, -90, 135, -135, 180, -180 };
    }
 
    // Update is called once per frame
    void Update()
    {
        InitializeNewEnemy();
    }

    private void InitializeNewEnemy()
    {
        float previousAngle = 0;
        float scaleDifference = 0;
        if(lastObject == null || lastObject.transform.localScale.x < initialScale-5)
        {
            GameObject obj = Instantiate(gameObjects[Random.Range(0, gameObjects.Length)], new Vector3(0, 0, 0), Quaternion.identity);
            if (obj.tag == "GroupEnemy")
            {
                previousAngle = initialAngles[Random.Range(0, initialAngles.Length)];
                scaleDifference = obj.transform.GetChild(1).gameObject.transform.localScale.x - obj.transform.GetChild(0).gameObject.transform.localScale.x;
                Debug.Log(scaleDifference);
                Debug.Log("Level speed" + levelSpeed.levelSpeed);
                for (int i=0; i < obj.transform.childCount; i++)
                {
                    GameObject temp = obj.transform.GetChild(i).gameObject;
                    temp.transform.Rotate(0, 0, previousAngle+temp.transform.rotation.z);
                    temp.GetComponent<Shrink>().InitializeSpeed(levelSpeed.levelSpeed);
                    temp.GetComponent<ChangeColor>().InitializeSpeed(levelSpeed.backgroundColorSpeed);
                    temp.transform.localScale = new Vector3(initialScale+i*scaleDifference, initialScale+i*scaleDifference);
                    levelSpeed.enemies.Add(temp);
                    levelDisplay.enemies.Add(temp);
                    lastObject = temp;
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
                lastObject = obj;
            }
        }
    }
}
