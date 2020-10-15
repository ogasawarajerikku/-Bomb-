using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/*参考サイト
 * https://gametukurikata.com/program/enemychasechara
 */
public class MoveEnemy : MonoBehaviour
{
    public enum EnemyState
    {
        Walk,
        Wait,
        Chase,
        Dead,
        Attack,
        Freeze,
        Damage
    };
    // 敵の状態
    [SerializeField]
    private EnemyState state;
    //　SetPositionスクリプト
    private SetPosition setPosition;
    //　待ち時間
    [SerializeField]
    private float waitTime = 5f;
    //　経過時間
    private float elapsedTime;
    //　プレイヤーTransform
    private Transform playerTransform;
    //　エージェント
    private NavMeshAgent navMeshAgent;
    //　回転スピード
    [SerializeField]
    private float rotateSpeed = 45f;
    [SerializeField, Tooltip("攻撃後のクールタイム"),Range(1f,100f)]
    float freezeTimeAfterAttack = 0;
    void Start()
    {
        setPosition = GetComponent<SetPosition>();
        setPosition.CreateRandomPosition();
        navMeshAgent = GetComponent<NavMeshAgent>();
        elapsedTime = 0f;
        SetState(EnemyState.Walk);
        playerTransform = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == EnemyState.Dead)
        {
            return;
        }
        //　見回りまたはキャラクターを追いかける状態
        if (state == EnemyState.Walk || state == EnemyState.Chase)
        {
            //　キャラクターを追いかける状態であればキャラクターの目的地を再設定
            if (state == EnemyState.Chase)
            {
                setPosition.SetDestination(playerTransform.position);
                navMeshAgent.SetDestination(setPosition.GetDestination());
            }
            //　エージェントの潜在的な速さを設定

            if (state == EnemyState.Walk)
            {
                //　目的地に到着したかどうかの判定
                if (navMeshAgent.remainingDistance < 0.1f)
                {
                    SetState(EnemyState.Wait);
                }
            }
            else if (state == EnemyState.Chase)
            {
                //　攻撃する距離だったら攻撃
                /*マジックナンバー解消する*/
                if (navMeshAgent.remainingDistance < 5f)
                {
                    SetState(EnemyState.Attack);
                }
            }
            //　到着していたら一定時間待つ
        }
        else if (state == EnemyState.Wait)
        {
            elapsedTime += Time.deltaTime;

            //　待ち時間を越えたら次の目的地を設定
            if (elapsedTime > waitTime)
            {
                SetState(EnemyState.Walk);
            }
            //　攻撃後のフリーズ状態
        }
        else if (state == EnemyState.Freeze)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > freezeTimeAfterAttack)
            {
                SetState(EnemyState.Walk);
            }
        }
        else if (state == EnemyState.Attack)
        {
            //　プレイヤーの方向を取得
            var playerDirection = new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z) - transform.position;
            //　敵の向きをプレイヤーの方向に少しづつ変える
            var dir = Vector3.RotateTowards(transform.forward, playerDirection,rotateSpeed * Time.deltaTime, 0f);
            //　算出した方向の角度を敵の角度に設定
            transform.rotation = Quaternion.LookRotation(dir);
            GetComponent<EnemyAttack>().Attack();
            SetState(EnemyState.Freeze);
        }
    }
    //　敵キャラクターの状態変更メソッド
    public void SetState(EnemyState tempState, Transform targetObj = null)
    {
        if (state == EnemyState.Dead)
        {
            return;
        }

        state = tempState;

        if (tempState == EnemyState.Walk)
        {
            elapsedTime = 0f;
            setPosition.CreateRandomPosition();
            navMeshAgent.SetDestination(setPosition.GetDestination());
            navMeshAgent.isStopped = false;
        }
        else if (tempState == EnemyState.Chase)
        {
            //　追いかける対象をセット
            playerTransform = targetObj;
            navMeshAgent.SetDestination(playerTransform.position);
            navMeshAgent.isStopped = false;
        }
        //待機状態
        else if (tempState == EnemyState.Wait)
        {
            elapsedTime = 0f;
        }
        //攻撃状態
        else if (tempState == EnemyState.Attack)
        {
            navMeshAgent.isStopped = true;
        }
        //攻撃後のクールタイム用
        else if (tempState == EnemyState.Freeze)
        {
            elapsedTime = 0f;
        }
        //被ダメージ時
        else if (tempState == EnemyState.Damage)
        {
            navMeshAgent.isStopped = true;
        }
        //死亡時
        else if (tempState == EnemyState.Dead)
        {
            Destroy(this.gameObject, 3f);
            navMeshAgent.isStopped = true;
        }
    }
    //　敵キャラクターの状態取得メソッド
    public EnemyState GetState()
    {
        return state;
    }
}
