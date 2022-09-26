using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoHUD : MonoBehaviour
{
    public void visInfo()
    {
    transform.position= new Vector2(0,-3);
    }

    public void unvisInfo()
    {
    transform.position= new Vector2(1000,-3);
    }

}
