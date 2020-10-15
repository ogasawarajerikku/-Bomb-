using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeRecover : ItemController
{
    //回復量
    float PlayerhealValue = 30f;
    // Start is called before the first frame update
    void Start()
    {
        itemText.text = "回復";
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<HpStatus>().PlusHitPoint(PlayerhealValue);
            PickUp();
        }
    }
}
