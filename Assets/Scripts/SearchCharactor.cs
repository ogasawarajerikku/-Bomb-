using UnityEngine;
//参考サイト
//https://gametukurikata.com/program/enemychasechara
public class SearchCharactor : MonoBehaviour
{
    /*この処理のせいで敵の動きがおかしくなる場合があるので要チェック*/
    private MainEnemy mainEnemy;

    void Start()
    {
        mainEnemy = GetComponentInParent<MainEnemy>();
    }

    void OnTriggerStay(Collider col)
    {
        //　プレイヤーキャラクターを発見
        if (col.tag == "Player")
        {
            //　敵キャラクターの状態を取得
            MainEnemy.EnemyState state = mainEnemy.GetState();
            //　敵キャラクターが追いかける状態でなければ追いかける設定に変更
            if (state != MainEnemy.EnemyState.Chase &&state!=MainEnemy.EnemyState.Freeze&& state != MainEnemy.EnemyState.Attack)
            {
                mainEnemy.SetState(MainEnemy.EnemyState.Chase, col.transform);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            mainEnemy.SetState(MainEnemy.EnemyState.Walk);
        }
    }
}
