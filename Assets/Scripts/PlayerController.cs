using UnityEngine;
using UnityEngine.UI;
//制作者　小笠原ジェリック

public class PlayerController : MonobitEngine.MonoBehaviour
{
    //MonobitViewコンポーネント
    MonobitEngine.MonobitView m_MonobitView = null;

    //移動速度
    float moveSpeed = 6f;

    //振り向き速度
    private float applySpeed = 4.5f;

    //カメラ本体
    private GameObject mainCamera = null;

    //カメラのスクリプト
    TPVCamera cameraScript = null;

    //プレイヤーが地についているか
    bool ground = false;

    //爆弾を射出する位置や角度
    [SerializeField]
    private GameObject shootPosObj = null;

    [SerializeField,Tooltip("ゲームオーバー用キャンバス")]
    GameObject gameoverCanvas = null;

    //液体射出場所の獲得
    public Vector3 InstantiatePosition
    {
        get { return shootPosObj.transform.position; }
    }


    // 弾の初速度
    private Vector3 shootVelocity = new Vector3(0, 0, 0);

    // 弾の初速度(読み取り専用)
    public Vector3 GetShootVelocity
    {
        get { return shootVelocity; }
    }
    //自身RigidBody
    private Rigidbody rb;


    //ジャンプ力
    //[SerializeField, Tooltip("ジャンプ力よっぽどのとき以外変えない")]
    //Vector3 jumpForce = new Vector3(0, 20, 0);

    //移動の入力受付
    float inputHorizontal;
    float inputVertical;

    //カメラ追従用の角度
    Quaternion targetRotation = new Quaternion(0,0,0,0);
    Vector3 forward = new Vector3(0,0,0);

    //投げる角度の限界
    private float maxThrowAngle = -45f;

