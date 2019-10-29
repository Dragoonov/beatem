using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotateSpeed;
    private float width;
    private float height;
    void Start()
    {
        rotateSpeed = 5f;
        width = Screen.width;
        height = Screen.height;
    }

    // Update is called once per frame
    void Update()
    {
        handleTouch();
        handleKeys();
    }

    private void handleTouch()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 position = touch.position;
            if(position.x < width / 2f)
            {
                transform.Rotate(0, 0, rotateSpeed);
            }
            else
            {
                transform.Rotate(0, 0, -rotateSpeed);
            }
        }
    }

    private void handleKeys()
    {
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, 0, rotateSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 0, -rotateSpeed);
        }
    }

    public void Enable(bool enable)
    {
        enabled = enable;
    }
}
