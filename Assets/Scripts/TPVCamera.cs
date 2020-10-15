using MonobitEngine;
using UnityEngine;
//制作者　小笠原ジェリック
[RequireComponent(typeof(Camera))]
public class  TPVCamera : MonobitEngine.MonoBehaviour
{
    //キャラクターオブジェクト
    GameObject playerObj=null;
    //　キャラクターのTransform
    [SerializeField]
    private Transform charaLookAtPosition = null;
    //　カメラの移動スピード
    private float cameraMoveSpeed = 4f;
    //　カメラの回転スピード
    private float cameraRotateSpeed = 90f;
    //　カメラのキャラクターからの相対値を指定
    [SerializeField]
    private Vector3 basePos = new Vector3(0f, 0f, 0f);
    //変更前のbasePos
    Vector3 defBasePos = new Vector3(0f, 0f, 0f);
    //プレイヤー追従前のカメラ位置、回転
    Vector3 startCameraPos = new Vector3(0f, 0f, 0f);
    Quaternion startCameraRot = new Quaternion(0f, 0f, 0f, 0f);
    //カメラのバグ回避
    Vector3 addPos = new Vector3(0f, 0.5f, 0f);
    // 障害物とするレイヤー
    [SerializeField]
    private LayerMask obstacleLayer = default;

    private void Start()
    {
        startCameraPos = transform.position;
        startCameraRot = transform.rotation;
        defBasePos = basePos;
    }
    void LateUpdate()
    {
        if (charaLookAtPosition == null)
        {
            GameObject obj = GameObject.Find("Player" + MonobitEngine.MonobitNetwork.player.ID);
            if(obj==null)
            {
                return;
            }
                if (obj.GetComponent<MonobitView>().isMine == true)
                {
                    playerObj = obj;
                }
                else 
                    obj.GetComponent<MonobitEngine.MonobitView>().RequestOwnership();
            if (playerObj == null)
                return;
            if (playerObj.tag != "DeadPlayer")
                charaLookAtPosition = playerObj.transform;
        }
        else if (playerObj.tag == "DeadPlayer")
        {
            playerObj = null;
            charaLookAtPosition = null;
            transform.position = startCameraPos;
            transform.rotation = startCameraRot;
        }
        if (charaLookAtPosition != null)
        {
            //　通常のカメラ位置を計算
            var cameraPos = charaLookAtPosition.position + (charaLookAtPosition.right * basePos.x) + (Vector3.up * basePos.y) + (-charaLookAtPosition.forward * basePos.z);
            //　カメラの位置をキャラクターの後ろ側に移動させる
            transform.position = Vector3.Lerp(transform.position, cameraPos, cameraMoveSpeed * Time.deltaTime);

            RaycastHit hit;
            //　キャラクターとカメラの間に障害物があったら障害物の位置にカメラを移動させる
            if (Physics.Linecast(charaLookAtPosition.position + addPos, transform.position, out hit, obstacleLayer))
            {
                transform.position = hit.point;
            }
            //　レイを視覚的に確認
            Debug.DrawLine(charaLookAtPosition.position + addPos, transform.position, Color.red, 0f, false);

            //　スピードを考慮してカメラの回転
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(charaLookAtPosition.position - transform.position), cameraRotateSpeed * Time.deltaTime);
        }
    }
       
    public void setBasePosX(float x)
    {
        basePos = new Vector3(x, basePos.y, basePos.z); 
    }
    public void resetBasePos()
    {
        basePos = defBasePos;
    }
}