    //爆弾投擲用
    [SerializeField, Tooltip("生成する爆弾")]
    GameObject bombObject = null;
    //生成した爆弾のRIgidBody
    Rigidbody bombRigid = null;
    //[SerializeField, Tooltip("プレイヤーが投げる角度が変わる速度"), Range(1f, 50f)]
    float angleChangeSpeed = 30f;
    //[SerializeField, Tooltip("プレイヤーが投げる力をためる速度"), Range(1f, 20f)]
    float powerChargeSpeed = 15f;
    //"投げる入力"
    float inputThrowClick = 0;
    //爆弾を射出する速さ
    float bombShotSpeed = 1f;
    //爆弾を射出する速さ(変更前)
    float defBombShotSpeed = 1f;
    //射出する速さの最大値
    float maxBombShotSpeed = 20f;
    //どの程度回転したか
    float nowEndRotate = 0;
    //攻撃可能フラグ
    bool canAttack = true;
    //攻撃クールタイム
    float attackFreezeTime = 1f;
    //経過時間
    float elapsedTime = 0;
    //爆弾最大所有数
    int maxBombCount = 20;
    //爆弾所有数
    int nowBombCount = 0;
    [SerializeField, Tooltip("爆弾所持数表示用のテキストオブジェクト")]
    GameObject bombCountTextObject = null;
    //表示用テキスト
    Text bombCountText = null;
    //爆発ダメージ
    float defExplosionDamage = 100;
    //攻撃可能にするか
    bool setAttack = true;
    //移動用
    Vector3 moveForward = new Vector3(0, 0, 0);
    private void Awake()
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
        mainCamera = GameObject.Find("Main Camera");
        cameraScript = mainCamera.GetComponent<TPVCamera>();
        rb = this.GetComponent<Rigidbody>();
        //デバッグ用の時間操作
        //Time.timeScale = 0.25f;
        //ゲームオーバーからリスタート時にタイムスケールをリセット
        Time.timeScale = 1f;
        nowBombCount = maxBombCount;
        bombCountTextObject.SetActive(true);
        bombCountText = bombCountTextObject.GetComponent<Text>();
        
    }
    void Update()
    {
        // オブジェクト所有権を所持しなければ実行しない
        if (!m_MonobitView.isMine)
           return;
        if(gameObject.tag=="DeadPlayer")
            return;
        forward = rb.transform.forward;
        inputHorizontal = Input.GetAxisRaw("Horizontal");
        inputVertical = Input.GetAxisRaw("Vertical");
        bombCountText.text = "" + nowBombCount;
        if (Input.GetKey(KeyCode.S))
            rb.AddForce(-forward);
        //左右で回転速度が変わってしまうのでそれを防ぐ
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            cameraScript.setBasePosX(0);
        else if(Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            cameraScript.resetBasePos();
        
        
        // 方向キーの入力値とカメラの向きから、移動方向を決定
        moveForward = transform.forward * inputVertical + Camera.main.transform.right * inputHorizontal;

        // キャラクターの向きをカメラから見て進行方向に
        if (moveForward != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(moveForward);
            if (!Input.GetKey(KeyCode.S))
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * applySpeed);
        }

        /*ジャンプ判定
        if (ground)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.velocity += jumpForce;
                ground = false;
            }
        }*/

        //ダッシュ
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed *= 2f;
            applySpeed *= 2f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed /= 2f;
            applySpeed /= 2f;
        }

        if (canAttack == false)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= attackFreezeTime)
            {
                if (setAttack == true)
                    canAttack = true;
                elapsedTime = 0;
            }
        }
        //徐々に投げる角度を上げていき飛ばす威力も伸ばす
        if (canAttack)
        {
            if (Input.GetMouseButton(0))
            {
                inputThrowClick = -1;
                shootPosObj.transform.Rotate(new Vector3(inputThrowClick * Time.deltaTime * angleChangeSpeed, 0F, 0F));
                nowEndRotate += inputThrowClick * Time.deltaTime * angleChangeSpeed;
                bombShotSpeed += Time.deltaTime * powerChargeSpeed;
            }
            //爆弾の残量を確認し投擲する
            if (Input.GetMouseButtonUp(0))
            {
                if (nowBombCount > 0)
                {
                    GameObject shot = MonobitEngine.MonobitNetwork.Instantiate(bombObject.name, shootPosObj.transform.position, bombObject.transform.rotation,0);
                    shot.GetComponent<Bomb>().SetBombDamage(defExplosionDamage);
                    bombRigid = shot.GetComponent<Rigidbody>();
                    bombRigid.AddForce(shootVelocity * bombRigid.mass, ForceMode.Impulse);
                    inputThrowClick = 0;
                    nowEndRotate = 0;
                    shootPosObj.transform.rotation = Quaternion.Euler(0, transform.localEulerAngles.y, 0);
                    bombShotSpeed = defBombShotSpeed;
                    canAttack = false;
                    nowBombCount--;
                }
            }
        }
        shootVelocity = shootPosObj.transform.forward * bombShotSpeed;
        //速度限界に達したら
        if (bombShotSpeed >= maxBombShotSpeed)
            bombShotSpeed = maxBombShotSpeed;
        //角度限界に達したら
        if (nowEndRotate < maxThrowAngle)
            shootPosObj.transform.rotation = Quaternion.Euler(maxThrowAngle, transform.localEulerAngles.y, 0);


        if (GetComponent<HpStatus>().GetHitPoint()<=0)
        {
            gameObject.tag = "DeadPlayer";
            //ゲームオーバー時の処理
            gameoverCanvas.SetActive(true);
        }
    }
    private void FixedUpdate()
    {
        // 移動方向にスピードを掛ける。ジャンプや落下がある場合は、別途Y軸方向の速度ベクトルを足す。
        if (ground)
            rb.velocity = moveForward * moveSpeed + new Vector3(0, rb.velocity.y, 0);
    }
    private void OnCollisionStay(Collision other)
    {
        //地面と触れたときに接地判定する
        if (other.gameObject.tag == "Ground")
            ground = true;
    }
    private void OnCollisionExit(Collision other)
    {
        //接地判定解除
        if (other.gameObject.tag == "Ground")
            ground = false;
    }
    public void AddBomb(int addValue)
    {
        nowBombCount += addValue;
    }
    public void AddBombDamage(float addDamage)
    {
        defExplosionDamage += addDamage;
    }
    public void SetCanAttack(bool flag)
    {
        canAttack = flag;
        setAttack = flag;
    }
}