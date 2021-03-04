using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    // for debug
    public bool turnRight;
    public bool turnLeft;
    public bool moveForward;
    public bool scan;
    public bool radar;

    public ParticleSystem boosterRight;
    public ParticleSystem boosterLeft;
    public ParticleSystem boosterRear;
    public GameObject scanner;
    public GameObject radarCircle;

    public float rotationSpeed=1f;
    public float translationSpeed=1f;
    public float radarSpeed = 0.5f;
    public float maxRadarScale = 100f;

    private float initRadarScale;

    private AudioSource engineAudioSource;
    private AudioSource scannerAudioSource;
    private AudioSource radarAudioSource;
    private AudioSource winAudioSource;

    [HideInInspector]
    public bool isMoving;

    private void Start()
    {
        isMoving = false;
        initRadarScale = radarCircle.transform.localScale.x;

        // Get all AudiouSources
        engineAudioSource = GetComponent<AudioSource>();
        scannerAudioSource = transform.Find("Scanner").GetComponent<AudioSource>();
        radarAudioSource = transform.Find("RadarCircle").GetComponent<AudioSource>();
        winAudioSource = transform.Find("WinSound").GetComponent<AudioSource>();
    }

    // for debug
    private void Update()
    {
        if (!isMoving)
        {
            if (turnRight) StartCoroutine(TurnRight());
            if (turnLeft) StartCoroutine(TurnLeft());
            if (moveForward) StartCoroutine(MoveForward());
            if (scan) StartCoroutine(Scan());
            if (radar) StartCoroutine(Radar());
        }
    }

    public IEnumerator TurnLeft()
    {
        isMoving = true;
        boosterRight.Play();
        engineAudioSource.pitch = Random.Range(3.5f, 3.7f);
        engineAudioSource.volume = 0.5f;

        float angle = 0f;
        float deltaAngle = Mathf.LerpAngle(0f, -90f, Time.deltaTime*rotationSpeed);

        while (angle > -90f)
        {
            if ((angle + deltaAngle) < -90f)
            {
                deltaAngle = -90 - angle;
                angle = -90f;
            }
            else
            {
                angle += deltaAngle;
            }
            transform.Rotate(0f, deltaAngle , 0f);
            yield return null;
        }

        boosterRight.Stop();
        engineAudioSource.pitch = 0.25f;
        engineAudioSource.volume = 1f;
        //turnLeft = false; // for debug
        isMoving = false;
    }

    public IEnumerator TurnRight()
    {
        isMoving = true;
        boosterLeft.Play();
        engineAudioSource.pitch = Random.Range(3.5f, 3.7f);
        engineAudioSource.volume = 0.5f;

        float angle = 0f;
        float deltaAngle = Mathf.LerpAngle(0f, 90f, Time.deltaTime*rotationSpeed);

        while (angle < 90f)
        {
            if ((angle + deltaAngle) > 90f)
            {
                deltaAngle = 90 - angle;
                angle = 90f;
            }
            else
            {
                angle += deltaAngle;
            }
            transform.Rotate(0f, deltaAngle, 0f);
            yield return null;
        }

        boosterLeft.Stop();
        engineAudioSource.pitch = 0.25f;
        engineAudioSource.volume = 1f;
        //turnRight = false; // for debug
        isMoving = false;
    }

    public IEnumerator UTurnLeft()
    {
        isMoving = true;
        boosterRight.Play();
        engineAudioSource.pitch = Random.Range(3.5f, 3.7f);
        engineAudioSource.volume = 0.5f;

        float angle = 0f;
        float deltaAngle = Mathf.LerpAngle(0f, -90f, Time.deltaTime * rotationSpeed);

        while (angle > -180f)
        {
            if ((angle + deltaAngle) < -180f)
            {
                deltaAngle = -180 - angle;
                angle = -180f;
            }
            else
            {
                angle += deltaAngle;
            }
            transform.Rotate(0f, deltaAngle, 0f);
            yield return null;
        }

        boosterRight.Stop();
        engineAudioSource.pitch = 0.25f;
        engineAudioSource.volume = 1f;
        //turnLeft = false; // for debug
        isMoving = false;
    }

    public IEnumerator UTurnRight()
    {
        isMoving = true;
        boosterLeft.Play();
        engineAudioSource.pitch = Random.Range(3.5f, 3.7f);
        engineAudioSource.volume = 0.5f;

        float angle = 0f;
        float deltaAngle = Mathf.LerpAngle(0f, 90f, Time.deltaTime * rotationSpeed);

        while (angle < 180f)
        {
            if ((angle + deltaAngle) > 180f)
            {
                deltaAngle = 180 - angle;
                angle = 180f;
            }
            else
            {
                angle += deltaAngle;
            }
            transform.Rotate(0f, deltaAngle, 0f);
            yield return null;
        }

        boosterLeft.Stop();
        engineAudioSource.pitch = 0.25f;
        engineAudioSource.volume = 1f;
        //turnRight = false; // for debug
        isMoving = false;
    }

    public IEnumerator MoveForward()
    {
        isMoving = true;
        boosterRear.Play();

        engineAudioSource.pitch = Random.Range(0.95f, 1.05f);

        float maxDist = 1.5f; //max dist = 1.5 because gridsize is 0.15

        float dist = 0f;
        float deltaDist = Mathf.LerpAngle(0f, maxDist, Time.deltaTime * translationSpeed);

        while (dist < maxDist)
        {
            if ((dist + deltaDist) > maxDist)
            {
                deltaDist = maxDist - dist;
                dist = maxDist;
            }
            else
            {
                dist += deltaDist;
            }
            transform.Translate(0f, 0f, deltaDist);
            yield return null;
        }

        boosterRear.Stop();
        engineAudioSource.pitch = 0.25f;
        //moveForward = false; // for debug
        isMoving = false;
    }

    public IEnumerator Scan()
    {
        isMoving = true;
        scanner.SetActive(true);
        scannerAudioSource.Play();

        float angle = 0f;
        float maxAngle = -45;
        float deltaAngle = Mathf.LerpAngle(0f, maxAngle, Time.deltaTime * 2.5f);

        while (angle > maxAngle)
        {
            if ((angle + deltaAngle) < maxAngle)
            {
                deltaAngle = maxAngle - angle;
                angle = maxAngle;
            }
            else
            {
                angle += deltaAngle;
            }
            scanner.transform.Rotate(deltaAngle, 0f, 0f);
            yield return null;
        }

        maxAngle = 45f;
        deltaAngle = Mathf.LerpAngle(0f, maxAngle, Time.deltaTime * 2.5f);
        while (angle < maxAngle)
        {
            if ((angle + deltaAngle) > maxAngle)
            {
                deltaAngle = maxAngle - angle;
                angle = maxAngle;
            }
            else
            {
                angle += deltaAngle;
            }
            scanner.transform.Rotate(deltaAngle, 0f, 0f);
            yield return null;
        }

        maxAngle = -45f;
        deltaAngle = Mathf.LerpAngle(0f, maxAngle, Time.deltaTime * 2.5f);
        while (angle > 0f)
        {
            if ((angle + deltaAngle) < 0f)
            {
                deltaAngle = 0f -angle;
                angle = 0f;
            }
            else
            {
                angle += deltaAngle;
            }
            scanner.transform.Rotate(deltaAngle, 0f, 0f);
            yield return null;
        }

        scanner.SetActive(false);
        //scan = false; // for debug
        isMoving = false;
    }

    public IEnumerator Radar()
    {
        isMoving = true;
        radarCircle.transform.localScale = new Vector3(initRadarScale, radarCircle.transform.localScale.y, initRadarScale);
        radarCircle.SetActive(true);
        radarAudioSource.Play();
        float deltaScale = Mathf.Lerp(initRadarScale, maxRadarScale, radarSpeed*Time.deltaTime);

        while(radarCircle.transform.localScale.x < maxRadarScale)
        {
            radarCircle.transform.localScale += new Vector3(deltaScale, 0f, deltaScale);
            yield return null;
        }

        radarCircle.SetActive(false);
        radar = false;
        isMoving = false;
    }

    public IEnumerator WinAnimation()
    {
        Vector3 initScale = transform.localScale;
        float deltaRotation = Mathf.Lerp(0, 5 * 360f, 2f);
        float deltaScale = Mathf.Lerp(0.2f, 1f, 2f);
        float scale = 1f;

        winAudioSource.Play();

        while(scale > 0.2)
        {
            // Shrink the drone
            scale -= deltaScale * Time.deltaTime;
            transform.localScale = initScale * scale;

            // Rotate the drone
            transform.RotateAround(transform.position, Vector3.up, deltaRotation * Time.deltaTime);

            yield return null;
        }
    }
}
