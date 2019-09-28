using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeBackgroundColor : MonoBehaviour
{
    // Start is called before the first frame update

    public Camera camera;
    byte color1, color2, color3;
    int colorTurn = 1;
    bool ascending = true;
    public byte changeColorSpeed;

    void Start()
    {
        camera = GetComponent<Camera>();
        camera.clearFlags = CameraClearFlags.SolidColor;
        color1 = color3 = 50;
        color2 = 200;
    }

    // Update is called once per frame
    void Update()
    {
        camera.backgroundColor = new Color32(color1, color2, color3, 255);
        updateColors();
    }

    void updateColors()
    {
        switch (colorTurn)
        {
            case 1:
                if (ascending)
                    color1 += changeColorSpeed;
                else
                    color1 -= changeColorSpeed;
                if (color1 <= 50 || color1 >= 200)
                {
                    colorTurn++;
                    ascending = !ascending;
                }
                break;
            case 2:
                if (ascending)
                    color2 += changeColorSpeed;
                else
                    color2 -= changeColorSpeed;
                if (color2 <= 50 || color2 >= 200)
                {
                    colorTurn++;
                    ascending = !ascending;
                }
                break;
            case 3:
                if (ascending)
                    color3 += changeColorSpeed;
                else
                    color3 -= changeColorSpeed;
                if (color3 <= 50 || color3 >= 200)
                {
                    colorTurn = 1;
                    ascending = !ascending;
                }
                break;
        }
    }

    public void SpeedUp(byte value)
    {
        changeColorSpeed += value; 
    }

    public void SlowDown(byte value)
    {
        changeColorSpeed -= value;
    }

    public void InitializeSpeed(byte value)
    {
        changeColorSpeed = value;
    }

    public void Enable(bool enable)
    {
        enabled = enable;
    }
}
