using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//制作者　小笠原ジェリック
public class MateriaiTilingFloor : MonoBehaviour
{
    Material material;
    public bool up;
    public bool vertical;
    public bool side;
    public bool child;
    float scaleX = 0;
    float scaleY = 0;
    float scaleZ = 0;
    private void Awake()
    {
        material = GetComponent<Renderer>().material;
            scaleX = transform.localScale.x;
            scaleY = transform.localScale.y;
            scaleZ = transform.localScale.z;
    }
    private void Start()
    {
        if (child)
        {
            scaleX = transform.parent.localScale.x;
            scaleY = transform.parent.localScale.y;
            scaleZ = transform.parent.localScale.z;
        }
        if (up)
        {
            material.SetTextureScale("_MainTex", new Vector2(scaleX, scaleZ));
            material.SetTextureScale("_BumpMap", new Vector2(scaleX, scaleZ));
        }
        if (vertical)
        {
            material.SetTextureScale("_MainTex", new Vector2(scaleZ, scaleX));
            material.SetTextureScale("_BumpMap", new Vector2(scaleZ, scaleX));
        }
        if (side)
        {
            material.SetTextureScale("_MainTex", new Vector2(scaleX, scaleY));
            material.SetTextureScale("_BumpMap", new Vector2(scaleX, scaleY));
        }
    }
}