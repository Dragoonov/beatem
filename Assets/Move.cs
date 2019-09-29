using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotateSpeed;
    void Start()
    {
        rotateSpeed = 5f;
    }

    // Update is called once per frame
    void Update()
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
