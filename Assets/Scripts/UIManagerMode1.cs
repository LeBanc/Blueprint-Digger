using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerMode1 : UIManager
{
    public GameObject radarButton;
    public GameObject buildingButton1;
    public GameObject buildingButton2;
    public GameObject buildingButton3;
    public GameObject buildingButton4;

    public GameObject[] buttonSlots = new GameObject[3];

    private Vector3 slot1 = new Vector3(-278f, 25f, 0f);
    private Vector3 slot2 = new Vector3(-100, 25f, 0f);
    private Vector3 slot3 = new Vector3(79.5f, 25f, 0f);

    private GameObject firstButton;
    private GameObject secondButton;
    private GameObject thirdButton;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize First Button
        if (buttonSlots[0] != null)
        {
            firstButton = Instantiate(buttonSlots[0], this.transform);
        }
        else
        {
            firstButton = Instantiate(SelectBuilding(), this.transform);
        }
        firstButton.transform.localPosition = slot1;
        firstButton.GetComponent<InitButton>().SetSlot(1);
        firstButton.GetComponent<InitButton>().Init();

        // Initialize Second Button
        if (buttonSlots[1] != null)
        {
            secondButton = Instantiate(buttonSlots[1], this.transform);
        }
        else
        {
            secondButton = Instantiate(SelectBuilding(), this.transform);
        }
        secondButton.transform.localPosition = slot2;
        secondButton.GetComponent<InitButton>().SetSlot(2);
        secondButton.GetComponent<InitButton>().Init();

        // Initialize Third Button
        if (buttonSlots[2] != null)
        {
            thirdButton = Instantiate(buttonSlots[2], this.transform);
        }
        else
        {
            thirdButton = Instantiate(SelectBuilding(), this.transform);
        }
        thirdButton.transform.localPosition = slot3;
        thirdButton.GetComponent<InitButton>().SetSlot(3);
        thirdButton.GetComponent<InitButton>().Init();
    }

    private GameObject SelectBuilding()
    {
        GameObject go = buildingButton1;
        float f = Random.Range(0f, 100f);
        if (f > 95f)
        {
            go = radarButton;
        }       
        else if(f > 85f)
        {
            go = buildingButton4;
        }else if(f > 70f)
        {
            go = buildingButton3;
        } else if(f > 40f)
        {
            go = buildingButton2;
        }
        return go;
    }

    public override void ChangeButton(int i)
    {
        switch (i)
        {
            case 1:
                StartCoroutine(firstButton.GetComponent<InitButton>().FadeOut());
                Destroy(firstButton,1.025f);
                firstButton = Instantiate(SelectBuilding(), this.transform);
                firstButton.transform.localPosition = slot1;
                firstButton.GetComponent<InitButton>().SetSlot(1);
                firstButton.GetComponent<InitButton>().Init();
                break;

            case 2:
                StartCoroutine(secondButton.GetComponent<InitButton>().FadeOut());
                Destroy(secondButton, 1);
                secondButton = Instantiate(SelectBuilding(), this.transform);
                secondButton.transform.localPosition = slot2;
                secondButton.GetComponent<InitButton>().SetSlot(2);
                secondButton.GetComponent<InitButton>().Init();
                break;

            case 3:
                StartCoroutine(thirdButton.GetComponent<InitButton>().FadeOut());
                Destroy(thirdButton, 1);
                thirdButton = Instantiate(SelectBuilding(), this.transform);
                thirdButton.transform.localPosition = slot3;
                thirdButton.GetComponent<InitButton>().SetSlot(3);
                thirdButton.GetComponent<InitButton>().Init();
                break;

            default:
                break;
        }
    }

}
