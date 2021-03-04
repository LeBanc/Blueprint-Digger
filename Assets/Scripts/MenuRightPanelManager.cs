using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRightPanelManager : MonoBehaviour
{
    public GameObject[] panelsList;

    private float speed;
    private Coroutine panelMov;
    private int selectedPanel;

    // Start is called before the first frame update
    void Start()
    {
        speed = Mathf.Lerp(0f, 640f, 1f);
        InitPanel();
    }

    public void InitPanel()
    {
        transform.localPosition = new Vector3(1600f, 0f, 0f);
        for (int i = 0; i < panelsList.Length; i++)
        {
            panelsList[i].SetActive(false);
        }
        selectedPanel = 0;
    }

    public void ChangePanel(int panel)
    {
        if (panelMov == null)
        { 
            panelMov = StartCoroutine(PanelChange(panel));
        }
    }

    private IEnumerator PanelChange(int panel)
    {
        // if the pannel is out
        if (selectedPanel != 0)
        {
            // retract the pannel
            while(transform.localPosition.x < 1600f )
            {
                transform.localPosition += new Vector3(speed*Time.deltaTime, 0f, 0f);
                yield return null;
            }
            transform.localPosition = new Vector3(1600f, 0f, 0f);
        }

        // then change the active pannel 0:None, 1:Adventure, 2:Puzzle, 3:Options, 4:Controls, 5:Credits
        // Start by deactivating all pannels
        for (int i = 0; i < panelsList.Length; i++)
        {
            panelsList[i].SetActive(false);
        }

        // then activate the chosen one if it is not the previous selectedPannel
        if(selectedPanel != panel)
        {
            yield return new WaitForSeconds(0.2f);

            panelsList[panel-1].SetActive(true);

            // then make the pannel go out
            while (transform.localPosition.x > 960f)
            {
                transform.localPosition += new Vector3(-speed * Time.deltaTime, 0f, 0f);
                yield return null;
            }
            transform.localPosition = new Vector3(960f, 0f, 0f);
            selectedPanel = panel;
        }
        else // the pannel is kept hidden and the selectedPannel is reset
        {
            selectedPanel = 0;
        }

        // clear pannelMov selection to allow a new Coroutine
        panelMov = null;
    }

}
