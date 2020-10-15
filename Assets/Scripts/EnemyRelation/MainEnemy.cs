using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/*参考サイト
 * https://gametukurikata.com/program/enemychasechara
 */
/*制作メモ
 * もう少し滑らかにプレイヤーを追従してほしい
 */
public class MainEnemy : MonoBehaviour
{
    public enum EnemyState
    {
        //歩行
        Walk,
        //待機
        Wait,
        //死亡
        Dead,
        //攻撃
        Attack,
        //攻撃後のクールタイム
        Freeze,
        //被ダメージ時
        Damage
    };
    // 敵の状態
    [SerializeField]
    private EnemyState state;
    //　SetPositionスクリプト
    private SetPosition setPosition;
    //死亡時処理用のスクリプト
    private DeadEnemy deadEnemyScript;
    //　待ち時間
    [SerializeField]
    private float waitTime = 5f;
    //　経過時間
    //[SerializeField]
    private float elapsedTime = 0;
    //　エージェント
    private NavMeshAgent navMeshAgent;
    //ゲーム内の全プレイヤー
    GameObject[] m_Player = null;
    [SerializeField, Tooltip("振り向き速度"), Range(0.5f, 2f)]
    private float rotateSpeed = 1f;
    [SerializeField, Tooltip("攻撃後のクールタイム"),Range(1f,10f)]
    private float freezeTimeAfterAttack = 1.5f;
    [SerializeField, Tooltip("攻撃継続時間"), Range(1f, 10f)]
    private float attackContinueTime = 5f;
    [SerializeField, Tooltip("攻撃距離"), Range(1f, 20f)]
    private float attackDistance = 10f;
    //攻撃対象のプレイヤーの場所
    Vector3 selectedPlayerPos = Vector3.zero;
    // MonobitView コンポーネント
    MonobitEngine.MonobitView m_MonobitView = null;

    Vector3 targetDirection = new Vector3(0,0,0);
    void Awake()
    {
        // すべての親オブジェクトに対して MonobitView コンポーネントを検索する
        if (GetComponentInParent<MonobitEngine.MonobitView>() != null)
        {
            m_MonobitView = GetComponentInParent<MonobitEngine.MonobitView>();
        }
        // 親オブジェクトに存在しない場合、すべての子オブジェクトに対して MonobitView コンポーネントを検索する
        else if (GetComponentInChildren<MonobitEngine.MonobitView>() != null)
        {
            m_MonobitView = GetComponentInChildren<MonobitEngine.MonobitView>();
        }
        // 親子オブジェクトに存在しない場合、自身のオブジェクトに対して MonobitView コンポーネントを検索して設定する
        else
        {
            m_MonobitView = GetComponent<MonobitEngine.MonobitView>();
        }
    }
    void Start()
    {
        setPosition = GetComponent<SetPosition>();
        setPosition.CreateRandomPosition();
        navMeshAgent = GetComponent<NavMeshAgent>();
        elapsedTime = 0f;
        SetState(EnemyState.Walk);
        deadEnemyScript = GetComponent<DeadEnemy>();
        m_Player = GameObject.FindGameObjectsWithTag("Player");
        MonobitEngine.MonobitNetwork.updateStreamRate = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if(!MonobitEngine.MonobitNetwork.isHost)
        {
            return;
        }
        if (state != EnemyState.Attack && state != EnemyState.Freeze)
        {
            if (ShouldShootPlayer())
            {
                SetState(EnemyState.Attack, selectedPlayerPos);
            }
        }
        //　見回りまたはキャラクターを追いかける状態
        if (state == EnemyState.Walk)
        {
            targetDirection = new Vector3(0, 0, 0);
            //　キャラクターを追いかける状態であればキャラクターの目的地を再設定
            if (state == EnemyState.Walk)
                //　ターゲットの方向を取得
                targetDirection = setPosition.GetDestination() - transform.position;
            //　目的地に到着したかどうかの判定
            if (navMeshAgent.remainingDistance < 0.1f)
            {
                SetState(EnemyState.Wait);
            }
            //　敵の向きをターゲットの方向に少しづつ変える
            var dir = Vector3.RotateTowards(transform.forward, targetDirection, rotateSpeed * Time.deltaTime, 0f);
            //　算出した方向の角度を敵の角度に設定
            transform.rotation = Quaternion.LookRotation(dir);
        }
        else if (state == EnemyState.Wait)
        {
            elapsedTime += Time.deltaTime;

            //　待ち時間を越えたら次の目的地を設定
            if (elapsedTime > waitTime)
            {
                SetState(EnemyState.Walk);
            }
        }

        else if (state == EnemyState.Freeze)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime > freezeTimeAfterAttack)
            {
                if (ShouldShootPlayer())
                    SetState(EnemyState.Attack);
                else
                    SetState(EnemyState.Walk);
            }
        }
        else if (state == EnemyState.Attack)
        {
            elapsedTime += Time.deltaTime;
            //　プレイヤーの方向を取得
            var targetDirection = new Vector3(selectedPlayerPos.x, selectedPlayerPos.y, selectedPlayerPos.z) - transform.position;
            //　敵の向きをプレイヤーの方向に少しづつ変える
            var dir = Vector3.RotateTowards(transform.forward, targetDirection, rotateSpeed * Time.deltaTime, 0f);
            //　算出した方向の角度を敵の角度に設定
            transform.rotation = Quaternion.LookRotation(dir);
           m_MonobitView.RPC("Attack", MonobitEngine.MonobitTargets.All);
            if (elapsedTime > attackContinueTime)
                SetState(EnemyState.Freeze);
        }
        if(GetComponent<HpStatus>().GetHitPoint()<=0)
        {
            SetState(EnemyState.Dead);
        }
    }

    //　敵キャラクターの状態変更メソッド
    public void SetState(EnemyState tempState, Vector3? targetObj = null)
    {
        
        state = tempState;

        if (tempState == EnemyState.Walk)
        {
            elapsedTime = 0f;
            setPosition.CreateRandomPosition();
            navMeshAgent.SetDestination(setPosition.GetDestination());
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
            elapsedTime = 0f;
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
            deadEnemyScript.Dead();
        }
    }

    //　敵キャラクターの状態取得メソッド
    public EnemyState GetState()
    {
        return state;
    }

    bool ShouldShootPlayer()
    {
        // ルーム入室中のみ、プレイヤー情報の更新を行なう
        if (MonobitEngine.MonobitNetwork.isConnect && MonobitEngine.MonobitNetwork.inRoom)
        {
            if (m_Player.Length != MonobitEngine.MonobitNetwork.room.playerCount)
            {
                // プレイヤー情報の再取得
                m_Player = GameObject.FindGameObjectsWithTag("Player");
            }
        }

        for (int i = 0; i < m_Player.Length; i++)
        {
            float distanceToPlayer = Vector3.Distance(m_Player[i].transform.position, transform.position);
            if (distanceToPlayer < attackDistance)
            {

                if (m_Player[i].gameObject.tag != "DeadPlayer")
                {
                    selectedPlayerPos = m_Player[i].transform.position;
                    Debug.Log(selectedPlayerPos);
                    return true;
                }
            }
        }
        return false;
    }
}
