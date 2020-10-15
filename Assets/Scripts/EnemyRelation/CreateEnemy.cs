using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemy : MonoBehaviour
{
    [SerializeField, Tooltip("生成する敵")]
    GameObject enemyPrefab = null;
    //敵生成位置作成用
    Vector3 createPosition = new Vector3(0, 0, 0);
    float x = 0;
    float y = 0;
    float z = 0;
    [SerializeField, Range(-100f, 0f)]
    float minPos = 0f;
    [SerializeField, Range(0f, 100f)]
    float maxPos = 0f;
    //経過時間
    float elapsedTime = 0;
    [SerializeField, Range(1f, 5f),Tooltip("生成間隔")]
    float createTime = 1f;
    private void Start()
    {
        y = transform.position.y;
    }
    // Update is called once per frame
    void Update()
    {
        // ホスト以外は処理をしない
        if (!MonobitEngine.MonobitNetwork.isHost)
        {
            return;
        }
        elapsedTime += Time.deltaTime;
        if(elapsedTime>createTime)
        {
            RandomCreateEnemy();
            elapsedTime = 0;
        }
    }
    void CheckEnemyCount()
    {

    }
    void RandomCreateEnemy()
    {
        //ランダムな座標作成
        x = Random.Range(minPos, maxPos);
        z = Random.Range(minPos, maxPos);
        createPosition = new Vector3(x, y, z);

        MonobitEngine.MonobitNetwork.Instantiate(enemyPrefab.name, createPosition, enemyPrefab.transform.rotation,0);
    }
}
