using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

    public DroneController drone;

    Camera mainCamera;
    Vector3 cameraDelatPos;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        cameraDelatPos = mainCamera.transform.position - drone.transform.position;
        StartCoroutine(DroneUpdate());
    }

    // Update is called once per frame
    IEnumerator DroneUpdate()
    {
        while (true)
        {
            if (!drone.isMoving)
            {
                yield return new WaitForSeconds(1.5f);
                int _action = Random.Range(0, 5);
                switch (_action)
                {
                    case 0:
                        StartCoroutine(drone.TurnLeft());
                        break;
                    case 1:
                        StartCoroutine(drone.TurnRight());
                        break;
                    case 2:
                        StartCoroutine(drone.MoveForward());
                        while (drone.isMoving)
                        {
                            yield return null;
                        }
                        StartCoroutine(drone.MoveForward());
                        break;
                    case 3:
                        StartCoroutine(drone.Scan());
                        break;
                    case 4:
                        StartCoroutine(drone.UTurnLeft());
                        break;
                    case 5:
                        StartCoroutine(drone.UTurnRight());
                        break;
                    default:
                        break;
                }
            }
            yield return null;
        }
    }

    private void Update()
    {
        mainCamera.transform.position = drone.transform.position + cameraDelatPos;
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
