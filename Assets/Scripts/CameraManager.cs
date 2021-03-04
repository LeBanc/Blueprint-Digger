using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Vector3 initPosition;

    // Start is called before the first frame update
    void Start()
    {
        initPosition = transform.position;    // save init position
    }

    // Update is called once per frame: update is used to move camera with mouse or keyboard
    public void CameraUpdate()
    {
        // Move up
        if (Input.GetKey(KeyCode.UpArrow) || (Input.mousePosition.y > Screen.height - 5))
        {
            transform.position = transform.position + new Vector3(0, 0, 0.1f);
        }

        // Move down
        if (Input.GetKey(KeyCode.DownArrow) || (Input.mousePosition.y < 5))
        {
            transform.position = transform.position + new Vector3(0, 0, -0.1f);
        }

        // Move right
        if (Input.GetKey(KeyCode.RightArrow) || (Input.mousePosition.x > Screen.width - 5))
        {
            transform.position = transform.position + new Vector3(0.1f, 0, 0);
        }

        // Move left
        if (Input.GetKey(KeyCode.LeftArrow) || (Input.mousePosition.x < 5))
        {
            transform.position = transform.position + new Vector3(-0.1f, 0, 0);
        }

        // Move back to init position
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = initPosition;
        }
    }

    // SetInitPosition function is used to set the camera initial position to a definite value
    public void SetInitPosition(Vector3 position)
    {
        initPosition = position;
    }
}
