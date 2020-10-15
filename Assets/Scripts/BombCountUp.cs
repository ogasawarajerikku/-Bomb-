using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCountUp : ItemController
{
    //爆弾増加量
    int bombAddValue = 5;
    // Start is called before the first frame update
    void Start()
    {
        itemText.text = "爆弾個数アップ";
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().AddBomb(bombAddValue);
            PickUp();
        }
    }
}
