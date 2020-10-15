
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Rigidbody rb = null;
    Vector3 force = new Vector3(0, 0, 0);
    public AudioClip sound1;
    AudioSource audioSource;
    [SerializeField, Tooltip("弾の攻撃力"), Range(1f,10f)]
    float bulletDamage = 1f;

    GameObject playerObject = null;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void Shoot(float bulletSpeed)
    {
        rb = GetComponent<Rigidbody>();
        force = transform.forward * bulletSpeed;
        rb.AddForce(force,ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            playerObject = collision.gameObject;

            playerObject.GetComponent<HpStatus>().MinusHitPoint(bulletDamage);
            audioSource.PlayOneShot(sound1);
        }
            Destroy(GetComponent<BulletController>());
        Destroy(gameObject, 3f);
    }
}
