using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OptionBlock : MonoBehaviour
{
    private Buff buff;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetBuff(Buff buff)
    {
        this.buff = buff;
        string text = "buff";
        switch (buff)
        {
            case Buff.BulletCapIncrease:
                text = "Magazine Capcity +1";
                break;
            case Buff.HPCountIncrease:
                text = "MAX HP +1";
                break;
            case Buff.DamageIncrease:
                text = "Damage +10%";
                break;
            case Buff.RecoverAllHP:
                text = "Recover All HP";
                break;
            case Buff.AutoReload:
                text = "Auto Reload";
                break;
            case Buff.AutoShot:
                text = "Auto Shot";
                break;
            case Buff.TimeSlowMult:
                text = "Increase Time Slow Mult";
                break;
            case Buff.TimeSlowTime:
                text = "Time Slow time +0.1s";
                break;
        }
        transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = text;
    }
    public void GetHit()
    {
        GameManager.Instance.EnhancePlayer(buff);
        GameManager.chooseRewardPhase = false;
    }
}
