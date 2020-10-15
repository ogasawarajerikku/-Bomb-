using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    /*ステータス*/
    //HP
    [Range(1f,100f)]
    float hitPoint = 1f;
    //移動速
    [Range(1f,100f)]
    float speed = 1f;

    /*武器*/
    //銃口の位置
    Transform mazzlePos = null;
    //生成する弾丸
    GameObject enemyBullet = null;

    //これはテスト移動用
    [SerializeField]
    GameObject player = null;
    NavMeshAgent navMeshAgent = null;
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.SetDestination(player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
