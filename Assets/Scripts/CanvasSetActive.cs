using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSetActive : MonoBehaviour
{
    [SerializeField, Tooltip("表示するキャンバス")]
    GameObject displayCanvas = null;
    [SerializeField, Tooltip("消去するキャンバス")]
    GameObject deleteCanvas = null;
    // Start is called before the first frame update
    public void OnButtonClick()
    {
        displayCanvas.SetActive(true);
        deleteCanvas.SetActive(false);
        if (Time.timeScale == 1f)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }
}
