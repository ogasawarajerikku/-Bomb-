using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCreateItem : MonoBehaviour
{
    //敵生成位置作成用
    Vector3 createPosition = new Vector3(0, 0, 0);
    float x = 0;
    float y = 2;
    float z = 0;
    [SerializeField, Range(-50f, 0f)]
    float minPos = 0f;
    [SerializeField, Range(0f, 50f)]
    float maxPos = 0f;
    //経過時間
    float elapsedTime = 0;
    [SerializeField, Range(1f, 5f), Tooltip("生成間隔")]
    float createTime = 1f;
    //どのアイテムを落とすか
    int minItemRange = 0;
    int maxItemRange = 0;
    int dropItem = 0;
    List<GameObject> objList = new List<GameObject>();
    [SerializeField] GameObject dropItem1 = null;
    [SerializeField] GameObject dropItem2 = null;
    [SerializeField] GameObject dropItem3 = null;
    [SerializeField] GameObject dropItem4 = null;

    //生成したアイテムたち
    List<GameObject> createdItems = new List<GameObject>();
    [SerializeField,Tooltip("ステージ上に生成できるアイテムの限界")]
    int createdItemMax = 30;

    // Update is called once per frame
    void Update()
    {
        // ホスト以外は処理をしない
        if (!MonobitEngine.MonobitNetwork.isHost)
        {
            return;
        }
        elapsedTime += Time.deltaTime;
        if (elapsedTime > createTime)
        {
            CreateItem();
            elapsedTime = 0;
        }
    }
    void CreateItem()
    {
        x = Random.Range(minPos, maxPos);
        z = Random.Range(minPos, maxPos);
        createPosition = new Vector3(x, y, z);
        //アイテムをリストで管理する
        objList.Add(dropItem1);
        objList.Add(dropItem2);
        objList.Add(dropItem3);
        //objList.Add(dropItem4);
        maxItemRange = objList.Count;
        dropItem = Random.Range(minItemRange, maxItemRange);
        if (createdItems.Count < createdItemMax)
        {
            GameObject drop = MonobitEngine.MonobitNetwork.Instantiate(objList[dropItem].name, createPosition, transform.rotation, 0);
            createdItems.Add(drop);
        }
    }
    public void PickUpItem()
    {
        createdItems.RemoveAt(0);
    }
}
