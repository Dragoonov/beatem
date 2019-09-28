using UnityEngine;

public class ChangeColor : MonoBehaviour
{

    // Reference to Sprite Renderer component
    private SpriteRenderer rend;

    // Color value that we can set in Inspector
    // It's White by default
    public Color colorToTurnTo;
    private Color currentColor;

    // Use this for initialization
    private void Start()
    {

        // Assign Renderer component to rend variable
        rend = GetComponent<SpriteRenderer>();
        colorToTurnTo = Color.black;
        // Change sprite color to selected color
        rend.color = colorToTurnTo;
        currentColor = rend.color;
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
}