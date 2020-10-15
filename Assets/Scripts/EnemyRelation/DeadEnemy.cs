using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DeadEnemy : MonoBehaviour
{
    [SerializeField, Tooltip("死亡時の色")]
    Material deathMaterial = null;
    //[SerializeField, Tooltip("スコア表示キャンバス")]
    GameObject scoreCanvas = null;
    //スコア管理スクリプト
    ScoreDirector scoreDirector = null;
    //[SerializeField, Tooltip("死亡時のスコア増加量")]
    int deadScore = 500;
    //死亡時のアイテムドロップ

    //どのアイテムを落とすか
    int minItemRange = 0;
    int maxItemRange = 0;
    int dropItem = 0;
    List<GameObject> objList = new List<GameObject>();
    [SerializeField] GameObject dropItem1 = null;
    [SerializeField] GameObject dropItem2 = null;
    [SerializeField] GameObject dropItem3 = null;
    [SerializeField] GameObject dropItem4 = null;
    private void Start()
    {
        scoreCanvas = GameObject.Find("GameScoreCanvas");
        scoreDirector = scoreCanvas.GetComponent<ScoreDirector>();
    }
    public void Dead()
    {
        //アイテムをリストで管理する
        objList.Add(dropItem1);
        objList.Add(dropItem2);
        objList.Add(dropItem3);
        objList.Add(dropItem4);
        maxItemRange = objList.Count;
        dropItem = Random.Range(minItemRange, maxItemRange);
        GameObject drop = MonobitEngine.MonobitNetwork.Instantiate(objList[dropItem].name, transform.position, transform.rotation,0) as GameObject;
        GetComponent<Renderer>().material = deathMaterial;
        scoreDirector.AddScore(deadScore);
        Destroy(GetComponent<NavMeshAgent>());
        Destroy(GetComponent<SetPosition>());
        Destroy(GetComponent<MainEnemy>());
        Destroy(GetComponent<EnemyAttack>());
        Destroy(this.gameObject, 20f);
    }
}
