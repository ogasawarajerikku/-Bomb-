using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;

public class GameDirector : MonobitEngine.MonoBehaviour
{
    bool gameClear = false;
    //死亡したプレイヤーの数
    int deadPlayerCount = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (gameClear == false)
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("DeadPlayer"))
            {
                /*死亡したプレイヤーの数をカウントしそれが自分を除いたプレイヤーの数と
                 * 一致したら
                */

                gameClear = true;
            }
        }
    }
}
