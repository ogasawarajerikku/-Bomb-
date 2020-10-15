
using UnityEngine;
//参考サイト
//http://hamken100.blogspot.com/2012/06/unity-gameobject.html
public class BombController : MonoBehaviour
{
    [SerializeField, Tooltip("爆発力")]
    float explosionPower = 0;
    [SerializeField, Tooltip("爆発影響範囲")]
    float explosionRadius = 0;
    [SerializeField, Tooltip("上方向にどの程度影響させるか")]
    float upwardsModifier = 0;
    [SerializeField, Tooltip("爆発エフェクト")]
    GameObject explosionEffect = null;
    float defExplosionDamage = 100;
    //爆発ダメージ
    float explosionDamage = 0;
    //敵と爆弾の距離
    float enemyDistance = 0;
    //威力減衰量
    int distanceDoubling = 5;
    Vector3 expPos = new Vector3(0, -2, 0);
    private void Start()
    {

    }
    void OnCollisionEnter(Collision collision)
    {
        // explosionRadiusには、任意の爆発影響半径を入れます
        Collider[] targets = Physics.OverlapSphere(transform.position, explosionRadius);


        foreach (Collider obj in targets)
        {
            if (obj && obj.tag != "Ground" && obj.tag != "Item")
            {
                obj.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, transform.position + expPos, explosionRadius, upwardsModifier);
            }
            //爆発範囲内に敵かプレイヤーがいた場合距離によるダメージ減衰を計算してダメージを与える
            //(プレイヤーだけ減衰量を増やしている)
            if (obj && obj.tag == "Enemy" || obj.tag == "Player")
            {
                explosionDamage = defExplosionDamage;
                enemyDistance = Vector3.Distance(transform.position, obj.transform.position);
                if (obj.tag == "Player")
                    explosionDamage -= enemyDistance * distanceDoubling * 2;
                else
                    explosionDamage -= enemyDistance * distanceDoubling;
                if (explosionDamage <= 0)
                    explosionDamage = 0;
                obj.gameObject.GetComponent<HpStatus>().MinusHitPoint(explosionDamage);
                explosionDamage = defExplosionDamage;
            }
        }
        GameObject e = Instantiate(explosionEffect) as GameObject;
        e.transform.position = transform.position;
        Destroy(gameObject);
    }
    public void SetBombDamage(float bombDamage)
    {
        defExplosionDamage = bombDamage;
    }

}
