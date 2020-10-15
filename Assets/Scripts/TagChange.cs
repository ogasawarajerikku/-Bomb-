using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//制作者　小笠原ジェリック
/// <summary>
/// プレイヤーの現在位置を取得しタグを変更する
/// </summary>
public class TagChange : MonoBehaviour
{
    GameObject player;
    //誤差調整用の数値
    float groundTopPos = 0f;
    void Start()
    {
        player = GameObject.Find("Player");
        groundTopPos = transform.localScale.y / 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y > transform.position.y+groundTopPos)
            gameObject.tag = "Ground";
        else
            gameObject.tag = "Wall";
    }
}
