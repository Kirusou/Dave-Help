using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class CharHUD : MonoBehaviour
{
    public Slider HPBar;
    
    public void SetHUD(Unit unit)
    {
        HPBar.maxValue=unit.maxHP;
        HPBar.value=unit.currentHP;
        
    }

    public void SetHP(int hp)
    {
        HPBar.value=hp;
    }
}
