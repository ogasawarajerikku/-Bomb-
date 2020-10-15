using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*参考サイト
 https://gametukurikata.com/ui/hpui
 */
public class HpStatus : MonoBehaviour
{
    //HP最大値
    [Range(1f, 100f)]
    int maxHitPoint = 100;
    //HP
    float hitPoint = 0;
    [SerializeField]
    GameObject HPUI = null;
    //HP表示用スライダー
    Slider hpSlider = null;
    void Start()
    {
        hpSlider = HPUI.transform.Find("HPBar").GetComponent<Slider>();
        hpSlider.value = 1f;
        hitPoint = maxHitPoint;
    }

    public void PlusHitPoint(float healValue)
    {
        this.hitPoint = hitPoint + healValue;
        UpdateHPValue();
    }
    public void MinusHitPoint(float damage)
    {
        this.hitPoint = hitPoint - damage;
        UpdateHPValue();
        if (hitPoint <= 0)
        {
            HideStatusUI();
        }
    }
    public float GetHitPoint()
    {
        return hitPoint;
    }
    public float GetMaxHitPoint()
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
