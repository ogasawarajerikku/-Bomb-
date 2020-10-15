using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomButtonsController : MonoBehaviour
{
    [SerializeField]
    List<GameObject> roomButtons = new List<GameObject>();
    [SerializeField]
    Text thisButtonText = null;
    bool roomButtonsStatus = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void inputButton()
    {
        for(int i = 0;i<roomButtons.Count;i++)
        {
            setFrag(roomButtons[i]);
        }
        if (thisButtonText.text == "↓")
            thisButtonText.text = "↑";
        else
            thisButtonText.text = "↓";
    }
    void setFrag(GameObject obj)
    {
        if (obj.activeInHierarchy == false)
            obj.SetActive(true);
        else
            obj.SetActive(false);
    }
}
