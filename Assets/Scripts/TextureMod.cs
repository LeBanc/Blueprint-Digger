using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureMod : MonoBehaviour
{

    public int x = 512;
    public int y = 512;
    private Texture2D _FoWTexture;

    private void Start()
    {
        _FoWTexture = new Texture2D(x, y);

        string[] s = transform.GetComponent<MeshRenderer>().material.GetTexturePropertyNames();
        /*for(int k = 0; k < s.Length; k++)
        {
            Debug.Log(s[k]);
        }*/
        transform.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", _FoWTexture);

        for(int i = 0; i < x; i++)
        {
            for(int j = 0; j < y; j++)
            {
                _FoWTexture.SetPixel(i, j, Color.black);
            }
        }
        _FoWTexture.Apply();
    }

    private void FixedUpdate()
    {
        int tempX = Random.Range(0, x);
        int tempY = Random.Range(0, y);
        Debug.Log(tempX + " - " + tempY);
        _FoWTexture.SetPixel(tempX, tempY, new Color(1, 1, 1, 0));
        _FoWTexture.Apply();
    }

}
