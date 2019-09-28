using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrink : MonoBehaviour
{
    // Start is called before the first frame update
    public float scaleSpeed;
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale -= new Vector3(scaleSpeed, scaleSpeed, scaleSpeed);
    }

    public void InitializeSpeed(float value)
    {
        scaleSpeed = value;
    }

    public void SpeedUp(float value)
    {
        scaleSpeed += value;
    }

    public void SlowDown(float value)
    {
        scaleSpeed -= value;
    }
}
