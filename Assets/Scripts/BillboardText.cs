using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 制作者　小笠原ジェリック
/// 制作日　2020/04/21
/// </summary>
public class BillboardText : MonoBehaviour
{
    GameObject billboardCanvas = null;
    GameObject billboardCanvasText = null;
    // Start is called before the first frame update
    private void Awake()
    {
        billboardCanvas = GameObject.Find("BillboardCanvas");
        billboardCanvasText = GameObject.Find("BillboardCanvasText");
    }
    void Start()
    {
        billboardCanvas.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            billboardCanvas.SetActive(true);
        billboardCanvasText.GetComponent<Text>().text = gameObject.GetComponent<Text>().text;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            billboardCanvas.SetActive(false);
    }
}
