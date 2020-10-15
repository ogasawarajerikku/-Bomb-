using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDamageUp : ItemController
{
    //威力増加量
    float addDamageValue = 15f;
    // Start is called before the first frame update
    void Start()
    {
        itemText.text = "爆弾威力アップ";
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().AddBombDamage(addDamageValue);
            PickUp();
        }
    }
}
