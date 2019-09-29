using UnityEngine;

public class ChangeColor : MonoBehaviour
{

    // Reference to Sprite Renderer component
    private SpriteRenderer rend;

    // Color value that we can set in Inspector
    // It's White by default
    public Color colorToTurnTo;
    private Color currentColor;

    byte color1, color2, color3;
    int colorTurn;
    bool ascending;
    public byte changeColorSpeed;

    // Use this for initialization
    private void Start()
    {

        // Assign Renderer component to rend variable
        rend = GetComponent<SpriteRenderer>();
        colorToTurnTo = Color.black;
        // Change sprite color to selected color
        rend.color = colorToTurnTo;
        currentColor = rend.color;

        color1 = color3 = 200;
        color2 = 50;
        ascending = false;
        colorTurn = 1;
    }

    private void Update()
    {
        if (currentColor != colorToTurnTo)
        {
            rend.color = colorToTurnTo;
            currentColor = colorToTurnTo;
        }
       
    }

    public void ChangeObjectColor(Color color)
    {
        colorToTurnTo = color;
    }

    public void UpdateColors()
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
        rend.color = new Color32(color1, color2, color3, 255);
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