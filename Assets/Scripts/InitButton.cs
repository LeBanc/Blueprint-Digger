using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitButton : MonoBehaviour
{

    public GameObject building;
    public bool radar;
    private Button button;
    private GridLocation grid;
    private int slotNumber;
    private Image img;

    // Start is called before the first frame update
    public void Init(bool _inhibit = false)
    {
        button = GetComponent<Button>();
        img = GetComponent<Image>();
        grid = FindObjectOfType<GridLocation>();

        if (!_inhibit)
        {
            if (radar)
            {
                button.onClick.AddListener(() => grid.StartRadar(slotNumber));
            }
            else
            {
                button.onClick.AddListener(() => grid.PlaceBuldings(building, slotNumber));
            }
            StartCoroutine(FadeIn());
        }
        else
        {
            button.interactable = false;
        }
    }

    public int GetSlot()
    {
        return slotNumber;
    }

    public void SetSlot(int s)
    {
        slotNumber = s;
    }

    public IEnumerator FadeIn()
    {
        float r, g, b;
        r = img.color.r;
        g = img.color.g;
        b = img.color.b;
        // loop over 1 second
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            img.color = new Color(r, g, b, i);
            yield return null;
        }
    }

    public IEnumerator FadeOut()
    {
        float r, g, b;
        r = img.color.r;
        g = img.color.g;
        b = img.color.b;
        // loop over 1 second
        for (float i = 1; i >= 0; i -= Time.deltaTime)
        {
            // set color with i as alpha
            if(img != null) img.color = new Color(r, g, b, i);
            yield return null;
        }
    }
}
