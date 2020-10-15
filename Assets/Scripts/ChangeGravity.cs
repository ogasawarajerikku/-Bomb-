using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//制作者　小笠原ジェリック
public class ChangeGravity : MonoBehaviour
{
    [SerializeField] private Vector3 localGravity = Vector3.zero;
    private Rigidbody rBody;

    // Use this for initialization
    private void Start()
    {
        rBody = this.GetComponent<Rigidbody>();
        rBody.useGravity = false; //最初にrigidBodyの重力を使わなくする
    }

    private void FixedUpdate()
    {
        SetLocalGravity(); //重力をAddForceでかけるメソッドを呼ぶ。FixedUpdateが好ましい。
    }

    private void SetLocalGravity()
    {
        rBody.AddForce(localGravity, ForceMode.Acceleration);
    }
}