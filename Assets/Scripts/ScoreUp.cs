using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreUp : ItemController
{
    //[SerializeField, Tooltip("取得時のスコア増加量")]
    int ItemGetScore = 1000;
    //[SerializeField, Tooltip("スコア表示キャンバス")]
    GameObject scoreCanvas = null;
    //スコア管理スクリプト
    ScoreDirector scoreDirector = null;
    // Start is called before the first frame update
    void Start()
    {
        scoreCanvas = GameObject.Find("GameScoreCanvas");
        scoreDirector = scoreCanvas.GetComponent<ScoreDirector>();
        itemText.text = "スコア";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            scoreDirector.AddScore(ItemGetScore);
            PickUp();
        }
    }
}
