using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * 制作メモ
 * 距離による威力減衰は考慮する
 */
public class EnemyAttack : MonobitEngine.MonoBehaviour
{

    [SerializeField,Tooltip("銃口")]
    GameObject mazzle = null;
    [SerializeField,Tooltip("生成する弾丸")]
    GameObject enemyBullet = null;
    [SerializeField, Tooltip("発射エフェクト")]
    GameObject firingEffect = null;
    //弾速
    [SerializeField, Range(1f, 5f)]
    float bulletSpeed = 1f;
    //発射間隔
    [SerializeField, Range(0f, 2f)]
    float firingInterval = 1f;
    //経過時間
    float elapsedTime = 0f;
    public AudioClip sound1;
    AudioSource audioSource;
    // MonobitView コンポーネント
    MonobitEngine.MonobitView m_MonobitView = null;
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
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    //設定した間隔での攻撃
    [MunRPC]
    void Attack()
    {
        if (elapsedTime >= firingInterval)
            elapsedTime = 0;
        if (elapsedTime == 0f)
        {
            GameObject shot = Instantiate(enemyBullet, mazzle.transform.position, mazzle.transform.rotation);
            shot.GetComponent<BulletController>().Shoot(bulletSpeed);
            audioSource.PlayOneShot(sound1);
            GameObject e = Instantiate(firingEffect) as GameObject;
            e.transform.position = mazzle.transform.position;
            e.transform.rotation = mazzle.transform.rotation;
        }
        elapsedTime += Time.deltaTime;
        Debug.Log("fire");
    }
}
