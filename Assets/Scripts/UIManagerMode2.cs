using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerMode2 : UIManager
{

    public GameObject firstButton;
    public GameObject secondButton;
    public GameObject thirdButton;
    public GameObject fourthButton;

    public int[] initNumbers = { 2, 2, 2, 0 };


    // Start is called before the first frame update
    void Awake()
    {
        // Initialize First Button
        firstButton.GetComponent<InitButton>().SetSlot(1);
        firstButton.GetComponent<InitButton>().Init(initNumbers[0] == 0);
        firstButton.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = initNumbers[0].ToString();
        
        // Initialize Second Button
        secondButton.GetComponent<InitButton>().SetSlot(2);
        secondButton.GetComponent<InitButton>().Init(initNumbers[1] == 0);
        secondButton.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = initNumbers[1].ToString();

        // Initialize Third Button
        thirdButton.GetComponent<InitButton>().SetSlot(3);
        thirdButton.GetComponent<InitButton>().Init(initNumbers[2] == 0);
        thirdButton.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = initNumbers[2].ToString();

        // Initialize Fourth Button
        fourthButton.GetComponent<InitButton>().SetSlot(4);
        fourthButton.GetComponent<InitButton>().Init(initNumbers[3] == 0);
        fourthButton.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = initNumbers[3].ToString();
    }

    public override void ChangeButton(int i)
    {
        switch (i)
        {
            case 1:
                initNumbers[0]--;
                firstButton.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = initNumbers[0].ToString();
                if (initNumbers[0] == 0)
                {
                    firstButton.GetComponent<Button>().onClick.RemoveAllListeners();
                    firstButton.GetComponent<Button>().interactable = false;
                }
                break;
            case 2:
                initNumbers[1]--;
                secondButton.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = initNumbers[1].ToString();
                if (initNumbers[1] == 0)
                {
                    secondButton.GetComponent<Button>().onClick.RemoveAllListeners();
                    secondButton.GetComponent<Button>().interactable = false;
                }
                break;
            case 3:
                initNumbers[2]--;
                thirdButton.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = initNumbers[2].ToString();
                if (initNumbers[2] == 0)
                {
                    thirdButton.GetComponent<Button>().onClick.RemoveAllListeners();
                    thirdButton.GetComponent<Button>().interactable = false;
                }
                break;
            case 4:
                initNumbers[3]--;
                fourthButton.transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = initNumbers[3].ToString();
                if (initNumbers[3] == 0)
                {
                    fourthButton.GetComponent<Button>().onClick.RemoveAllListeners();
                    fourthButton.GetComponent<Button>().interactable = false;
                }
                break;
            default:
                break;
        }
    }
}
