using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableTrashWhenBuilding : MonoBehaviour
{
    private Transform trashFolder;

    // Start is called before the first frame update
    void Start()
    {
        trashFolder = transform.Find("Trash");
    }

    //EnableTrash function activate Trash Transform and all its children
    public void EnableTrash()
    {
        trashFolder = transform.Find("Trash");
        trashFolder.gameObject.SetActive(true);
    }
}
