using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*参考サイト
 https://gametukurikata.com/ui/hpui
 */
public class EnemyStatus : MonoBehaviour
{
    //HP最大値
    [Range(1f, 100f)]
    int maxHitPoint = 100;
    //HP
    int hitPoint = 0;
    [SerializeField]
    GameObject HPUI = null;
    //HP表示用スライダー
    Slider hpSlider = null;
    void Start()
    {
        hpSlider = HPUI.transform.Find("HPBar").GetComponent<Slider>();
        hpSlider.value = 1f;
    }

    public void SetHitPoint(int hp)
    {
        this.hitPoint = hp;
        //HPUIの更新
        UpdateHPValue();
        if (hitPoint <= 0)
        {
            HideStatusUI();
        }
    }
    public int GetHitPoint()
    {
        return hitPoint;
    }
    public int GetMaxHitPoint()
    {
        return maxHitPoint;
    }
    public void HideStatusUI()
    {
        HPUI.SetActive(false);
    }
    public void UpdateHPValue()
    {
        hpSlider.value = (float)GetHitPoint() / (float)GetMaxHitPoint();
    }
}
